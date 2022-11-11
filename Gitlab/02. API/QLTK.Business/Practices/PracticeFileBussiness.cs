using NTS.Common;
using NTS.Model.PracticeFile;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.PracticeFiles
{
    public class PracticeFileBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public PracticeFileModel GetPracticeFileInfo(PracticeFileModel model)
        {
            try
            {
                var listfile = (from a in db.PracticeFiles.AsNoTracking()
                                join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                                join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                                join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                where a.PracticeId.Equals(model.PracticeId)
                                select new PracticeFileModel
                                {
                                    Id = a.Id,
                                    PracticeId = a.PracticeId,
                                    FileName = a.FileName,
                                    FilePath = a.FilePath,
                                    Size = a.Size.ToString(),
                                    Description = a.Description,
                                    CreateBy = a.CreateBy,
                                    CreateByName = d.Name,
                                    CreateDate = a.CreateDate,
                                }).ToList();

                listfile.AddRange((from a in db.DocumentObjects.AsNoTracking()
                                   join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                   join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                                   join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                   where a.ObjectId.Equals(model.PracticeId) && a.ObjectType == Constants.ObjectType_Practice
                                   select new PracticeFileModel
                                   {
                                       Id = b.Id,
                                       PracticeId = model.PracticeId,
                                       FileName = b.Name,
                                       FilePath = b.Id,
                                       Description = b.Description,
                                       CreateBy = b.CreateBy,
                                       CreateByName = d.Name,
                                       CreateDate = b.CreateDate,
                                       IsDocument = true
                                   }).ToList());

                model.ListFile = listfile.OrderBy(i => i.FileName).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddPracticeFiles(PracticeFileModel model)
        {
            var prac = db.Practices.Where(o => o.Id.Equals(model.PracticeId)).FirstOrDefault();
            using (var trans = db.Database.BeginTransaction())
            {
                var practices = db.PracticeFiles.Where(u => u.PracticeId.Equals(model.PracticeId)).ToList();
                if (practices.Count > 0)
                {
                    db.PracticeFiles.RemoveRange(practices);
                }

                var documentObjects = db.DocumentObjects.Where(u => u.ObjectId.Equals(model.PracticeId) && u.ObjectType == Constants.ObjectType_Practice).ToList();
                if (documentObjects.Count > 0)
                {
                    db.DocumentObjects.RemoveRange(documentObjects);
                }
                try
                {
                    if (model.ListFile.Count > 0)
                    {
                        List<PracticeFile> listFileEntity = new List<PracticeFile>();
                        foreach (var item in model.ListFile)
                        {
                            if (!item.IsDocument)
                            {
                                if (item.FilePath != null && item.FilePath != "")
                                {
                                    PracticeFile fileEntity = new PracticeFile()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        PracticeId = model.PracticeId,
                                        FileName = item.FileName,
                                        FilePath = item.FilePath,
                                        Size = Convert.ToInt32(item.Size),
                                        Description = item.Description,
                                        CreateBy = model.CreateBy,
                                        CreateDate = DateTime.Now
                                    };
                                    listFileEntity.Add(fileEntity);
                                }
                            }
                            else
                            {
                                DocumentObject documentObject = new DocumentObject()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    DocumentId = item.Id,
                                    ObjectId = model.PracticeId,
                                    ObjectType = Constants.ObjectType_Practice
                                };

                                db.DocumentObjects.Add(documentObject);
                            }
                        }
                        db.PracticeFiles.AddRange(listFileEntity);
                        prac.PracticeExist = true;
                    }
                    else
                    {
                        prac.PracticeExist = false;
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Practice, model.PracticeId, string.Empty, "Tài liệu");

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }
    }
}
