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
        public string[] ExcludedExtensions { get; set; }
        public string[] ExcludedDirs { get; set; }

        public static Config Load(string fileName)
        {
			Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(fileName));
			return config;
		}

		public void Merge(Config config)
		{
			if (config.Mode == ConfigFileMode.Override)
			{
				ExcludedExtensions = config.ExcludedExtensions;
				ExcludedDirs = config.ExcludedDirs;
			}
			else if (config.Mode == ConfigFileMode.Append)
			{
				ExcludedDirs = ExcludedDirs.Union(config.ExcludedDirs).ToArray();
				ExcludedExtensions = ExcludedExtensions.Union(config.ExcludedExtensions).ToArray();
			}
		}

        public static Config Default = new Config
        {
			Mode = ConfigFileMode.Override, //doesn't really matter which mode you choose in default config
            ExcludedExtensions = "user,csproj,aps,pch,vspscc,vssscc,ncb,suo,tlb,tlh,bak,log,lib,scc".Split(','),
            ExcludedDirs = "bin,obj".Split(',')
        };
    }

    public enum ConfigFileMode
    {
        Override,
        Append
    }
}
