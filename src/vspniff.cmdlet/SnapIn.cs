using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace VSpniff.Cmdlet
{
    [RunInstaller(true)]
    public class SnapIn : PSSnapIn
    {
        public override string Description
        {
            get { return "Visual Studio project's not included files finder"; }
        }

        public override string Name
        {
            get { return "VSpniff"; }
        }

        public override string Vendor
        {
            get { return "Tomasz Subik"; }
        }
    }
}
