using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.QLTKMP;
using NTS.Model.Repositories;
using NTS.Model.Combobox;

namespace QLTK.Business.QLTKMP
{
    public class QLTKMPBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public SearchResultModel<QLTKMPResultModel> SearchMaterialParameter(QLTKMPSearchModel modelSearch)
        {
            SearchResultModel<QLTKMPResultModel> searchResult = new SearchResultModel<QLTKMPResultModel>();
            try
            {
                var dataQuery = (from a in db.MaterialParameters.AsNoTracking()
                                 join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id
                                 where c.Id.Equals(modelSearch.Id)
                                 orderby a.Name
                                 select new QLTKMPResultModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name
                                 }).AsQueryable();

                //checks conditions
                //if (!string.IsNullOrEmpty(modelSearch.Name))
                //{
                //    dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                //}
                //if (!string.IsNullOrEmpty(modelSearch.Code))
                //{
                //    dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(modelSearch.Code));
                //}
                //if (!string.IsNullOrEmpty(modelSearch.PhoneNumber))
                //{
                //    dataQuery = dataQuery.Where(r => r.PhoneNumber.ToUpper().Contains(modelSearch.PhoneNumber));
                //}
                //if (!string.IsNullOrEmpty(modelSearch.JobPositionId))
                //{
                //    dataQuery = dataQuery.Where(r => r.JobPositionId.Equals(modelSearch.JobPositionId));
                //}
                //if (modelSearch.Gender != null)
                //{
                //    dataQuery = dataQuery.Where(r => r.Gender == modelSearch.Gender);
                //}
                //searchResult.TotalItem = dataQuery.Count();
                searchResult.ListResult = dataQuery.ToList();
                searchResult.TotalItem = searchResult.ListResult.Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        public void DeleteMaterialGroup(QLTKMPModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var materialParameter = db.MaterialParameters.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (materialParameter == null)
                    {
                        throw new Exception("Nhóm vật tư không tồn tại");
                    }

                    var materialParameterValueList = db.MaterialParameterValues.Where(u => u.MaterialParameterId.Equals(model.Id)).ToList();
                    if (materialParameterValueList != null && materialParameterValueList.Count() > 0)
                    {
                        db.MaterialParameterValues.RemoveRange(materialParameterValueList);
                    }

                    db.MaterialParameters.Remove(materialParameter);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
                }
            }
        }

        public void AddMaterialParameter(QLTKMPModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    MaterialParameter newMaterialParameter = new MaterialParameter
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Description = model.Description,
                        MaterialGroupId = model.MaterialGroupId,
                        Index = model.Index,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };
                    db.MaterialParameters.Add(newMaterialParameter);

                    if (model.MaterialParameterValueList != null)
                    {
                        foreach (var value in model.MaterialParameterValueList)
                        {
                            MaterialParameterValue newMaterialParameterValue = new MaterialParameterValue
                            {
                                Id = Guid.NewGuid().ToString(),
                                Value = value,
                                MaterialParameterId = newMaterialParameter.Id,
                                Index = model.Index,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now
                            };
                            db.MaterialParameterValues.Add(newMaterialParameterValue);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }
        }

        public void UpdateMaterialParameter(QLTKMPModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //edit
                    var newMaterialParameter = db.MaterialParameters.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newMaterialParameter.Name = model.Name;
                    newMaterialParameter.UpdateBy = model.CreateBy;
                    newMaterialParameter.UpdateDate = DateTime.Now;
                    //save
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {

                    //error
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }
        }
    }
}
