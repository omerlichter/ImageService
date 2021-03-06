﻿using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        #region Members
        private IImageServiceModal m_modal;
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="modal">modal</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// execute the command.
        /// </summary>
        /// <param name="args">args of the command</param>
        /// <param name="result">return the result</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
            return m_modal.AddFile(args[0], out result);
        }
    }
}
