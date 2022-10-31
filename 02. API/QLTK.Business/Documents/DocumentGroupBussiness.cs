using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.DocumentGroups;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Documents
{
    public class DocumentGroupBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm nhóm tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<DocumentGroupSearchResultModel> SearchDocumentGroup(DocumentGroupSearchModel searchModel)
        {
            SearchResultModel<DocumentGroupSearchResultModel> searchResult = new SearchResultModel<DocumentGroupSearchResultModel>();
            var dataQuery = (from a in db.DocumentGroups.AsNoTracking()
                             orderby a.Code
                             select new DocumentGroupSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Code = a.Code
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            //var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        /// <summary>
        /// Xóa nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        public void DeleteDocumentGroup(DocumentGroupModel model)
        {
            var documentGroupExist = db.DocumentGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (documentGroupExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentGroup);
            }

            var documentGoupChildExist = db.DocumentGroups.FirstOrDefault(a => a.ParentId.Equals(model.Id));
            if (documentGoupChildExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.DocumentGroup);
            }

            var documentUsed = db.Documents.AsNoTracking().FirstOrDefault(a => a.DocumentGroupId.Equals(model.Id));
            if (documentUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.DocumentGroup);
            }

            try
            {
                db.DocumentGroups.Remove(documentGroupExist);

                var NameOrCode = documentGroupExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<DocumentGroupHistoryModel>(documentGroupExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_DocumentGroup, documentGroupExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        public void CreateDocumentGroup(DocumentGroupModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var documentGroupNameExists = db.DocumentGroups.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (documentGroupNameExists != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DocumentGroup);
            }

            var documentGroupCodeExists = db.DocumentGroups.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (documentGroupCodeExists != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.DocumentGroup);
            }

            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.ParentId = null;
            }

            try
            {
                NTS.Model.Repositories.DocumentGroup documentGroup = new NTS.Model.Repositories.DocumentGroup
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = model.Code,
                    ParentId = model.ParentId,
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.DocumentGroups.Add(documentGroup);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, documentGroup.Name, documentGroup.Id, Constants.LOG_DocumentGroup);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DocumentGroupModel GetDocumentGroup(DocumentGroupModel model)
        {
            var resultInfo = db.DocumentGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DocumentGroupModel
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentGroup);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        public void UpdateDocumentGroup(DocumentGroupModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var documentGroupUpdate = db.DocumentGroups.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (documentGroupUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentGroup);
            }

            var documentGroupNameExist = db.DocumentGroups.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (documentGroupNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DocumentGroup);
            }

            var documentGroupCodeExist = db.DocumentGroups.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (documentGroupCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.DocumentGroup);
            }

            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.ParentId = null;
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<DocumentGroupHistoryModel>(documentGroupUpdate);
            try
            {
                documentGroupUpdate.Name = model.Name;
                documentGroupUpdate.Code = model.Code;
                documentGroupUpdate.ParentId = model.ParentId;
                documentGroupUpdate.Note = model.Note;
                documentGroupUpdate.UpdateBy = model.UpdateBy;
                documentGroupUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<DocumentGroupHistoryModel>(documentGroupUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_DocumentGroup, documentGroupUpdate.Id, documentGroupUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
