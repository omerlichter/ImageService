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
                DateTime date;
                try
                {
                    // get the taken date of the image
                    date = this.GetDateTakenFromImage(path);   
                } catch
                {
                    try
                    {
                        date = File.GetCreationTime(path);
                    } catch (Exception e)
                    {
                        result = false;
                        return "error in getting taken or creation time: " + e.Message; 
                    }
                }

                try
                {
                    // create the output folder if is not already exist
                    Directory.CreateDirectory(m_OutputFolder);

                    string year = date.Year.ToString();
                    string month = date.Month.ToString();

                    // create the thumbnails folder
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
                    // craete the year and month folders
                    string targetPath = "\\" + year + "\\" + month;
                  
                    Directory.CreateDirectory(m_OutputFolder + targetPath);
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + targetPath);

                    string fullTargetPath = m_OutputFolder + "\\" + year + "\\" + month + "\\";
                    string fullTargetThumbnailsPath = m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\";

                    string fileNameWithoutExtention = Path.GetFileNameWithoutExtension(path);
                    string fileExtention = Path.GetExtension(path);
                    string newFileName = fileNameWithoutExtention + fileExtention;

                    int count = 1;

                    // copy the file if not exist already
                    while (File.Exists(fullTargetPath + newFileName))
                    {
                        string tmpFileName = string.Format("{0}({1})", fileNameWithoutExtention, count++);
                        newFileName = tmpFileName + fileExtention;
                    }

                    if (!File.Exists(fullTargetPath + newFileName))
                    {
                        File.Move(path, fullTargetPath + newFileName);
                    }

                    // create thumbnail if not exist already
                    if (!File.Exists(fullTargetThumbnailsPath + newFileName))
                    {
                        using (Image image = Image.FromFile(fullTargetPath + newFileName))
                        using (Image thumbnail = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero))
                        {
                            thumbnail.Save(fullTargetThumbnailsPath + newFileName);
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
