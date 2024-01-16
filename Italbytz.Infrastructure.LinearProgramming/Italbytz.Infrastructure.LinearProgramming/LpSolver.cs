using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Italbytz.Infrastructure.LinearProgramming {

    public class LpSolver
    {
        public LpSolver(ILoggerFactory loggerFactory) =>
            LoggingExtensions.InitLoggers(loggerFactory);

        public void Test()
    {
        alglib.minlp.minlpstate state = new(); 
        alglib.minlp.minlpcreate(2,state,null);
        alglib.minlp.minlpsetcost(state,new []{-6.0,-5.0},null);
        alglib.minlp.minlpsetbcall(state, 0.0, double.PositiveInfinity,null);
        alglib.minlp.minlpaddlc2dense(state, new[] { 1.0, 1.0 },
            double.NegativeInfinity, 5.0,null);
        alglib.minlp.minlpaddlc2dense(state, new[] { 3.0, 2.0 },
            double.NegativeInfinity, 12.0,null);
        alglib.minlp.minlpoptimize(state,null);
        alglib.minlp.minlpreport report = new();
        var x = new double[2];
        alglib.minlp.minlpresults(state, ref x, report, null);
    }

    public void Test2()
    {
        try
        {
            //
            // This example demonstrates how to minimize
            //
            //     F(x0,x1) = -0.1*x0 - x1
            //
            // subject to box constraints
            //
            //     -1 <= x0,x1 <= +1 
            //
            // and general linear constraints
            //
            //     x0 - x1 >= -1
            //     x0 + x1 <=  1
            //
            // We use dual simplex solver provided by ALGLIB for this task. Box
            // constraints are specified by means of constraint vectors bndl and
            // bndu (we have bndl<=x<=bndu). General linear constraints are
            // specified as AL<=A*x<=AU, with AL/AU being 2x1 vectors and A being
            // 2x2 matrix.
            //
            // NOTE: some/all components of AL/AU can be +-INF, same applies to
            //       bndl/bndu. You can also have AL[I]=AU[i] (as well as
            //       BndL[i]=BndU[i]).
            //
            var a = new double[,]{{1,-1},{1,+1}};
            var al = new[]{-1,-double.PositiveInfinity};
            var au = new[]{double.PositiveInfinity,+1};
            var c = new[]{-0.1,-1};
            var s = new double[]{1,1};
            var bndl = new double[]{-1,-1};
            var bndu = new double[]{+1,+1};
            alglib.minlpreport rep;

            alglib.minlpcreate(2, out var state);

            //
            // Set cost vector, box constraints, general linear constraints.
            //
            // Box constraints can be set in one call to minlpsetbc() or minlpsetbcall()
            // (latter sets same constraints for all variables and accepts two scalars
            // instead of two vectors).
            //
            // General linear constraints can be specified in several ways:
            // * minlpsetlc2dense() - accepts dense 2D array as input; sometimes this
            //   approach is more convenient, although less memory-efficient.
            // * minlpsetlc2() - accepts sparse matrix as input
            // * minlpaddlc2dense() - appends one row to the current set of constraints;
            //   row being appended is specified as dense vector
            // * minlpaddlc2() - appends one row to the current set of constraints;
            //   row being appended is specified as sparse set of elements
            // Independently from specific function being used, LP solver uses sparse
            // storage format for internal representation of constraints.
            //
            alglib.minlpsetcost(state, c);
            alglib.minlpsetbc(state, bndl, bndu);
            alglib.minlpsetlc2dense(state, a, al, au, 2);

            //
            // Set scale of the parameters.
            //
            // It is strongly recommended that you set scale of your variables.
            // Knowing their scales is essential for evaluation of stopping criteria
            // and for preconditioning of the algorithm steps.
            // You can find more information on scaling at http://www.alglib.net/optimization/scaling.php
            //
            alglib.minlpsetscale(state, s);

            // Solve
            alglib.minlpoptimize(state);
            alglib.minlpresults(state, out var x, out rep);
            Console.WriteLine("{0}", alglib.ap.format(x,3)); // EXPECTED: [0,1]
        }
        catch(alglib.alglibexception alglibException)
        {
            Console.WriteLine("ALGLIB exception with message '{0}'", alglibException.msg);
        }
    }
    }
}