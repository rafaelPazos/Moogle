using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Document
    {
        //static List<Document> documents;
        //float score;
        string snippet;
        public static string[] pureContent;
        public Document()
        {
            //documents = new List<Document>();
        }
        /*private void LoadFilesContent()
        {
            string[] fileNames = Directory.GetFiles("../Content");

            for (int i = 0; i < fileNames.Length; i++)
            {
                if (fileNames[i] == "../Content/.gitignore")
                    continue;

                StreamReader reader = new StreamReader(fileNames[i]);
                string fileContent = reader.ReadToEnd();
                Document aux = new Document();
                aux.Content = fileContent;
                documents.Add(aux);
            }
        }
        private void SeparateContent()
        {

        }*/
        public string Content { get; set; }
        public string[] PureContent { get; set; }
        public float Score { get; set; }
    }
}
