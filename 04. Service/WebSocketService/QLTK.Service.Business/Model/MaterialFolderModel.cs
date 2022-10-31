using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class MaterialFolderModel
    {
        public string Manufature { get; set; }
        public string Name { get; set; }
        public bool IsExist { get; set; }
        public List<FileDocumentModel> ListMaterialDocument3DFolderModel { get; set; }

        public MaterialFolderModel()
        {
            ListMaterialDocument3DFolderModel = new List<FileDocumentModel>();
        }
    }
    //public class MaterialDocument3DFolderModel
    //{
    //    public string Name { get; set; }
    //    public List<FileDocumentModel> ListDocument3D { get; set; }
    //    public MaterialDocument3DFolderModel()
    //    {
    //        ListDocument3D = new List<FileDocumentModel>();

    //    }
    //}

    public class FileDocumentModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
