using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace VSpniff.VSPackage.Extensions
{
    public static class OutPutWindowPaneExtensions
    {
        public static void OutputLine(this OutputWindowPane owp, string message)
        {
            owp.OutputString(message);
            owp.OutputString(Environment.NewLine);
        }
    }
}
