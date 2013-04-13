using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace VSpniff.Core
{
    public class Config
    {
        public ConfigFileMode Mode { get; set; }
        public string ExcludedExtensions { get; set; }
        public string ExcludedDirs { get; set; }

        public static Config Load(string fileName)
        {
			string json = File.ReadAllText(fileName);
			Config config = JsonConvert.DeserializeObject<Config>(json);
			//just to backward compability
			if (config == null)
			{
				config = ParseFileOldWay(fileName);
			}

			return config;
		}
		
		private static Config ParseFileOldWay(string fileName)
		{
			Config config = new Config();
			foreach (string line in File.ReadLines(fileName))
			{
				var workline = line.Trim().ToLower();

				if (workline.StartsWith("mode:"))
				{
					string mode = workline.Replace("mode:", "");
					if (mode == "override")
					{
						config.Mode = ConfigFileMode.Override;
					}
					else if (mode == "append")
					{
						config.Mode = ConfigFileMode.Append;
					}
				}
				else if (workline.StartsWith("excludedextensions:"))
				{
					config.ExcludedExtensions = workline.Replace("excludedextensions:", "");
				}
				else if (line.ToLower().StartsWith("excludeddirs:"))
				{
					config.ExcludedDirs = workline.Replace("excludeddirs:", "");
				}
			}
			return config;
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
