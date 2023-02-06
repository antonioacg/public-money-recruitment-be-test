using Moq;
using TechTalk.SpecFlow;

namespace VacationRental.Application.Tests.Extensions
{
    internal static class ScenarioContextExtensions
    {
        internal static Mock<T> SetMock<T>(this ScenarioContext scenarioContext) where T : class
        {
            var mock = new Mock<T>();
            scenarioContext.Set(mock);
            scenarioContext.Set(() => scenarioContext.GetMock<T>().Object);
            return mock;
        }

        internal static Mock<T> GetMock<T>(this ScenarioContext scenarioContext) where T : class
        {
            return scenarioContext.Get<Mock<T>>();
        }

        internal static Mock<T> GetOrSetMock<T>(this ScenarioContext scenarioContext) where T : class
        {
            if (scenarioContext.TryGetValue<Mock<T>>(out var mock))
                return mock;
            return scenarioContext.SetMock<T>();
        }
    }
}
