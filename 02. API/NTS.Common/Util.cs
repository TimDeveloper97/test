using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NTS.Common
{
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string ConvertTitleToTag(string title)
        {
            string strReturn = "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            strReturn = Regex.Replace(title, "[^\\w\\s]", string.Empty).Replace(" ", "-").ToLower();
            string strFormD = strReturn.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, string.Empty).Replace("đ", "d");
        }

        /// <summary>
        /// Xóa ký tự đặc biệt trong mã
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacter(string code)
        {

            code = code.Replace("*", "");
            code = code.Replace("{", "");
            code = code.Replace("}", "");
            code = code.Replace("!", "");
            code = code.Replace("^", "");
            code = code.Replace("<", "");
            code = code.Replace(">", "");
            code = code.Replace("?", "");
            code = code.Replace("|", "");
            code = code.Replace("_", "");
            code = code.Replace(" ", "");
            return code;
        }

        public static bool CheckSpecialCharacter(string code)
        {
            bool isHave = false;
            if (code.Contains("*") || code.Contains("{") || code.Contains("}") || code.Contains("!") || code.Contains("^") || code.Contains("<")
                || code.Contains(">") || code.Contains("?") || code.Contains("|") || code.Contains(",") || code.Contains("_") || code.Contains(" "))
            {
                isHave = true;
            }

            return isHave;
        }

        /// <summary>
        /// Xóa unicode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string RemoveUnicodeCharacter(string code)
        {
            var normalizedString = code.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetIndexParent(string indexChild)
        {
            if (indexChild.LastIndexOf(".") != -1)
            {
                return indexChild.Substring(0, indexChild.LastIndexOf("."));
            }

            return string.Empty;
        }
    }
}
