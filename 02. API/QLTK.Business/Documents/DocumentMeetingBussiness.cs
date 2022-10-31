using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.DocumentMeeting;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.DocumentMeeting
{
    public class DocumentMeetingBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<DocumentMeetingModel> SearchDocumentMeeting(DocumentMeetingSearchModel searchModel)
        {
            SearchResultModel<DocumentMeetingModel> searchResult = new SearchResultModel<DocumentMeetingModel>();
            var dataQuery = (from a in db.DocumentMeetings.AsNoTracking()
                             where a.DocumentId.Equals(searchModel.DocumentId)
                             orderby a.MeetingDate descending
                             select new DocumentMeetingModel
                             {
                                 Id = a.Id,
                                 DocumentId = a.DocumentId,
                                 FileName = a.FileName,
                                 FileSize = a.FileSize,
                                 Path = a.Path,
                                 PDFPath = a.PDFPath,
                                 Note = a.Note,
                                 MeetingDate = a.MeetingDate
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();

            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        public void DeleteDocumentMeeting(DocumentMeetingModel model)
        {
            var documentMeetingExist = db.DocumentMeetings.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (documentMeetingExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentMeeting);
            }

            try
            {
                db.DocumentMeetings.Remove(documentMeetingExist);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void CreateDocumentMeeting(ReportMeetingModel model, string userId)
        {
            try
            {
                foreach (var item in model.MeetingFiles)
                {
                    NTS.Model.Repositories.DocumentMeeting documentMeeting = new NTS.Model.Repositories.DocumentMeeting
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = model.DocumentId,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        Path = item.Path,
                        PDFPath = item.PDFPath,
                        Note = item.Note,
                        MeetingDate = item.MeetingDate,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    db.DocumentMeetings.Add(documentMeeting);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void UpdateDocumentMeeting(ReportMeetingModel model, string userId)
        {
            var document = db.Documents.FirstOrDefault(a => a.Id.Equals(model.DocumentId));
            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            try
            {
                var documentMeetingExist = db.DocumentMeetings.Where(a => a.DocumentId.Equals(model.DocumentId)).ToList();
                db.DocumentMeetings.RemoveRange(documentMeetingExist);

                foreach (var item in model.MeetingFiles)
                {
                    NTS.Model.Repositories.DocumentMeeting documentMeeting = new NTS.Model.Repositories.DocumentMeeting
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = model.DocumentId,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        Path = item.Path,
                        PDFPath = item.PDFPath,
                        Note = item.Note,
                        MeetingDate = item.MeetingDate,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    db.DocumentMeetings.Add(documentMeeting);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public ReportMeetingModel GetDocumentMeeting(ReportMeetingModel model)
        {
            var documentMeetings = (from p in db.DocumentMeetings.AsNoTracking()
                                    where p.DocumentId.Equals(model.DocumentId)
                                    select new DocumentMeetingModel
                                    {
                                        Id = p.Id,
                                        DocumentId = model.DocumentId,
                                        FileName = p.FileName,
                                        FileSize = p.FileSize,
                                        Path = p.Path,
                                        PDFPath = p.PDFPath,
                                        Note = p.Note,
                                        MeetingDate = p.MeetingDate,

                                    }).ToList();

            model.MeetingFiles = documentMeetings;
            if (documentMeetings.Count > 0)
            {
                model.MeetingDate = documentMeetings[0].MeetingDate;
            }

            return model;
        }
    }
}
