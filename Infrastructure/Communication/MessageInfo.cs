using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageService.Infrastructure.Communication
{
    public class MessageInfo
    {
        public string Args { get; set; }
        public CommandEnum ID { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        public MessageInfo(CommandEnum id, string args)
        {
            this.Args = args;
            this.ID = id;
        }
    }
}
