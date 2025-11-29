using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Project_Management.setup;
using Newtonsoft.Json;
using Project_Management.Models;

namespace Project_Management.setup
{
    public class setEditor
    { 
          public static string getTextString(string jsonString)
        {
            var paragraphs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project_Management.setup.Paragraph>>(jsonString);
            string textString = String.Join("\n", paragraphs.Select(p => String.Join("\n", p.Children.Select(c => c.Text))));
            return textString;
        }
        public static string getJsonString(string textString)
        {
            //Convert the content text to Json
            var contentJsonText = textString.Split('\n').Select(text => new Paragraph
            {
                Type = "paragraph",
                Children = new List<Child>
                    {
                        new Child { Text = text }
                    }
            }).ToList();
            //serialize the JsonText
            string jsonString = JsonConvert.SerializeObject(contentJsonText);

            return jsonString;
        }
    }
}