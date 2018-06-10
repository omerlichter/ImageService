using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Photo
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public int Month { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailPath")]
        public string ThumbnailPath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "RealPath")]
        public string RealPath { get; set; }

        public Photo(string name, string path, string ThumbnailPath, int year, int month, string realPath)
        {
            this.Name = name;
            this.Path = path;
            this.ThumbnailPath = ThumbnailPath;
            this.Year = year;
            this.Month = month;
            this.RealPath = realPath;
        }
    }
}