using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class DataDistributionFileHistoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FolderContain { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public int GetType { get; set; }
        public string FilterThongSo { get; set; }
        public bool FilterMaVatLieu { get; set; }
        public string FilterDonVi { get; set; }
        public string FilterManufacturer { get; set; }
        public string FilterRawMaterial { get; set; }
        public string FilterRawMaterialCode { get; set; }
        public string FilterMaterialCodeStart { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public bool MAT { get; set; }
        public bool TEM { get; set; }
        public string Extension { get; set; }

        public string DataDistributionId { get; set; }
        public string DataDistributionFileLinkId { get; set; }

        public bool IsFolderMaterial { get; set; }
    }
}
