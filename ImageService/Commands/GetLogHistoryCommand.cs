using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ImageService.Commands
{
    class GetLogHistoryCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            result = true;
            return "";
        }
    }
}
