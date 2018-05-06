using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Communication
{
    public class ConfigData
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }
        public List<string> Handlers { get; set; }

        public ConfigData(string outputDir, string sourceName, string logName, int thumbnailSize, List<string> handlers)
        {
            this.OutputDir = outputDir;
            this.SourceName = sourceName;
            this.LogName = logName;
            this.ThumbnailSize = thumbnailSize;
            this.Handlers = handlers;
        }
    }
}
