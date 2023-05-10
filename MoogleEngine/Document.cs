using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Document
    {
        public Document()
        {
            Snippet = "";
            FileName = "";
            Score = 0;
        }
        public string Content { get; set; }
        public string[] PureContent { get; set; }
        public float Score { get; set; }
        public string Snippet { get; set; }
        public string FileName { get; set; }
    }
}
