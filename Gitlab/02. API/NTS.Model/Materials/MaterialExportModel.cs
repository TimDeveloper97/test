using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialExportModel
    {
        public string Id { get; set; }
        public string Stt { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Code { get; set; }
        public string RawMaterialCode { get; set; }
        public string DV { get; set; }
        public string SL { get; set; }
        public string VL { get; set; }
        public string KL { get; set; }
        public string ManufactureName { get; set; }
        public string ManufactureCode { get; set; }
        public string Note { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public string CreateBy { get; set; }
        public bool IsUsuallyUse { get; set; }
        public string Status { get; set; }
        public bool Is3DExist { get; set; }
        public List<int> ListIndexPart { get; set; }
        public List<string> ListEror { get; set; }
        public List<MaterialExportModel> ListDMVTNotDB { get; set; }
        public MaterialExportModel()
        {
            ListDMVTNotDB = new List<MaterialExportModel>();
            ListIndexPart = new List<int>();
            ListEror = new List<string>();
        }
    }
}
