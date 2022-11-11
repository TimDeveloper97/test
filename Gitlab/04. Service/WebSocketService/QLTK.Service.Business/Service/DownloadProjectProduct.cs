using NTS.Common;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Service
{
    public class DownloadProjectProduct
    {
        private ApiUtil apiUtil = new ApiUtil();

        public bool DownloadProductDocuments(DownloadProjectProductModel model)
        {
            bool checkOther = false;

            var resultApi = apiUtil.GetDataDownloadProductDocuments(model);
            if (resultApi.ListProjectProductFolder.Count > 0)
            {
                var root = Path.Combine(model.DownloadPath, resultApi.Name);

                CreateFolder(root);

                foreach (var item in resultApi.ListProjectProductFolder)
                {
                    var pathProduct = Path.Combine(root, item.Name);
                    CreateFolder(pathProduct);

                    var pathHDSD = Path.Combine(pathProduct, "HDSD");
                    CreateFolder(pathHDSD);
                    var pathHDTH = Path.Combine(pathProduct, "HDTH");
                    CreateFolder(pathHDTH);
                    var pathDatasheet = Path.Combine(pathProduct, "Datasheet");
                    CreateFolder(pathDatasheet);

                    if(item.ListHDSD.Count > 0)
                    {
                        foreach (var ite in item.ListHDSD)
                        {
                            apiUtil.DownloadFile(model.ApiFileUrl, ite.Path, pathHDSD, ite.Name);
                        }
                    }

                    if (item.ListHDTH.Count > 0)
                    {
                        foreach (var ite in item.ListHDTH)
                        {
                            apiUtil.DownloadFile(model.ApiFileUrl, ite.Path, pathHDTH, ite.Name);
                        }
                    }

                    if (item.ListDataSheet.Count > 0)
                    {
                        foreach (var ite in item.ListDataSheet)
                        {
                            apiUtil.DownloadFile(model.ApiFileUrl, ite.Path, pathDatasheet, ite.Name);
                        }
                    }

                    apiUtil.DownloadFiles(model.ApiUrl, item.FileExcel.Path, pathProduct, item.FileExcel.Name);
                    checkOther = true;
                }
            }
            else
            {
                throw NTSException.CreateInstance("Không có dữ liệu để tải!");
            }

            return checkOther;
        }

        public void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                List<string> pathChild = path.Split('/').ToList();

                string root = pathChild[0];

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                for (int i = 1; i < pathChild.Count; i++)
                {
                    root += "/" + pathChild[i];
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                }

            }
        }
    }
}
