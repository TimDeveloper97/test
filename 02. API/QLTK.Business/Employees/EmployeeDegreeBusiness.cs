using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.EmployeeDegree;
using System;
using System.Linq;
using NTS.Common;
using NTS.Common.Resource;

namespace QLTK.Business.EmployeeDegrees
{
    public class EmployeeDegreeBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm bằng cấp
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<EmployeeDegreeResultModel> SearchModel(EmployeeDegreeSearcModel modelSearch)
        {
            SearchResultModel<EmployeeDegreeResultModel> searchResult = new SearchResultModel<EmployeeDegreeResultModel>();
            var dataQuey = (from a in db.Employees.AsNoTracking()
                            join b in db.EmployeeDegrees.AsNoTracking() on a.Id equals b.EmployeeId
                            join c in db.Qualifications.AsNoTracking() on b.QualificationId equals c.Id
                            join d in db.Classifications.AsNoTracking() on b.ClassificationId equals d.Id into bd
                            from db in bd.DefaultIfEmpty() 
                            where b.EmployeeId.Equals(modelSearch.EmployeeId)
                            orderby b.Code
                            select new EmployeeDegreeResultModel
                            {
                                Id = b.Id,
                                QualificationId = b.Id,
                                QualificationName = c.Name,
                                EmployeeId = b.Id,
                                Year = b.Year,
                                Name = b.Name,
                                Code = b.Code,
                                School = b.School,
                                Rank = b.Rank,
                                ClassIficationName = db.Name,
                                Description = b.Description
                            }).AsQueryable();
            searchResult.TotalItem = dataQuey.Count();
            var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// Xóa bằng cấp
        /// </summary>
        /// <param name="model"></param>
        public void Deletes(EmployeeDegreeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Delete = db.EmployeeDegrees.Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
                    if (Delete != null)
                    {

                        db.EmployeeDegrees.Remove(Delete);
                        db.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeDegrees);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
                //luu Log lich su
                string decription = "Xóa bằng cấp tên là: " + model.Name;
                LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
            }
        }

        /// <summary>
        /// Thêm mới bằng cấp
        /// </summary>
        /// <param name="model"></param>
        public void Adds(EmployeeDegreeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // Check tên 
                if (db.EmployeeDegrees.AsNoTracking().Where(o => o.Name.Equals(model.Name) && o.EmployeeId.Equals(model.EmployeeId)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.EmployeeDegrees);
                }

                // check mã 
                if (db.EmployeeDegrees.AsNoTracking().Where(o => o.Code.Equals(model.Code) && o.EmployeeId.Equals(model.EmployeeId)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.EmployeeDegrees);
                }
                try
                {
                    EmployeeDegree newEmployeeDegree = new EmployeeDegree()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = model.EmployeeId,
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        School = model.School.NTSTrim(),
                        Rank = model.Rank.NTSTrim(),
                        Year = model.Year,
                        ClassificationId = model.ClassIficationId,
                        QualificationId = model.QualificationId,
                        Description = model.Description,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.EmployeeDegrees.Add(newEmployeeDegree);
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

        /// <summary>
        /// Get bằng cấp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EmployeeDegreeModel GetInfos(EmployeeDegreeModel model)
        {
            var employeedegree = db.EmployeeDegrees.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
            if (employeedegree == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeDegrees);
            }
            try
            {
                var resuldInfor = db.EmployeeDegrees.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new EmployeeDegreeModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    School = p.School,
                    Rank = p.Rank,
                    Year = p.Year,
                    ClassIficationId = p.ClassificationId,
                    QualificationId = p.QualificationId,
                    Description = p.Description
                }).FirstOrDefault();

                return resuldInfor;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Update bằng cấp
        /// </summary>
        /// <param name="model"></param>
        public void Updates(EmployeeDegreeModel model)
        {
            string NameOld = "";
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.EmployeeDegrees.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.EmployeeDegrees);
                }
                if (db.EmployeeDegrees.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Code.Equals(o.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.EmployeeDegrees);
                }
                try
                {
                    var groupEdit = db.EmployeeDegrees.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
                    NameOld = groupEdit.Name.NTSTrim();
                    groupEdit.Code = model.Code.NTSTrim();
                    groupEdit.QualificationId = model.QualificationId.NTSTrim();
                    groupEdit.Name = model.Name.NTSTrim();
                    groupEdit.Year = model.Year;
                    groupEdit.Rank = model.Rank.NTSTrim();
                    groupEdit.School = model.School.NTSTrim();
                    groupEdit.Description = model.Description.NTSTrim();
                    groupEdit.UpdateBy = model.CreateBy;
                    groupEdit.ClassificationId = model.ClassIficationId;
                    groupEdit.UpdateDate = DateTime.Now;
                    groupEdit.CreateBy = model.CreateBy;
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            try
            {
                string decription = String.Empty;
                if (NameOld.ToLower() == model.Name.ToLower())
                {
                    decription = "Cập nhật bằng cấp tên là: " + NameOld;
                }
                else
                {
                    decription = "Cập nhật bằng cấp có tên ban đầu là:  " + NameOld + " thành " + model.Name; ;
                }
                LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
            }
            catch (Exception) { }
        }
    }
}
