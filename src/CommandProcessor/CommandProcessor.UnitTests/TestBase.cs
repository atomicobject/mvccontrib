using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.CommandProcessor.UnitTests
{
	[TestFixture]
	public abstract class TestBase
	{
		/// <summary>
		/// Create a mock
		/// </summary>
		/// <typeparam name="T">Type to be mocked</typeparam>
		/// <param name="argumentsForConstructor">Constructor arguments</param>
		/// <returns>T</returns>
		protected static T M<T>(params object[] argumentsForConstructor) where T : class
		{
			return MockRepository.GenerateMock<T>(argumentsForConstructor);
		}

		/// <summary>
		/// Create a stub
		/// </summary>
		/// <typeparam name="T">Type to be stubbed</typeparam>
		/// <param name="argumentsForConstructor">Constructor arguments</param>
		/// <returns>T</returns>
		protected static T S<T>(params object[] argumentsForConstructor) where T : class
		{
			return MockRepository.GenerateStub<T>(argumentsForConstructor);
		}

		/// <summary>
		/// Create a partial mock (for mocking entities)
		/// </summary>
		/// <typeparam name="T">Type to be partial mocked</typeparam>
		/// <param name="argumentsForConstructor">Constructor arguments</param>
		/// <returns>T</returns>
		protected static T PartialMock<T>(params object[] argumentsForConstructor) where T : class
		{
			return MockRepository.GenerateStub<T>(argumentsForConstructor);
		}

		//protected static AutoMocker<T> AutoMock<T>() where T : class
		//{
		//    return new RhinoAutoMocker<T>();
		//}

		//protected static ISystemClock ClockStub(DateTime time)
		//{
		//    var clock = S<ISystemClock>();
		//    clock.Stub(x => x.GetCurrentDateTime()).Return(time);
		//    return clock;
		//}

		//protected static IUserSession CurrentUserStub(User user)
		//{
		//    var session = S<IUserSession>();
		//    session.Stub(x => x.GetCurrentUser()).Return(user);
		//    return session;
		//}
	}
}