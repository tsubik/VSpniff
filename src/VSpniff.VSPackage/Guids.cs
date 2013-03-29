// Guids.cs
// MUST match guids.h
using System;

namespace TomaszSubik.VSpniff_VSPackage
{
    static class GuidList
    {
        public const string guidVSpniff_VSPackagePkgString = "8e79f6eb-ac7a-467f-b754-384ab70778b9";
        public const string guidVSpniff_VSPackageCmdSetString = "518d4dcd-a613-43dd-b3aa-612f4cb7ce3b";

        public static readonly Guid guidVSpniff_VSPackageCmdSet = new Guid(guidVSpniff_VSPackageCmdSetString);
    };
}