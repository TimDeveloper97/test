using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ModulesModel
    {
        private int iD;
        private string code;
        private string name;
        private int moduleGroupID;
        private string path;
        private string note;
        private string description;
        private int status;
        private string specifications;
        private int fileElectric;
        private int fileElectronic;
        private int fileMechanics;
        private int fileProgram;
        private string createdBy;
        private DateTime? createdDate;
        private string updatedBy;
        private DateTime? updatedDate;
        private string imagePath;
        private decimal orderTime;
        private int costGroupID;
        private decimal price;
        private decimal realPrice;
        private decimal tradePrice;
        private decimal userPrice;
        private decimal vAT;
        private int isCheckCTok;
        private int stopStatus;
        private int isHMI;
        private int isPLC;
        private int isUpdatedFilm;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int ModuleGroupID
        {
            get { return moduleGroupID; }
            set { moduleGroupID = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Specifications
        {
            get { return specifications; }
            set { specifications = value; }
        }

        public int FileElectric
        {
            get { return fileElectric; }
            set { fileElectric = value; }
        }

        public int FileElectronic
        {
            get { return fileElectronic; }
            set { fileElectronic = value; }
        }

        public int FileMechanics
        {
            get { return fileMechanics; }
            set { fileMechanics = value; }
        }

        public int FileProgram
        {
            get { return fileProgram; }
            set { fileProgram = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public DateTime? UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        public decimal OrderTime
        {
            get { return orderTime; }
            set { orderTime = value; }
        }

        public int CostGroupID
        {
            get { return costGroupID; }
            set { costGroupID = value; }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public decimal RealPrice
        {
            get { return realPrice; }
            set { realPrice = value; }
        }

        public decimal TradePrice
        {
            get { return tradePrice; }
            set { tradePrice = value; }
        }

        public decimal UserPrice
        {
            get { return userPrice; }
            set { userPrice = value; }
        }

        public decimal VAT
        {
            get { return vAT; }
            set { vAT = value; }
        }

        public int IsCheckCTok
        {
            get { return isCheckCTok; }
            set { isCheckCTok = value; }
        }

        public int StopStatus
        {
            get { return stopStatus; }
            set { stopStatus = value; }
        }

        public int IsHMI
        {
            get { return isHMI; }
            set { isHMI = value; }
        }

        public int IsPLC
        {
            get { return isPLC; }
            set { isPLC = value; }
        }

        public int IsUpdatedFilm
        {
            get { return isUpdatedFilm; }
            set { isUpdatedFilm = value; }
        }
    }
}
