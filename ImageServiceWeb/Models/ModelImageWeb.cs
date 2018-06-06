using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ImageServiceWeb.Client;
using System.IO;
using System.Web.Hosting;

namespace ImageServiceWeb.Models
{
    public class ModelImageWeb
    {
        public event EventHandler Update;

        private string outputDir;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ServerStatus")]
        public int ServerStatus { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "NumOfImages")]
        public int NumOfImages { get; set; }

        [Required]
        [Display(Name = "StudentsInfo")]
        public List<StudentInfo> Students { get; set; }

        public ModelImageWeb()
        {
            this.Students = new List<StudentInfo>();
            outputDir = "";
            ModelConfig modelConfig = ModelConfig.Instance;
            modelConfig.Update += UpdateHandler;
        }

        private void UpdateHandler(Object sender, EventArgs args)
        {
            this.outputDir = ModelConfig.Instance.OutputDir;
            this.NumOfImages = GetNumberOfImagesInFile(this.outputDir);
            this.Students = GetStudentsFromFile();
            Update?.Invoke(this, null);
        }

        private int GetNumberOfImagesInFile(string outputFile)
        {
            int counter = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(outputFile);
                foreach (DirectoryInfo directory in di.GetDirectories())
                { 
                    if (directory.Name.Equals("Thumbnails"))
                    {
                        continue;
                    }
                    counter += directory.GetFiles("*.PNG", SearchOption.AllDirectories).Length;
                    counter += directory.GetFiles("*.BMP", SearchOption.AllDirectories).Length;
                    counter += directory.GetFiles("*.GIF", SearchOption.AllDirectories).Length;
                    counter += directory.GetFiles("*.JPG", SearchOption.AllDirectories).Length;
                }
            } catch (Exception e)
            {

            }
            return counter;
        }

        private List<StudentInfo> GetStudentsFromFile()
        {
            List<StudentInfo> studentsList = new List<StudentInfo>();
            try
            {
                String file = HostingEnvironment.MapPath(@"/App_Data/Details.txt");
                FileStream fs = new FileStream(file, FileMode.Open);
                StreamReader sw = new StreamReader(fs);
                String s;
                while ((s = sw.ReadLine()) != null)
                {
                    string[] stringSplit = s.Split(';');
                    StudentInfo si = new StudentInfo(stringSplit[0], stringSplit[1], int.Parse(stringSplit[2]));
                    studentsList.Add(si);
                }
                fs.Close();
            } catch (Exception e)
            {

            }
            return studentsList;
        }
    }

    public class StudentInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }

        public StudentInfo(string firstName, string lastName, int id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.ID = id;
        }
    }
}