using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.CustomerContact;
using NTS.Model.Customers;
using NTS.Model.DocumentGroups;
using NTS.Model.FlowStage;
using NTS.Model.Product;
using NTS.Model.ProductGroup;
using NTS.Model.ProjectProducts;
using NTS.Model.QLTKGROUPMODUL;
using NTS.Model.QLTKMG;
using NTS.Model.QLTKMODULE;
using NTS.Model.QuestionGroup;
using NTS.Model.Repositories;
using NTS.Model.SalaryLevel;
using NTS.Model.ProjectPhase;
using System;
using System.Collections.Generic;
using System.Linq;
using NTS.Model.Recruitments.Interviews;
using NTS.Model.Employees;
using NTS.Model.GroupJob;
using NTS.Model.GroupSupplier;
using NTS.Model.GroupManufacture;
using NTS.Model.Quotation;

namespace NTS.Business.Combobox
{
    public class AttachmentBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public void CreatFile(List<AttachmentModel> model, string CreateBy, string Id)
        {
            var dateNow = DateTime.Now;

            if (model.Count() > 0)
            {
                
                foreach (var item in model)
                {
                    if (item.FilePath != null)
                    {
                        Attachment attachment = new Attachment()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreateBy = CreateBy,
                            CreateDate = dateNow,
                            UpdateDate = dateNow,
                            UpdateBy = CreateBy,
                            ObjectId = Id,
                            Name = item.Name,
                            FilePath = item.FilePath,
                            FileName = item.FileName,
                            Size = item.Size.Value,
                            Extention = item.Extention,
                            Thumbnail = item.Thumbnail,
                            HashValue = item.HashValue,
                            Description = item.Description,
                        };
                        db.Attachments.Add(attachment);
                    }

                }
                
            }
            db.SaveChanges();
        }


    }
}
