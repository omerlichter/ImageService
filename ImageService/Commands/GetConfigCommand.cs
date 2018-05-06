using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using ImageService.Infrastructure.Communication;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                string outputDir = ConfigurationManager.AppSettings.Get("OutputDir").ToString();
                string sourceName = ConfigurationManager.AppSettings.Get("SourceName").ToString();
                string logName = ConfigurationManager.AppSettings.Get("LogName").ToString();
                int thumbnailSize = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
                List<string> handlers = new List<string>(ConfigurationManager.AppSettings.Get("Handler").Split(';'));

                ConfigData cd = new ConfigData(outputDir, sourceName, logName, thumbnailSize, handlers);
                string data = JsonConvert.SerializeObject(cd);
                result = true;
                return data;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
