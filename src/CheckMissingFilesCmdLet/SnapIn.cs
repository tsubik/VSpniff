using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace CheckVSMissingFilesCmdLet
{
    [RunInstaller(true)]
    public class SnapIn : PSSnapIn
    {
        public override string Description
        {
            get { return "CheckmissingFiles"; }
        }

        public override string Name
        {
            get { return "CheckmissingFiles"; }
        }

        public override string Vendor
        {
            get { return "Tomasz Subik"; }
        }
    }
}
