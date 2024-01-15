using System;
using System.IO;
using LpSolveDotNet;
using Microsoft.Extensions.Logging;

namespace Italbytz.Infrastructure.LinearProgramming {

    public class LpSolver
    {
        public LpSolver(ILoggerFactory loggerFactory) =>
            LoggingExtensions.InitLoggers(loggerFactory);

        private void Logfunc(IntPtr lp, IntPtr userhandle, string buf)
        {
            this.Log(LogLevel.Information, buf);
        }

        public void SolveFromLpSolveNativeFormat(string lpformat)
        {
            LpSolve.Init();
            var lpTempFile = Path.GetTempFileName();
            using var outputFile = new StreamWriter(lpTempFile);
            outputFile.Write(lpformat);
            outputFile.Close();

            var lp = LpSolve.read_LP(lpTempFile, 0, null);
            lp.put_logfunc(Logfunc, IntPtr.Zero);
            var statuscode = lp.solve();

            var objTempFile = Path.GetTempFileName();
            lp.set_outputfile(objTempFile);
            lp.print_objective();
            lp.set_outputfile(null);

            using (var inputFile = new StreamReader(objTempFile))
            {
                var line = inputFile.ReadToEnd();
                this.Log(LogLevel.Information, line);
            }

            var solTempFile = Path.GetTempFileName();
            lp.set_outputfile(solTempFile);
            lp.print_solution(1);
            lp.set_outputfile(null);

            using (var inputFile = new StreamReader(solTempFile))
            {
                var line = inputFile.ReadToEnd();
                this.Log(LogLevel.Information, line);
            }
        }
    }
}