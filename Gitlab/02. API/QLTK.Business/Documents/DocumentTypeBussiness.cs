using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DocumentType;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.DocumentType
{
    public class DocumentTypeBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        /// <summary>
        /// Tìm kiếm loại tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<DocumentTypeSearchResultModel> SearchDocumentType(DocumentTypeSearchModel searchModel)
        {
            SearchResultModel<DocumentTypeSearchResultModel> searchResult = new SearchResultModel<DocumentTypeSearchResultModel>();
            var dataQuery = (from a in db.DocumentTypes.AsNoTracking()
                             select new DocumentTypeSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Description = a.Description,
                                 Code = a.Code
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()) || u.Code.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            //if (!string.IsNullOrEmpty(searchModel.Code))
            //{
            //    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            //}

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        /// <summary>
        /// Xóa loại tài liệu
        /// </summary>
        /// <param name="model"></param>
        public void DeleteDocumentType(DocumentTypeModel model)
        {
            var documentTypeExist = db.DocumentTypes.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (documentTypeExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentType);
            }


            var documentUsed = db.Documents.AsNoTracking().FirstOrDefault(a => a.DocumentTypeId.Equals(model.Id));
            if (documentUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.DocumentType);
            }

            try
            {
                db.DocumentTypes.Remove(documentTypeExist);

                var NameOrCode = documentTypeExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<DocumentTypeHistoryModel>(documentTypeExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_DocumentType, documentTypeExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới loại tài liệu
        /// </summary>
        /// <param name="model"></param>
        public void CreateDocumentType(DocumentTypeModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var DocumentTypeNameExits = db.DocumentTypes.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (DocumentTypeNameExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DocumentType);
            }

            var DocumentTypeCodeExits = db.DocumentTypes.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (DocumentTypeCodeExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.DocumentType);
            }

            try
            {
                NTS.Model.Repositories.DocumentType DocumentType = new NTS.Model.Repositories.DocumentType
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = model.Code,
                    Name = model.Name.NTSTrim(),
                    Description = model.Description,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.DocumentTypes.Add(DocumentType);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, DocumentType.Name, DocumentType.Id, Constants.LOG_DocumentType);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin loại tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DocumentTypeModel GetDocumentType(DocumentTypeModel model)
        {
            var resultInfo = db.DocumentTypes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DocumentTypeModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentType);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật loại tài liệu
        /// </summary>
        /// <param name="model"></param>
        public void UpdateDocumentType(DocumentTypeModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var documentTypeUpdate = db.DocumentTypes.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (documentTypeUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentType);
            }

            var documentTypeNameExist = db.DocumentTypes.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (documentTypeNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DocumentType);
            }

            var documentTypeCodeExist = db.DocumentTypes.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (documentTypeCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.DocumentType);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<DocumentTypeHistoryModel>(documentTypeUpdate);
            try
            {
                documentTypeUpdate.Name = model.Name;
                documentTypeUpdate.Code = model.Code;
                documentTypeUpdate.Description = model.Description;
                documentTypeUpdate.UpdateBy = model.UpdateBy;
                documentTypeUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<DocumentTypeHistoryModel>(documentTypeUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_DocumentType, documentTypeUpdate.Id, documentTypeUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
