using Microsoft.Office.Interop.Outlook;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.ProjectAttch;
using NTS.Model.Projects.ProjectAttch;
using NTS.Model.Repositories;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Exception = System.Exception;

namespace QLTK.Business.ProjectAttch
{
    public class ProjectAttchBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private List<ProjectAttchModel> _documentList = new List<ProjectAttchModel>();

        public SearchResultModel<ProjectAttchModel> GetProjectAttach(ProjectAttchSearchModel modelSearch, string userId)
        {
            List<string> listParentId = new List<string>();
            SearchResultModel<ProjectAttchModel> projectAttch = new SearchResultModel<ProjectAttchModel>();

            var projectAttchModels = (from b in db.ProjectAttaches.AsNoTracking()
                                      where b.ProjectId.Equals(modelSearch.ProjectId)
                                      join u in db.Users.AsNoTracking() on b.CreateBy equals u.Id
                                      join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                      join c in db.Customers.AsNoTracking() on b.CustomerId equals c.Id into bc
                                      from bcn in bc.DefaultIfEmpty()
                                      join s in db.Suppliers.AsNoTracking() on b.SupplierId equals s.Id into bs
                                      from bsn in bs.DefaultIfEmpty()
                                      orderby b.Index
                                      select new ProjectAttchModel
                                      {
                                          Id = b.Id,
                                          ProjectId = b.ProjectId,
                                          Name = b.Name,
                                          Description = b.Description,
                                          FileName = b.FileName,
                                          FileSize = b.FileSize,
                                          Path = b.Path,
                                          Note = b.Note,
                                          CreateByName = e.Name,
                                          CreateDate = b.CreateDate,
                                          Type = b.Type,
                                          PromulgateDate = b.PromulgateDate,
                                          PromulgateType = b.PromulgateType,
                                          GroupName = b.GroupName,
                                          PromulgateCode = b.PromulgateType == Constants.ProjectAttach_PromulgateType_Customer ? (bcn != null ? bcn.Code : string.Empty) : (bsn != null ? bsn.Code : string.Empty),
                                          PromulgateName = b.PromulgateType == Constants.ProjectAttach_PromulgateType_Customer ? (bcn != null ? bcn.Name : string.Empty) : (bsn != null ? bsn.Name : string.Empty),
                                          CustomerId = b.CustomerId,
                                          SupplierId = b.SupplierId,
                                          IsRequired = b.IsRequired,
                                          ParentId = b.ParentId,
                                          PDFLinkFile = b.Path.EndsWith(".pdf") ? b.Path : b.PDFLinkFile,
                                      }).ToList();

            List<string> groupIds = new List<string>();
            groupIds.Add(modelSearch.ParentId);

            List<ProjectDocumentGroup> groupDocument = new List<ProjectDocumentGroup>();
            groupDocument = db.ProjectDocumentGroups.AsNoTracking().Where(r => r.ProjectId.Equals(modelSearch.ProjectId)).ToList();
            if (!string.IsNullOrEmpty(modelSearch.ParentId))
            {

                // Kiểm tra xem nhóm được lựa chọn tìm kiếm là Chủng loại hay Nhóm tài liệu
                var isType = db.ProjectAttachTabTypes.Where(r => r.Id.Equals(modelSearch.ParentId)).Any();

                if (isType)
                {
                    // Lấy danh sách nhóm thuộc chủng loại
                    var groupChildIds = (from r in db.ProjectDocumentGroups.AsNoTracking()
                                         where r.ProjectDocumentTabTypeId.Equals(modelSearch.ParentId) && r.ProjectId.Equals(modelSearch.ProjectId)
                                         select r.Id).ToList();

                    if (groupChildIds.Count > 0)
                    {
                        groupIds.AddRange(groupChildIds);
                    }
                }


                // Đệ quy để lấy tất cả danh sách các nhóm con
                for (int i = 0; i < groupIds.Count; i++)
                {
                    GetListDocument(projectAttchModels, groupDocument, groupIds[i]);
                }
            }
            else
            {
                _documentList = projectAttchModels;
            }

            var permisson = db.ProjectAttachUsers.AsNoTracking();
            var documentTypes = db.ProjectAttachTabTypes.ToList();
            var documentGroup = db.ProjectDocumentGroups.ToList();
            foreach (var item in _documentList.ToList())
            {
                bool isPermission = permisson.Where(r => r.ProjectAttachId.Equals(item.Id) && r.UserId.Equals(userId)).Any();
                if (!isPermission)
                {
                    _documentList.Remove(item);
                }
                else
                {
                    var type = string.IsNullOrEmpty(GetType(documentTypes, documentGroup, item.ParentId)) ? null :
                    GetType(documentTypes, documentGroup, item.ParentId).Substring(0, GetType(documentTypes, documentGroup, item.ParentId).IndexOf('/') == -1 ?
                    (GetType(documentTypes, documentGroup, item.ParentId).Length) : GetType(documentTypes, documentGroup, item.ParentId).IndexOf('/'));
                    item.GroupName = type != null ? type + "/" + (groupDocument.Where(r => r.Id.Equals(item.ParentId)).FirstOrDefault() != null ? groupDocument.Where(r => r.Id.Equals(item.ParentId)).FirstOrDefault().GroupStructure : String.Empty) : String.Empty;
                }
            }
            projectAttch.ListResult = _documentList;
            return projectAttch;
        }

        public ProjectAttchModel GetProjectAttachInfo(ProjectAttchSearchModel searchModel, string userId)
        {
            ProjectAttchModel projectAttch = new ProjectAttchModel();

            var projectAttchModel = (from b in db.ProjectAttaches.AsNoTracking()
                                     where b.Id.Equals(searchModel.Id)
                                     join u in db.Users.AsNoTracking() on b.CreateBy equals u.Id
                                     join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                     join c in db.Customers.AsNoTracking() on b.CustomerId equals c.Id into bc
                                     from bcn in bc.DefaultIfEmpty()
                                     join s in db.Suppliers.AsNoTracking() on b.SupplierId equals s.Id into bs
                                     from bsn in bs.DefaultIfEmpty()
                                     orderby b.Index
                                     select new ProjectAttchModel
                                     {
                                         Id = b.Id,
                                         ProjectId = b.ProjectId,
                                         Name = b.Name,
                                         Description = b.Description,
                                         FileName = b.FileName,
                                         FileSize = b.FileSize,
                                         Path = b.Path,
                                         Note = b.Note,
                                         CreateByName = e.Name,
                                         CreateDate = b.CreateDate,
                                         Type = b.Type,
                                         PromulgateDate = b.PromulgateDate,
                                         PromulgateType = b.PromulgateType,
                                         GroupName = b.GroupName,
                                         PromulgateCode = b.PromulgateType == Constants.ProjectAttach_PromulgateType_Customer ? (bcn != null ? bcn.Code : string.Empty) : (bsn != null ? bsn.Code : string.Empty),
                                         PromulgateName = b.PromulgateType == Constants.ProjectAttach_PromulgateType_Customer ? (bcn != null ? bcn.Name : string.Empty) : (bsn != null ? bsn.Name : string.Empty),
                                         CustomerId = b.CustomerId,
                                         SupplierId = b.SupplierId,
                                         IsRequired = b.IsRequired,
                                         ParentId = b.ParentId,
                                     }).FirstOrDefault();


            projectAttch = projectAttchModel;
            //Lấy list user
            projectAttch.ListUser = (from b in db.ProjectAttachUsers.AsNoTracking()
                                     join u in db.Users.AsNoTracking() on b.UserId equals u.Id
                                     join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                     where b.ProjectAttachId.Equals(projectAttch.Id)

                                     select new ProjectAttchUserModel
                                     {
                                         Id = e.Id,
                                         Name = e.Name,
                                         Code = e.Code,
                                         Email = e.Email,
                                         PhoneNumber = e.PhoneNumber,
                                         DepartmentName = db.Departments.FirstOrDefault(a => a.Id.Equals(e.DepartmentId)).Name,
                                         UserId = u.Id,
                                         ProjectAttachId = b.ProjectAttachId,
                                     }).ToList();
            return projectAttch;
        }

        private void GetListDocument(List<ProjectAttchModel> fileAttach, List<ProjectDocumentGroup> groups, string groupId)
        {
            var list = fileAttach.Where(r => !string.IsNullOrEmpty(r.ParentId) && r.ParentId.Equals(groupId)).ToList();

            foreach (var file in list)
            {
                if (!_documentList.Any(r => r.Id.Equals(file.Id)))
                {
                    _documentList.Add(file);
                }
            }
            // Danh sách các nhóm con
            List<ProjectDocumentGroup> childs = groups.Where(r => !string.IsNullOrEmpty(r.ParentId) && r.ParentId.Equals(groupId)).ToList();

            foreach (var item in childs)
            {
                GetListDocument(fileAttach, groups, item.Id);
            }
        }


        public void CheckNameProjectAttach(ProjectAttchModel model)
        {
            var name = !string.IsNullOrEmpty(model.Name) ? model.Name.NTSTrim().ToUpper() : string.Empty;
            var projectAttach = db.ProjectAttaches.Where(a => a.ProjectId.Equals(model.ProjectId) && a.ParentId.Equals(model.ParentId) && a.Name.ToUpper().Equals(name)).ToList();
            if (projectAttach.Count() > 0)
            {
                throw NTSException.CreateInstance("Tài liệu: " + name + ". Đã tồn tại không thể thêm!");
            }
        }

        public List<string> GetListParent(string id, List<ProjectAttachTabType> saleProjectTypes)
        {
            List<string> listChild = new List<string>();
            var moduleGroup = saleProjectTypes.Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(moduleGroup);
            if (moduleGroup.Count > 0)
            {
                foreach (var item in moduleGroup)
                {
                    listChild.AddRange(GetListParent(item, saleProjectTypes));
                }
            }
            return listChild;
        }

        public string ExportExcelProjectAttach(string projectId, string userId)
        {
            var dataQuey = (from b in db.ProjectAttaches.AsNoTracking()
                            where b.ProjectId.Equals(projectId)
                            join u in db.Users.AsNoTracking() on b.CreateBy equals u.Id
                            join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                            join c in db.Customers.AsNoTracking() on b.CustomerId equals c.Id into bc
                            from bcn in bc.DefaultIfEmpty()
                            join s in db.Suppliers.AsNoTracking() on b.SupplierId equals s.Id into bs
                            from bsn in bs.DefaultIfEmpty()
                            join m in db.ProjectAttachTabTypes.AsNoTracking() on b.ParentId equals m.Id into am
                            from acn in am.DefaultIfEmpty()
                            orderby b.GroupName ascending, b.Name ascending, b.PromulgateDate ascending, b.CreateDate ascending
                            select new ProjectAttchModel
                            {
                                Id = b.Id,
                                ProjectId = b.ProjectId,
                                Name = b.Name,
                                Description = b.Description,
                                FileName = b.FileName,
                                FileSize = b.FileSize,
                                Path = b.Path,
                                Note = b.Note,
                                CreateByName = e.Name,
                                CreateDate = b.CreateDate,
                                Type = b.Type,
                                PromulgateDate = b.PromulgateDate,
                                PromulgateType = b.PromulgateType,
                                GroupName = b.GroupName,
                                PromulgateCode = b.PromulgateType == Constants.ProjectAttach_PromulgateType_Customer ? (bcn != null ? bcn.Code : string.Empty) : (bsn != null ? bsn.Code : string.Empty),
                                PromulgateName = b.PromulgateType == Constants.ProjectAttach_PromulgateType_Customer ? (bcn != null ? bcn.Name : string.Empty) : (bsn != null ? bsn.Name : string.Empty),
                                CustomerId = b.CustomerId,
                                SupplierId = b.SupplierId,
                                IsRequired = b.IsRequired,
                                ParentId = b.ParentId,
                            }).AsQueryable();
            var listResult = dataQuey.OrderBy(t => t.Name).ToList();
            List<string> Ids = new List<string>();
            List<ProjectAttchModel> listProjectAttach = new List<ProjectAttchModel>();
            foreach (var item in listResult)
            {
                item.ListUser = (from b in db.ProjectAttachUsers.AsNoTracking()
                                 join u in db.Users.AsNoTracking() on b.UserId equals u.Id
                                 join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                 where b.ProjectAttachId.Equals(item.Id)

                                 select new ProjectAttchUserModel
                                 {
                                     Id = e.Id,
                                     Name = e.Name,
                                     Code = e.Code,
                                     Email = e.Email,
                                     PhoneNumber = e.PhoneNumber,
                                     DepartmentName = db.Departments.FirstOrDefault(a => a.Id.Equals(e.DepartmentId)).Name,
                                     UserId = u.Id,
                                     ProjectAttachId = b.ProjectAttachId,

                                 }).ToList();
                foreach (var user in item.ListUser)
                {
                    if (user.UserId.Equals(userId))
                    {
                        Ids.Add(item.Id);
                        break;
                    }
                }
            }
            foreach (var item in listResult)
            {
                foreach (var id in Ids)
                {
                    if (item.Id.Equals(id))
                    {
                        listProjectAttach.Add(item);
                    }
                }
            }
            //try
            //{
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProjectAttach.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = listProjectAttach.Count;

            //tất cả chủng loại tài liệu
            var documentTypes = db.ProjectAttachTabTypes.ToList();
            var documentGroup = db.ProjectDocumentGroups.ToList();


            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
            listProjectAttach = listProjectAttach.OrderByDescending(a => a.Type).ToList();
            if (listProjectAttach.Count() == 0)
            {
                throw NTSException.CreateInstance("Không có tài liệu để export!");
            }
            var listExport = listProjectAttach.Select((a, i) => new
            {
                Index = i + 1,
                type = string.IsNullOrEmpty(GetType(documentTypes, documentGroup, a.ParentId)) ? null :
                GetType(documentTypes, documentGroup, a.ParentId).Substring(0, GetType(documentTypes, documentGroup, a.ParentId).IndexOf('/') == -1 ?
                (GetType(documentTypes, documentGroup, a.ParentId).Length) : GetType(documentTypes, documentGroup, a.ParentId).IndexOf('/')),
                GroupName = string.IsNullOrEmpty(GetType(documentTypes, documentGroup, a.ParentId)) ? null :
                GetType(documentTypes, documentGroup, a.ParentId).Substring(GetType(documentTypes, documentGroup, a.ParentId).IndexOf('/') == -1 ?
                (GetType(documentTypes, documentGroup, a.ParentId).Length) :
                GetType(documentTypes, documentGroup, a.ParentId).IndexOf('/') + 1),
                a.Name,
                a.PromulgateCode,
                a.PromulgateName,
                a.PromulgateDate,
                a.Description,
                a.FileName,
                size = string.IsNullOrEmpty(String.Format("{0:0.000}", ((a.FileSize / 1024) / 1024))) ? "0 MB" : String.Format("{0:0.000}", ((a.FileSize / 1024) / 1024)) + " MB",
                createDate = a.CreateDate.Date.ToStringDDMMYY(),
                a.CreateByName,
            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders.Color = ExcelKnownColors.Black;
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách tài liệu dự án" + ".xlsx");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách tài liệu dự án" + ".xlsx";

            return resultPathClient;
        }

        public string GetType(List<ProjectAttachTabType> pTypes, List<ProjectDocumentGroup> pDocs, string Id)
        {
            var TabType = pTypes.FirstOrDefault(a => a.Id.Equals(Id));
            if (TabType != null)
            {
                return TabType.Name;
            }
            var DocType = pDocs.FirstOrDefault(a => a.Id.Equals(Id));
            if (DocType != null)
            {
                return GetType(pTypes, pDocs, DocType.ProjectDocumentTabTypeId) + "/" + DocType.Name;
            }
            return null;
        }

        public void ImportFileProjectAttach(string userId, HttpPostedFile file, string projectId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }


            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            ProjectAttach documenAttach;
            try
            {
                var listCustomer = db.Customers.ToList();
                var listSupplier = db.Suppliers.ToList();
                int countDuplicateDb = 0;
                int countDuplicateL = 0;
                int index = 1;
                bool checkDate = true;
                List<ProjectAttach> projectAttches = new List<ProjectAttach>();
                List<ProjectAttachUser> projectAttcheUsers = new List<ProjectAttachUser>();
                List<ProjectDocumentGroup> projectDocumentGroups = new List<ProjectDocumentGroup>();

                Dictionary<string, string> dictionaryGroups = new Dictionary<string, string>();
                var projectDocumentGroupsx = db.ProjectDocumentGroups.Where(a => a.ProjectId.Equals(projectId)).ToList();
                foreach (var item in projectDocumentGroupsx)
                {
                    if (!string.IsNullOrEmpty(item.GroupStructure))
                    {

                        if (!dictionaryGroups.ContainsKey(item.GroupStructure))
                        {
                            dictionaryGroups.Add(item.GroupStructure, item.Id);
                        }
                    }
                }

                var projectDocTypes = db.ProjectAttachTabTypes.ToList();
                string parentId;
                string groupStructure;
                string parentName;

                for (int i = 2; i <= rowCount; i++)
                {
                    documenAttach = new ProjectAttach();
                    if (string.IsNullOrEmpty(sheet[i, 1].Value) && string.IsNullOrEmpty(sheet[i, 2].Value) && string.IsNullOrEmpty(sheet[i, 3].Value)
                        && string.IsNullOrEmpty(sheet[i, 4].Value) && string.IsNullOrEmpty(sheet[i, 5].Value) && string.IsNullOrEmpty(sheet[i, 6].Value)
                        && string.IsNullOrEmpty(sheet[i, 7].Value) && string.IsNullOrEmpty(sheet[i, 8].Value))
                    {
                        continue;
                    }

                    documenAttach.Id = Guid.NewGuid().ToString();
                    documenAttach.ProjectId = projectId;

                    if (!string.IsNullOrEmpty(sheet[i, 4].Value.Trim()))
                    {
                        documenAttach.Name = sheet[i, 4].Value.Trim();
                    }
                    documenAttach.Index = index;
                    documenAttach.Description = sheet[i, 7].Value.Trim();
                    documenAttach.CreateBy = userId;
                    documenAttach.CreateDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(sheet[i, 5].Value.Trim()))
                    {
                        if (sheet[i, 5].Value.Trim().ToUpper().Equals("KHÁCH HÀNG"))
                        {
                            documenAttach.PromulgateType = 1;
                        }
                        else if (sheet[i, 5].Value.Trim().ToUpper().Equals("NHÀ CUNG CẤP"))
                        {
                            documenAttach.PromulgateType = 2;
                        }
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 2].Value.Trim()))
                    {
                        var projectDocmentType = sheet[i, 2].Value.Trim();

                        // Lấy thông tin chủng loại tài liệu
                        var documentType = projectDocTypes.FirstOrDefault(a => a.Name.Equals(projectDocmentType));

                        if (documentType != null)
                        {

                            if (!string.IsNullOrEmpty(sheet[i, 3].Value.Trim()))
                            {
                                var group = sheet[i, 3].Value.Trim();

                                dictionaryGroups.TryGetValue(group, out parentId);

                                if (parentId != null)
                                {
                                    documenAttach.ParentId = parentId;
                                }
                                else
                                {
                                    var arr = group.Split('/');
                                    groupStructure = string.Empty;

                                    for (int j = 0; j < arr.Length; j++)
                                    {
                                        parentName = groupStructure;

                                        if (j > 0)
                                        {
                                            groupStructure = groupStructure + "/" + arr[j].Trim();
                                        }
                                        else
                                        {
                                            groupStructure = arr[j].Trim();
                                        }

                                        dictionaryGroups.TryGetValue(groupStructure, out parentId);

                                        // Trường hợp chưa có nhóm thì đưa vào danh sách để thêm nhóm
                                        if (parentId == null)
                                        {
                                            dictionaryGroups.TryGetValue(parentName, out parentId);

                                            ProjectDocumentGroup projectDocumentGroup = new ProjectDocumentGroup();
                                            projectDocumentGroup.Id = Guid.NewGuid().ToString();
                                            projectDocumentGroup.Name = arr[j].Trim();
                                            projectDocumentGroup.ProjectId = projectId;
                                            projectDocumentGroup.ProjectDocumentTabTypeId = documentType.Id;
                                            projectDocumentGroup.GroupStructure = groupStructure;
                                            projectDocumentGroup.ParentId = parentId;

                                            projectDocumentGroups.Add(projectDocumentGroup);
                                            dictionaryGroups.Add(groupStructure, projectDocumentGroup.Id);
                                        }
                                        if (j == arr.Length - 1)
                                        {
                                            dictionaryGroups.TryGetValue(groupStructure, out parentId);
                                            documenAttach.ParentId = parentId;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Trường hợp ko có nhóm thì gán vào nhóm Chủng loại tài liệu
                                documenAttach.ParentId = documentType.Id;
                            }
                        }
                        else
                        {
                            throw NTSException.CreateInstance($"Chủng loại tài liệu dòng <{i}> không đúng định dạng!");
                        }
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 8].Value))
                    {
                        if (documenAttach.PromulgateType == 1)
                        {
                            var customer = listCustomer.Where(x => x.Code.Equals(sheet[i, 8].Value)).FirstOrDefault();
                            if (customer != null)
                            {
                                documenAttach.CustomerId = customer.Id;
                            }
                            else
                            {
                                documenAttach.CustomerId = null;
                            }
                        }
                        else if (documenAttach.PromulgateType == 2)
                        {
                            var supplier = listSupplier.Where(x => x.Code.Equals(sheet[i, 8].Value)).FirstOrDefault();
                            if (supplier != null)
                            {
                                documenAttach.SupplierId = supplier.Id;
                            }
                            else
                            {
                                documenAttach.SupplierId = null;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 6].Value))
                    {
                        try
                        {
                            documenAttach.PromulgateDate = DateTime.Parse(sheet[i, 6].Value);
                        }
                        catch (Exception)
                        {
                            checkDate = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 9].Value))
                    {
                        var status = sheet[i, 9].Value;
                        if (status.ToUpper().Equals("CÓ"))
                        {
                            documenAttach.IsRequired = true;
                        }
                        else if (status.ToUpper().Equals("KHÔNG"))
                        {
                            documenAttach.IsRequired = false;
                        }
                    }
                    else
                    {
                        documenAttach.IsRequired = false;
                    }
                    #endregion
                    if (string.IsNullOrEmpty(sheet[i, 2].Value))
                    {
                        throw NTSException.CreateInstance($"Loại tài liệu dòng <{i}> không đươc để trống!");
                        //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                    }
                    if (string.IsNullOrEmpty(documenAttach.Name))
                    {
                        throw NTSException.CreateInstance($"Tên tài liệu dòng <{i}> không đươc để trống!");
                        //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                    }
                    else
                    {
                        var projectAttachDb = db.ProjectAttaches.Where(pa => pa.ProjectId.Equals(documenAttach.ProjectId) && pa.ParentId.Equals(documenAttach.ParentId)).ToList();
                        var projectAttachL = projectAttches.Where(pa => pa.ProjectId.Equals(documenAttach.ProjectId) && pa.ParentId.Equals(documenAttach.ParentId)).ToList();
                        countDuplicateDb = projectAttachDb.Where(po => po.Name.ToUpper().Equals(documenAttach.Name.ToUpper())).ToList().Count();
                        countDuplicateL = projectAttachL.Where(po => po.Name.ToUpper().Equals(documenAttach.Name.ToUpper())).ToList().Count();
                        if (countDuplicateDb > 0 || countDuplicateL > 0)
                        {
                            throw NTSException.CreateInstance($"Tên tài liệu dòng <{i}> đã tồn tại!");
                        }
                    }

                    if (string.IsNullOrEmpty(sheet[i, 5].Value))
                    {
                        throw NTSException.CreateInstance($"Loại đơn vị ban hành dòng <{i}> không đươc để trống!");
                        //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                    }
                    else if (documenAttach.PromulgateType != 1 && documenAttach.PromulgateType != 2)
                    {
                        throw NTSException.CreateInstance($"Loại đơn vị ban hành dòng <{i}> không đúng định dạng!");
                    }
                    if (!checkDate)
                    {
                        throw NTSException.CreateInstance($"Ngày ban hành dòng <{i}> không đúng định dạng!");
                    }

                    if (documenAttach.PromulgateType == 1 && string.IsNullOrEmpty(documenAttach.CustomerId) && !string.IsNullOrEmpty(sheet[i, 8].Value))
                    {
                        throw NTSException.CreateInstance($"Mã ban hành dòng <{i}> không tìm thấy!");
                    }

                    if (documenAttach.PromulgateType == 2 && string.IsNullOrEmpty(documenAttach.SupplierId) && !string.IsNullOrEmpty(sheet[i, 8].Value))
                    {
                        throw NTSException.CreateInstance($"Mã ban hành dòng <{i}> không tìm thấy!");
                    }
                    ProjectAttachUser pau = new ProjectAttachUser();
                    pau.Id = Guid.NewGuid().ToString();
                    pau.UserId = userId;
                    pau.ProjectAttachId = documenAttach.Id;
                    projectAttcheUsers.Add(pau);
                    projectAttches.Add(documenAttach);
                    index++;
                }

                db.ProjectDocumentGroups.AddRange(projectDocumentGroups);
                db.ProjectAttaches.AddRange(projectAttches);
                db.ProjectAttachUsers.AddRange(projectAttcheUsers);
                db.SaveChanges();
            }
            catch (NTSException ntsEx)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ntsEx;
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
        }

        public string GetGroupInTemplate()
        {
            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/TaiLieu_Import_Template.xlsx";

            return resultPathClient;
        }

        public void AddType(ProjectAttchTabTypeModel model)
        {
            var PTabTypes = db.ProjectAttachTabTypes.ToList();
            var PDocTypes = db.ProjectDocumentGroups.ToList();

            bool isType = PTabTypes.Where(r => r.Id.Equals(model.ParentId)).Any();
            bool isExisted;

            if (isType)
            {
                isExisted = PDocTypes.Where(r => r.ProjectId.Equals(model.ProjectId) && r.ProjectDocumentTabTypeId.Equals(model.ParentId) && r.Name.ToUpper().Equals(model.Name.ToUpper())).Any();
            }
            else
            {
                isExisted = PDocTypes.Where(r => r.ProjectId.Equals(model.ProjectId) && model.ParentId.Equals(r.ParentId) && r.Name.ToUpper().Equals(model.Name.ToUpper())).Any();
            }

            if (isExisted)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, "Tên nhóm ");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProjectDocumentGroup projectDocumentGroup = new ProjectDocumentGroup();
                    projectDocumentGroup.Id = Guid.NewGuid().ToString();
                    projectDocumentGroup.Name = model.Name.Trim();
                    projectDocumentGroup.ProjectId = model.ProjectId;

                    if (isType)
                    {
                        projectDocumentGroup.GroupStructure = model.Name.Trim();
                        projectDocumentGroup.ProjectDocumentTabTypeId = model.ParentId;
                    }
                    else
                    {
                        projectDocumentGroup.ParentId = model.ParentId;

                        var parentGroup = db.ProjectDocumentGroups.FirstOrDefault(a => a.Id.Equals(model.ParentId));

                        projectDocumentGroup.GroupStructure = parentGroup.GroupStructure + "/" + model.Name.Trim();
                        projectDocumentGroup.ProjectDocumentTabTypeId = parentGroup.ProjectDocumentTabTypeId;

                    }
                    db.ProjectDocumentGroups.Add(projectDocumentGroup);
                    UserLogUtil.LogHistotyAdd(db, "", projectDocumentGroup.Name, projectDocumentGroup.Id, Constants.LOG_SaleProductTypes);

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

        public void AddProjectAttach(ProjectAttchInfoModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.JuridicalFiles.Count > 0)
                    {
                        int index = 1;
                        List<ProjectAttach> listFileEntity = new List<ProjectAttach>();
                        ProjectAttach projectAttach;
                        List<string> AttachNames = new List<string>();
                        foreach (var item in model.JuridicalFiles)
                        {
                            int count = 0;
                            var check = AttachNames.FirstOrDefault(a => item.Name.Equals(a));
                            if (check == null)
                            {
                                AttachNames.Add(item.Name);
                            }
                            else
                            {
                                count++;
                            }
                            if (string.IsNullOrEmpty(item.Id))
                            {
                                projectAttach = new ProjectAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectId = model.ProjectId,
                                    Name = !string.IsNullOrEmpty(item.Name) ? item.Name.NTSTrim().ToUpper() : string.Empty,
                                    Index = index,
                                    Description = item.Description,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    Path = item.Path,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now,
                                    PromulgateType = item.PromulgateType,
                                    Type = Constants.ProjectAttach_Type_Juridical,
                                    CustomerId = item.CustomerId,
                                    PromulgateDate = item.PromulgateDate,
                                    SupplierId = item.SupplierId,
                                    GroupName = !string.IsNullOrEmpty(item.GroupName) ? item.GroupName.NTSTrim().ToUpper() : null,
                                    IsRequired = item.IsRequired,
                                    ParentId = item.ParentId,
                                    PDFLinkFile = item.PDFLinkFile,
                                };
                                listFileEntity.Add(projectAttach);

                                if (listFileEntity.Count > 0)
                                {
                                    foreach (var a in listFileEntity)
                                    {
                                        List<ProjectAttachUser> lists = new List<ProjectAttachUser>();
                                        if (!string.IsNullOrEmpty(a.Id))
                                        {
                                            foreach (var b in item.ListUser)
                                            {
                                                if (!b.IsDelete)
                                                {
                                                    NTS.Model.Repositories.ProjectAttachUser projectAttachUser = new NTS.Model.Repositories.ProjectAttachUser()
                                                    {
                                                        Id = Guid.NewGuid().ToString(),
                                                        ProjectAttachId = projectAttach.Id,
                                                        UserId = b.Id
                                                    };

                                                    db.ProjectAttachUsers.Add(projectAttachUser);
                                                }
                                            }
                                        }

                                        if (model.CreateBy != null)
                                        {
                                            var userCreate = lists.FirstOrDefault(cm => cm.ProjectAttachId.Equals(a.Id) && cm.UserId.Equals(model.CreateBy));
                                            if (userCreate == null)
                                            {
                                                NTS.Model.Repositories.ProjectAttachUser projectAttachUser = new NTS.Model.Repositories.ProjectAttachUser()
                                                {
                                                    Id = Guid.NewGuid().ToString(),
                                                    ProjectAttachId = projectAttach.Id,
                                                    UserId = model.CreateBy,
                                                };

                                                db.ProjectAttachUsers.Add(projectAttachUser);
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                projectAttach = db.ProjectAttaches.FirstOrDefault(r => r.Id.Equals(item.Id));
                                if (projectAttach == null)
                                {
                                    continue;
                                }

                                if (item.IsDelete)
                                {
                                    var projectAttchesUser = db.ProjectAttachUsers.Where(a => a.ProjectAttachId.Equals(item.Id)).ToList();
                                    db.ProjectAttachUsers.RemoveRange(projectAttchesUser);
                                    db.ProjectAttaches.Remove(projectAttach);
                                }
                                else
                                {
                                    projectAttach.Name = !string.IsNullOrEmpty(item.Name) ? item.Name.NTSTrim().ToUpper() : string.Empty;
                                    projectAttach.Index = index;
                                    projectAttach.Description = item.Description;
                                    if (string.Compare(item.Path, projectAttach.Path) != 0)
                                    {
                                        projectAttach.FileName = item.FileName;
                                        projectAttach.FileSize = item.FileSize;
                                        projectAttach.Path = item.Path;
                                        projectAttach.CreateBy = model.CreateBy;
                                        projectAttach.CreateDate = DateTime.Now;
                                        projectAttach.PDFLinkFile = item.PDFLinkFile;
                                    }

                                    projectAttach.PromulgateType = item.PromulgateType;
                                    projectAttach.CustomerId = item.CustomerId;
                                    projectAttach.PromulgateDate = item.PromulgateDate;
                                    projectAttach.SupplierId = item.SupplierId;
                                    projectAttach.GroupName = !string.IsNullOrEmpty(item.GroupName) ? item.GroupName.NTSTrim().ToUpper() : null;
                                    projectAttach.IsRequired = item.IsRequired;
                                    projectAttach.ParentId = item.ParentId;

                                    if (item.ListUser != null && count == 0)
                                    {
                                        foreach (var a in item.ListUser)
                                        {
                                            var pju = db.ProjectAttachUsers.FirstOrDefault(b => b.UserId.Equals(a.UserId) && b.ProjectAttachId.Equals(a.ProjectAttachId));
                                            if (a.IsDelete)
                                            {
                                                if (pju != null)
                                                {
                                                    db.ProjectAttachUsers.Remove(pju);
                                                }
                                            }
                                            else
                                            {
                                                if (pju == null)
                                                {
                                                    NTS.Model.Repositories.ProjectAttachUser projectAttachUser = new NTS.Model.Repositories.ProjectAttachUser()
                                                    {
                                                        Id = Guid.NewGuid().ToString(),
                                                        ProjectAttachId = item.Id,
                                                        UserId = a.Id
                                                    };

                                                    db.ProjectAttachUsers.Add(projectAttachUser);
                                                }
                                            }
                                        }


                                    }



                                }
                            }
                            index++;
                        }

                        db.ProjectAttaches.AddRange(listFileEntity);
                    }


                    //UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Project, model.ProjectId, string.Empty, "Tài liệu");

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

        public void DeleteProjectAttach(ProjectAttchModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var projectAttach = db.ProjectAttaches.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (projectAttach == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
                    }
                    var projectAttchesUser = db.ProjectAttachUsers.Where(a => a.ProjectAttachId.Equals(model.Id)).ToList();
                    db.ProjectAttachUsers.RemoveRange(projectAttchesUser);

                    db.ProjectAttaches.Remove(projectAttach);
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

        public SearchResultModel<ProjectAttchModel> SearchDocumentFile(ProjectAttchModel searchModel)
        {
            SearchResultModel<ProjectAttchModel> searchResult = new SearchResultModel<ProjectAttchModel>();

            var dataQuery = (from a in db.ProjectAttaches.AsNoTracking()
                             where a.Id.Equals(searchModel.Id)
                             select new ProjectAttchModel
                             {
                                 Id = a.Id,
                                 ProjectId = a.ProjectId,
                                 FileName = a.FileName,
                                 Description = a.Description,
                                 Path = a.Path,
                                 PDFLinkFile = a.PDFLinkFile,
                                 Name = a.Name
                             }).AsQueryable();


            var listDocumenFile = dataQuery.ToList();

            searchResult.ListResult.AddRange(listDocumenFile);


            searchResult.TotalItem = searchResult.ListResult.Count();
            return searchResult;
        }

        public ProjectAttchResultModel GetAttachProject(String id)
        {

            var dataQuey = (from b in db.ProjectAttaches.AsNoTracking()
                            where b.Id.Equals(id)
                            select new ProjectAttchResultModel
                            {
                                Id = b.Id,
                                ProjectId = b.ProjectId,
                                Name = b.Name,
                                Description = b.Description,
                                FileName = b.FileName,
                                FileSize = b.FileSize,
                                Path = b.Path,
                                Note = b.Note,
                                CreateDate = b.CreateDate,
                                Type = b.Type,
                                PromulgateDate = b.PromulgateDate,
                                PromulgateType = b.PromulgateType,
                                GroupName = b.GroupName,
                                CustomerId = b.CustomerId,
                                SupplierId = b.SupplierId,
                                IsRequired = b.IsRequired,
                                ParentId = b.ParentId,
                            }).FirstOrDefault();

            return dataQuey;

        }

        public string GetIdType(List<ProjectAttachTabType> pTypes, List<ProjectDocumentGroup> pDocs, string Id)
        {
            var TabType = pTypes.FirstOrDefault(a => a.Id.Equals(Id));
            if (TabType != null)
            {
                return TabType.Id;
            }
            var DocType = pDocs.FirstOrDefault(a => a.Id.Equals(Id));
            if (DocType != null)
            {
                return GetType(pTypes, pDocs, DocType.ProjectDocumentTabTypeId);
            }
            return null;
        }

        public object GetTypeInfo(ProjectAttchTabTypeModel model)
        {
            var resultInfo = db.ProjectAttachTabTypes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProjectAttchTabTypeModel
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Code = p.Code,
                Name = p.Name,
            }).FirstOrDefault();
            if (resultInfo != null)
            {
                return resultInfo;
            }

            var projectAttachDocument = db.ProjectDocumentGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).FirstOrDefault();

            ProjectAttchTabTypeModel pj = new ProjectAttchTabTypeModel();
            pj.Id = projectAttachDocument.Id;
            pj.ParentId = projectAttachDocument.ParentId;
            pj.Name = projectAttachDocument.Name;
            if (projectAttachDocument != null)
            {
                if (pj.ParentId == null)
                {
                    pj.ParentId = projectAttachDocument.ProjectDocumentTabTypeId;
                }
                return pj;
            }
            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectAttachTabType);
        }

        public void UpdateType(ProjectAttchTabTypeModel model)
        {
            if (db.ProjectAttachTabTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }
            if (db.ProjectDocumentGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newUnit = db.ProjectAttachTabTypes.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    if (newUnit != null)
                    {
                        newUnit.ParentId = model.ParentId;
                        newUnit.Name = model.Name.Trim();
                        newUnit.Code = !string.IsNullOrEmpty(model.Code) ? model.Code.Trim() : string.Empty;

                        if (string.IsNullOrEmpty(newUnit.ParentId))
                        {
                            newUnit.ParentId = null;
                        }
                    }
                    else
                    {
                        var projectDocumentGroup = db.ProjectDocumentGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                        projectDocumentGroup.ParentId = !string.IsNullOrEmpty(model.ParentId) ? model.ParentId.Trim() : string.Empty;
                        projectDocumentGroup.ProjectDocumentTabTypeId = !string.IsNullOrEmpty(model.ParentId) ? model.ParentId.Trim() : string.Empty;
                        projectDocumentGroup.Name = model.Name.Trim();
                        projectDocumentGroup.GroupStructure = projectDocumentGroup.GroupStructure + "/" + model.Name.Trim();
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

        public void DeleteType(string Id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                //if (db.ProjectAttaches.AsNoTracking().Where(r => !string.IsNullOrEmpty(r.ParentId) && r.ParentId.Equals(Id)).Count() > 0)
                //{
                //    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectAttachTabType);
                //}

                try
                {
                    var unit = db.ProjectAttachTabTypes.FirstOrDefault(u => u.Id.Equals(Id));
                    //if (unit == null)
                    //{
                    //    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
                    //}
                    if (unit != null)
                    {
                        var types = db.ProjectAttachTabTypes.ToList();
                        var projectattch = db.ProjectAttaches.Where(s => s.ParentId.Equals(Id)).ToList();
                        if (projectattch.Count > 0)
                        {
                            foreach (var item in projectattch)
                            {
                                var projectAttchUser = db.ProjectAttachUsers.Where(b => b.ProjectAttachId.Equals(item.Id)).ToList();
                                db.ProjectAttachUsers.RemoveRange(projectAttchUser);
                            }
                        }

                        RemoveChild(types, unit.Id);

                        db.ProjectAttaches.RemoveRange(projectattch);
                        db.ProjectAttachTabTypes.Remove(unit);
                    }
                    else
                    {
                        var projectDoucument = db.ProjectDocumentGroups.FirstOrDefault(u => u.Id.Equals(Id));

                        var types = db.ProjectDocumentGroups.ToList();
                        var projectattch = db.ProjectAttaches.Where(s => s.ParentId.Equals(Id)).ToList();
                        if (projectattch.Count > 0)
                        {
                            foreach (var item in projectattch)
                            {
                                var projectAttchUser = db.ProjectAttachUsers.Where(b => b.ProjectAttachId.Equals(item.Id)).ToList();
                                db.ProjectAttachUsers.RemoveRange(projectAttchUser);
                            }
                        }

                        RemoveChildDocumentGroup(types, projectDoucument.Id);

                        db.ProjectAttaches.RemoveRange(projectattch);
                        db.ProjectDocumentGroups.Remove(projectDoucument);
                    }


                    //var NameOrCode = unit.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ProjectAttchTabTypeModel>(unit);
                    //UserLogUtil.LogHistotyDelete(db, model.Code, Constants.LOG_SaleProductTypes, unit.Id, NameOrCode, jsonBefor);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(Id, ex);
                }
            }
        }

        private void RemoveChild(List<ProjectAttachTabType> types, string parrentId)
        {
            var childs = types.Where(r => parrentId.Equals(r.ParentId)).ToList();

            foreach (var item in childs)
            {
                if (db.ProjectAttachTabTypes.AsNoTracking().Where(r => r.ParentId.Equals(item.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectAttachTabType);
                }

                RemoveChild(types, item.Id);

                var projectattch = db.ProjectAttaches.Where(s => s.ParentId.Equals(item.Id)).ToList();
                if (projectattch.Count > 0)
                {
                    foreach (var ite in projectattch)
                    {
                        var projectAttchUser = db.ProjectAttachUsers.Where(b => b.ProjectAttachId.Equals(ite.Id)).ToList();
                        db.ProjectAttachUsers.RemoveRange(projectAttchUser);
                    }
                }

                db.ProjectAttaches.RemoveRange(projectattch);
                db.ProjectAttachTabTypes.Remove(item);
            }
        }

        private void RemoveChildDocumentGroup(List<ProjectDocumentGroup> types, string parrentId)
        {
            var childs = types.Where(r => parrentId.Equals(r.ParentId)).ToList();

            foreach (var item in childs)
            {
                if (db.ProjectDocumentGroups.AsNoTracking().Where(r => r.ParentId.Equals(item.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectAttachTabType);
                }

                RemoveChildDocumentGroup(types, item.Id);

                var projectattch = db.ProjectAttaches.Where(s => s.ParentId.Equals(item.Id)).ToList();
                if (projectattch.Count > 0)
                {
                    foreach (var ite in projectattch)
                    {
                        var projectAttchUser = db.ProjectAttachUsers.Where(b => b.ProjectAttachId.Equals(ite.Id)).ToList();
                        db.ProjectAttachUsers.RemoveRange(projectAttchUser);
                    }
                }

                db.ProjectAttaches.RemoveRange(projectattch);
                db.ProjectDocumentGroups.Remove(item);
            }
        }


    }
}
