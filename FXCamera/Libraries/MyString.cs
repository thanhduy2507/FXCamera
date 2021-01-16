using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FXCameraDbConText.Libraries
{
    public static class MyString
    {
   

        public static string GenerateSlug(this string s)
        {
            string[][] symbols=
            {
                new string[]{"[áàảãạăắẳẵằặâấầẩẫậ]","a"},
                new string[]{"[đ]","d"},
                new string[]{"[éèẻẽẹêếềểễệ]","e"},
                new string[]{"[íìỉĩị]","i"},
                new string[]{"[úùủũụưứừửữự]","u"},
                new string[]{"[ơớờởỡợôốồổỗộ]","o"},
                new string[]{"[ýỳỷỹỵ]","y"},
                new string[]{"[\\s'\";,]","-"}
            };
            s = s.ToLower();
            foreach (var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
          
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        public static string HasPass( string pass)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(pass);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            return hasPass;
        }
    }
}