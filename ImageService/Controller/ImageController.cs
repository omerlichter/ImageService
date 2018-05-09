using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        #region Members
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="modal">modal</param>
        public ImageController(IImageServiceModal modal, ILoggingHistory loggingHistory)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                // For Now will contain NEW_FILE_COMMAND
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_modal)},
                {(int)CommandEnum.GetConfigCommand, new GetConfigCommand()},
                {(int)CommandEnum.LogCommand, new GetLogHistoryCommand(loggingHistory) }
            };
        }

        /// <summary>
        /// execute the command.
        /// </summary>
        /// <param name="commandID">command id</param>
        /// <param name="args">args of the command</param>
        /// <param name="resultSuccesful">return if the execute succeed</param>
        /// <returns>return the comand return if succeed, return exception else</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.ContainsKey(commandID))
            {
                return commands[commandID].Execute(args, out resultSuccesful);
            }
            resultSuccesful = false;
            return "error: the command ID not exist";
        }

        /// <summary>
        /// set the close command
        /// </summary>
        /// <param name="imageServer"></param>
        public void SetCloseCommand(ImageServer imageServer)
        {
            commands.Add((int)CommandEnum.CloseCommand, new CloseCommand(imageServer));
        }
    }
}
