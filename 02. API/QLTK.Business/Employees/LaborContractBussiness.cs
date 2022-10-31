using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.LaborContract;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.LaborContract
{
    public class LaborContractBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        /// <summary>
        /// Tìm kiếm hợp đồng lao động
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<LaborContractSearchResultModel> SearchLaborContract(LaborContractSearchModel searchModel)
        {
            SearchResultModel<LaborContractSearchResultModel> searchResult = new SearchResultModel<LaborContractSearchResultModel>();
            var dataQuery = (from a in db.LaborContracts.AsNoTracking()
                             select new LaborContractSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Type = a.Type,
                                 Note = a.Note
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (searchModel.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == searchModel.Type);
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        public void DeleteLaborContract(LaborContractModel model)
        {
            var LaborContractExist = db.LaborContracts.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (LaborContractExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.LaborContract);
            }

            var LaborContractUsed = db.EmployeeContracts.AsNoTracking().FirstOrDefault(a => a.LaborContractId.Equals(model.Id));
            if (LaborContractUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.LaborContract);
            }

            try
            {
                db.LaborContracts.Remove(LaborContractExist);

                var NameOrCode = LaborContractExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<LaborContractHistoryModel>(LaborContractExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_LaborContract, LaborContractExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        public void CreateLaborContract(LaborContractModel model)
        {
            model.Name = model.Name.NTSTrim();
            var LaborContractExits = db.LaborContracts.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (LaborContractExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.LaborContract);
            }

            try
            {
                NTS.Model.Repositories.LaborContract LaborContract = new NTS.Model.Repositories.LaborContract
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Type = model.Type,
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.LaborContracts.Add(LaborContract);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, LaborContract.Name, LaborContract.Id, Constants.LOG_LaborContract);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LaborContractModel GetLaborContract(LaborContractModel model)
        {
            var resultInfo = db.LaborContracts.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new LaborContractModel
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.LaborContract);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        public void UpdateLaborContract(LaborContractModel model)
        {
            model.Name = model.Name.NTSTrim();
            var LaborContractUpdate = db.LaborContracts.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (LaborContractUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.LaborContract);
            }

            var LaborContractNameExist = db.LaborContracts.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (LaborContractNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.LaborContract);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<LaborContractHistoryModel>(LaborContractUpdate);

            try
            {
                LaborContractUpdate.Name = model.Name;
                LaborContractUpdate.Type = model.Type;
                LaborContractUpdate.Note = model.Note;
                LaborContractUpdate.UpdateBy = model.UpdateBy;
                LaborContractUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<LaborContractHistoryModel>(LaborContractUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_LaborContract, LaborContractUpdate.Id, LaborContractUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
