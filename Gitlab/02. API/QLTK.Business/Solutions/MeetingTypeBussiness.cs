using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.MeetingType;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Solutions
{
    public class MeetingTypeBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<MeetingTypeModel> Search()
        {
            SearchResultModel<MeetingTypeModel> searchResult = new SearchResultModel<MeetingTypeModel>();

            var dataQuery = (from a in db.MeetingTypes.AsNoTracking()
                             orderby a.Code
                             select new MeetingTypeModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Code = a.Code,
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void CreateMeetingType(MeetingTypeModel model, string userId)
        {
            if (db.MeetingTypes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.MeetingType);
            }

            if (db.MeetingTypes.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.MeetingType);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    MeetingType meetingType = new MeetingType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        ParentId = model.ParentId,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };

                    db.MeetingTypes.Add(meetingType);
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
        public void UpdateMeetingType(MeetingTypeModel model, string userId)
        {
            if (model.Code.Count()>1)
            {
                throw NTSException.CreateInstance("Mã chủng loại không được nhiều hơn 1 ký tự");
            }
            if (db.MeetingTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.MeetingType);
            }

            if (db.MeetingTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.MeetingType);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var meetingType = db.MeetingTypes.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    meetingType.Name = model.Name.NTSTrim();
                    meetingType.Code = model.Code.NTSTrim();
                    meetingType.ParentId = model.ParentId;
                    meetingType.UpdateBy = userId;
                    meetingType.UpdateDate = DateTime.Now;
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

        public void DeleteMeetingType(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var meeting = db.Meetings.AsNoTracking().Where(m => m.MeetingTypeId.Equals(id)).FirstOrDefault();
                if (meeting != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MeetingType);
                }

                var listChild = db.MeetingTypes.Where(t => t.ParentId.Equals(id)).ToList();
                if(listChild.Count > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MeetingType);
                }    

                try
                {
                    var meetingType = db.MeetingTypes.FirstOrDefault(u => u.Id.Equals(id));
                    if (meetingType == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MeetingType);
                    }

                    db.MeetingTypes.Remove(meetingType);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(id, ex);
                }
            }
        }
        public MeetingTypeModel GetById(string id)
        {
            var resultInfo = db.MeetingTypes.AsNoTracking().Where(u => id.Equals(u.Id)).Select(p => new MeetingTypeModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MeetingType);
            }

            return resultInfo;
        }
    }
}
