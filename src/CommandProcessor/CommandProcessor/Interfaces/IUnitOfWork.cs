using System;

//using NHibernate;

namespace MvcContrib.CommandProcessor.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		void Invalidate();
	}
}