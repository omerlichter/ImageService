using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private readonly string[] filters = {".jpg", ".png", ".gif", ".bmp"};      // the filters
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

		// Implement Here!
        public DirectoyHandler(IImageController controller, ILoggingService Logging)
        {
            this.m_controller = controller;
            this.m_logging = Logging;
        }

        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(DirectoryChanged);
            this.m_dirWatcher.Created += new FileSystemEventHandler(DirectoryChanged);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool resultSuccesful;
            if (e.RequestDirPath.Equals(this.m_path))
            {
                m_controller.ExecuteCommand(e.CommandID, e.Args, out resultSuccesful);
                if (resultSuccesful == false)
                {
                    m_logging.Log("error on execute command", MessageTypeEnum.FAIL);
                }
                m_logging.Log("the command execute succesful", MessageTypeEnum.INFO);
            }
        }

        private void DirectoryChanged(object source, FileSystemEventArgs e)
        {
            string[] args = { e.FullPath };
            string fileType = Path.GetExtension(e.FullPath).ToLower();
            if (filters.Contains(fileType))
            {
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, this.m_path);
                this.OnCommandRecieved(this, eventArgs);
            }
        }
    }
}
