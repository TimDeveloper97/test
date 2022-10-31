using Inventor;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Model.Solution;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpaInvClass;

namespace BMS
{
    public class IPTDetail
    {
        public static string Hang;
        public static string DeliveryPeriod;
        public static string Price;
        public static string VL;
        public static string Weight;
        public static string Properties;
        public static string Unit;
        public static string MaVatLieu;
        public static string Note;
        public static string Name;
        public static string Code;
        public static string User;
        public static Image Image;
        public static string FileName;
        public static string Author;

        public static ImageResult imageResult;
        public static void GetPathUpLoadImage(string filePath, string apiUrl, string token,string name)
        {
            name = name + ".png";
            var data = LoadData(filePath);
            imageResult = ApiUtil.UploadFile(apiUrl, Image, token, name);
            imageResult.FileName = name;
        }

        public static CInvPartInfor.MyIproperties LoadData(string filePath)
        {
            CInvPartInfor Ivt = new CInvPartInfor();
            CInvPartInfor.MyIproperties mData = Ivt.GetPartInfor(filePath);

            Hang = mData.Company;
            Note = mData.Comments;
            Price = mData.Cost.ToString();
            DeliveryPeriod = mData.Delivery.ToString();
            Code = mData.PartNumber;
            Unit = mData.Manager;
            VL = mData.Material;
            Name = mData.Title;
            Weight = "";
            MaVatLieu = mData.Subject;
            Properties = mData.Category;
            User = mData.Author;
            Image = mData.thumbNail;
            FileName = mData.FileName;
            Author = mData.Author;
            return mData;
        }

        public static void WriteDataPrice(string filePath, double Price, int DeliveryPeriod)
        {
            CInvPartInfor Ivt = new CInvPartInfor();
            CInvPartInfor.MyIproperties mData = Ivt.GetPartInfor(filePath);
            Ivt.UpDateIpropertiesAPP(mData.FileName, Price, DeliveryPeriod);
        }

        public static void WriteData(CInvPartInfor.MyIproperties mData)
        {
            CInvPartInfor Ivt = new CInvPartInfor();
            Ivt.UpdateAllIpropertiesAPP(mData.FileName, mData);
        }

        public static CInvPartInfor.MyIproperties GetIproperties(string filePath)
        {
            CInvPartInfor Ivt = new CInvPartInfor();
            CInvPartInfor.MyIproperties mData = Ivt.GetPartInfor(filePath);
            return mData;
        }

        public static void WriteName(string filePath, string materialName)
        {
            CInvPartInfor Ivt = new CInvPartInfor();
            CInvPartInfor.MyIproperties mData = Ivt.GetPartInfor(filePath);
            mData.Title = materialName;
            Ivt.UpdateAllIpropertiesAPP(filePath, mData);
        }
    }
}
