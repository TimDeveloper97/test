using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.SketchAttach;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTK.Business.Users;

namespace QLTK.Business.Sketches
{
    public class SketchesBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        public SketchAttachModel GetListInfo(SketchAttachModel model)
        {
            try
            {
                var listsketch = (from a in db.SketchAttaches.AsNoTracking()
                                  where a.ModuleId.Equals(model.ModuleId)
                                  select new SketchAttachModel
                                  {
                                      Id = a.Id,
                                      ModuleId = a.ModuleId,
                                      FileName = a.FileName,
                                      FileSize = a.FileSize,
                                      Path = a.Path,
                                      Note = a.Note
                                  }).ToList();
                model.ListFileSketches = listsketch;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddSketch(SketchAttachModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.ListDelete.Count > 0)
                    {
                        foreach (var item in model.ListDelete)
                        {
                            if (item.Id != "")
                            {
                                var sketchAttach = db.SketchAttaches.FirstOrDefault(u => u.Id.Equals(item.Id));
                                db.SketchAttaches.Remove(sketchAttach);
                                var sketchAttachHistories = db.SketchAttachHistories.Where(u => u.SketchAttachId.Equals(item.Id)).ToList();
                                if (sketchAttachHistories.Count > 0)
                                {
                                    db.SketchAttachHistories.RemoveRange(sketchAttachHistories);
                                }
                            }
                        }
                    }

                    if (model.ListFileSketches.Count > 0)
                    {
                        List<SketchAttach> listFileEntity = new List<SketchAttach>();
                        List<SketchAttachHistory> listFile = new List<SketchAttachHistory>();
                        foreach (var item in model.ListFileSketches)
                        {
                            if (item.Id == "" && db.SketchAttaches.AsNoTracking().Where(o => o.FileName.Equals(item.FileName)).Count() == 0)
                            {
                                SketchAttach fileEntity = new SketchAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ModuleId = model.ModuleId,
                                    FileName = item.FileName,
                                    Path = item.Path,
                                    FileSize = Convert.ToInt32(item.FileSize),
                                    Note = item.Note,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now
                                };
                                SketchAttachHistory file = new SketchAttachHistory()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SketchAttachId = fileEntity.Id,
                                    FileSize = fileEntity.FileSize,
                                    Path = fileEntity.Path,
                                    Note = fileEntity.Note,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                                listFile.Add(file);
                            }
                            else if (db.SketchAttaches.AsNoTracking().Where(o => o.FileName.Equals(item.FileName)).Count() > 0)
                            {
                                var sketchAttach = db.SketchAttaches.Where(r => r.FileName.Equals(item.FileName)).FirstOrDefault();
                                SketchAttachHistory file = new SketchAttachHistory()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SketchAttachId = sketchAttach.Id,
                                    FileSize = sketchAttach.FileSize,
                                    Path = sketchAttach.Path,
                                    Note = sketchAttach.Note,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now
                                };
                                listFile.Add(file);
                                sketchAttach.Id = sketchAttach.Id;
                                sketchAttach.ModuleId = sketchAttach.ModuleId;
                                sketchAttach.FileName = item.FileName;
                                sketchAttach.FileSize = item.FileSize;
                                sketchAttach.Path = item.Path;
                                sketchAttach.Note = item.Note;
                                sketchAttach.CreateBy = model.UpdateBy;
                                sketchAttach.CreateDate = DateTime.Now;
                            }
                        }
                        db.SketchAttaches.AddRange(listFileEntity);
                        db.SketchAttachHistories.AddRange(listFile);
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Module, model.ModuleId, string.Empty, "Phác thảo thiết kế");

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

        public SketchAttachHistoryModel GetListHistoryInfo(SketchAttachHistoryModel model)
        {
            try
            {
                var listsketch = (from a in db.SketchAttachHistories.AsNoTracking()
                                  join b in db.SketchAttaches.AsNoTracking() on a.SketchAttachId equals b.Id
                                  where a.SketchAttachId.Equals(model.Id)
                                  orderby a.CreateDate descending
                                  select new SketchAttachHistoryModel
                                  {
                                      Id = a.Id,
                                      SketchAttachId = a.SketchAttachId,
                                      FileName = b.FileName,
                                      FileSize = a.FileSize,
                                      Path = a.Path,
                                      Note = a.Note,
                                      CreateDate = a.CreateDate,
                                  }).ToList();
                model.ListHistory = listsketch;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

