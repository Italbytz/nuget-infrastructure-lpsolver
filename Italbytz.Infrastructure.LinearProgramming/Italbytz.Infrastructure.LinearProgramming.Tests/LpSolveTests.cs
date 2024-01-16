using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.Infrastructure.LinearProgramming.Tests
{
    public class LpSolveTests
    {
        private static ILoggerFactory _loggerFactory =
            NullLoggerFactory.Instance;

        private LpSolver? _lpsolve;

        [SetUp]
        public void Setup()
        {
            _loggerFactory =
                LoggerFactory.Create(builder => builder.AddConsole());
            _lpsolve = new LpSolver(_loggerFactory);
        }

        [Test]
        public void Test1()
        {
            _lpsolve.Test();
            Assert.Pass();
        }
    
        [Test]
        public void Test2()
        {
            _lpsolve.Test2();
            Assert.Pass();
        }
    }
}