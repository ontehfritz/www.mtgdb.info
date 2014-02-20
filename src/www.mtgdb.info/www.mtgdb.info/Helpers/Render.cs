using System;
using System.Text;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class RenderHtml
    {
        /// <summary>
        /// Text the specified text.
        /// </summary>
        /// <param name="text">Text.</param>
        public static string Text(string text)
        {
            string html = text;
            html = html.Replace("{Tap}", "<img src='/content/images/mana/tap.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{White}", "<img src='/content/images/mana/w.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Green}", "<img src='/content/images/mana/g.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Blue}", "<img src='/content/images/mana/u.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Black}", "<img src='/content/images/mana/b.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Red}", "<img src='/content/images/mana/r.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Red or White}", "<img src='/content/images/mana/rw.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Black or Green}", "<img src='/content/images/mana/bg.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Blue or Black}", "<img src='/content/images/mana/ub.png' style='width:20px;height:20px;'/>");
            html = html.Replace("{Green or White}", "<img src='/content/images/mana/gw.png' style='width:20px;height:20px;'/>");


            html = html.Replace("{1}", "<span class='badge'>1</span>");
            html = html.Replace("{2}", "<span class='badge'>2</span>");
            html = html.Replace("{3}", "<span class='badge'>3</span>");
            html = html.Replace("{4}", "<span class='badge'>4</span>");
            html = html.Replace("{5}", "<span class='badge'>5</span>");
            html = html.Replace("{6}", "<span class='badge'>6</span>");
            html = html.Replace("{7}", "<span class='badge'>7</span>");
            html = html.Replace("{8}", "<span class='badge'>8</span>");
            html = html.Replace("{9}", "<span class='badge'>9</span>");
            html = html.Replace("{Variable Colorless}", "<span class='badge'>X</span>");

            return html;
        }
        /// <summary>
        /// Color the specified text.
        /// </summary>
        /// <param name="text">Text.</param>
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
        /// <summary>
        /// Mana the specified text.
        /// </summary>
        /// <param name="text">Text.</param>
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
            syntax.Add("{b/u}", "<img src='/content/images/mana/ub.png' class='symbol' />");

            string key = "";
            string number = "";
            Stack<string> stack = new Stack<string> ();
            foreach (char c in text) 
            {
                if (stack.Count == 0) 
                {
                    if (Char.IsNumber (c)) 
                    {
                        //done: 89096 double digit mana cost
                        number += c.ToString ();
                        continue;
                    }
                    else if(number != "")
                    {
                        html.Append (string.Format (syntax ["#"], number));
                        number = "";
                    }

                    if(c == 'X')
                    {
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
    }
}