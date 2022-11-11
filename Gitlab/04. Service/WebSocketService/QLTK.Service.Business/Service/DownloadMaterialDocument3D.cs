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
    public class DownloadMaterialDocument3D
    {
        private ApiUtil apiUtil = new ApiUtil();
        public MaterialStatusDownload DownloadMaterialDocument3Ds(DowloadMaterialDocument model)
        {
            List<FileErrorModel> listError = new List<FileErrorModel>();
            var nameFoler = "Thietke.Ck";
            bool checkOther = true;
            string moduleCode;
            var resultApi = apiUtil.GetDataDownloadMaterialDocument3Ds(model);
            if (resultApi.Count > 0)
            {
                var count = 0;
                var ModuleCode = model.Material[0].ModuleCode;
                moduleCode = ModuleCode;
                try
                {

                    var folderFather = Path.Combine(model.DownloadPath, nameFoler);
                    CreateFolder(folderFather);
                    var folderFather1 = Path.Combine(folderFather, ModuleCode.Substring(0, 4));
                    CreateFolder(folderFather1);
                    var folderFather2 = Path.Combine(folderFather1, ModuleCode.Substring(0, 6));
                    CreateFolder(folderFather2);
                    var folderFather3 = Path.Combine(folderFather2, ModuleCode + ".Ck");
                    CreateFolder(folderFather3);
                    var folderFather4 = Path.Combine(folderFather3, "3D." + ModuleCode);
                    CreateFolder(folderFather4);
                    var folderFather5 = Path.Combine(folderFather4, "COM." + ModuleCode);
                    CreateFolder(folderFather5);

                    foreach (var resul in resultApi)
                    {
                        if (resul.IsExist)
                        {
                            if (resul.ListMaterialDocument3DFolderModel.Count == 0)
                            {
                                FileErrorModel file = new FileErrorModel();
                                file.Materialname = resul.Name;
                                file.ManuafactureName = resul.Manufature;
                                file.ErrorMessage = "Không có file để tải";
                                listError.Add(file);
                            }
                            if (resul.ListMaterialDocument3DFolderModel.Count > 0)
                            {
                                var r = Path.Combine(folderFather5, resul.Manufature);
                                CreateFolder(r);

                                foreach (var item in resul.ListMaterialDocument3DFolderModel)
                                {
                                    var b = apiUtil.DownloadFile(model.ApiFileUrl, item.Path, r, item.Name);
                                    if (!b)
                                    {
                                        FileErrorModel file = new FileErrorModel();
                                        file.Materialname = resul.Name;
                                        file.ManuafactureName = resul.Manufature;
                                        file.ErrorMessage = "Không tồn tại file trên server để tải!";
                                        listError.Add(file);
                                    }
                                }
                            }
                        }
                        else
                        {
                            FileErrorModel file = new FileErrorModel();
                            file.Materialname = resul.Name;
                            file.ManuafactureName = resul.Manufature;
                            file.ErrorMessage = "Vật tư không tồn tại!";
                            listError.Add(file);
                        }
                        count++;
                    }

                }
                catch (Exception ex)
                {
                    checkOther = false;
                }
            }
            else
            {
                throw NTSException.CreateInstance("Không có dữ liệu để tải!");
            }
            MaterialStatusDownload msd = new MaterialStatusDownload();
            msd.IsSuccess = checkOther;
            if (listError.Count() > 0)
            {
                model.ListError = listError;
                msd.LinkExcel = apiUtil.ExportExcelListError(model);
            }

            return msd;
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
