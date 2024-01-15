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
        public void TestSimpleLPWithIntegerSolution()
        {
            const string lp = """
                              // Objective function
                              max: + 6*x0 + 5*x1;
                              // constraints
                               + 1*x0 + 1*x1 <= 5;
                               + 3*x0 + 2*x1 <= 12;
                              """;
            _lpsolve!.SolveFromLpSolveNativeFormat(lp);
        }

        [Test]
        public void TestSimpleLPWithNoIntegerSolution()
        {
            const string lp = """
                              // Objective function
                              max: + 5*x0 + 6*x1;
                              // constraints
                               + 1*x0 + 1*x1 <= 5;
                               + 4*x0 + 7*x1 <= 28;
                              """;
            _lpsolve!.SolveFromLpSolveNativeFormat(lp);
        }

        [Test]
        public void TestSimpleILP()
        {
            const string lp = """
                              // Objective function
                              max: + 5*x0 + 6*x1;
                              // constraints
                               + 1*x0 + 1*x1 <= 5;
                               + 4*x0 + 7*x1 <= 28;
                              // declaration
                              int x0, x1;
                              """;
            _lpsolve!.SolveFromLpSolveNativeFormat(lp);
        }
    }
}