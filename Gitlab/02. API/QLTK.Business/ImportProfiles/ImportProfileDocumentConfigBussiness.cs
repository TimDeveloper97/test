using NTS.Common;
using NTS.Model.HistoryVersion;
using NTS.Model.Holiday;
using NTS.Model.ImportProfileDocumentConfigs;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ImportProfiles
{
    public class ImportProfileDocumentConfigBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public List<ImportProfileDocumentConfigModel> GetDocumentConfigByType(int step)
        {
            var documents = (from d in db.ImportProfileDocumentConfigs.AsNoTracking()
                             where d.Step == step
                             select new ImportProfileDocumentConfigModel
                             {
                                 IsRequired = d.IsRequired,
                                 Step = d.Step,
                                 Name = d.Name,
                                 Note = d.Note
                             }).ToList();

            return documents;
        }

        public void UpdateDocumentConfig(ImportProfileDocumentConfigUpdateModel documentConfig, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var documents = db.ImportProfileDocumentConfigs.Where(r => r.Step == documentConfig.Step);
                    db.ImportProfileDocumentConfigs.RemoveRange(documents);

                    foreach (var item in documentConfig.Documents)
                    {
                        if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Name.Trim()))
                        {
                            db.ImportProfileDocumentConfigs.Add(new ImportProfileDocumentConfig
                            {
                                Id = Guid.NewGuid().ToString(),
                                IsRequired = item.IsRequired,
                                Name = item.Name,
                                Note = item.Note,
                                Step = documentConfig.Step,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            });
                        }
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_ImportProfileDocumentConfig, documentConfig.Step.ToString(), GetTyName(documentConfig.Step), "Cập nhật");

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(documentConfig, ex);
                }
            }
        }


        private string GetTyName(int type)
        {
            switch (type)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    return "Xác định nhà cung cấp";
                case Constants.ImportProfile_Step_Contract:
                    return "Làm hợp đồng";
                case Constants.ImportProfile_Step_Customs:
                    return "Thủ tục thông quan";
                case Constants.ImportProfile_Step_Import:
                    return "Nhập kho";
                case Constants.ImportProfile_Step_Payment:
                    return "Thanh toán";
                case Constants.ImportProfile_Step_Production:
                    return "Theo dõi tiến độ sản xuất";
                case Constants.ImportProfile_Step_Transport:
                    return "Lựa chọn nhà vận chuyển";
                default:
                    return string.Empty;
            }
        }
    }
}
