using System;
using System.Text;
using System.Collections.Generic;
using System.Web;

namespace MtgDb.Info
{
    public class RenderHtml
    {
		public static string Name(string text)
		{
			string html =  HttpUtility.HtmlEncode(text);

			html = html.Replace ("(R)","&#174;");

			return html;
		}
        /// <summary>
        /// Text the specified text.
        /// </summary>
        /// <param name="text">Text.</param>
        public static string Text(string text)
        {
            string html =  HttpUtility.HtmlEncode(text);
			html = html.Replace("{Tap}", "<i class='symbol symbol_T'></i>");
			html = html.Replace("ocT", "<i class='symbol symbol_T'></i>");
			html = html.Replace("{Untap}", "<i class='symbol symbol_UT'></i>");
			html = html.Replace("{White}", "<i class='symbol symbol_W'></i>");
			html = html.Replace("{Blue}", "<i class='symbol symbol_U'></i>");
			html = html.Replace("{Black}", "<i class='symbol symbol_B'></i>");
			html = html.Replace("{Red}", "<i class='symbol symbol_R'></i>");
			html = html.Replace("{Green}", "<i class='symbol symbol_G'></i>");
			html = html.Replace("{W}", "<i class='symbol symbol_W'></i>");
			html = html.Replace("{U}", "<i class='symbol symbol_U'></i>");
			html = html.Replace("{B}", "<i class='symbol symbol_B'></i>");
			html = html.Replace("{R}", "<i class='symbol symbol_R'></i>");
			html = html.Replace("{G}", "<i class='symbol symbol_G'></i>");
			html = html.Replace("oW", "<i class='symbol symbol_W'></i>");
			html = html.Replace("oU", "<i class='symbol symbol_U'></i>");
			html = html.Replace("oB", "<i class='symbol symbol_B'></i>");
			html = html.Replace("oR", "<i class='symbol symbol_R'></i>");
			html = html.Replace("oG", "<i class='symbol symbol_G'></i>");
			html = html.Replace("{Red or White}", "<i class='symbol symbol_RW'></i>");
			html = html.Replace("{Black or Green}", "<i class='symbol symbol_BG'></i>");
			html = html.Replace("{Blue or Black}", "<i class='symbol symbol_UB'></i>");
			html = html.Replace("{Green or White}", "<i class='symbol symbol_GW'></i>");
			html = html.Replace("{Phyrexian White}", "<i class='symbol symbol_WP'></i>");
			html = html.Replace("{Phyrexian Blue}", "<i class='symbol symbol_UP'></i>");
			html = html.Replace("{Phyrexian Black}", "<i class='symbol symbol_BP'></i>");
			html = html.Replace("{Phyrexian Red}", "<i class='symbol symbol_RP'></i>");
			html = html.Replace("{Phyrexian Green}", "<i class='symbol symbol_GP'></i>");
			html = html.Replace("{Two or White}", "<i class='symbol symbol_2W'></i>");
			html = html.Replace("{Two or Blue}", "<i class='symbol symbol_2U'></i>");
			html = html.Replace("{Two or Black}", "<i class='symbol symbol_2B'></i>");
			html = html.Replace("{Two or Red}", "<i class='symbol symbol_2R'></i>");
			html = html.Replace("{Two or Green}", "<i class='symbol symbol_2G'></i>");
			html = html.Replace("{Snow}", "<i class='symbol symbol_S'></i>");

			html = html.Replace("{1}", "<i class='symbol symbol_1'></i>");
			html = html.Replace("{2}", "<i class='symbol symbol_2'></i>");
			html = html.Replace("{3}", "<i class='symbol symbol_3'></i>");
			html = html.Replace("{4}", "<i class='symbol symbol_4'></i>");
			html = html.Replace("{5}", "<i class='symbol symbol_5'></i>");
			html = html.Replace("{6}", "<i class='symbol symbol_6'></i>");
			html = html.Replace("{7}", "<i class='symbol symbol_7'></i>");
			html = html.Replace("{8}", "<i class='symbol symbol_8'></i>");
			html = html.Replace("{9}", "<i class='symbol symbol_9'></i>");
			html = html.Replace("{10}", "<i class='symbol symbol_10'></i>");
			html = html.Replace("{11}", "<i class='symbol symbol_11'></i>");
			html = html.Replace("{12}", "<i class='symbol symbol_12'></i>");
			html = html.Replace("{13}", "<i class='symbol symbol_13'></i>");
			html = html.Replace("{14}", "<i class='symbol symbol_14'></i>");
			html = html.Replace("{15}", "<i class='symbol symbol_15'></i>");
			html = html.Replace("{16}", "<i class='symbol symbol_16'></i>");
			html = html.Replace("{17}", "<i class='symbol symbol_17'></i>");
			html = html.Replace("{18}", "<i class='symbol symbol_18'></i>");
			html = html.Replace("{19}", "<i class='symbol symbol_19'></i>");
			html = html.Replace("{20}", "<i class='symbol symbol_20'></i>");
			html = html.Replace("{100}", "<i class='symbol symbol_100'></i>");
			html = html.Replace("{Infinite}", "<i class='symbol symbol_Infinite'></i>");
			html = html.Replace("{1/2}", "<i class='symbol symbol_Half'></i>");
			html = html.Replace("o1", "<i class='symbol symbol_1'></i>");
			html = html.Replace("o2", "<i class='symbol symbol_2'></i>");
			html = html.Replace("o3", "<i class='symbol symbol_3'></i>");
			html = html.Replace("o4", "<i class='symbol symbol_4'></i>");
			html = html.Replace("o5", "<i class='symbol symbol_5'></i>");
			html = html.Replace("{Variable Colorless}", "<i class='symbol symbol_X'></i>");
			html = html.Replace("oX", "<i class='symbol symbol_X'></i>");
			html = html.Replace("{Half a Red}", "<i class='symbol symbol_HR'></i>");

			html = html.Replace ("\n","<br/>");
			html = html.Replace ("(R)","&#174;");
			html = html.Replace ("1/2","&#189;");
			html = html.Replace ("(","<em>(");
			html = html.Replace (")",")</em>");

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
			syntax.Add ("x", "<i class='symbol symbol_X'></i>");
			syntax.Add ("y", "<i class='symbol symbol_Y'></i>");
			syntax.Add ("z", "<i class='symbol symbol_Z'></i>");
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
			syntax.Add("{b/u}", "<i class='symbol symbol_BU'></i>");
			syntax.Add ("{wp}", "<i class='symbol symbol_WP'></i>");
			syntax.Add ("{up}", "<i class='symbol symbol_UP'></i>");
			syntax.Add ("{bp}", "<i class='symbol symbol_BP'></i>");
			syntax.Add ("{rp}", "<i class='symbol symbol_RP'></i>");
			syntax.Add ("{gp}", "<i class='symbol symbol_GP'></i>");
			syntax.Add ("{2/w}", "<i class='symbol symbol_2W'></i>");
			syntax.Add ("{2/u}", "<i class='symbol symbol_2U'></i>");
			syntax.Add ("{2/b}", "<i class='symbol symbol_2B'></i>");
			syntax.Add ("{2/r}", "<i class='symbol symbol_2R'></i>");
			syntax.Add ("{2/g}", "<i class='symbol symbol_2G'></i>");

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

			if (html.Length < 1) {
				html.Append (string.Format("<i class='symbol symbol_{0}'></i>",text));
			}	 

            html.Append (key);
            return html.ToString();
        }
    }
}