using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;
using NTS.Model.ModuleRoomType;
using NTS.Utils;
using NTS.Common;
using NTS.Common.Resource;


namespace QLTK.Business.RoomType
{
    public class RoomType
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<RoomTypeResultModel> SearchRoomType(RoomTypeSearchModel modelSearch)
        {
            SearchResultModel<RoomTypeResultModel> searchResult = new SearchResultModel<RoomTypeResultModel>();

            var dataQuery = (from a in db.RoomTypes.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new RoomTypeResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
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
            return searchResult;
        }


        public object GetRoomTypeInfo(RoomTypeModel model)
        {
            var resultInfo = db.RoomTypes.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new RoomTypeModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RoomType);
            }

            return resultInfo;
        }

        public void AddRoomType(RoomTypeModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.RoomType newRoomType = new NTS.Model.Repositories.RoomType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        Description = model.Description,
                    };

                    db.RoomTypes.Add(newRoomType);
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

        public void UpdateRoomType(RoomTypeModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newRoomType = db.RoomTypes.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    newRoomType.Name = model.Name;
                    newRoomType.Code = model.Code;
                    newRoomType.Description = model.Description;
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

        public void deleteRoomType(RoomTypeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var roomType = db.ClassRooms.AsNoTracking().Where(u => u.RoomTypeId.Equals(model.Id)).FirstOrDefault();

                if (roomType != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.RoomType);
                }

                try
                {
                    var _roomType = db.RoomTypes.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_roomType == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RoomType);
                    }

                    db.RoomTypes.Remove(_roomType);
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

        private void CheckExistedForAdd(RoomTypeModel model)
        {
            if (db.RoomTypes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.RoomType);
            }

            if (db.RoomTypes.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.RoomType);
            }
        }

        public void CheckExistedForUpdate(RoomTypeModel model)
        {
            if (db.RoomTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.RoomType);
            }

            if (db.RoomTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.RoomType);
            }

        }
    }
}
