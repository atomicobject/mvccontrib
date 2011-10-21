using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Services.Transaction;
using Castle.Windsor;
using MvcContrib.Castle;
using MvcContrib.Services;
using NUnit.Framework;
using Rhino.Mocks;
using IDependencyResolver = Castle.MicroKernel.IDependencyResolver;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture, Obsolete]
	public class MvcTransactionAttributeTester
	{
		private ITransactionManager manager;
		private MvcTransactionAttribute attribute;

		[SetUp]
		public void SetUp()
		{
			var container = new WindsorContainer();
			DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
			container.Register(Component.For<ITransactionManager>().ImplementedBy<TestITransactionManager>().Named("transaction.manager"));
			manager = DependencyResolver.Current.GetService<ITransactionManager>();
            attribute = new MvcTransactionAttribute();
		}

		[Test]
		public void TransactionStarted_OnActionExecuting()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());

			Assert.AreEqual(1, ((TestITransactionManager)manager).CreateTransactionCalled);
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).BeginCalled);
		}

		[Test, ExpectedException(typeof(TransactionException))]
		public void NonStartedTransactionThrows_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuted(GetActionExecutedContext(null));
		}
        
		[Test]
		public void TransactionCommitted_AndDisposed_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			attribute.OnActionExecuted(GetActionExecutedContext(null));
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}

		[Test]
		public void TransactionRolledback_WhenExection_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			Exception thrown = null;
			try
			{
				attribute.OnActionExecuted(GetActionExecutedContext(new Exception("Exception")));
			}
			catch(Exception e)
			{
				thrown = e;
			}
			Assert.IsNotNull(thrown);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}

		[Test]
		public void TransactionRolledback_WhenIsRollbackOnlySet_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			manager.CurrentTransaction.SetRollbackOnly();
			attribute.OnActionExecuted(GetActionExecutedContext(null));
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}


		[Test]
		public void TransactionExceptionsRethrownWithNoRollback_WhenIsRollbackOnlySet_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			Exception thrown = null;
			try
			{
				attribute.OnActionExecuted(GetActionExecutedContext(new TransactionException("Exception")));
			}
			catch (Exception e)
			{
				thrown = e;
			}
			Assert.IsNotNull(thrown);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}

		[Test]
		public void CanCreateDifferentAttributesWithCorrectParameters()
		{
			var attribute1 = new MvcTransactionAttribute();
			
			var attribute2 = new MvcTransactionAttribute(TransactionMode.Supported);
			Assert.AreEqual(TransactionMode.Supported,attribute2.TransactionMode);
			var attribute3 = new MvcTransactionAttribute(TransactionMode.RequiresNew, IsolationMode.Chaos);
			Assert.AreEqual(TransactionMode.RequiresNew, attribute3.TransactionMode);
			Assert.AreEqual(IsolationMode.Chaos, attribute3.IsolationMode);
		}

		private static ActionExecutingContext GetActionExecutingContext()
		{
			var actionExecutingContext = new ActionExecutingContext(GetControllerContext(), MockRepository.GenerateStub<ActionDescriptor>(), new Dictionary<string, object>());
			return actionExecutingContext;
		}

		private static ActionExecutedContext GetActionExecutedContext(Exception e)
		{
			var actionExecutingContext = new ActionExecutedContext(GetControllerContext(), MockRepository.GenerateStub<ActionDescriptor>() ,false, e);
			return actionExecutingContext;
		}

		private static ControllerContext GetControllerContext()
		{
			var controller = new TestingController();
			var mockHttpContext = MockRepository.GenerateStub<HttpContextBase>();
			var controllerContext = new ControllerContext(mockHttpContext, new RouteData(), controller);
			controller.ControllerContext = controllerContext;
			return controllerContext;
		}


		internal class TestingController : Controller
		{
		}

		internal class TestITransactionManager : ITransactionManager, IFacility
		{
			public int CreateTransactionCalled { get; set; }
			public int DisposeTransactionCalled { get; set; }

			public ITransaction CreateTransaction(TransactionMode transactionMode, IsolationMode isolationMode)
			{
				return CreateTransaction(transactionMode, isolationMode, false);
			}

			public ITransaction CreateTransaction(TransactionMode transactionMode, IsolationMode isolationMode,
			                                      bool distributedTransaction)
			{
				CreateTransactionCalled++;
				return (CurrentTransaction = new TestITransaction());
			}

			public void Dispose(ITransaction transaction)
			{
				DisposeTransactionCalled++;
			}

			public ITransaction CurrentTransaction { get; set; }

			public void Init(IKernel kernel, IConfiguration facilityConfig) {}
			public void Terminate() {}

			#region ITransactionManager Members

			public event EventHandler<TransactionEventArgs> ChildTransactionCreated;

			protected void OnChildTransactionCreated(TransactionEventArgs e)
			{
				if(ChildTransactionCreated != null)
				{
					ChildTransactionCreated(this, e);
				}
			}


			public event EventHandler<TransactionEventArgs> TransactionCreated;

			protected void OnTransactionCreated(TransactionEventArgs e)
			{
				if(TransactionCreated != null)
				{
					TransactionCreated(this, e);
				}
			}

			public event EventHandler<TransactionEventArgs> TransactionDisposed;

			protected void OnTransactionDisposed(TransactionEventArgs e)
			{
				if(TransactionDisposed != null)
				{
					TransactionDisposed(this, e);
				}
			}

			#endregion

			#region IEventPublisher Members

			public event EventHandler<TransactionEventArgs> TransactionCompleted;

			protected void OnTransactionCompleted(TransactionEventArgs e)
			{
				if(TransactionCompleted != null)
				{
					TransactionCompleted(this, e);
				}
			}

			public event EventHandler<TransactionFailedEventArgs> TransactionFailed;

			protected void OnTransactionFailed(TransactionFailedEventArgs e)
			{
				if(TransactionFailed != null)
				{
					TransactionFailed(this, e);
				}
			}

			public event EventHandler<TransactionEventArgs> TransactionRolledBack;

			protected void OnTransactionRolledBack(TransactionEventArgs e)
			{
				if(TransactionRolledBack != null)
				{
					TransactionRolledBack(this, e);
				}
			}

			#endregion
		}

		internal class TestITransaction : ITransaction
		{
			public int BeginCalled { get; set; }
			public int CommitCalled { get; set; }
			public int RollbackCalled { get; set; }

			public void Begin()
			{
				BeginCalled++;
			}

			public void Commit()
			{
				CommitCalled++;
			}

			public void Rollback()
			{
				RollbackCalled++;
			}

			public void SetRollbackOnly()
			{
				IsRollbackOnlySet = true;
			}


			public bool IsRollbackOnlySet { get; set; }

			#region ITransaction Members

			public IDictionary Context
			{
				get { throw new NotImplementedException(); }
			}

			public void Enlist(IResource resource)
			{
				throw new NotImplementedException();
			}

			public bool IsAmbient
			{
				get { throw new NotImplementedException(); }
			}

			public bool IsChildTransaction
			{
				get { throw new NotImplementedException(); }
			}

			public IsolationMode IsolationMode
			{
				get { throw new NotImplementedException(); }
			}

			public string Name
			{
				get { throw new NotImplementedException(); }
			}

			public void RegisterSynchronization(ISynchronization synchronization)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<IResource> Resources()
			{
				throw new NotImplementedException();
			}

			public TransactionStatus Status
			{
				get { throw new NotImplementedException(); }
			}

			public TransactionMode TransactionMode
			{
				get { throw new NotImplementedException(); }
			}

			#endregion
		}
	
		private class WindsorDependencyResolver : System.Web.Mvc.IDependencyResolver
		{
			IWindsorContainer _container;
			public WindsorDependencyResolver(IWindsorContainer container)
			{
				_container = container;
			}


			public object GetService(Type serviceType)
			{
				return _container.Resolve(serviceType);
			}

			public IEnumerable<object> GetServices(Type serviceType)
			{
				yield break;
			}
		}
	}

}
