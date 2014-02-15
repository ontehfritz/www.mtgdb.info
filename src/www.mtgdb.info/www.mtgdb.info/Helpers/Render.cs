using System;
using System.Text;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class RenderHtml
    {
       
        public static string Color(string text)
        {
            string html = text.ToLower();
            html = html.Replace ("white", "<img src='/content/images/mana/w.png' style='width:20px;height:20px;'/>");
            html = html.Replace ("blue", "<img src='/content/images/mana/u.png' style='width:20px;height:20px;'/>");
            html = html.Replace ("black", "<img src='/content/images/mana/b.png' style='width:20px;height:20px;'/>");
            html = html.Replace ("red", "<img src='/content/images/mana/r.png' style='width:20px;height:20px;'/>");
            html = html.Replace ("green", "<img src='/content/images/mana/g.png' style='width:20px;height:20px;'/>");
            
            return html;
        }

        public static string Mana(string text)
        {
            if (text == null || text == "")
                return text;

            text = text.ToLower ();

            StringBuilder html = new StringBuilder ();

            Dictionary<string, string> syntax = new Dictionary<string, string> ();
            syntax.Add ("#", "<span class='badge'>{0}</span>");
            syntax.Add ("w", "<img src='/content/images/mana/w.png' class='symbol' />");
            syntax.Add ("u", "<img src='/content/images/mana/u.png' class='symbol' />");
            syntax.Add ("b", "<img src='/content/images/mana/b.png' class='symbol' />");
            syntax.Add ("r", "<img src='/content/images/mana/r.png' class='symbol' />");
            syntax.Add ("g", "<img src='/content/images/mana/g.png' class='symbol' />");
            syntax.Add ("{r/w}", "<img src='/content/images/mana/rw.png' class='symbol' />");
            syntax.Add ("{g/r}", "<img src='/content/images/mana/rg.png' class='symbol' />");
            syntax.Add ("{b/r}", "<img src='/content/images/mana/br.png' class='symbol' />");
            syntax.Add ("{u/w}", "<img src='/content/images/mana/wu.png' class='symbol' />");
            syntax.Add ("{u/r}", "<img src='/content/images/mana/ur.png' class='symbol' />");
            syntax.Add ("{b/g}", "<img src='/content/images/mana/bg.png' class='symbol' />");
            syntax.Add ("{g/w}", "<img src='/content/images/mana/gw.png' class='symbol' />");
            syntax.Add ("{wp}", "<img src='/content/images/mana/pw.png' class='symbol' />");
            syntax.Add ("{up}", "<img src='/content/images/mana/pu.png' class='symbol' />");
            syntax.Add ("{bp}", "<img src='/content/images/mana/pb.png' class='symbol' />");
            syntax.Add ("{rp}", "<img src='/content/images/mana/pr.png' class='symbol' />");
            syntax.Add ("{gp}", "<img src='/content/images/mana/pg.png' class='symbol' />");

            string key = "";
            Stack<string> stack = new Stack<string> ();
            foreach (char c in text) 
            {
                if (stack.Count == 0) 
                {
                    if (Char.IsNumber (c) || c == 'x') 
                    {
                        //todo: 89096 double digit mana cost
                        html.Append (string.Format (syntax ["#"], c.ToString ().ToUpper()));
                        continue;
                    }

                    if(syntax.ContainsKey(c.ToString()))
                    {
                        html.Append (syntax [c.ToString()]);
                        continue;
                    }
                }

                if (c == '{') {
                    stack.Push ("{");
                    key = "{";
                } 
                else if (c == '}') 
                {
                    key += "}";
                    stack.Pop ();

                    if(syntax.ContainsKey(key))
                    {
                        html.Append (syntax[key]);
                        key = "";
                    }
                    else
                    {
                        html.Append (key);
                        key = "";
                    }
                }
                else if(stack.Count > 0)
                {
                    key += c.ToString ();
                }
                else
                {
                    html.Append (c.ToString());
                }
            }

            html.Append (key);
            return html.ToString();
        }

        private static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}