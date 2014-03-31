using System;
using System.Text;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class Element
    {
        public string Text; 
        public string Value; 
    }

    public class Html
    {
        public static string DropDown(string name, 
            List<Element> elements, string value = null)
        {
            string opt = "<option value=\"{0}\">{1}</option>";
            string selectedOpt = "<option value=\"{0}\" selected>{1}</option>";

            StringBuilder html = new StringBuilder();
            html.Append(string.Format("<select name=\"{0}\">", name));

            foreach(Element element in elements)
            {
                if(element.Value.ToLower() == value.ToLower())
                {
                    html.Append(string.Format(selectedOpt, element.Value, element.Text));
                }
                else
                {
                    html.Append(string.Format(opt, element.Value, element.Text));
                }
            }

            html.Append("</select>");

            return html.ToString();
        }
    }
}

