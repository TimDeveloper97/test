using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.CustomerRequirement;
using NTS.Model.Meeting;
using NTS.Model.MeetingAttach;
using NTS.Model.MeetingContent;
using NTS.Model.MeetingCustomerContact;
using NTS.Model.MeetingEmployee;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Solutions
{
    public class MeetingBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly CustomerRequirementBussiness _customerRequireBussiness = new CustomerRequirementBussiness();

        public SearchResultModel<MeetingSearchResultModel> SearchMeeting(MeetingSearchModel modelSearch)
        {
            SearchResultModel<MeetingSearchResultModel> searchResult = new SearchResultModel<MeetingSearchResultModel>();
            List<MeetingSearchResultModel> listResult = new List<MeetingSearchResultModel>();

            var dataQuery = (from a in db.Meetings.AsNoTracking()
                             join b in db.MeetingTypes.AsNoTracking() on a.MeetingTypeId equals b.Id
                             join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id into ac
                             from acx in ac.DefaultIfEmpty()
                             join d in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals d.Id into ad
                             from adx in ad.DefaultIfEmpty()
                             join user in db.Users on a.CreateBy equals user.Id
                             join emp in db.Employees on user.EmployeeId equals emp.Id
                             select new MeetingSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Type = a.Type,
                                 MeetingTypeId = b.Id,
                                 MeetingTypeName = b.Name,
                                 Address = a.Address,
                                 Description = a.Description,
                                 CustomerCode = acx != null ? acx.Code : string.Empty,
                                 CustomerName = acx != null ? acx.Name : string.Empty,
                                 CustomerContactName = adx != null ? adx.Name : string.Empty,
                                 Email = adx != null ? adx.Email : string.Empty,
                                 PhoneNumber = adx != null ? adx.PhoneNumber : string.Empty,
                                 StrStartTime = a.StartTime,
                                 StrEndTime = a.EndTime,
                                 StrRealStartTime = a.RealStartTime,
                                 StrRealEndTime = a.RealEndTime,
                                 Time = a.Time,
                                 Status = a.Status,
                                 Step = a.Step,
                                 CreateDate = a.CreateDate,
                                 MeetingDate = a.MeetingDate,
                                 Request = a.Request,
                                 RequestDate = a.RequestDate,
                                 CreateByName = emp.Name,
                                 EmployeeId = emp.Id,
                                 CreateBy = a.CreateBy,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.MeetingTypeId))
            {
                dataQuery = dataQuery.Where(u => u.MeetingTypeId.Equals(modelSearch.MeetingTypeId));
            }

            if (modelSearch.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == modelSearch.Type.Value);
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            // Tìm kiếm theo người tạo
            if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => u.EmployeeId.Equals(modelSearch.EmployeeId));
            }

            // Tìm kiếm theo Tên khách hàng
            if (!string.IsNullOrEmpty(modelSearch.Customer))
            {
                dataQuery = dataQuery.Where(u => u.CustomerName.ToUpper().Contains(modelSearch.Customer.ToUpper()));
            }

            // Tìm kiếm theo Nội dung yêu cầu
            if (!string.IsNullOrEmpty(modelSearch.Request))
            {
                dataQuery = dataQuery.Where(u => u.Request.ToUpper().Contains(modelSearch.Request.ToUpper()));
            }

            // Tìm kiếm thời gian Meeting từ ngày
            if (modelSearch.StartDate.HasValue)
            {
                var startDate = DateTimeUtils.ConvertDateFrom(modelSearch.StartDate.Value);
                dataQuery = dataQuery.Where(u => u.MeetingDate.HasValue && u.MeetingDate.Value > startDate);
            }

            // Tìm kiếm thời gian Meeting đến ngày
            if (modelSearch.EndDate.HasValue)
            {
                var endDate = DateTimeUtils.ConvertDateTo(modelSearch.EndDate.Value);
                dataQuery = dataQuery.Where(u => u.MeetingDate.HasValue && u.MeetingDate.Value < endDate);
            }

            if (modelSearch.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == modelSearch.Status.Value);
            }

            List<string> meetingIds;
            List<string> createByIds = (from r in db.Employees
                                        join u in db.Users on r.Id equals u.EmployeeId
                                        where r.DepartmentId.Equals(modelSearch.DepartmentId)
                                        select u.Id).ToList();


            // Trường hợp là Ban giám đốc thì có thể xem tất cả các Cuộc họp trong công ty
            if (!modelSearch.IsBGĐ)
            {
                // Trường hợp là Trưởng phòng thì xem được của phòng ban mình
                if (!modelSearch.IsTrPhong)
                {
                    // Lấy danh sách các Meeting có mình tham gia
                    meetingIds = db.MeetingUsers.Where(t => t.UserId.Equals(modelSearch.UserId)).Select(t => t.MeetingId).ToList();

                    // Chỉ lấy những Meeting do mình tạo, hoặc có mình tham gia
                    dataQuery = dataQuery.Where(t => t.CreateBy.Equals(modelSearch.UserId) || meetingIds.Contains(t.Id));
                }
                else
                {
                    // Lấy danh sách các Meeting có mình tham gia
                    meetingIds = db.MeetingUsers.Where(t => t.UserId.Equals(modelSearch.UserId)).Select(t => t.MeetingId).ToList();

                    // Chỉ lấy những Meeting do mình tạo, hoặc có mình tham gia, hoặc do nhân viên của mình tạo
                    dataQuery = dataQuery.Where(t => t.CreateBy.Equals(modelSearch.UserId) || meetingIds.Contains(t.Id) || createByIds.Contains(t.CreateBy));
                }
            }

            searchResult.TotalItem = dataQuery.Count();

            // Thực hiện sắp xếp kết quả
            if (modelSearch.Status.HasValue)
            {
                if (modelSearch.Status.Value == Constants.Meeting_Status_NoPlan)
                {
                    // sắp sếp theo ngày KH liên hệ, từ to đến bé
                    listResult = dataQuery.OrderByDescending(t => t.RequestDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                }
                else if (modelSearch.Status.Value == Constants.Meeting_Status_HasPlan)
                {
                    // sắp xếp theo Ngày meeting, từ to đến bé
                    listResult = dataQuery.OrderByDescending(t => t.MeetingDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                }
                else
                {
                    listResult = dataQuery.OrderByDescending(t => t.CreateDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                }
            }
            else
            {
                listResult = dataQuery.OrderByDescending(t => t.CreateDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }

            if (listResult.Count > 0)
            {
                foreach (var item in listResult)
                {
                    item.StartTime = JsonConvert.DeserializeObject<object>(item.StrStartTime);
                    item.EndTime = JsonConvert.DeserializeObject<object>(item.StrEndTime);

                    if (!string.IsNullOrEmpty(item.StrRealStartTime))
                    {
                        item.RealStartTime = JsonConvert.DeserializeObject<object>(item.StrRealStartTime);
                    }

                    if (!string.IsNullOrEmpty(item.StrRealEndTime))
                    {
                        item.RealEndTime = JsonConvert.DeserializeObject<object>(item.StrRealEndTime);
                    }
                }
            }

            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void DeleteMeetingRequimentNeedHandle(MeetingContentModel model)
        {
            var meetingContent = db.MeetingContents.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (meetingContent != null)
            {
                var meetingContentAttachs = db.MeetingContentAttaches.Where(a => a.MeetingContentId.Equals(model.Id));
                if (meetingContentAttachs != null)
                {
                    db.MeetingContentAttaches.RemoveRange(meetingContentAttachs);
                }
                db.MeetingContents.Remove(meetingContent);
            }
            db.SaveChanges();
        }

        public void CreateCustomerRequimentMeetingContent(CustomerRequirementCreateModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(model.MeetingId));
                if (meeting == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
                }
                try
                {
                    string code = "";
                    var maxIndex = db.CustomerRequirements.AsNoTracking().Select(r => r.Index).DefaultIfEmpty(0).Max();
                    maxIndex++;
                    code = $"YC.{string.Format("{0:0000}", maxIndex)}";
                    CustomerRequirement customerRequirement = new CustomerRequirement
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerId = model.CustomerId,
                        CustomerContactId = model.CustomerContactId,
                        MeetingCode = model.MeetingCode,
                        Name = model.Name,
                        Code = code,
                        RequestType = model.RequestType,
                        Petitioner = model.Petitioner,
                        DepartmentRequest = model.DepartmentRequest,
                        Reciever = model.Reciever,
                        DepartmentReceive = model.DepartmentReceive,
                        RealFinishDate = model.RealFinishDate,
                        RequestSource = model.RequestSource,
                        Status = model.Status,
                        Note = model.Note,
                        Index = maxIndex,
                        Version = model.Version,
                        Budget = model.Budget,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        ProjectPhaseId = model.ProjectPhaseId,
                        Competitor = model.Competitor,
                        CustomerSupplier = model.CustomerSupplier,
                        PriorityLevel = model.PriorityLevel,
                        PlanFinishDate = model.PlanFinishDate,
                        StartDate = model.StartDate,
                        Duration = model.Duration,
                        //CodeChar = model.CodeChar 
                    };
                    db.CustomerRequirements.Add(customerRequirement);
                foreach(var item in model.ListContent)
                {
                    var customerRequirementContent = db.CustomerRequirementContents.FirstOrDefault(a=>a.MeetingContentId.Equals(item.Id));
                    if(customerRequirementContent== null)
                    {
                        CustomerRequirementContent content = new CustomerRequirementContent();
                        content.Id = Guid.NewGuid().ToString();
                        content.Code = customerRequirement.Code;
                        content.CustomerRequirementId = customerRequirement.Id;
                        content.MeetingContentId = item.Id;
                        content.FinishDate = item.FinishDate;
                        content.RequestBy = item.RequestBy;
                        content.Request = item.Request;
                        content.Solution = item.Solution;
                        content.Note = item.Note;
                        content.CreateDate = item.CreateDate;
                        db.CustomerRequirementContents.Add(content);

                        foreach (var itemAttach in item.MeetingContentAttaches)
                        {
                            CustomerRequirementAttach attach = new CustomerRequirementAttach();
                            attach.Id = Guid.NewGuid().ToString();
                            attach.CustomerRequirementId = customerRequirement.Id;
                            attach.FileName = itemAttach.FileName;
                            attach.FilePath = itemAttach.FilePath;
                            attach.CreateDate = itemAttach.CreateDate;
                            attach.FileSize = itemAttach.FileSize;
                            attach.CreateBy = model.CreateBy;
                            attach.Name = model.Name;
                            db.CustomerRequirementAttaches.Add(attach);
                        }
                    }
                    
                }
                meeting.Step = Constants.Meeting_Step_Finish;
                meeting.Status = Constants.Meeting_Status_Finish;
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

        public MeetingInfoModel GetRequimentContent(string id)
        {
            MeetingInfoModel result = new MeetingInfoModel();

            result.ListContent = (from a in db.MeetingContents.AsNoTracking()
                                      //join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                  where a.MeetingId.Equals(id)
                                  orderby a.Request
                                  select new MeetingContentModel
                                  {
                                      Id = a.Id,
                                      MeetingId = a.MeetingId,
                                      Request = a.Request,
                                      Solution = a.Solution,
                                      FinishDate = a.FinishDate,
                                      Note = a.Note,
                                      CreateBy = a.RequestBy,
                                      CreateDate = a.CreateDate,
                                      EmployeeName = db.CustomerContacts.FirstOrDefault(m => m.Id.Equals(a.RequestBy)).Name,
                                      Code = a.Code,
                                      Checked = db.CustomerRequirementContents.Where(r => r.MeetingContentId.Equals(a.Id)).Any(),

                                  }).ToList();
            foreach (var item in result.ListContent)
            {
                item.MeetingContentAttaches = (from a in db.MeetingContentAttaches.AsNoTracking()
                                               join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                               where a.MeetingId.Equals(id) && a.MeetingContentId.Equals(item.Id)
                                               select new MeetingContentAttachModel
                                               {
                                                   Id = a.Id,
                                                   MeetingId = a.MeetingId,
                                                   MeetingContentId = item.Id,
                                                   Name = a.Name,
                                                   Note = a.Note,
                                                   FileName = a.FileName,
                                                   FilePath = a.FilePath,
                                                   FileSize = a.FileSize,
                                                   CreateDate = a.CreateDate,
                                                   CreateName = b.UserName,
                                                   IsDelete = false
                                               }).ToList();
                List<string> fileNames = new List<string>();
                foreach (var file in item.MeetingContentAttaches)
                {
                    fileNames.Add(file.FileName);
                }
                item.NameFiles = String.Join(", ", fileNames.ToArray());
            }
            return result;
        }

        public void UpdateMeetingFileRequimentNeedHandle(MeetingContentAttachModel model, string userId)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                MeetingContentAttach meetingContentAttach = new MeetingContentAttach();
                meetingContentAttach.Id = Guid.NewGuid().ToString();
                meetingContentAttach.MeetingId = model.MeetingId;
                meetingContentAttach.MeetingContentId = model.MeetingContentId;
                meetingContentAttach.Name = model.Name;
                meetingContentAttach.Note = model.Note;
                meetingContentAttach.FilePath = model.FilePath;
                meetingContentAttach.FileName = model.FileName;
                meetingContentAttach.FileSize = model.FileSize;
                meetingContentAttach.CreateBy = userId;
                meetingContentAttach.CreateDate = DateTime.Now;
                meetingContentAttach.UpdateBy = userId;
                meetingContentAttach.UpdateDate = DateTime.Now;
                meetingContentAttach.Type = 0;
                db.MeetingContentAttaches.Add(meetingContentAttach);
            }
            else
            {
                var meetingContentAttach = db.MeetingContentAttaches.FirstOrDefault(a => a.Id.Equals(model.Id));
                meetingContentAttach.Name = model.Name;
                meetingContentAttach.Note = model.Note;
                meetingContentAttach.FilePath = model.FilePath;
                meetingContentAttach.FileName = model.FileName;
                meetingContentAttach.FileSize = model.FileSize;
                meetingContentAttach.CreateBy = userId;
                meetingContentAttach.CreateDate = DateTime.Now;
                meetingContentAttach.UpdateBy = userId;
                meetingContentAttach.UpdateDate = DateTime.Now;
                meetingContentAttach.Type = 0;
            }
            db.SaveChanges();
        }

        public void DeleteMeetingFileRequimentNeedHandle(MeetingContentAttachModel model, string userId)
        {
            var meetingContentAttach = db.MeetingContentAttaches.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (meetingContentAttach != null)
            {
                db.MeetingContentAttaches.Remove(meetingContentAttach);
            }
            db.SaveChanges();
        }

        public void UpdateMeetingRequimentNeedHandle(MeetingContentModel model, string usserId)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                MeetingContent meetingContent = new MeetingContent();
                meetingContent.Id = Guid.NewGuid().ToString();
                meetingContent.MeetingId = model.MeetingId;
                meetingContent.Request = model.Request;
                meetingContent.Solution = model.Solution;
                meetingContent.FinishDate = model.FinishDate;
                meetingContent.Note = model.Note;
                meetingContent.CreateBy = usserId;
                meetingContent.CreateDate = DateTime.Now;
                meetingContent.Code = model.Code;
                meetingContent.RequestBy = model.CreateBy;
                if (model.MeetingContentAttaches.Count() > 0)
                {
                    foreach (var item in model.MeetingContentAttaches)
                    {
                        MeetingContentAttach meetingContentAttach = new MeetingContentAttach();
                        meetingContentAttach.Id = Guid.NewGuid().ToString();
                        meetingContentAttach.MeetingId = model.MeetingId;
                        meetingContentAttach.MeetingContentId = meetingContent.Id;
                        meetingContentAttach.Name = item.Name;
                        meetingContentAttach.Note = item.Note;
                        meetingContentAttach.FilePath = item.FilePath;
                        meetingContentAttach.FileName = item.FileName;
                        meetingContentAttach.FileSize = item.FileSize;
                        meetingContentAttach.CreateBy = usserId;
                        meetingContentAttach.CreateDate = DateTime.Now;
                        meetingContentAttach.UpdateBy = usserId;
                        meetingContentAttach.UpdateDate = DateTime.Now;
                        meetingContentAttach.Type = 0;
                        db.MeetingContentAttaches.Add(meetingContentAttach);
                    }
                }
                db.MeetingContents.Add(meetingContent);
            }
            else
            {
                var meetingContent = db.MeetingContents.FirstOrDefault(a => a.Id.Equals(model.Id));
                meetingContent.MeetingId = model.MeetingId;
                meetingContent.Request = model.Request;
                meetingContent.Solution = model.Solution;
                meetingContent.FinishDate = model.FinishDate;
                meetingContent.Note = model.Note;
                meetingContent.Code = model.Code;
                meetingContent.RequestBy = model.CreateBy;
                //meetingContent.CreateDate = (DateTime)model.CreateDate;

                if (model.MeetingContentAttaches.Count() > 0)
                {
                    foreach (var item in model.MeetingContentAttaches)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            MeetingContentAttach meetingContentAttach = new MeetingContentAttach();
                            meetingContentAttach.Id = Guid.NewGuid().ToString();
                            meetingContentAttach.MeetingId = model.MeetingId;
                            meetingContentAttach.MeetingContentId = meetingContent.Id;
                            meetingContentAttach.Name = item.Name;
                            meetingContentAttach.Note = item.Note;
                            meetingContentAttach.FilePath = item.FilePath;
                            meetingContentAttach.FileName = item.FileName;
                            meetingContentAttach.FileSize = item.FileSize;
                            meetingContentAttach.CreateBy = usserId;
                            meetingContentAttach.CreateDate = DateTime.Now;
                            meetingContentAttach.UpdateBy = usserId;
                            meetingContentAttach.UpdateDate = DateTime.Now;
                            meetingContentAttach.Type = 0;
                            db.MeetingContentAttaches.Add(meetingContentAttach);
                        }
                    }
                }
            }
            db.SaveChanges();
        }
        public MeetingStatisticEntity Statistic(MeetingSearchModel modelSearch)
        {
            MeetingStatisticEntity searchResultModel = new MeetingStatisticEntity();

            var meetings = (from a in db.Meetings.AsNoTracking()
                            select new
                            {
                                a.Id,
                                a.Status,
                                a.MeetingDate,
                                a.CreateBy,
                            }).ToList();

            List<string> createByIds = (from r in db.Employees
                                        join u in db.Users on r.Id equals u.EmployeeId
                                        where r.DepartmentId.Equals(modelSearch.DepartmentId)
                                        select u.Id).ToList();

            List<string> meetingIds;
            // Trường hợp là Ban giám đốc thì có thể xem tất cả các Cuộc họp trong công ty
            if (!modelSearch.IsBGĐ)
            {
                // Trường hợp là Trưởng phòng thì xem được của phòng ban mình
                if (!modelSearch.IsTrPhong)
                {
                    // Lấy danh sách các Meeting có mình tham gia
                    meetingIds = db.MeetingUsers.Where(t => t.UserId.Equals(modelSearch.UserId)).Select(t => t.MeetingId).ToList();

                    // Chỉ lấy những Meeting do mình tạo, hoặc có mình tham gia
                    meetings = meetings.Where(t => t.CreateBy.Equals(modelSearch.UserId) || meetingIds.Contains(t.Id)).ToList();
                }
                else
                {
                    // Lấy danh sách các Meeting có mình tham gia
                    meetingIds = db.MeetingUsers.Where(t => t.UserId.Equals(modelSearch.UserId)).Select(t => t.MeetingId).ToList();

                    // Chỉ lấy những Meeting do mình tạo, hoặc có mình tham gia, hoặc do nhân viên của mình tạo
                    meetings = meetings.Where(t => t.CreateBy.Equals(modelSearch.UserId) || meetingIds.Contains(t.Id) || createByIds.Contains(t.CreateBy)).ToList();
                }
            }

            searchResultModel.TotalNoPlan = meetings.Count(r => r.Status == Constants.Meeting_Status_NoPlan);
            searchResultModel.TotalPlaned = meetings.Count(r => r.Status == Constants.Meeting_Status_HasPlan);
            searchResultModel.TotalFinished = meetings.Count(r => r.Status == Constants.Meeting_Status_Finish);

            return searchResultModel;
        }

        public SearchResultModel<MeetingSearchResultModel> SearchMeetingFinish(MeetingSearchModel modelSearch)
        {
            SearchResultModel<MeetingSearchResultModel> searchResult = new SearchResultModel<MeetingSearchResultModel>();

            var dataQuery = (from a in db.Meetings.AsNoTracking()
                             join b in db.MeetingTypes.AsNoTracking() on a.MeetingTypeId equals b.Id
                             join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id into ac
                             from acx in ac.DefaultIfEmpty()
                             join d in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals d.Id into ad
                             from adx in ad.DefaultIfEmpty()
                             where a.Status == Constants.Meeting_Status_Finish
                             select new MeetingSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Type = a.Type,
                                 MeetingTypeId = b.Id,
                                 MeetingTypeName = b.Name,
                                 Address = a.Address,
                                 CustomerCode = acx != null ? acx.Code : string.Empty,
                                 CustomerName = acx != null ? acx.Name : string.Empty,
                                 CustomerContactName = adx != null ? adx.Name : string.Empty,
                                 Email = adx != null ? adx.Email : string.Empty,
                                 PhoneNumber = adx != null ? adx.PhoneNumber : string.Empty,
                                 StrStartTime = a.StartTime,
                                 StrEndTime = a.EndTime,
                                 StrRealStartTime = a.RealStartTime,
                                 StrRealEndTime = a.RealEndTime,
                                 Time = a.Time,
                                 Status = a.Status
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.MeetingTypeId))
            {
                dataQuery = dataQuery.Where(u => u.MeetingTypeId.Equals(modelSearch.MeetingTypeId));
            }

            if (modelSearch.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == modelSearch.Type.Value);
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.UserId))
            {
                var listMeetingUser = db.MeetingUsers.Where(t => t.UserId.Equals(modelSearch.UserId)).Select(t => t.MeetingId).ToList();

                if (listMeetingUser.Count > 0)
                {
                    dataQuery = dataQuery.Where(t => listMeetingUser.Contains(t.Id));
                }
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.OrderBy(t => t.Code).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            if (listResult.Count > 0)
            {
                foreach (var item in listResult)
                {
                    item.StartTime = JsonConvert.DeserializeObject<object>(item.StrStartTime);
                    item.EndTime = JsonConvert.DeserializeObject<object>(item.StrEndTime);

                    if (!string.IsNullOrEmpty(item.StrRealStartTime))
                    {
                        item.RealStartTime = JsonConvert.DeserializeObject<object>(item.StrRealStartTime);
                    }

                    if (!string.IsNullOrEmpty(item.StrRealEndTime))
                    {
                        item.RealEndTime = JsonConvert.DeserializeObject<object>(item.StrRealEndTime);
                    }
                }
            }

            searchResult.ListResult = listResult;
            return searchResult;
        }

        public MeetingCodeModel GenerateCode(MeetingCodeCharModel model)
        {
            var dateNow = DateTime.Now;
            string code = "";
            var maxIndex = db.Meetings.AsNoTracking().Where(t => t.CodeChar.Equals(model.CodeChar)).Select(r => r.Index).DefaultIfEmpty(0).Max();
            maxIndex++;
            code = $"{model.CodeChar}{string.Format("{0:0000}", maxIndex)}";

            return new MeetingCodeModel
            {
                Code = code,
                Index = maxIndex
            };
        }

        public void CreateMeeting(MeetingCreateModel model, string userId)
        {
            List<MeetingCustomerContact> listCustomerContact = new List<MeetingCustomerContact>();
            List<MeetingUser> listEmployee = new List<MeetingUser>();
            List<MeetingContent> listContent = new List<MeetingContent>();
            List<MeetingAttach> listAttach = new List<MeetingAttach>();

            int maxIndex = db.Meetings.AsNoTracking().Where(t => t.CodeChar.Equals(model.CodeChar)).Select(r => r.Index).DefaultIfEmpty(0).Max() + 1;

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Meeting meeting = new Meeting
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = $"{model.CodeChar}{string.Format("{0:0000}", maxIndex)}",
                        Name = model.Name.NTSTrim(),
                        CustomerId = model.CustomerId,
                        CustomerContactId = model.CustomerContactId,
                        MeetingDate = model.MeetingDate,
                        Description = model.Description,
                        StartTime = JsonConvert.SerializeObject(model.StartTime),
                        EndTime = JsonConvert.SerializeObject(model.EndTime),
                        Type = model.Type,
                        MeetingTypeId = model.MeetingTypeId,
                        RealStartTime = JsonConvert.SerializeObject(model.RealStartTime),
                        RealEndTime = JsonConvert.SerializeObject(model.RealEndTime),
                        Time = model.Time,
                        CodeChar = model.CodeChar,
                        Index = maxIndex,
                        Address = model.Address,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                        Status = Constants.Meeting_Status_NoPlan,
                        Step = Constants.Meeting_Step_Confirm,
                        Request = model.Request,
                        RequestDate = model.RequestDate,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                    };

                    db.Meetings.Add(meeting);

                    if (model.ListCustomerContact.Count > 0)
                    {
                        foreach (var item in model.ListCustomerContact)
                        {
                            MeetingCustomerContact meetingCustomerContact = new MeetingCustomerContact()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MeetingId = meeting.Id,
                                CustomerContactId = item.Id
                            };
                            listCustomerContact.Add(meetingCustomerContact);
                        }
                        db.MeetingCustomerContacts.AddRange(listCustomerContact);
                    }

                    if (model.ListUser.Count > 0)
                    {
                        foreach (var item in model.ListUser)
                        {
                            MeetingUser meetingUser = new MeetingUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MeetingId = meeting.Id,
                                UserId = item.Id
                            };
                            listEmployee.Add(meetingUser);
                        }
                        db.MeetingUsers.AddRange(listEmployee);
                    }

                    if (model.ListContent.Count > 0)
                    {
                        foreach (var item in model.ListContent)
                        {
                            MeetingContent content = new MeetingContent()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MeetingId = meeting.Id,
                                Request = item.Request,
                                Solution = item.Solution,
                                FinishDate = item.FinishDate,
                                Note = item.Note,
                                #region thaint 

                                #endregion
                            };
                            listContent.Add(content);
                        }
                        db.MeetingContents.AddRange(listContent);
                    }

                    if (model.ListAttach.Count > 0)
                    {
                        foreach (var item in model.ListAttach)
                        {
                            MeetingAttach attach = new MeetingAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MeetingId = meeting.Id,
                                Name = item.Name,
                                Note = item.Note,
                                FilePath = item.FilePath,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };
                            listAttach.Add(attach);
                        }
                        db.MeetingAttaches.AddRange(listAttach);
                    }


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
        /// Chi tiết meeting
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MeetingInfoModel GetMeetingInfoById(string id)
        {
            var result = (from a in db.Meetings.AsNoTracking()
                          join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                          join c in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals c.Id
                          join d in db.MeetingTypes.AsNoTracking() on a.MeetingTypeId equals d.Id
                          where a.Id.Equals(id)
                          select new MeetingInfoModel
                          {
                              Id = a.Id,
                              Code = a.Code,
                              Name = a.Name,
                              CustomerId = a.CustomerId,
                              CustomerContactId = a.CustomerContactId,
                              CustomerContactName = c.Name,
                              Type = a.Type,
                              MeetingDate = a.MeetingDate,
                              MeetingTypeId = a.MeetingTypeId,
                              MeetingTypeName = d.Name,
                              StrStartTime = a.StartTime,
                              StrEndTime = a.EndTime,
                              StrRealStartTime = a.RealStartTime,
                              StrRealEndTime = a.RealEndTime,
                              Time = a.Time,
                              CustomerName = b.Name,
                              CustomerCode = b.Code,
                              PhoneNumber = c.PhoneNumber,
                              Description = a.Description,
                              Email = c.Email,
                              Address = a.Address,
                              Status = a.Status,
                              Step = a.Step,
                              Index = a.Index,
                              CodeChar = a.CodeChar,
                              Request = a.Request,
                              RequestDate = a.RequestDate,
                              Latitude = a.Latitude,
                              Longitude = a.Longitude,
                          }).FirstOrDefault();


            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }
            result.StartTime = JsonConvert.DeserializeObject<object>(result.StrStartTime);
            result.EndTime = JsonConvert.DeserializeObject<object>(result.StrEndTime);

            if (!string.IsNullOrEmpty(result.StrRealStartTime))
            {
                result.RealStartTime = JsonConvert.DeserializeObject<object>(result.StrRealStartTime);
            }

            if (!string.IsNullOrEmpty(result.StrRealEndTime))
            {
                result.RealEndTime = JsonConvert.DeserializeObject<object>(result.StrRealEndTime);
            }

            result.ListAttach = (from a in db.MeetingAttaches.AsNoTracking()
                                 join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                 where a.MeetingId.Equals(result.Id)
                                 where a.Type.Equals(0)
                                 select new MeetingAttachModel
                                 {
                                     Id = a.Id,
                                     MeetingId = a.MeetingId,
                                     Name = a.Name,
                                     Note = a.Note,
                                     FileName = a.FileName,
                                     FilePath = a.FilePath,
                                     FileSize = a.FileSize,
                                     CreateDate = a.CreateDate,
                                     CreateName = b.UserName,
                                     IsDelete = false
                                 }).ToList();

            result.ListAttachPerformStep = (from a in db.MeetingAttaches.AsNoTracking()
                                            join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                            where a.MeetingId.Equals(result.Id)
                                            where a.Type.Equals(1)
                                            select new MeetingAttachModel
                                            {
                                                Id = a.Id,
                                                MeetingId = a.MeetingId,
                                                Name = a.Name,
                                                Note = a.Note,
                                                FileName = a.FileName,
                                                FilePath = a.FilePath,
                                                FileSize = a.FileSize,
                                                CreateDate = a.CreateDate,
                                                CreateName = b.UserName,
                                                IsDelete = false
                                            }).ToList();

            result.ListUser = (from a in db.MeetingUsers.AsNoTracking()
                               join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                               join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                               join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                               where a.MeetingId.Equals(result.Id)
                               select new MeetingUserInfoModel
                               {
                                   MeetingUserId = a.Id,
                                   MeetingId = a.MeetingId,
                                   UserId = a.UserId,
                                   Code = c.Code,
                                   Name = c.Name,
                                   PhoneNumber = c.PhoneNumber,
                                   Email = c.Email,
                                   Status = c.Status,
                                   DepartmentName = d.Name,
                                   ImagePath = c.ImagePath,
                                   IsNew = false
                               }).ToList();

            result.ListCustomerContact = (from a in db.MeetingCustomerContacts.AsNoTracking()
                                          join b in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals b.Id
                                          into t
                                          from b in t.DefaultIfEmpty()
                                          join c in db.Customers.AsNoTracking() on b.CustomerId equals c.Id
                                          into k
                                          from c in k.DefaultIfEmpty()

                                          where (a.MeetingId.Equals(result.Id))
                                          orderby a.Name ascending
                                          select new NTS.Model.MeetingCustomerContact.MeetingCustomerContactInfoModel
                                          {
                                              Id = b.Id,
                                              MeetingCustomerContactId = a.Id,
                                              MeetingId = a.MeetingId,
                                              CustomerContactId = a.CustomerContactId,
                                              Name = a.CustomerContactId != null ? b.Name : a.Name,
                                              PhoneNumber = a.CustomerContactId != null ? b.PhoneNumber : a.Phone,
                                              Position = a.CustomerContactId != null ? b.Position : a.Position,
                                              Email = a.CustomerContactId != null ? b.Email : a.Email,
                                              CustomerName = a.CustomerContactId != null ? c.Name : a.CompanyName,
                                              IsNew = false,
                                              Avatar = b.Avatar,
                                          }).ToList();

            result.ListContent = (from a in db.MeetingContents.AsNoTracking()
                                      //join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                  where a.MeetingId.Equals(result.Id)
                                  orderby a.Request
                                  select new MeetingContentModel
                                  {
                                      Id = a.Id,
                                      MeetingId = a.MeetingId,
                                      Request = a.Request,
                                      Solution = a.Solution,
                                      FinishDate = a.FinishDate,
                                      Note = a.Note,
                                      CreateBy = a.RequestBy,
                                      CreateDate = a.CreateDate,
                                      EmployeeName = db.CustomerContacts.FirstOrDefault(m => m.Id.Equals(a.RequestBy)).Name,
                                      Code = a.Code,
                                      Checked = db.CustomerRequirementContents.Where(r => r.MeetingContentId.Equals(a.Id)).Any(),

                                  }).ToList();
            foreach (var item in result.ListContent)
            {
                item.MeetingContentAttaches = (from a in db.MeetingContentAttaches.AsNoTracking()
                                               join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                               where a.MeetingId.Equals(result.Id) && a.MeetingContentId.Equals(item.Id)
                                               select new MeetingContentAttachModel
                                               {
                                                   Id = a.Id,
                                                   MeetingId = a.MeetingId,
                                                   MeetingContentId = item.Id,
                                                   Name = a.Name,
                                                   Note = a.Note,
                                                   FileName = a.FileName,
                                                   FilePath = a.FilePath,
                                                   FileSize = a.FileSize,
                                                   CreateDate = a.CreateDate,
                                                   CreateName = b.UserName,
                                                   IsDelete = false
                                               }).ToList();
                List<string> fileNames = new List<string>();
                foreach (var file in item.MeetingContentAttaches)
                {
                    fileNames.Add(file.FileName);
                }
                item.NameFiles = String.Join(", ", fileNames.ToArray());
            }
            return result;
        }

        public void UpdateMeeting(string id, MeetingInfoModel model, string userId)
        {
            List<MeetingCustomerContact> listCustomerContact = new List<MeetingCustomerContact>();
            List<MeetingUser> listEmployee = new List<MeetingUser>();
            List<MeetingContent> listContent = new List<MeetingContent>();
            List<MeetingAttach> listAttach = new List<MeetingAttach>();

            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(id));
            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }
            var checkCode = db.Meetings.FirstOrDefault(a => a.Code.Equals(model.Code));
            if (checkCode != null && !meeting.Code.Equals(checkCode.Code))
            {
                throw NTSException.CreateInstance("Mã meeting đã tồn tại!", TextResourceKey.Meeting);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.Name = model.Name.NTSTrim();
                    meeting.CustomerId = model.CustomerId;
                    meeting.CustomerContactId = model.CustomerContactId;
                    meeting.StartTime = JsonConvert.SerializeObject(model.StartTime);
                    meeting.EndTime = JsonConvert.SerializeObject(model.EndTime);
                    meeting.Type = model.Type;
                    meeting.MeetingDate = model.MeetingDate;
                    meeting.MeetingTypeId = model.MeetingTypeId;
                    meeting.RealStartTime = JsonConvert.SerializeObject(model.RealStartTime);
                    meeting.RealEndTime = JsonConvert.SerializeObject(model.RealEndTime);
                    meeting.Time = model.Time;
                    meeting.Address = model.Address;
                    meeting.Status = model.Status;
                    meeting.Step = model.Step;
                    meeting.UpdateBy = userId;
                    meeting.UpdateDate = DateTime.Now;
                    meeting.ReasonCancel = model.ReasonCancel;
                    meeting.Index = model.Index;
                    meeting.CodeChar = model.CodeChar;
                    meeting.Code = model.Code;
                    meeting.Description = model.Description;

                    meeting.Request = model.Request;
                    meeting.RequestDate = model.RequestDate;
                    meeting.Latitude = model.Latitude;
                    meeting.Longitude = model.Longitude;

                    if (model.Status == Constants.Meeting_Status_NoPlan && model.MeetingDate.HasValue)
                    {
                        meeting.Status = Constants.Meeting_Status_HasPlan;
                    }

                    var customerContacts = db.MeetingCustomerContacts.Where(t => t.MeetingId.Equals(id)).ToList();
                    if (customerContacts.Count > 0)
                    {
                        db.MeetingCustomerContacts.RemoveRange(customerContacts);
                    }

                    if (model.ListCustomerContact.Count > 0)
                    {
                        foreach (var item in model.ListCustomerContact)
                        {
                            if (item.CustomerContactId != null)
                            {
                                var _customerContact = db.CustomerContacts.Where(t => t.Id.Equals(item.CustomerContactId)).FirstOrDefault();
                                if (_customerContact == null)
                                {
                                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerContact);
                                }
                                _customerContact.Name = item.Name;
                                _customerContact.Email = item.Email == null ? "" : item.Email;
                                _customerContact.Position = item.Position;
                                _customerContact.PhoneNumber = item.PhoneNumber == null ? "" : item.PhoneNumber;
                                if (_customerContact.CustomerId != null)
                                {
                                    var _customer = db.Customers.Where(t => t.Id.Equals(_customerContact.CustomerId)).FirstOrDefault();
                                    if (_customer == null)
                                    {
                                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Customer);
                                    }
                                    _customer.Name = item.CustomerName;
                                    _customer.UpdateDate = DateTime.Now;
                                    _customer.UpdateBy = userId;
                                }
                                db.SaveChanges();


                            }
                            if (item.IsNew)
                            {
                                MeetingCustomerContact meetingCustomerContact = new MeetingCustomerContact()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    CustomerContactId = item.Id,
                                    Name = item.Name,
                                    Phone = item.PhoneNumber,
                                    CompanyName = item.CustomerName,
                                    Email = item.Email,
                                    Position = item.Position

                                };
                                listCustomerContact.Add(meetingCustomerContact);
                            }
                            else
                            {
                                MeetingCustomerContact meetingCustomerContact = new MeetingCustomerContact()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    CustomerContactId = item.CustomerContactId,
                                    Name = item.Name,
                                    Phone = item.PhoneNumber,
                                    CompanyName = item.CustomerName,
                                    Email = item.Email,
                                    Position = item.Position
                                };
                                listCustomerContact.Add(meetingCustomerContact);
                            }
                        }
                        db.MeetingCustomerContacts.AddRange(listCustomerContact);
                    }

                    var users = db.MeetingUsers.Where(t => t.MeetingId.Equals(id)).ToList();
                    if (users.Count > 0)
                    {
                        db.MeetingUsers.RemoveRange(users);
                    }
                    if (model.ListUser.Count > 0)
                    {
                        foreach (var item in model.ListUser)
                        {
                            if (item.IsNew)
                            {
                                MeetingUser meetingUser = new MeetingUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    UserId = item.Id
                                };
                                listEmployee.Add(meetingUser);
                            }
                            else
                            {
                                MeetingUser meetingUser = new MeetingUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    UserId = item.UserId
                                };
                                listEmployee.Add(meetingUser);
                            }

                        }
                        db.MeetingUsers.AddRange(listEmployee);
                    }

                    var attachs = db.MeetingAttaches.Where(t => t.MeetingId.Equals(id) && t.Type == 0).ToList();
                    if (attachs.Count > 0)
                    {
                        db.MeetingAttaches.RemoveRange(attachs);

                    }
                    if (model.ListAttach.Count > 0)
                    {
                        foreach (var item in model.ListAttach)
                        {
                            if (!item.IsDelete)
                            {
                                MeetingAttach attach = new MeetingAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    Name = item.Name,
                                    Note = item.Note,
                                    FilePath = item.FilePath,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now,
                                    Type = 0,
                                };
                                listAttach.Add(attach);
                            }

                        }
                        db.MeetingAttaches.AddRange(listAttach);

                    }

                    var attachPerformSteps = db.MeetingAttaches.Where(t => t.MeetingId.Equals(id) && t.Type == 1).ToList();
                    if (attachPerformSteps.Count > 0)
                    {
                        db.MeetingAttaches.RemoveRange(attachPerformSteps);

                    }
                    if (model.ListAttachPerformStep.Count > 0)
                    {
                        foreach (var item in model.ListAttachPerformStep)
                        {
                            if (!item.IsDelete)
                            {
                                MeetingAttach attachPerformStep = new MeetingAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    Name = item.Name,
                                    Note = item.Note,
                                    FilePath = item.FilePath,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now,
                                    Type = 1,
                                };
                                listAttach.Add(attachPerformStep);
                            }

                        }
                        db.MeetingAttaches.AddRange(listAttach);
                    }
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
        /// Cập nhật thông tin trên Tab Thực hiện Meeting
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <exception cref="NTSLogException"></exception>
        public void UpdateMeetingPerforming(MeetingPerformingEntity model, string userId)
        {
            List<MeetingCustomerContact> listCustomerContact = new List<MeetingCustomerContact>();
            List<MeetingAttach> listAttach = new List<MeetingAttach>();

            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(model.MeetingId));
            if (meeting == null)
            {
                return;
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    // Cập nhật  thời gian thực tế tiến hành Meeting
                    meeting.RealStartTime = JsonConvert.SerializeObject(model.RealStartTime);
                    meeting.RealEndTime = JsonConvert.SerializeObject(model.RealEndTime);

                    // Xóa danh sách người của Khách hàng tham gia
                    var customerContacts = db.MeetingCustomerContacts.Where(t => t.MeetingId.Equals(model.MeetingId)).ToList();
                    if (customerContacts.Count > 0)
                    {
                        db.MeetingCustomerContacts.RemoveRange(customerContacts);
                    }

                    foreach (var item in model.ListCustomerContact)
                    {
                        MeetingCustomerContact meetingCustomerContact = new MeetingCustomerContact()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetingId = meeting.Id,
                            CustomerContactId = item.CustomerContactId,
                            Name = item.Name,
                            Phone = item.PhoneNumber,
                            CompanyName = item.CustomerName,
                            Email = item.Email,
                            Position = item.Position
                        };
                        listCustomerContact.Add(meetingCustomerContact);
                    }

                    // Lưu danh sách người của Khách hàng tham gia
                    db.MeetingCustomerContacts.AddRange(listCustomerContact);                   

                    // Xóa danh sách file đính kèm,
                    var attachPerformSteps = db.MeetingAttaches.Where(t => t.MeetingId.Equals(model.MeetingId) && t.Type == 1).ToList();
                    if (attachPerformSteps.Count > 0)
                    {
                        db.MeetingAttaches.RemoveRange(attachPerformSteps);

                    }

                    // Thêm lại danh sách file đính kèm
                    foreach (var item in model.ListAttachPerformStep)
                    {
                        MeetingAttach attachPerformStep = new MeetingAttach()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetingId = meeting.Id,
                            Name = item.Name,
                            Note = item.Note,
                            FilePath = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now,
                            Type = 1,
                        };
                        listAttach.Add(attachPerformStep);
                    }
                    db.MeetingAttaches.AddRange(listAttach);

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
        /// Cập nhật thông tin Meeting
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <exception cref="NTSLogException"></exception>
        public void UpdateInfo(MeetingInfoEntity model, string userId)
        {
            List<MeetingAttach> listAttach = new List<MeetingAttach>();

            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(model.MeetingId));

            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.Name = model.Name.NTSTrim();
                    meeting.CustomerId = model.CustomerId;
                    meeting.CustomerContactId = model.CustomerContactId;
                    meeting.Type = model.Type;
                    meeting.MeetingTypeId = model.MeetingTypeId;
                    meeting.UpdateBy = userId;
                    meeting.UpdateDate = DateTime.Now;
                    meeting.Request = model.Request;
                    meeting.RequestDate = model.RequestDate;

                    var attachs = db.MeetingAttaches.Where(t => t.MeetingId.Equals(model.MeetingId) && t.Type == 0).ToList();
                    if (attachs.Count > 0)
                    {
                        db.MeetingAttaches.RemoveRange(attachs);

                    }
                    if (model.ListAttach.Count > 0)
                    {
                        foreach (var item in model.ListAttach)
                        {
                            if (!item.IsDelete)
                            {
                                MeetingAttach attach = new MeetingAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetingId = meeting.Id,
                                    Name = item.Name,
                                    Note = item.Note,
                                    FilePath = item.FilePath,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now,
                                    Type = 0,
                                };
                                listAttach.Add(attach);
                            }
                        }

                        db.MeetingAttaches.AddRange(listAttach);
                    }

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
        /// Cập nhật kế hoạch Meeting
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <exception cref="NTSLogException"></exception>
        public void UpdatePlan(MeetingPlanEntity model, string userId)
        {
            List<MeetingAttach> listAttach = new List<MeetingAttach>();
            List<MeetingUser> listEmployee = new List<MeetingUser>();

            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(model.MeetingId));

            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.StartTime = JsonConvert.SerializeObject(model.StartTime);
                    meeting.EndTime = JsonConvert.SerializeObject(model.EndTime);
                    meeting.MeetingDate = model.MeetingDate;
                    meeting.Time = model.Time;
                    meeting.Address = model.Address;
                    meeting.Latitude = model.Latitude;
                    meeting.Longitude = model.Longitude;
                    meeting.Status = Constants.Meeting_Status_HasPlan;
                    meeting.UpdateBy = userId;
                    meeting.UpdateDate = DateTime.Now;

                    // Xóa danh sách người tham gia cũ
                    var users = db.MeetingUsers.Where(t => t.MeetingId.Equals(model.MeetingId)).ToList();

                    if (users.Count > 0)
                    {
                        db.MeetingUsers.RemoveRange(users);
                    }

                    // Lưu danh sách người tham gia phía TPA
                    foreach (var item in model.ListUser)
                    {
                        MeetingUser meetingUser = new MeetingUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetingId = meeting.Id,
                            UserId = item.Id
                        };
                        listEmployee.Add(meetingUser);
                    }
                    db.MeetingUsers.AddRange(listEmployee);

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
        /// Tạo mới yêu cầu khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <exception cref="NTSLogException"></exception>
        public void CreateRequirement(MeetingRequirementEntity model, string userId)
        {
            List<MeetingContentAttach> listAttach = new List<MeetingContentAttach>();

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    MeetingContent meetingContent = new MeetingContent
                    {
                        Id = Guid.NewGuid().ToString(),
                        MeetingId = model.MeetingId,
                        Code = model.Code,
                        Request = model.Request,
                        RequestBy = model.RequestBy,
                        Solution = model.Solution,
                        FinishDate = model.FinishDate,
                        Note = model.Note,
                        Index = 0,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.MeetingContents.Add(meetingContent);

                    foreach (var file in model.AttachFiles)
                    {
                        MeetingContentAttach attachFile = new MeetingContentAttach()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetingId = model.MeetingId,
                            MeetingContentId = meetingContent.Id,

                            Type = 1,
                            Name = file.Name,
                            Note = file.Note,
                            FileName = file.FileName,
                            FilePath = file.FilePath,
                            FileSize = file.FileSize,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now,
                        };

                        listAttach.Add(attachFile);
                    }

                    db.MeetingContentAttaches.AddRange(listAttach);

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
        /// Lấy thông tin cho tiết yêu cầu khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NTSLogException"></exception>
        public MeetingRequirementEntity GetDetailRequirement(string meetingContentId)
        {
            MeetingRequirementEntity meetingRequirementEntity = new MeetingRequirementEntity();

            var meetingContent = db.MeetingContents.FirstOrDefault(t => t.Id.Equals(meetingContentId));
            if (meetingContent == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }

            meetingRequirementEntity.MeetingId = meetingContent.MeetingId;
            meetingRequirementEntity.Id = meetingContent.Id;
            meetingRequirementEntity.Request = meetingContent.Request;
            meetingRequirementEntity.RequestBy = meetingContent.RequestBy;
            meetingRequirementEntity.Solution = meetingContent.Solution;
            meetingRequirementEntity.Note = meetingContent.Note;
            meetingRequirementEntity.FinishDate = meetingContent.FinishDate;
            meetingRequirementEntity.Code = meetingContent.Code;
            meetingRequirementEntity.AttachFiles = (from r in db.MeetingContentAttaches.AsNoTracking()
                                                    where r.MeetingContentId.Equals(meetingContentId)
                                                    select new MeetingContentAttachModel()
                                                    {
                                                        Id = r.Id,
                                                        MeetingContentId = r.MeetingContentId,
                                                        FileName = r.FileName,
                                                        FileSize = r.FileSize,
                                                        FilePath = r.FilePath,
                                                        Name = r.Name,
                                                        MeetingId = r.MeetingId,
                                                        Note = r.Note,
                                                    }).ToList();
            return meetingRequirementEntity;
        }

        /// <summary>
        /// Cập nhật yêu cầu khách hàng trong Meeting
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <exception cref="NTSLogException"></exception>
        public void UpdateRequirement(MeetingRequirementEntity model, string userId)
        {
            List<MeetingContentAttach> listAttach = new List<MeetingContentAttach>();

            var meetingContent = db.MeetingContents.FirstOrDefault(t => t.Id.Equals(model.Id));
            if (meetingContent == null)
            {
                return;
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meetingContent.Code = model.Code;
                    meetingContent.Request = model.Request;
                    meetingContent.RequestBy = model.RequestBy;
                    meetingContent.Solution = model.Solution;
                    meetingContent.FinishDate = model.FinishDate;
                    meetingContent.Note = model.Note;
                    meetingContent.CreateBy = userId;
                    meetingContent.CreateDate = DateTime.Now;

                    // Xóa tất cả các file đính kèm cũ
                    var attaches = db.MeetingContentAttaches.Where(t => t.MeetingContentId.Equals(meetingContent.Id)).ToList();
                    if (attaches.Count > 0)
                    {
                        db.MeetingContentAttaches.RemoveRange(attaches);
                    }

                    // Thêm lại danh sách file đính kèm vào DB
                    foreach (var file in model.AttachFiles)
                    {
                        MeetingContentAttach attachFile = new MeetingContentAttach()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetingId = meetingContent.MeetingId,
                            MeetingContentId = meetingContent.Id,
                            Type = 1,
                            Name = file.Name,
                            Note = file.Note,
                            FileName = file.FileName,
                            FilePath = file.FilePath,
                            FileSize = file.FileSize,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now,
                        };

                        listAttach.Add(attachFile);
                    }

                    db.MeetingContentAttaches.AddRange(listAttach);

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
        /// Xóa yêu cầu khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NTSLogException"></exception>
        public void DeleteRequirement(string meetingContentId)
        {
            List<MeetingContentAttach> listAttach = new List<MeetingContentAttach>();

            var meetingContent = db.MeetingContents.FirstOrDefault(t => t.Id.Equals(meetingContentId));
            if (meetingContent == null)
            {
                return;
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    // Xóa tất cả các file đính kèm cũ
                    var attaches = db.MeetingContentAttaches.Where(t => t.MeetingContentId.Equals(meetingContent.Id)).ToList();
                    if (attaches.Count > 0)
                    {
                        db.MeetingContentAttaches.RemoveRange(attaches);
                    }

                    // Xóa dữ liệu trong bảng MeetingContent
                    db.MeetingContents.Remove(meetingContent);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(meetingContentId, ex);
                }
            }
        }

        public void UpdateMeetingCustomerRequirement(string id, MeetingInfoModel model, string userId)
        {
            List<string> guids = new List<string>();
            var meetingContents = db.MeetingContents.Where(x => x.MeetingId.Equals(id)).ToList();
            try
            {
                foreach (var modelId in model.ListContent)
                {
                    guids.Add(modelId.Id);
                }
                foreach (var cid in meetingContents)
                {
                    if (!guids.Contains(cid.Id))
                    {
                        var meetingContentRemove = db.MeetingContents.FirstOrDefault(x => x.Id.Equals(cid.Id));
                        db.MeetingContents.Remove(meetingContentRemove);
                    }
                }
                foreach (var item in model.ListContent)
                {
                    if (item.Id == null) // tạo mới
                    {
                        var meetingContent = new MeetingContent
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetingId = id,
                            Request = item.Request,
                            Solution = item.Solution,
                            FinishDate = item.FinishDate,
                            Note = item.Note,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            Code = item.Code,

                        };
                        db.MeetingContents.Add(meetingContent);
                    }
                    else // cập nhật
                    {
                        var meetingContent = db.MeetingContents.FirstOrDefault(x => x.Id.Equals(item.Id));
                        //meetingContent.Checked = item.Checked;
                        meetingContent.Request = item.Request;
                        meetingContent.Solution = item.Solution;
                        meetingContent.Code = item.Code;
                        meetingContent.FinishDate = item.FinishDate;
                        meetingContent.Note = item.Note;
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public void DeleteMeeting(string id, string userId)
        {
            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(id));
            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var attachs = db.MeetingAttaches.Where(t => t.MeetingId.Equals(id));
                    db.MeetingAttaches.RemoveRange(attachs);

                    var meetingContentActack = db.MeetingContentAttaches.Where(x => x.MeetingId.Equals(id)).ToList();
                    db.MeetingContentAttaches.RemoveRange(meetingContentActack);

                    var contents = db.MeetingContents.Where(t => t.MeetingId.Equals(id));
                    foreach (var item in contents)
                    {
                        var customerRequirementContents = db.CustomerRequirementContents.Where(c => c.MeetingContentId.Equals(item.Id));
                        db.CustomerRequirementContents.RemoveRange(customerRequirementContents);
                    }
                    db.MeetingContents.RemoveRange(contents);

                    var customerContacts = db.MeetingCustomerContacts.Where(t => t.MeetingId.Equals(id));
                    db.MeetingCustomerContacts.RemoveRange(customerContacts);

                    var users = db.MeetingUsers.Where(t => t.MeetingId.Equals(id));
                    db.MeetingUsers.RemoveRange(users);

                    db.Meetings.Remove(meeting);

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

        public void DoMeeting(string id)
        {
            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(id));
            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.Step = Constants.Meeting_Step_DoMeeting;
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

        public void CancelMeeting(MeetingCancelModel model)
        {
            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(model.Id));
            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.ReasonCancel = model.ReasonCancel;
                    meeting.Status = Constants.Meeting_Status_Cancel;
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

        public void FinishMeeting(string id, string userId)
        {
            List<CustomerRequirementContent> customerRequirementContents = new List<CustomerRequirementContent>();
            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(id));
            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }

            // lấy ra danh sách dữ liệu được đánh dấu
            var meetingContents = db.MeetingContents.Where(t => (t.MeetingId.Equals(id))).ToList();

            foreach (var meetingContent1 in meetingContents) // duyệt từng yêu cầu trong meeting
            {
                // lấy ra dữ liệu yêu cầu trong  yêu cầu khách hàng với điều kiện có meetingcontentid = id meeting
                var customerRequirmentContent = db.CustomerRequirementContents.FirstOrDefault(x => x.MeetingContentId.Equals(meetingContent1.Id));
                if (customerRequirmentContent == null) // kiểm tra nếu như dữ liệu chưa có trong yêu cầu khách hàng thì mới thêm vào
                {

                    var content = new CustomerRequirementContent()
                    {
                        Id = Guid.NewGuid().ToString(),
                        //CustomerRequirementId = require.Id,
                        MeetingContentId = meetingContent1.Id,
                        Request = meetingContent1.Request,
                        Solution = meetingContent1.Solution,
                        FinishDate = meetingContent1.FinishDate,
                        Note = meetingContent1.Note,
                        Code = meetingContent1.Code,
                        CreateDate = meetingContent1.CreateDate,


                    };
                    customerRequirementContents.Add(content);
                }
            }
            if (customerRequirementContents.Count > 0)
            {
                // tạo customerRequirmentId
                var codeModel = _customerRequireBussiness.GenerateCode();
                var require = new CustomerRequirement()
                {
                    Id = Guid.NewGuid().ToString(),
                    MeetingCode = meeting.Code,
                    Code = codeModel.Code,
                    Index = codeModel.Index,
                    CustomerId = meeting.CustomerId,
                    Step = 1,
                    CustomerContactId = meeting.CustomerContactId,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now
                };

                db.CustomerRequirements.Add(require);
                foreach (var content in customerRequirementContents)
                {
                    content.CustomerRequirementId = require.Id;
                }
                db.CustomerRequirementContents.AddRange(customerRequirementContents);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.Step = Constants.Meeting_Step_Finish;
                    meeting.Status = Constants.Meeting_Status_Finish;
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

        public void AddCustomerRequirement(MeetingInfoModel meetingInfoModel, string id, string userId)
        {
            List<CustomerRequirementContent> customerRequirementContents = new List<CustomerRequirementContent>();
            var meeting = db.Meetings.FirstOrDefault(t => t.Id.Equals(id));
            if (meeting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    meeting.Step = Constants.Meeting_Step_Finish;
                    meeting.Status = Constants.Meeting_Status_Finish;
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

        public MeetingCustomerContactGetInfoModel GetMeetingCustomerContactInfo(MeetingCustomerContactModel model)
        {
            var result = (from a in db.MeetingCustomerContacts.AsNoTracking()

                          where a.Id.Equals(model.Id)
                          select new MeetingCustomerContactGetInfoModel
                          {
                              Id = a.Id,
                              Name = a.Name,
                              CustomerContactId = a.CustomerContactId,
                              PhoneNumber = a.Phone,
                              CustomerName = a.CompanyName,
                              Email = a.Email,
                              Position = a.Position,
                          }).FirstOrDefault();


            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }
            return result;
        }

        public string ExportExcel(string id, MeetingInfoModel model)
        {
            var result = (from a in db.Meetings.AsNoTracking()
                          join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                          join c in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals c.Id
                          join d in db.MeetingTypes.AsNoTracking() on a.MeetingTypeId equals d.Id
                          where a.Id.Equals(id)
                          select new MeetingInfoModel
                          {
                              Id = a.Id,
                              Code = a.Code,
                              Name = a.Name,
                              CustomerId = a.CustomerId,
                              CustomerContactId = a.CustomerContactId,
                              CustomerContactName = c.Name,
                              Type = a.Type,
                              MeetingDate = a.MeetingDate,
                              MeetingTypeId = a.MeetingTypeId,
                              MeetingTypeName = d.Name,
                              StrStartTime = a.StartTime,
                              StrEndTime = a.EndTime,
                              StrRealStartTime = a.RealStartTime,
                              StrRealEndTime = a.RealEndTime,
                              Time = a.Time,
                              CustomerName = b.Name,
                              CustomerCode = b.Code,
                              PhoneNumber = c.PhoneNumber,
                              Description = a.Description,
                              Email = c.Email,
                              Address = a.Address,
                              Status = a.Status,
                              Step = a.Step
                          }).FirstOrDefault();


            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Meeting);
            }


            result.ListUser = (from a in db.MeetingUsers.AsNoTracking()
                               join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                               join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                               join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                               where a.MeetingId.Equals(result.Id)
                               select new MeetingUserInfoModel
                               {
                                   MeetingUserId = a.Id,
                                   MeetingId = a.MeetingId,
                                   UserId = a.UserId,
                                   Code = c.Code,
                                   Name = c.Name,
                                   PhoneNumber = c.PhoneNumber,
                                   Email = c.Email,
                                   Status = c.Status,
                                   DepartmentName = d.Name,
                                   IsNew = false
                               }).ToList();

            result.ListCustomerContact = (from a in db.MeetingCustomerContacts.AsNoTracking()
                                          join b in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals b.Id
                                          into t
                                          from b in t.DefaultIfEmpty()
                                          join c in db.Customers.AsNoTracking() on b.CustomerId equals c.Id
                                          into k
                                          from c in k.DefaultIfEmpty()

                                          where (a.MeetingId.Equals(result.Id))
                                          orderby a.Name ascending
                                          select new NTS.Model.MeetingCustomerContact.MeetingCustomerContactInfoModel
                                          {
                                              MeetingCustomerContactId = a.Id,
                                              MeetingId = a.MeetingId,
                                              CustomerContactId = a.CustomerContactId,
                                              Name = a.CustomerContactId != null ? b.Name : a.Name,
                                              PhoneNumber = a.CustomerContactId != null ? b.PhoneNumber : a.Phone,
                                              Position = a.CustomerContactId != null ? b.Position : a.Position,
                                              Email = a.CustomerContactId != null ? b.Email : a.Email,
                                              CustomerName = a.CustomerContactId != null ? c.Name : a.CompanyName,
                                              IsNew = false
                                          }).ToList();

            result.ListContent = (from a in db.MeetingContents.AsNoTracking()
                                  join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                  where a.MeetingId.Equals(result.Id)
                                  select new MeetingContentModel
                                  {
                                      Id = a.Id,
                                      MeetingId = a.MeetingId,
                                      Request = a.Request,
                                      Solution = a.Solution,
                                      FinishDate = a.FinishDate,
                                      Note = a.Note,
                                      CreateBy = b.UserName
                                  }).ToList();


            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Meeting_Template.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];


                var totalUser = result.ListUser.Count();
                var totalCustomerContact = result.ListCustomerContact.Count();

                IRange iRangeData = sheet.FindFirst("<dataDiaDiem>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = result.Address;
                }


                IRange rangeMeetingTime = sheet.FindFirst("<dataMeetingTime>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeMeetingTime.Text = rangeMeetingTime.Text.Replace("<dataMeetingTime>", (result.MeetingDate.HasValue ? result.MeetingDate.Value.ToString("dd/MM/yyy") : "--/--/----"));


                iRangeData = sheet.FindFirst("<dataCustomerContact>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<dataCustomerContact>", string.Empty);

                    var customerContactExport = result.ListCustomerContact.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                    }).ToList();

                    if (customerContactExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, customerContactExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(customerContactExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<dataUser>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<dataUser>", string.Empty);

                    var userExport = result.ListUser.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.DepartmentName,
                    }).ToList();

                    //if (userExport.Count > 2)
                    //{
                    //    sheet.InsertRow(iRangeData.Row + 1, userExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    //}

                    sheet.ImportData(userExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<dataContent>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<dataContent>", string.Empty);

                    var contentExport = result.ListContent.Select((o, i) => new
                    {
                        o.Request,
                    }).ToList();

                    if (contentExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, contentExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(contentExport, iRangeData.Row, iRangeData.Column, false);
                }

                //var listExport = result.Select((a, i) => new
                //{

                //    a.Code,
                //    a.Name,
                //    a.Address,
                //    a.Description,
                //});

                //if (listExport.Count() > 1)
                //{
                //    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                //}
                //sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Biên bản cuộc họp" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Biên bản cuộc họp" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

    }





}
