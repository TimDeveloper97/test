using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Candidates;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.RecruitmentChannels;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Recruitments
{
    public class RecruitmentChannelBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm kênh tuyển dụng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<RecruitmentChannelSearchResultModel> SearchChannels(RecruitmentChannelSearchModel searchModel)
        {
            SearchResultModel<RecruitmentChannelSearchResultModel> searchResult = new SearchResultModel<RecruitmentChannelSearchResultModel>();
            var dataQuery = (from a in db.RecruitmentChannels.AsNoTracking()
                             select new RecruitmentChannelSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 Status = a.Status
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status);
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            var interview = db.Interviews.AsNoTracking().ToList();
            var candidateApplie = db.CandidateApplies.AsNoTracking().ToList();
            var candidate = (from a in db.Candidates.AsNoTracking()
                             join b in db.CandidateApplies.AsNoTracking() on a.Id equals b.CandidateId
                             orderby b.ApplyDate descending
                             select new CandidateSearchResultsModel
                             {
                                 Id = a.Id,
                                 ProfileStatus = b.ProfileStatus,
                                 CandidateApplyId = b.Id,
                                 RecruitmentChannelId = a.RecruitmentChannelId
                             }).ToList();
            foreach (var item in candidate.ToList())
            {
                int total = interview.Where(r => r.CandidateApplyId.Equals(item.CandidateApplyId)).Count();
                int unInterview = interview.Where(r => r.CandidateApplyId.Equals(item.CandidateApplyId) && r.Status == 0).Count();
                int pass = interview.Where(r => r.CandidateApplyId.Equals(item.CandidateApplyId) && r.Status == 2).Count();
                int fail = interview.Where(r => r.CandidateApplyId.Equals(item.CandidateApplyId) && r.Status == 1).Count();

                if (total > 0)
                {
                    if (unInterview > 0)
                    {
                        item.InterviewStatus = 0;

                    }
                    else if (pass == total)
                    {
                        item.InterviewStatus = 2;
                    }
                    else
                    {
                        item.InterviewStatus = 1;
                    }
                }
                else
                {
                    item.InterviewStatus = 0;
                }
            }

            if (searchModel.DateFrom != null || searchModel.DateTo != null)
            {
                foreach (var item in listResult)
                {
                    var listCandidates = db.Candidates.Where(ca => ca.RecruitmentChannelId.Equals(item.Id));
                    if (searchModel.DateFrom.HasValue)
                    {
                        //listCandidates = listCandidates.Where(u => u.ApplyDate >= searchModel.DateFrom.Value);
                    }

                    if (searchModel.DateTo.HasValue)
                    {
                        //listCandidates = listCandidates.Where(u => u.ApplyDate <= searchModel.DateTo);
                    }

                    item.RecruitmentNumber = listCandidates.ToList().Count;
                    foreach (var ca in listCandidates.ToList())
                    {

                        List<CandidateApply> candidateApplies = new List<CandidateApply>();
                        foreach (var cdda in candidateApplie.ToList())
                        {
                            foreach (var cdd in candidate)
                            {
                                if (!string.IsNullOrEmpty(cdd.RecruitmentChannelId))
                                {
                                    if (cdd.RecruitmentChannelId.Equals(ca.RecruitmentChannelId) && cdda.ProfileStatus == 2 && cdda.CandidateId.Equals(cdd.Id))
                                    {
                                        candidateApplies.Add(cdda);
                                    }
                                }
                            }
                        }
                        var liscandidateApplies = candidateApplies.GroupBy(g => g.CandidateId).Select(s => s.Key).ToList();
                        item.CandidateNumber = liscandidateApplies.Count;

                        List<CandidateApply> candidateApplies1 = new List<CandidateApply>();
                        foreach (var cdda in candidateApplie.ToList())
                        {
                            foreach (var cdd in candidate)
                            {
                                if (!string.IsNullOrEmpty(cdd.RecruitmentChannelId))
                                {
                                    if (cdd.RecruitmentChannelId.Equals(ca.RecruitmentChannelId) && cdd.InterviewStatus == 2 && cdda.ProfileStatus == 2 && cdda.CandidateId.Equals(cdd.Id))
                                    {
                                        candidateApplies1.Add(cdda);
                                    }
                                }
                            }
                        }
                        var liscandidateApplies1 = candidateApplies1.GroupBy(g => g.CandidateId).Select(s => s.Key).ToList();
                        item.RecruitmentReceive = liscandidateApplies1.Count;

                    }


                }
            }
            else
            {
                foreach (var item in listResult)
                {
                    var listCandidates = db.Candidates.Where(ca => ca.RecruitmentChannelId.Equals(item.Id));
                    item.RecruitmentNumber = listCandidates.ToList().Count;

                    //item.CandidateNumber = (from a in candidateApplie
                    //                        join b in candidate on a.CandidateId equals b.Id
                    //                        where b.RecruitmentChannelId.Equals(item.Id) && a.ProfileStatus == Constants.CandidateApply_ProfileStatus
                    //                        group a by new { a.CandidateId } into g
                    //                        select new
                    //                        {
                    //                            g.Key.CandidateId,
                    //                        }).ToList().Count();

                    List<CandidateApply> candidateApplies = new List<CandidateApply>();
                    foreach(var cdda in candidateApplie.ToList())
                    {
                        foreach(var cdd in candidate)
                        {
                            if (!string.IsNullOrEmpty(cdd.RecruitmentChannelId)) {
                                if (cdd.RecruitmentChannelId.Equals(item.Id) && cdda.ProfileStatus == 2 && cdda.CandidateId.Equals(cdd.Id))
                                {
                                    candidateApplies.Add(cdda);
                                }
                            }
                        }
                    }
                    var liscandidateApplies = candidateApplies.GroupBy(g => g.CandidateId).Select(s => s.Key).ToList();
                    item.CandidateNumber = liscandidateApplies.Count;

                    List<CandidateApply> candidateApplies1 = new List<CandidateApply>();
                    foreach (var cdda in candidateApplie.ToList())
                    {
                        foreach (var cdd in candidate)
                        {
                            if (!string.IsNullOrEmpty(cdd.RecruitmentChannelId))
                            {
                                if (cdd.RecruitmentChannelId.Equals(item.Id) && cdd.InterviewStatus == 2 && cdda.ProfileStatus == 2 && cdda.CandidateId.Equals(cdd.Id))
                                {
                                    candidateApplies1.Add(cdda);
                                }
                            }
                        }
                    }
                    var liscandidateApplies1 = candidateApplies1.GroupBy(g => g.CandidateId).Select(s => s.Key).ToList();
                    item.RecruitmentReceive = liscandidateApplies1.Count;

                }
            }

            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        public void DeletetChannel(RecruitmentChannelModel model)
        {
            var channelExist = db.RecruitmentChannels.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (channelExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentChannel);
            }

            var channelUsed = db.Candidates.AsNoTracking().FirstOrDefault(a => a.RecruitmentChannelId.Equals(model.Id));
            if (channelUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.RecruitmentChannel);
            }

            try
            {
                db.RecruitmentChannels.Remove(channelExist);

                var NameOrCode = channelExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<RecruitmentChannelHistoryModel>(channelExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Reason, channelExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        public void CreatetChannel(RecruitmentChannelModel model)
        {
            model.Name = model.Name.NTSTrim();
            var channelExits = db.RecruitmentChannels.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (channelExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.RecruitmentChannel);
            }

            try
            {
                NTS.Model.Repositories.RecruitmentChannel channel = new NTS.Model.Repositories.RecruitmentChannel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    Status = model.Status,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.RecruitmentChannels.Add(channel);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, channel.Name, channel.Id, Constants.LOG_Reason);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RecruitmentChannelModel GetChannelById(RecruitmentChannelModel model)
        {
            var resultInfo = db.RecruitmentChannels.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new RecruitmentChannelModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note,
                Status = p.Status
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentChannel);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        public void UpdatetChannel(RecruitmentChannelModel model)
        {
            model.Name = model.Name.NTSTrim();
            var channelUpdate = db.RecruitmentChannels.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (channelUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonEndWorking);
            }

            //var jsonBefore = AutoMapperConfig.Mapper.Map<RecruitmentChannelHistoryModel>(channelUpdate);

            var reasonNameExist = db.RecruitmentChannels.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonEndWorking);
            }

            try
            {
                channelUpdate.Name = model.Name;
                channelUpdate.Note = model.Note;
                channelUpdate.Status = model.Status;
                channelUpdate.UpdateBy = model.UpdateBy;
                channelUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<RecruitmentChannelHistoryModel>(channelUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Reason, channelUpdate.Id, channelUpdate.Name, jsonBefore, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
