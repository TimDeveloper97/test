using QLTK.Service.Model.Manufactures;
using QLTK.Service.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Materials
{
    public class DataCheckDMVTModel
    {
        //public List<Design3DModel> MaterialDesign3D { get; set; }
        public List<CheckMatrialModel> Materials { get; set; }
        public List<CheckRawMaterialModel> RawMaterials { get; set; }
        public List<CheckManufactureModel> Manufactures { get; set; }
        public List<CheckUnitModel> Units { get; set; }
        public List<CheckConverUnitModel> ConverUnits { get; set; }

        public DataCheckDMVTModel()
        {
            //MaterialDesign3D = new List<Design3DModel>();
            Materials = new List<CheckMatrialModel>();
            RawMaterials = new List<CheckRawMaterialModel>();
            Manufactures = new List<CheckManufactureModel>();
            Units = new List<CheckUnitModel>();
            ConverUnits = new List<CheckConverUnitModel>();
        }
    }

    public class CheckMatrialModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UnitName { get; set; }
        public string ManufactureCode { get; set; }
        public bool Is3DExist { get; set; }
        public string Status { get; set; }
    }

    public class CheckManufactureModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
    }

    public class CheckRawMaterialModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CheckDesign3DModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; }

    }

    public class CheckConverUnitModel
    {
        public string Id { get; set; }
        public string MaterialCode { get; set; }
    }

    public class CheckUnitModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
