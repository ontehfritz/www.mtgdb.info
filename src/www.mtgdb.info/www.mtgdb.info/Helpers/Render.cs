using System;
using System.Text;
using System.Collections.Generic;
using System.Web;

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
            string html =  HttpUtility.HtmlEncode(text);
			html = html.Replace("{Tap}", "<i class='symbol symbol_T'></i>");
			html = html.Replace("{White}", "<i class='symbol symbol_W'></i>");
			html = html.Replace("{Blue}", "<i class='symbol symbol_U'></i>");
			html = html.Replace("{Black}", "<i class='symbol symbol_B'></i>");
			html = html.Replace("{Red}", "<i class='symbol symbol_R'></i>");
			html = html.Replace("{Green}", "<i class='symbol symbol_G'></i>");
			html = html.Replace("{Red or White}", "<i class='symbol symbol_RW'></i>");
			html = html.Replace("{Black or Green}", "<i class='symbol symbol_BG'></i>");
			html = html.Replace("{Blue or Black}", "<i class='symbol symbol_UB'></i>");
			html = html.Replace("{Green or White}", "<i class='symbol symbol_GW'></i>");


			html = html.Replace("{1}", "<i class='symbol symbol_1'></i>");
			html = html.Replace("{2}", "<i class='symbol symbol_2'></i>");
			html = html.Replace("{3}", "<i class='symbol symbol_3'></i>");
			html = html.Replace("{4}", "<i class='symbol symbol_4'></i>");
			html = html.Replace("{5}", "<i class='symbol symbol_5'></i>");
			html = html.Replace("{6}", "<i class='symbol symbol_6'></i>");
			html = html.Replace("{7}", "<i class='symbol symbol_7'></i>");
			html = html.Replace("{8}", "<i class='symbol symbol_8'></i>");
			html = html.Replace("{9}", "<i class='symbol symbol_9'></i>");
			html = html.Replace("{Variable Colorless}", "<i class='symbol symbol_X'></i>");

            return html;
        }
        /// <summary>
        /// Color the specified text.
        /// </summary>
        /// <param name="text">Text.</param>
        public static string Color(string text)
        {
            string html = text.ToLower();
			html = html.Replace ("none", "<span class='label label-color color-n'>Colorless</span>");
			html = html.Replace ("white", "<span class='label label-color color-w'>White</span>");
			html = html.Replace ("blue", "<span class='label label-color color-u'>Blue</span>");
			html = html.Replace ("black", "<span class='label label-color color-b'>Black</span>");
			html = html.Replace ("red", "<span class='label label-color color-r'>Red</span>");
			html = html.Replace ("green", "<span class='label label-color color-g'>Green</span>");
            
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

            text = HttpUtility.HtmlEncode(text.ToLower ());

            StringBuilder html = new StringBuilder ();

            Dictionary<string, string> syntax = new Dictionary<string, string> ();
			syntax.Add ("#", "<i class='symbol symbol_{0}'></i>");
			syntax.Add ("0", "<i class='symbol symbol_0'></i>");
			syntax.Add ("x", "<i class='symbol symbol_X'></i>");
			syntax.Add ("w", "<i class='symbol symbol_W'></i>");
			syntax.Add ("u", "<i class='symbol symbol_U'></i>");
			syntax.Add ("b", "<i class='symbol symbol_B'></i>");
			syntax.Add ("r", "<i class='symbol symbol_R'></i>");
			syntax.Add ("g", "<i class='symbol symbol_G'></i>");
			syntax.Add ("{r/w}", "<i class='symbol symbol_RW'></i>");
			syntax.Add ("{g/r}", "<i class='symbol symbol_GR'></i>");
			syntax.Add ("{b/r}", "<i class='symbol symbol_BR'></i>");
			syntax.Add ("{u/w}", "<i class='symbol symbol_UW'></i>");
			syntax.Add ("{u/r}", "<i class='symbol symbol_UR'></i>");
			syntax.Add ("{b/g}", "<i class='symbol symbol_BG'></i>");
			syntax.Add ("{g/w}", "<i class='symbol symbol_GW'></i>");
			syntax.Add ("{wp}", "<i class='symbol symbol_WP'></i>");
			syntax.Add ("{up}", "<i class='symbol symbol_UP'></i>");
			syntax.Add ("{bp}", "<i class='symbol symbol_BP'></i>");
			syntax.Add ("{rp}", "<i class='symbol symbol_RP'></i>");
			syntax.Add ("{gp}", "<i class='symbol symbol_GP'></i>");
			syntax.Add("{b/u}", "<i class='symbol symbol_BU'></i>");

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