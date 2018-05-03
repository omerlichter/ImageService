using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// execute the command.
        /// </summary>
        /// <param name="commandID">command id</param>
        /// <param name="args">args of the command</param>
        /// <param name="resultSuccesful">return if the execute succeed</param>
        /// <returns>return the comand return if succeed, return exception else</returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}
