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
    public class DowloadFileGoogle
    {
        public List<string> DowloadFileGoogles(DowloadFileGoogleModel model)
        {
            GoogleApi googleApi = new GoogleApi();
            List<string> lstError = new List<string>();
            var listStrucFile = model.ListStrucFile.Where(i => i.Type == model.Type && model.ModuleId.Equals(i.ModuleId)).ToList();
            var listModule = model.ListModuleDesignDocument.Where(i => model.ModuleId.Equals(i.ModuleId)).ToList();
            var listModulMaterial = model.ListModuleMaterial.Where(a => model.ModuleId.Equals(a.ModuleId)).ToList();
            List<DataDistributionFileModel> dataDistributionFiles = new List<DataDistributionFileModel>();
            foreach (var item in listStrucFile)
            {
                if (item.ListFile.Count > 0)
                {
                    var listGetTypeTrue = item.ListFile.Where(i => 1 == i.GetType).ToList();
                    var listGetTypeFalse = item.ListFile.Where(i => 0 == i.GetType).ToList();
                    if (listGetTypeTrue.Count > 0)
                    {
                        dataDistributionFiles.AddRange(listGetTypeTrue);
                    }
                    if (listGetTypeFalse.Count > 0)
                    {
                        var data = CheckFolderAll(listModulMaterial, listGetTypeFalse, listModule);
                        dataDistributionFiles.AddRange(data);
                    }

                }
            }
            List<CompareModel> listCompare = new List<CompareModel>();
            foreach (var item in dataDistributionFiles)
            {
                var compare = listModule.Where(i => i.Path.Equals(item.FolderContain));
                if (compare.Count() > 0)
                {
                    foreach (var items in compare)
                    {
                        listCompare.Add(new CompareModel
                        {
                            Id = items.Id,
                            Name = items.Name,
                            Path = items.Path,
                            FolderContain = item.FolderContain
                        });
                    }
                }

            }

            foreach (var item in listCompare)
            {
                try
                {
                    var link = model.Path + "/" + item.Path.Replace("/" + item.Name, "");
                    CreateFolder(link);
                    googleApi.DownloadFile(item.Id, item.Name, link);
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Contains("[404]"))
                    {
                        lstError.Add("File " + item.Name + " không tồn tại trên google drive!");
                    }
                        
                    continue;
                }
                
            }

            return lstError;
        }


        //Kiểm tra thư mục nhiều file
        public List<DataDistributionFileModel> CheckFolderAll(List<ModuleMaterialModel> listModuleMaterial, List<DataDistributionFileModel> listGetTypeFalse, List<ModuleDesignDocumentModel> listModule)
        {
            List<DataDistributionFileModel> dataDistributionFiles = new List<DataDistributionFileModel>();
            foreach (var item in listGetTypeFalse)
            {
                if (!string.IsNullOrEmpty(item.Extension))
                {
                    var data = ReadExcel(listModuleMaterial, item, listModule);
                    dataDistributionFiles.AddRange(data);
                }
                else
                {
                    var link = listModule.Where(i => i.Path.Contains(item.FolderContain));
                    if (link.Count() > 0)
                    {
                        foreach (var items in link)
                        {
                            item.FolderContain = items.Path;
                            dataDistributionFiles.Add(item);
                        }
                    }
                }
            }
            return dataDistributionFiles;
        }

        //Kiểm tra thông số, mã vật liệu, đơn vị
        public List<DataDistributionFileModel> ReadExcel(List<ModuleMaterialModel> listModuleMaterial, DataDistributionFileModel model, List<ModuleDesignDocumentModel> listModule)
        {
            var data = listModuleMaterial.ToList();
            List<DataDistributionFileModel> dataDistributionFiles = new List<DataDistributionFileModel>();
            if (!string.IsNullOrEmpty(model.FilterThongSo))
            {
                data = data.Where(i => model.FilterThongSo.Equals(i.Specification, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (model.FilterMaVatLieu == true)
            {
                data = data.Where(i => !string.IsNullOrEmpty(i.RawMaterialCode)).ToList();
            }
            if (!string.IsNullOrEmpty(model.FilterDonVi))
            {
                data = data.Where(i => model.FilterDonVi.Equals(i.UnitName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(model.FilterThongSo) || model.FilterMaVatLieu == true || !string.IsNullOrEmpty(model.FilterDonVi))
            {

                foreach (var item in data)
                {
                    //var link = model.FolderContain + "/" + item.MaterialCode;
                    var modelAdd = new DataDistributionFileModel();
                    modelAdd.FolderContain = model.FolderContain + "/" + item.MaterialCode + model.Extension;
                    dataDistributionFiles.Add(modelAdd);
                }
            }
            else
            {
                var link = listModule.Where(i => i.Path.Contains(model.FolderContain) && i.Name.Contains(model.Extension));
                if (link.Count() > 0)
                {
                    foreach (var items in link)
                    {
                        var modelAdd = new DataDistributionFileModel();
                        modelAdd.FolderContain = items.Path;
                        dataDistributionFiles.Add(modelAdd);
                    }
                }
                dataDistributionFiles.Add(model);
                return dataDistributionFiles;
            }
            return dataDistributionFiles;
        }

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                List<string> pathChild = path.Split('/').ToList();

                string root = pathChild[0];

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

        public List<string> DownloadFileModuleGoogle(DowloadFileGoogleModel model)
        {
            List<string> listRe = new List<string>();
            if (model.ListSelect.Count >  0)
            {
                foreach (var item in model.ListSelect)
                {
                    model.ModuleId = item.Id;
                    var listFile = DowloadFileGoogles(model);
                    listRe.AddRange(listFile);
                }
            }
            return listRe;
        }

    }
}
