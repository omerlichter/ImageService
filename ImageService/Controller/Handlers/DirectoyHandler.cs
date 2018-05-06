using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region Properties
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="Logging">logger</param>
        public DirectoyHandler(IImageController controller, ILoggingService Logging)
        {
            this.m_controller = controller;
            this.m_logging = Logging;
        }

        /// <summary>
        /// start handle and monitoring the directory.
        /// </summary>
        /// <param name="dirPath">path to the directory</param>
        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            try
            {
                // add dirWatcher
                this.m_dirWatcher = new FileSystemWatcher(this.m_path);
                // add events
                this.m_dirWatcher.Created += new FileSystemEventHandler(DirectoryChanged);
                this.m_dirWatcher.EnableRaisingEvents = true;
            }
            catch (Exception e)
            {
                m_logging.Log(e.Message, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// on command recieved.
        /// </summary>
        /// <param name="sender">the object that send the event</param>
        /// <param name="e">event args</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool resultSuccesful;

            if (e.RequestDirPath == this.m_path || e.RequestDirPath == "*")
            {
                // if the command is close
                if (e.CommandID == (int)CommandEnum.CloseCommand)
                {

                    m_logging.Log("close command execute in handler", MessageTypeEnum.INFO);
                    EndHandler();
                    return;
                }
                else
                {
                    string msg = m_controller.ExecuteCommand(e.CommandID, e.Args, out resultSuccesful);
                    if (resultSuccesful == false)
                    {
                        // fail
                        m_logging.Log("error on execute command with ID: " + Enum.GetName(typeof(CommandEnum), e.CommandID) + ", message: " + msg, MessageTypeEnum.FAIL);
                    }
                    else
                    {
                        // succeed
                        m_logging.Log("the command with ID: " + Enum.GetName(typeof(CommandEnum), e.CommandID) + " execute successfully", MessageTypeEnum.INFO);
                    }
                }
            }
        }

        /// <summary>
        /// called when new file created in the directory, or file changed
        /// </summary>
        /// <param name="source">the object that send the event</param>
        /// <param name="e">event args</param>
        private void DirectoryChanged(object source, FileSystemEventArgs e)
        {
            // args for the controller
            string[] args = { e.FullPath };
            // get the file type
            string fileType = Path.GetExtension(e.FullPath).ToLower();
            // check if the file type is {.bmp, .jpg, .png, .gif}
            if (filters.Contains(fileType))
            {
                // send command
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, this.m_path);
                this.OnCommandRecieved(this, eventArgs);
            }
        }

        /// <summary>
        /// close the handler, stop monitoring the directory and send event of closing.
        /// </summary>
        private void EndHandler()
        {
            try
            {
                this.m_dirWatcher.EnableRaisingEvents = false;
                this.m_dirWatcher.Created -= new FileSystemEventHandler(DirectoryChanged);
                this.m_dirWatcher.Changed -= new FileSystemEventHandler(DirectoryChanged);
            } catch (Exception e)
            {
                m_logging.Log(e.Message, MessageTypeEnum.FAIL);
            }

            DirectoryCloseEventArgs closeArgs = new DirectoryCloseEventArgs(this.m_path, "directory " + this.m_path + "closed");

            DirectoryClose?.Invoke(this, closeArgs);
        }
    }
}
