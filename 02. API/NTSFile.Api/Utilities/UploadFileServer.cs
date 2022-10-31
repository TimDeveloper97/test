using Ionic.Zip;
using NTS.Common;
using NTSFile.Api.Utilities;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Presentation;
using Syncfusion.PresentationToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace NTSFile.Api
{
    public class UploadFileServer
    {
        private static List<string> ConvertPDFExtention = new List<string>() { ".xls", ".xlsx", ".doc", ".docx", ".ppt", ".pptx" };
        public static bool ThumbnailCallback()
        {
            return false;
        }

        public static Bitmap CreateImageThumbnail(HttpPostedFile imageUpload)
        {
            int width = 120;
            Bitmap bitmapResult = null;
            try
            {
                var brush = new SolidBrush(Color.Black);
                Stream inputStream = imageUpload.InputStream;
                Image.GetThumbnailImageAbort thumbnailCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Bitmap bitmapImage = new Bitmap(inputStream);
                Image image = bitmapImage.GetThumbnailImage(width, (bitmapImage.Height * 120) / bitmapImage.Width, thumbnailCallback, IntPtr.Zero);
                bitmapResult = new Bitmap(image);
            }
            catch { }
            return bitmapResult;
        }

        /// <summary>
        /// Hàm upload file lên server
        /// </summary>
        /// <param name="file">File ảnh cần upload lên server</param>
        /// <param name="teacher">Đối tượng để lưu link ảnh sau khi upload</param>
        public static ImageResult UploadFile(HttpPostedFile file, string folderName)
        {
            var fileArray = file.FileName.ToString().Split('.');
            string fileName = Guid.NewGuid().ToString() + "." + fileArray[fileArray.Length - 1];
            ImageResult imageResult = new ImageResult();

            string pathFolder = Constants.fileUpload + folderName;
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            string fileResult = string.Empty;
            // Kiểm tra folder là tên của ProjectId đã tồn tại chưa.
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }
            // kiểm tra size file > 0
            if (file.ContentLength > 0)
            {
                file.SaveAs(pathFolderServer + fileName);
                fileResult = pathFolder + fileName;

            }

            imageResult.FileSize = file.ContentLength;
            imageResult.FileUrl = fileResult;
            imageResult.FileUrlThum = fileResult;
            imageResult.FileName = file.FileName;
            imageResult.FullFileUrl = pathFolderServer + fileName;
            return imageResult;
        }


        /// <summary>
        /// Hàm upload file lên server
        /// </summary>
        /// <param name="file">File ảnh cần upload lên server</param>
        /// <param name="teacher">Đối tượng để lưu link ảnh sau khi upload</param>
        public static ImageResult UploadFileConvertPDF(HttpPostedFile file, string folderName)
        {
            string extention = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extention;
            ImageResult imageResult = new ImageResult();

            string pathFolder = NTSFile.Api.Constants.fileUpload + folderName;
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            string fileResult = string.Empty;
            // Kiểm tra folder là tên của ProjectId đã tồn tại chưa.
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }
            // kiểm tra size file > 0
            if (file.ContentLength > 0)
            {
                file.SaveAs(pathFolderServer + fileName);
                fileResult = pathFolder + fileName;

                if (ConvertPDFExtention.IndexOf(extention) != -1)
                {
                    string pdfLink = ConvertFileToPDF(pathFolderServer + fileName, pathFolderServer, fileName);
                    if (!string.IsNullOrEmpty(pdfLink))
                    {
                        imageResult.FilePDFUrl = pathFolder + ConvertFileToPDF(pathFolderServer + fileName, pathFolderServer, fileName);
                    }
                    else
                    {
                        imageResult.FilePDFUrl = string.Empty;
                    }                    
                }
            }

            imageResult.FileSize = file.ContentLength;
            imageResult.FileUrl = fileResult;
            imageResult.FileUrlThum = fileResult;
            imageResult.FileName = file.FileName;

            return imageResult;
        }

        private static string ConvertFileToPDF(string pathFile, string pathServer, string fileName)
        {
            string filePDFName = null;
            string fileExtentions = Path.GetExtension(fileName);
            try
            {
                if (fileExtentions.Equals(".xlsx") || fileExtentions.Equals(".xls"))
                {
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IApplication application = excelEngine.Excel;
                        application.DefaultVersion = ExcelVersion.Excel2013;
                        IWorkbook workbook = application.Workbooks.Open(pathFile);
                        //  IWorkbook workbook = application.Workbooks.Open(pathFile, ExcelOpenType.Automatic);

                        //Open the Excel document to Convert
                        ExcelToPdfConverter converter = new ExcelToPdfConverter(workbook);

                        //Initialize PDF document
                        PdfDocument pdfDocument = new PdfDocument();

                        //Convert Excel document into PDF document
                        pdfDocument = converter.Convert();

                        filePDFName = Path.GetFileNameWithoutExtension(fileName) + "_pdf.pdf";
                        //Save the PDF file
                        pdfDocument.Save(Path.Combine(pathServer, filePDFName));
                    }
                }
                if (fileExtentions.Equals(".docx") || fileExtentions.Equals(".doc"))
                {
                    WordDocument wordDocument = new WordDocument(pathFile, Syncfusion.DocIO.FormatType.Docx);
                    //Initializes the ChartToImageConverter for converting charts during Word to pdf conversion
                    wordDocument.ChartToImageConverter = new ChartToImageConverter();
                    //Creates an instance of the DocToPDFConverter
                    DocToPDFConverter converter = new DocToPDFConverter();
                    //Converts Word document into PDF document
                    PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                    filePDFName = Path.GetFileNameWithoutExtension(fileName) + "_pdf.pdf";
                    //Saves the PDF file 
                    pdfDocument.Save(Path.Combine(pathServer, filePDFName));
                }
                if (fileExtentions.Equals(".ppt") || fileExtentions.Equals(".pptx"))
                {
                    //   IPresentation presentation = Presentation.Open("Syncfusion Presentation.pptx");
                    IPresentation presentation = Presentation.Open(pathFile);
                    //Converts the PowerPoint Presentation into PDF document
                    PdfDocument pdfDocument = PresentationToPdfConverter.Convert(presentation);
                    filePDFName = Path.GetFileNameWithoutExtension(fileName) + "_pdf.pdf";
                    //Saves the PDF file 
                    pdfDocument.Save(Path.Combine(pathServer, filePDFName));
                    pdfDocument.Close(true);
                    //Closes the Presentation
                    presentation.Close();
                }
            }
            catch (Exception ex)
            {

            }
            
            return filePDFName;
        }

        public static ImageResult UploadImage(HttpPostedFile file, string folderName)
        {


            var fileArray = file.FileName.ToString().Split('.');
            string fileName = Guid.NewGuid().ToString() + "." + fileArray[fileArray.Length - 1];
            ImageResult imageResult = new ImageResult();
            #region[ảnh thường]
            string pathFolder = NTSFile.Api.Constants.fileUpload + folderName;
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            string fileResult = string.Empty;
            // Kiểm tra folder là tên của ProjectId đã tồn tại chưa.
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }
            // kiểm tra size file > 0
            if (file.ContentLength > 0)
            {
                file.SaveAs(pathFolderServer + fileName);
                fileResult = pathFolder + fileName;
            }
            #endregion
            Image image = Image.FromFile(pathFolderServer + fileName);
            Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
            thumb.Save(Path.ChangeExtension(pathFolderServer + fileName, "thumb"));
            imageResult.FileUrl = fileResult;
            imageResult.FileUrlThum = fileResult;
            return imageResult;
        }
        public static void DeleteFile(string fileName)
        {
            // Xóa folder chứa file ảnh đại diện cũ
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = HostingEnvironment.MapPath("~/" + fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagebase64"></param>
        /// <param name="folderName"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string SaveImageBase64(string imagebase64, string folderName, string imagePath)
        {
            byte[] bytes = Convert.FromBase64String(imagebase64.Replace("data:image/png;base64,", ""));

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            string pathFolder = "fileUpload/" + folderName + "/";
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            string pathFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_Anh.png";
            image.Save(pathFolderServer + pathFile);

            if (!string.IsNullOrEmpty(imagePath))
            {
                File.Delete(HostingEnvironment.MapPath("~/" + imagePath));
            }

            return "/" + pathFolder + pathFile;
        }

        public static string GetImageBase64(string imagePath)
        {
            string pathFile = HostingEnvironment.MapPath("~/" + imagePath);

            if (!File.Exists(pathFile))
            {
                return string.Empty;
            }

            string base64 = string.Empty;

            using (Image image = Image.FromFile(pathFile))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64 = Convert.ToBase64String(imageBytes);
                }
            }

            return "data:image/png; base64," + base64;
        }

        /// <summary>
        /// Lưu file ảnh từ url image
        /// </summary>
        /// <param name="imageUrl">Url image</param>
        /// <param name="folderName"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string SaveFileUrlToServer(string fileUrl, string folderName, string imagePath)
        {
            string pathFolder = NTSFile.Api.Constants.fileUpload + folderName;
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }
            WebClient wc = new WebClient();
            wc.DownloadFile(fileUrl, pathFolderServer + imagePath);
            return pathFolder + imagePath;
        }

        public static ResultDownload DownAllDocumentProcess(ListDataModel model)
        {
            int countErro = 0;
            var result = new ResultDownload();
            string tempError = string.Empty;
            var listString = new List<string>();
            var listPath = model.ListDatashet;
            if (listPath.Count == 0)
            {
                throw NTSException.CreateInstance("Không có file để tải");
            }

            string folder = Guid.NewGuid().ToString();
            string fileName = model.Name + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".zip";
            string pathExport = HttpContext.Current.Server.MapPath("~/" + NTSFile.Api.Constants.FolderExport + folder);
            Directory.CreateDirectory(pathExport);
            string pathZip = Path.Combine(pathExport, fileName);
            string pathCopy = string.Empty;
            try
            {
                string tempPath = string.Empty;

                for (int i = 0; i < listPath.Count(); i++)
                {
                    tempPath = listPath[i].Path;
                    listPath[i].Path = HostingEnvironment.MapPath("~/" + listPath[i].Path);
                    if (!File.Exists(listPath[i].Path))
                    {
                        countErro++;
                        tempError += "! ";
                        throw NTSException.CreateInstance("Không tồn tại file trên server!");
                    }
                    else
                    {
                        pathCopy = Path.Combine(pathExport, listPath[i].FileName);

                        File.Copy(listPath[i].Path, pathCopy);

                        listString.Add(pathCopy);
                    }
                }
                using (var zip = new ZipFile())
                {
                    zip.UseUnicodeAsNecessary = true;
                    zip.AddFiles(listString, Path.GetFileName(model.Name));
                    zip.Save(pathZip);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new NTSLogException(model, ex);
            }

            if (!string.IsNullOrEmpty(tempError))
            {
                tempError = countErro + "/ tổng " + listPath.Count() + " file không tìm thấy hoặc đã bị xóa";
            }

            result.Error = tempError;
            result.PathZip = NTSFile.Api.Constants.FolderExport + folder + "/" + fileName;
            return result;
        }
    }
}