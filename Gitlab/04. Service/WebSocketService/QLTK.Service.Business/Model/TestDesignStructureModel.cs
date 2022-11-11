using QLTK.Service.Model;
using QLTK.Service.Model.Materials;
using QLTK.Service.Model.Checkbox;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class TestDesignStructureModel
    {
        public string ApiUrl { get; set; }
        public string FileApiUrl { get; set; }

        /// <summary>
        /// Đường dẫn file danh mục vật tư
        /// </summary>
        public string PathFileMaterial { get; set; }

        /// <summary>
        /// Đường dẫn file thiết kế 2D tổng
        /// </summary>
        public string PathFileIDW { get; set; }

        /// <summary>
        /// Đường dẫn thư mục chữa bản cứng CAD
        /// </summary>
        public string PathFolderBCCAD { get; set; }

        /// <summary>
        /// Đường dẫn thư mục chữa file CAD
        /// </summary>
        public string PathFolderFileCAD { get; set; }

        /// <summary>
        /// Đường dẫn thư mục MAT
        /// </summary>
        public string PathFolderMAT { get; set; }

        /// <summary>
        /// Đường dẫn thư mục JGS
        /// </summary>
        public string PathFolderIGS { get; set; }

        /// <summary>
        /// Đường dẫn thư mục 3D
        /// </summary>
        public string PathFolder3D { get; set; }

        /// <summary>
        /// Lấy tên file name
        /// </summary>
        public string FileName { get; set; }

        public string Token { get; set; }
        public string SelectedPath { get; set; }
        public int Type { get; set; }
        public string ModuleCode { get; set; }
        public string PathDownload { get; set; }
        public string ModuleGroupCode { get; set; }
        public List<Design3DModel> List3D { get; set; }
        public List<MaterialModel> ListMaterialDB { get; set; }
        public List<ModuleDesignDocumentModel> ListModuleDesignDocument { get; set; }
        public List<RawMaterialModel> ListRawMaterial { get; set; }
        public List<ErrorModel> ListModuleError { get; set; }
        public List<ConverUnitModel> ListConvertUnit { get; set; }
        public List<DesignStructureModel> ListDesignStructure { get; set; }
        public List<DesignStructureFileModel> ListDesignStructureFile { get; set; }
        public List<ModuleModel> Module { get; set; }
        public List<ManufactureResultModel> ListManufacture { get; set; }
        public List<UnitModel> ListUnit { get; set; }
        public List<ModuleModel> ListModule { get; set; }
        public CheckModel CheckModel { get; set; }
    }
}
