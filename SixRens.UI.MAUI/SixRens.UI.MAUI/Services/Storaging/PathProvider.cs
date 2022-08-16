﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Services.Paths
{
    public class PathProvider
    {
        private readonly string basePath;
        public PathProvider(IFileSystem fileSystem)
        {
            this.basePath = fileSystem.AppDataDirectory;
        }
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path, basePath);
        }
        public DirectoryInfo GetDirectory(string path, bool autoCreate = true)
        {
            var result = new DirectoryInfo(GetFullPath(path));
            if (autoCreate)
                result.Create();
            return result;
        }
        public FileInfo GetFile(string path)
        {
            return new(GetFullPath(path));
        }
        public DirectoryInfo PluginPackages => GetDirectory("plugins");
        public DirectoryInfo Presets => GetDirectory("presets");
    }
}
