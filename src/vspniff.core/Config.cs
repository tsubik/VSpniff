using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VSpniff.Core
{
    public struct Config
    {
        public ConfigFileMode Mode { get; set; }
        public string ExcludedExtensions { get; set; }
        public string ExcludedDirs { get; set; }

        public void Load(string fileName)
        {
            foreach (string line in File.ReadLines(fileName))
            {
                var workline = line.Trim().ToLower();

                if (workline.StartsWith("mode:"))
                {
                    string mode = workline.Replace("mode:", "");
                    if(mode == "override")
                    {
                        Mode = ConfigFileMode.Override;
                    }
                    else if(mode == "append")
                    {
                        Mode = ConfigFileMode.Append;
                    }
                }
                else if (workline.StartsWith("excludedextensions:"))
                {
                    ExcludedExtensions = workline.Replace("excludedextensions:", "");
                }
                else if (line.ToLower().StartsWith("excludeddirs:"))
                {
                    ExcludedDirs = workline.Replace("excludeddirs:", "");
                }
            }
        }

        public static Config Default = new Config
        {
            Mode = ConfigFileMode.Override, //doesn't really matter which mode you choose in default config
            ExcludedExtensions = "user, csproj, aps, pch, vspscc, vssscc, ncb, suo, tlb, tlh, bak, log, lib, scc",
            ExcludedDirs = "bin, obj"
        };
    }

    public enum ConfigFileMode
    {
        Override,
        Append
    }
}
