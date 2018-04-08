using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="outputFolder">output folder of the images</param>
        /// <param name="thumbnailSize">thumbnail size</param>
        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// copy the file in the directory, create new directory in output directory of year and month.
        /// </summary>
        /// <param name="path">path of the image</param>
        /// <param name="result">return if the command succed</param>
        /// <returns>if succed return the path of the image, else return the exception</returns>
        public string AddFile(string path, out bool result)
        {
           if (File.Exists(path))
           {
                try
                {
                    // get the creation time of the image
                    DateTime date = this.GetDateTakenFromImage(path);
                    string year = date.Year.ToString();
                    string month = date.Month.ToString();

                    // create the output folder if is not already exist
                    Directory.CreateDirectory(m_OutputFolder);

                    // create the thumbnails folder
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
                    // craete the year and month folders
                    string targetPath = "\\" + year + "\\" + month;
                    Directory.CreateDirectory(m_OutputFolder + targetPath);
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + targetPath);

                    // copy the file if not exist already
                    if (!File.Exists(m_OutputFolder + targetPath + "\\" + Path.GetFileName(path)))
                    {
                        File.Move(path, m_OutputFolder + targetPath + "\\" + Path.GetFileName(path));
                    }
                    else
                    {
                        result = false;
                        return "file name already exist";
                    }

                    // create thumbnail if not exist already
                    if (!File.Exists(m_OutputFolder + "\\" + "Thumbnails" + targetPath + "\\" + Path.GetFileName(path)))
                    {
                        using (Image image = Image.FromFile(path))
                        using (Image thumbnail = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero))
                        {
                            thumbnail.Save(m_OutputFolder + "\\" + "Thumbnails" + targetPath + "\\" + Path.GetFileName(path));
                        }
                    }

                    // return the path of the image
                    result = true;
                    return m_OutputFolder + targetPath;
                }
                catch(Exception e)
                {
                    // return the exception
                    result = false;
                    return e.Message;
                }
           } 
           else
           {
                result = false;
                return "file not exist";
           }
        }

        //retrieves the datetime WITHOUT loading the whole image
        private DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    }
}
