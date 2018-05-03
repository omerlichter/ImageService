using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        #region Properties
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="type">type of the message</param>
        /// <param name="message">message</param>
        public MessageRecievedEventArgs(MessageTypeEnum type, string message)
        {
            Status = type;
            Message = message;
        }
    }
}
