using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility.VB6;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Inventor;

namespace TpaInvClass
{
    public class CInvPartInfor
    {
        // Fields
        private ApprenticeServerComponent m_Apprentice = null;
        private ApprenticeServerDocument apprenticeDoc = null;

        // Methods
        public CInvPartInfor()
        {
            if (ReferenceEquals(this.m_Apprentice, null))
            {
                try
                {
                    this.m_Apprentice = new ApprenticeServerComponent();
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    Interaction.MsgBox("Lỗi !. B\x00f3 tay th\x00f4i", MsgBoxStyle.ApplicationModal, null);
                    ProjectData.ClearProjectError();
                }
            }
        }

        public string GetDirectory(string mPath, int mDeep)
        {
            string str4 = "";
            int num = 0;
            int num2 = 0;
            string str2 = "";
            string str5 = "";
            string expression = "";
            int num6 = Strings.Len(mPath);
            int start = 1;
            while (true)
            {
                int num8 = num6;
                if (start > num8)
                {
                    string str;
                    if (mDeep == -1)
                    {
                        str = expression;
                    }
                    else
                    {
                        str5 = "";
                        int num7 = Strings.Len(expression);
                        int num5 = 1;
                        while (true)
                        {
                            num8 = num7;
                            if (num5 > num8)
                            {
                                str = "";
                            }
                            else
                            {
                                str2 = Strings.Mid(expression, num5, 1);
                                str5 = str5 + str2;
                                if (str2 == @"\")
                                {
                                    str4 = str5;
                                    num++;
                                }
                                if (mDeep != (num2 - num))
                                {
                                    num5++;
                                    continue;
                                }
                                str = str4;
                            }
                            break;
                        }
                    }
                    return str;
                }
                str2 = Strings.Mid(mPath, start, 1);
                str5 = str5 + str2;
                if (str2 == @"\")
                {
                    expression = str5;
                    num2++;
                }
                start++;
            }
        }

        public MyIproperties GetPartInfor(string mPath)
        {
            MyIproperties iproperties;
            MyIproperties iproperties2 = new MyIproperties();
            if (!System.IO.File.Exists(mPath))
            {
                Interaction.MsgBox("File kh\x00f4ng tồn tại", MsgBoxStyle.ApplicationModal, null);
                iproperties = new MyIproperties();
            }
            else
            {
                try
                {
                    this.apprenticeDoc = this.m_Apprentice.Open(mPath);
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    Interaction.MsgBox("Lỗi !. B\x00f3 tay th\x00f4i", MsgBoxStyle.ApplicationModal, null);
                    ProjectData.ClearProjectError();
                }
                if (ReferenceEquals(this.apprenticeDoc, null))
                {
                    iproperties2.Empty();
                    iproperties = iproperties2;
                }
                else
                {
                    PropertySet set = this.apprenticeDoc.PropertySets["Inventor Document Summary Information"];
                    PropertySet set3 = this.apprenticeDoc.PropertySets["Inventor Summary Information"];
                    PropertySet set2 = this.apprenticeDoc.PropertySets["Design Tracking Properties"];
                    iproperties2.DisplayName = this.apprenticeDoc.DisplayName;
                    iproperties2.FileName = this.apprenticeDoc.FullFileName;
                    iproperties2.Dir = this.GetDirectory(this.apprenticeDoc.FullFileName, -1);
                    iproperties2.Title = Conversions.ToString(set3["Title"].Value);
                    iproperties2.Comments = Conversions.ToString(set3["Comments"].Value);
                    iproperties2.Author = Conversions.ToString(set3["Author"].Value);
                    iproperties2.Subject = Conversions.ToString(set3["Subject"].Value);
                    iproperties2.Manager = Conversions.ToString(set["Manager"].Value);
                    iproperties2.Company = Conversions.ToString(set["Company"].Value);
                    iproperties2.Category = Conversions.ToString(set["Category"].Value);
                    iproperties2.Cost = Conversions.ToDouble(set2["Cost"].Value);
                    iproperties2.Delivery = (int)Math.Round(Conversion.Val(set2["Manufacturer"].Value));
                    iproperties2.Description = Conversions.ToString(set2["Description"].Value);
                    iproperties2.Material = Conversions.ToString(set2["Material"].Value);
                    iproperties2.PartNumber = Conversions.ToString(set2["Part Number"].Value);
                    iproperties2.thumbNail = Support.IPictureDispToImage(set3["Thumbnail"].Value);
                    iproperties = iproperties2;
                }
            }
            return iproperties;
        }

        public void UpdateAllIpropertiesAPP(string mPath, MyIproperties mPropeties)
        {
            if (!System.IO.File.Exists(mPath))
            {
                Interaction.MsgBox("File kh\x00f4ng tồn tại", MsgBoxStyle.ApplicationModal, null);
            }
            else
            {
                try
                {
                    this.apprenticeDoc = this.m_Apprentice.Open(mPath);
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    Interaction.MsgBox("Lỗi !. B\x00f3 tay th\x00f4i", MsgBoxStyle.ApplicationModal, null);
                    ProjectData.ClearProjectError();
                }
                if (!ReferenceEquals(this.apprenticeDoc, null))
                {
                    PropertySet set = this.apprenticeDoc.PropertySets["Inventor Document Summary Information"];
                    PropertySet set3 = this.apprenticeDoc.PropertySets["Inventor Summary Information"];
                    PropertySet set2 = this.apprenticeDoc.PropertySets["Design Tracking Properties"];
                    set3["Title"].Value = mPropeties.Title;
                    set3["Comments"].Value = mPropeties.Comments;
                    set3["Author"].Value = mPropeties.Author;
                    set3["Subject"].Value = mPropeties.Subject;
                    set["Manager"].Value = mPropeties.Manager;
                    set["Company"].Value = mPropeties.Company;
                    set["Category"].Value = mPropeties.Category;
                    set2["Cost"].Value = mPropeties.Cost;
                    set2["Manufacturer"].Value = mPropeties.Delivery;
                    set2["Description"].Value = mPropeties.Description;
                    set2["Material"].Value = mPropeties.Material;
                    set2["Part Number"].Value = mPropeties.PartNumber;
                    this.apprenticeDoc.PropertySets.FlushToFile();
                    this.apprenticeDoc.Close();
                }
            }
        }

        public void UpDateIpropertiesAPP(string mPath, double Cost, int Delivery)
        {
            if (!System.IO.File.Exists(mPath))
            {
                Interaction.MsgBox("File kh\x00f4ng tồn tại", MsgBoxStyle.ApplicationModal, null);
            }
            else
            {
                try
                {
                    this.apprenticeDoc = this.m_Apprentice.Open(mPath);
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    Interaction.MsgBox("Lỗi !. B\x00f3 tay th\x00f4i", MsgBoxStyle.ApplicationModal, null);
                    ProjectData.ClearProjectError();
                }
                if (!ReferenceEquals(this.apprenticeDoc, null))
                {
                    PropertySet set = this.apprenticeDoc.PropertySets["Design Tracking Properties"];
                    set["Cost"].Value = Cost;
                    set["Manufacturer"].Value = Delivery;
                    this.apprenticeDoc.PropertySets.FlushToFile();
                    this.apprenticeDoc.Close();
                }
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct MyIproperties
        {
            public string FileName;
            public string DisplayName;
            public string Dir;
            public string PartNumber;
            public string Material;
            public string Description;
            public string Manager;
            public string Company;
            public string Title;
            public string Comments;
            public string Category;
            public double Cost;
            public int Delivery;
            public string Author;
            public string Subject;
            public Image thumbNail;
            public void Empty()
            {
                this.DisplayName = "";
                this.FileName = "";
                this.Dir = "";
                this.PartNumber = "";
                this.Material = "";
                this.Description = "";
                this.Manager = "";
                this.Company = "";
                this.Title = "";
                this.Comments = "";
                this.Category = "";
                this.Author = "";
                this.Subject = "";
                this.Cost = 0.0;
                this.Delivery = 0;
                this.thumbNail = null;
            }

            public static bool operator !=(CInvPartInfor.MyIproperties Val1, CInvPartInfor.MyIproperties Val2) =>
                !((((((((((((Val1.Category == Val2.Category) & (Val1.Comments == Val2.Comments)) & (Val1.Company == Val2.Company)) & (Val1.Description == Val2.Description)) & (Val1.Dir == Val2.Dir)) & (Val1.DisplayName == Val2.DisplayName)) & (Val1.FileName == Val2.FileName)) & (Val1.Material == Val2.Material)) & (Val1.Manager == Val2.Manager)) & (Val1.PartNumber == Val2.PartNumber)) & (Val1.Subject == Val2.Subject)) & (Val1.Title == Val2.Title));

            public static bool operator ==(CInvPartInfor.MyIproperties Val1, CInvPartInfor.MyIproperties Val2) =>
                ((((((((((((Val1.Category == Val2.Category) & (Val1.Comments == Val2.Comments)) & (Val1.Company == Val2.Company)) & (Val1.Description == Val2.Description)) & (Val1.Dir == Val2.Dir)) & (Val1.DisplayName == Val2.DisplayName)) & (Val1.FileName == Val2.FileName)) & (Val1.Material == Val2.Material)) & (Val1.Manager == Val2.Manager)) & (Val1.PartNumber == Val2.PartNumber)) & (Val1.Subject == Val2.Subject)) & (Val1.Title == Val2.Title));
        }
    }
}
