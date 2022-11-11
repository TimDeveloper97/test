using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace NTS.Common
{
    public static class DocUtil
    {
        public static WTable GetTableByFindText(this WordDocument document, string textFind)
        {
            var text = document.Find(textFind, false, true);
            WTextRange a = text.GetAsOneRange();
            Entity entity = a.Owner;
            while (!(entity is WTable))
            {
                if (entity.Owner != null)
                {
                    entity = entity.Owner;
                }
                else
                    break;
            }

            if (entity is WTable)
            {
                return entity as WTable;
            }
            else
            {
                return null;
            }
        }

        public static string NTSOverHtmlString(this string stringData)
        {

            //string cleaned = new Regex("style=\"[^\"]*\"").Replace(stringData, "");
            // cleaned = new Regex("(?<=class=\")([^\"]*)\\babc\\w*\\b([^\"]*)(?=\")").Replace(cleaned, "$1$2");
            //cleaned = cleaned.Replace("<span", "<span style=\"font-size: 14.0pt; font-family: 'Times New Roman';\"");
            if (!string.IsNullOrEmpty(stringData))
            {
                List<int> listIndex = new List<int>();
                for (int i = 0; i < stringData.Length; i++)
                {
                    if (i + 1 < stringData.Length && stringData[i].Equals('<') && stringData[i + 1].Equals('p'))
                    {
                        int indexSty = stringData.IndexOf("style=\"", i + 1);

                        if (indexSty != -1)
                        {
                            for (int j = indexSty + 7; j < stringData.Length; j++)
                            {
                                if (stringData[j].Equals('"'))
                                {
                                    listIndex.Add(j);
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int i = listIndex.Count - 1; i >= 0; i--)
                {
                    stringData = stringData.Insert(listIndex[i], "line-height: 130%;padding-top: 0;padding-bottom: 0; font-size: 14.0pt; font-family: 'Times New Roman';");
                }

                if (stringData.Contains("<p>"))
                {
                    stringData = stringData.Replace("<p>", "<p style=\"line-height: 130%;padding-top: 0;padding-bottom: 0;font-size: 14.0pt; font-family: 'Times New Roman';\">");
                }

                string ret = new Regex(@"line-height.+?;").Replace(stringData, "line-height: 130%;");
                ret = new Regex(@"mso-margin-top-alt.+?;").Replace(ret, string.Empty);
                ret = new Regex(@"mso-margin-bottom-alt.+?;").Replace(ret, string.Empty);
                return ret;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringData"></param>
        /// <param name="lineSpacing">Cách dòng (VD: 1,3 = 130)</param>
        /// <returns></returns>
        public static string NTSOverHtmlString(this string stringData, int lineSpacing, bool isCenter = false)
        {

            //string cleaned = new Regex("style=\"[^\"]*\"").Replace(stringData, "");
            // cleaned = new Regex("(?<=class=\")([^\"]*)\\babc\\w*\\b([^\"]*)(?=\")").Replace(cleaned, "$1$2");
            //cleaned = cleaned.Replace("<span", "<span style=\"font-size: 14.0pt; font-family: 'Times New Roman';\"");
            if (!string.IsNullOrEmpty(stringData))
            {

                List<int> listIndex = new List<int>();
                for (int i = 0; i < stringData.Length; i++)
                {
                    if (i + 1 < stringData.Length && stringData[i].Equals('<') && stringData[i + 1].Equals('p'))
                    {
                        int indexSty = stringData.IndexOf("style=\"", i + 1);

                        if (indexSty != -1)
                        {
                            for (int j = indexSty + 7; j < stringData.Length; j++)
                            {
                                if (stringData[j].Equals('"'))
                                {
                                    listIndex.Add(j);
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int i = listIndex.Count - 1; i >= 0; i--)
                {
                    if (isCenter)
                    {
                        stringData = stringData.Insert(listIndex[i], "line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0; font-size: 14.0pt; font-family: 'Times New Roman';text-align: center;");
                    }
                    else
                    {
                        stringData = stringData.Insert(listIndex[i], "line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0; font-size: 14.0pt; font-family: 'Times New Roman';");
                    }
                }

                if (stringData.Contains("<p>"))
                {
                    if (isCenter)
                    {
                        stringData = stringData.Replace("<p>", "<p style=\"line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0;font-size: 14.0pt; font-family: 'Times New Roman';text-align: center;\">");
                    }
                    else
                    {
                        stringData = stringData.Replace("<p>", "<p style=\"line-height: " + lineSpacing.ToString() + "%;padding-top: 0;padding-bottom: 0;font-size: 14.0pt; font-family: 'Times New Roman';\">");
                    }
                }

                string ret = new Regex(@"line-height.+?;").Replace(stringData, "line-height: " + lineSpacing.ToString() + "%;");
                ret = new Regex(@"mso-margin-top-alt.+?;").Replace(ret, string.Empty);
                ret = new Regex(@"mso-margin-bottom-alt.+?;").Replace(ret, string.Empty);
                return ret;
            }
            else
                return string.Empty;
        }

        public static string NTSRemoveHtmlString(this string stringData)
        {
            return new Regex(@"<[^>]*>").Replace(stringData, string.Empty);
        }

        public static void NTSReplaceFirst(this WordDocument document, string given, string replace)
        {
            document.ReplaceFirst = true;
            document.Replace(given, !string.IsNullOrEmpty(replace) ? replace : string.Empty, false, true);
            document.ReplaceFirst = false;
        }

        public static void NTSRemoveFirst(this WordDocument document, string given)
        {
            document.ReplaceFirst = true;
            var textSection = document.Find(given, false, true);
            WTextRange textRange = textSection.GetAsOneRange();
            //Get the owner textbody of the paragraph and remove the paragraph from it 
            WTextBody ownerTextBody = textRange.OwnerParagraph.OwnerTextBody;
            if (ownerTextBody != null)
                ownerTextBody.ChildEntities.Remove(textRange.OwnerParagraph);
        }

        public static void NTSReplaceHtml(this WordDocument document, string given, string html)
        {
            WordDocument replaceDoc = new WordDocument();
            IWSection htmlContent = replaceDoc.AddSection();
            if (htmlContent.Body.IsValidXHTML(html, XHTMLValidationType.Transitional))
            {
                htmlContent.Body.InsertXHTML(html);
            }
            document.Replace(given, replaceDoc, false, false);
        }

        public static void NTSReplaceImage(this WordDocument document, string given, string path)
        {
            WordDocument replate = new WordDocument();
            //string filepath = HostingEnvironment.MapPath("~/" + path);
            string filepath = path;
            if (File.Exists(filepath))
            {
                IWParagraph paragraph = replate.AddSection().AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                WPicture mImage = (WPicture)paragraph.AppendPicture(Image.FromFile(filepath));
                //mImage.HeightScale = 70f;
                //mImage.WidthScale = 70f;
                mImage.HeightScale = 220f;
                mImage.WidthScale = 280f;
            }
            document.ReplaceFirst = true;
            document.Replace(given, replate, false, true);
            document.ReplaceFirst = false;
        }


        public static void NTSAddText(this IWSection section, string text, float fontSize, bool bold, HorizontalAlignment align)
        {
            IWParagraph mPara = section.AddParagraph();
            mPara.ParagraphFormat.HorizontalAlignment = align;
            IWTextRange mtext = mPara.AppendText(text);
            mtext.CharacterFormat.FontSize = fontSize;
            mtext.CharacterFormat.Bold = bold;
            mtext.CharacterFormat.FontName = "Times New Roman";
        }

        public static void NTSReplaceAll(this WordDocument document, string given, string replace)
        {
            document.ReplaceFirst = false;
            document.Replace(given, !string.IsNullOrEmpty(replace) ? replace : string.Empty, false, true);
        }
    }

}
