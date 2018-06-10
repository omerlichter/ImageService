using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ModelPhotos
    {
        public event EventHandler Update;

        private static ModelConfig modelConfig;
        private static string[] extends = { ".jpg", ".bmp", ".png", ".gif" };
        private string output_dir;

        public string LastPhoto { get; set; }
        public List<Photo> Photos { get; set; }

        public ModelPhotos()
        {
            this.Photos = new List<Photo>();
            modelConfig = new ModelConfig();
            modelConfig.Update += UpdateHandler;
        }

        public void CheckUpdate()
        {
            this.output_dir = modelConfig.OutputDir;
            this.Photos = new List<Photo>();
            GetImages();
        }

        private void UpdateHandler(Object sender, EventArgs args)
        {
            this.output_dir = modelConfig.OutputDir;
            this.Photos = new List<Photo>();
            this.LastPhoto = "";
            GetImages();
            Update?.Invoke(this, null);
        }

        private void GetImages()
        {
            if (this.output_dir == "")
            {
                return;
            }
            string ThumbnailsDir = output_dir + "\\Thumbnails";
            DirectoryInfo ThumbnailstDirInfo = new DirectoryInfo(ThumbnailsDir);
            foreach (DirectoryInfo yearDir in ThumbnailstDirInfo.GetDirectories())
            {
                foreach(DirectoryInfo monthDir in yearDir.GetDirectories())
                {
                    foreach(FileInfo thumbnail in monthDir.GetFiles())
                    {
                        if (extends.Contains(thumbnail.Extension.ToLower()))
                        {
                            try
                            {
                                string relPath = @"~\" + Path.GetFileName(output_dir) + thumbnail.FullName.Replace(output_dir, string.Empty);
                                string name = thumbnail.Name;
                                string thumbnailPath = relPath;
                                string path = relPath.Replace("Thumbnails\\", string.Empty);
                                int year = int.Parse(yearDir.Name);
                                int month = int.Parse(monthDir.Name);
                                Photo photo = new Photo(name, path, thumbnailPath, year, month, thumbnail.FullName);
                                this.Photos.Add(photo);
                            } catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public Photo GetPhotoFromPath(string path)
        {
            Photo thePhoto = null;
            foreach (Photo photo in this.Photos)
            {
                if (photo.ThumbnailPath == path)
                {
                    thePhoto = photo;
                }
            }
            return thePhoto;
        }

        public void DeletePhoto()
        {
            try
            {
                Photo thePhotoToDelete = null;
                foreach(Photo photo in this.Photos)
                {
                    if (photo.ThumbnailPath == this.LastPhoto)
                    {
                        thePhotoToDelete = photo;
                    }
                }
                string path = thePhotoToDelete.RealPath.Replace("Thumbnails\\", string.Empty);
                File.Delete(thePhotoToDelete.RealPath);
                File.Delete(path);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}