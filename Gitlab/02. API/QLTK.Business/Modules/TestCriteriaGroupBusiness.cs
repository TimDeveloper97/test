using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.Repositories;
using NTS.Model.TestCriteriaGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.TestCriteriaGroups
{
    public class TestCriteriaGroupBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm nhóm tiêu chí
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public object SearchTestCriterialGroup(TestCriteriaGroupSearchModel modelSearch)
        {
            SearchResultModel<TestCriteriaGroupModel> searchResult = new SearchResultModel<TestCriteriaGroupModel>();
            try
            {
                var dataQuery = (from a in db.TestCriteriaGroups.AsNoTracking()
                                 orderby a.Code
                                 select new TestCriteriaGroupModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     Note = a.Note,
                                     CreateDate = a.CreateDate,
                                     CreateBy = a.CreateBy,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,
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
                searchResult.ListResult = dataQuery.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Xóa nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void DeleteTestCriteralGroup(TestCriteriaGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var testCriteria = db.TestCriterias.AsNoTracking().Where(m => m.TestCriteriaGroupId.Equals(model.Id)).FirstOrDefault();
                if (testCriteria != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.TestCriteriaGroup);
                }
                try
                {
                    var testCriteriaGroup = db.TestCriteriaGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (testCriteriaGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TestCriteriaGroup);
                    }
                    db.TestCriteriaGroups.Remove(testCriteriaGroup);
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
        ///  Get nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetTestCriteralGroup(TestCriteriaGroupModel model)
        {
            var resuldInfor = db.TestCriteriaGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new TestCriteriaGroupModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Note = p.Note,
            }).FirstOrDefault();
            if (resuldInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TestCriteriaGroup);
            }
            return resuldInfor;
        }

        /// <summary>
        ///  Thêm mới nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void AddTestCriteralGroup(TestCriteriaGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // Check tên nhóm tiêu chí đã tồn tại chưa
                if (db.TestCriteriaGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.TestCriteriaGroup);
                }
                // check mã nhóm tiêu chí đã tồn tại chưa
                if (db.TestCriteriaGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TestCriteriaGroup);
                }
                try
                {
                    TestCriteriaGroup newtestCriteriaGroup = new TestCriteriaGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Note = model.Note.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.TestCriteriaGroups.Add(newtestCriteriaGroup);
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
        /// Update nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void UpdateTestCriteralGroup(TestCriteriaGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.TestCriteriaGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.TestCriteriaGroup);
                }

                if (db.TestCriteriaGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TestCriteriaGroup);
                }
                try
                {
                    var newtestCri = db.TestCriteriaGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newtestCri.Name = model.Name.NTSTrim();
                    newtestCri.Note = model.Note.NTSTrim();
                    newtestCri.Code = model.Code.NTSTrim();
                    newtestCri.UpdateBy = model.UpdateBy;
                    newtestCri.UpdateDate = DateTime.Now;



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
