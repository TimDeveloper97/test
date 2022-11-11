using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.JobGroup;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.JobGroups
{
    public class JobGroupBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm nhóm việc làm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public object SearchJobGroup(JobGroupSearchModel modelSearch)
        {
            SearchResultModel<JobGroupModel> searchResult = new SearchResultModel<JobGroupModel>();
            try
            {
                
                var dataQuery = (from a in db.JobGroups.AsNoTracking()
                                 orderby a.Code
                                 select new JobGroupModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     Description = a.Description,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }
                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
                }
                searchResult.TotalItem = dataQuery.Count();
                var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Xóa nhóm việc làm
        /// </summary>
        /// <param name="model"></param>
        public void DeleteJobGroup(JobGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var jobGroups = db.Jobs.AsNoTracking().Where(m => m.JobGroupId.Equals(model.Id)).FirstOrDefault();
                if (jobGroups != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.JobGroup);
                }
                try
                {
                    var jobGroup = db.JobGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (jobGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.JobGroup);
                    }
                    db.JobGroups.Remove(jobGroup);
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
        ///  lấy dữ liệu nhóm việc làm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetJobGroup(JobGroupModel model)
        {
            var resuldInfor = db.JobGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new JobGroupModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                CreateDate = p.CreateDate,
                CreateBy = p.CreateBy,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate,
            }).FirstOrDefault();
            if (resuldInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.JobGroup);
            }
            return resuldInfor;
        }

        /// <summary>
        ///  Thêm mới nhóm việc làm
        /// </summary>
        /// <param name="model"></param>
        public void AddJobGroup(JobGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // Check tên nhóm việc làm đã tồn tại chưa
                if (db.JobGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.JobGroup);
                }
                // check mã nhóm việc làm đã tồn tại chưa
                if (db.JobGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.JobGroup);
                }
                try
                {
                    JobGroup newJobGroup = new JobGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.JobGroups.Add(newJobGroup);
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
        /// Update nhóm việc làm
        /// </summary>
        /// <param name="model"></param>
        public void UpdateJobGroup(JobGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.JobGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.JobGroup);
                }

                if (db.JobGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.JobGroup);
                }
                try
                {
                    var newJobGroup = db.JobGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newJobGroup.Name = model.Name.NTSTrim();
                    newJobGroup.Description = model.Description.NTSTrim();
                    newJobGroup.Code = model.Code.NTSTrim();
                    newJobGroup.UpdateBy = model.UpdateBy;
                    newJobGroup.UpdateDate = DateTime.Now;

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
