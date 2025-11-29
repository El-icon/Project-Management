using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Management.setup
{
    public class Paragraph
    {
        public string Type { get; set; }
        public List<Child> Children { get; set; }
    }
}