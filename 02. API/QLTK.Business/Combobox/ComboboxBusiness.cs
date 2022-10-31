using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.CustomerContact;
using NTS.Model.Customers;
using NTS.Model.DocumentGroups;
using NTS.Model.FlowStage;
using NTS.Model.Product;
using NTS.Model.ProductGroup;
using NTS.Model.ProjectProducts;
using NTS.Model.QLTKGROUPMODUL;
using NTS.Model.QLTKMG;
using NTS.Model.QLTKMODULE;
using NTS.Model.QuestionGroup;
using NTS.Model.Repositories;
using NTS.Model.SalaryLevel;
using NTS.Model.ProjectPhase;
using System;
using System.Collections.Generic;
using System.Linq;
using NTS.Model.Recruitments.Interviews;
using NTS.Model.Employees;
using NTS.Model.GroupJob;
using NTS.Model.GroupSupplier;
using NTS.Model.GroupManufacture;

namespace NTS.Business.Combobox
{
    public class ComboboxBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public List<ComboboxResult> GetCbbEmployees()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId into ab
                                 from abn in ab.DefaultIfEmpty()
                                 where a.Status == Constants.Employee_Status_Use
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.PhoneNumber,
                                     Code = a.Code,
                                     ObjectId = abn == null ? "" : abn.Id
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetCbbEmployeeByDepartmentId(string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();

            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 where (string.IsNullOrEmpty(departmentId) || a.DepartmentId.Equals(departmentId)) && a.Status == Constants.Employee_Status_Use
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();

                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }

            return searchResult;
        }

        public List<ComboboxResult> GetCbbEmployeesStatus()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 where a.Status == 1
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListGroupModule()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ModuleGroups.AsNoTracking()
                                 where a.ParentId.Length == 0
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListParentMaterialGroup(QLTKMGModel model)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.MaterialGroups.AsNoTracking()
                                 where string.IsNullOrEmpty(a.ParentId) && (!a.Id.Equals(model.Id))
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListMaterialGroup()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.MaterialGroups.AsNoTracking()
                                 join b in db.MaterialGroupTPAs.AsNoTracking() on a.MaterialGroupTPAId equals b.Id
                                 orderby a.Code
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     ParentId = a.ParentId,
                                     Exten = b.Name
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListMaterialGroupParent()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.MaterialGroups.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     ParentId = a.ParentId
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetCbbDepartmentBySBUId(string sbuId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                if (sbuId == "null" || string.IsNullOrEmpty(sbuId))
                {
                    var ListModel = (from a in db.Departments.AsNoTracking()
                                     orderby a.Name
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }
                else
                {
                    var ListModel = (from a in db.Departments.AsNoTracking()
                                     where string.IsNullOrEmpty(sbuId) || a.SBUId.Equals(sbuId)
                                     orderby a.Name
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListDepartment()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Departments.AsNoTracking()
                                 orderby a.Name
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListDepartmentUse()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Departments.AsNoTracking()
                                 where Constants.Department_Status_Use.Equals(a.Status)
                                 orderby a.Name
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListManufacture()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Manufactures.AsNoTracking()
                                 orderby a.Code
                                 where a.Status != "1"
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     Exten = a.MaterialType,
                                     LeadTime = a.LeadTime,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListIndustry()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Industries.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxMultilevelResult> GetGroupModule()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.ModuleGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     ParentId = a.ParentId
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public List<ComboboxMultilevelResult> GetGroupProduct()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.ProductGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     ParentId = a.ParentId
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxIndexResult> GetListJobPosition()
        {
            List<ComboboxIndexResult> searchResult = new List<ComboboxIndexResult>();
            try
            {
                var ListModel = (from a in db.JobPositions.AsNoTracking()
                                 orderby a.Index
                                 select new ComboboxIndexResult()
                                 {
                                     Id = a.Id,
                                     Index = a.Index,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxIndexResult addFirstIndex = new ComboboxIndexResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.JobPositions.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxIndexResult addIndex = new ComboboxIndexResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Index = (maxIndex + 1);
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }

            return searchResult;
        }

        public List<ComboboxIndexResult> GetCBBJobPosition()
        {
            List<ComboboxIndexResult> searchResult = new List<ComboboxIndexResult>();
            try
            {
                var ListModel = (from a in db.JobPositions.AsNoTracking()
                                 orderby a.Index
                                 select new ComboboxIndexResult()
                                 {
                                     Id = a.Id,
                                     Index = a.Index,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }

            return searchResult;
        }

        public List<ComboboxResult> GetListQualification()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Qualifications.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListGroupUsers(string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var employee = db.Employees.FirstOrDefault(i => i.Id.Equals(departmentId));
                if (employee != null)
                {
                    var ListModel = (from a in db.GroupUsers.AsNoTracking()
                                     where a.DepartmentId.Equals(employee.DepartmentId) && a.IsDisable == Constants.Active
                                     orderby a.Name ascending
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                     }).AsQueryable();
                    searchResult = ListModel.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListModuleErrorTypesParent(string type)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ErrorGroups.AsNoTracking()
                                 where (a.Type.Equals(type))
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListUnit()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Units.AsNoTracking()
                                 orderby a.Index ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Index.ToString(),
                                     Index = a.Index
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxResult addFirstIndex = new ComboboxResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Exten = "1";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.Units.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxResult addIndex = new ComboboxResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Exten = (maxIndex + 1).ToString();
                    addIndex.Index = maxIndex + 1;
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
   
        public object GetCbbModule()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var listGroup = (from o in db.ModuleGroups.AsNoTracking()
                                       orderby o.Code
                                       select new
                                       {
                                           Id = o.Id,
                                           Code = o.Code,
                                           Name = o.Name,
                                           ParentId = o.ParentId,
                                       }).ToList();
                var data = (from a in db.Modules.AsNoTracking()
                            select new
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                ParentId = a.ModuleGroupId,
                            }).ToList();

                listGroup.AddRange(data);
                return listGroup;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
        }
        public object GetCbbProduct()
        {
            try
            {
                var listGroup = (from o in db.ProductGroups.AsNoTracking()
                                       orderby o.Code
                                       select new 
                                       {
                                           Id = o.Id,
                                           Code = o.Code,
                                           Name = o.Name,
                                           ParentId = o.ParentId,
                                       }).ToList();
                var data = (from a in db.Products.AsNoTracking()
                            select new
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                ParentId = a.ProductGroupId,
                            }).ToList();

                listGroup.AddRange(data);
                return listGroup;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            
        }

        

        public List<ComboboxResult> GetListModulesByGroup(string groupId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Modules.AsNoTracking()
                                 where a.ModuleGroupId.Equals(groupId)
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListProject()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Projects.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public string GenCodeModuleError()
        {
            var result = string.Empty;
            var systemParam = db.SystemParams.FirstOrDefault(u => u.KeyParam.Equals(Constants.CodeModuleError));
            if (systemParam == null)
            {
                throw new Exception("Chưa cấu hình mã lỗi (SystemParam)");
            }
            try
            {
                var dateNow = DateTime.Now;
                var year = int.Parse(systemParam.ExtenParam);
                var numberCode = 0;
                if (year == dateNow.Year)
                {
                    //sinh số tiếp
                    numberCode = int.Parse(systemParam.ValueParam) + 1;
                }
                else
                {
                    //sinh số về từ đầu
                    numberCode = 1;
                    systemParam.ExtenParam = dateNow.Year.ToString();
                }
                result = dateNow.ToString("yy") + "L." + numberCode.ToString("D4");
                systemParam.ValueParam = numberCode.ToString();
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return result;
        }

        public List<ComboboxResult> GetListManagers()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 orderby a.Code
                                 where a.Status == 1 && a.JobPositionId.Equals("3")
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetEmployeeByDepartment(string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                if (string.IsNullOrEmpty(departmentId) || departmentId == "null")
                {
                    var ListModel = (from a in db.Employees.AsNoTracking()
                                     where a.Status == Constants.Employee_Status_Use
                                     orderby a.Name ascending
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }
                else
                {
                    var ListModel = (from a in db.Employees.AsNoTracking()
                                     where a.DepartmentId.Equals(departmentId) && a.Status == Constants.Employee_Status_Use
                                     orderby a.Name ascending
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetEmployeeByDepartmentWithStatus(string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 where a.DepartmentId.Equals(departmentId) && a.Status == 1
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<TreeNode<ComboboxResult>> GetListDepartmentTreeNode()
        {
            List<TreeNode<ComboboxResult>> searchResult = new List<TreeNode<ComboboxResult>>();
            try
            {
                TreeNode<ComboboxResult> parentNode;
                var ListModel = (from a in db.Departments.AsNoTracking()
                                 where int.Parse(a.Status) == 0
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).ToList();
                foreach (var groupModule in ListModel)
                {
                    parentNode = new TreeNode<ComboboxResult>();
                    parentNode.data = groupModule;
                    searchResult.Add(parentNode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListPracticeGroup()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.PracticeGroups.AsNoTracking()
                                 where a.ParentId.Length == 0
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public object GetListRawMaterial()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.RawMaterials.AsNoTracking()
                                 orderby a.Index ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Index.ToString()
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxResult addFirstIndex = new ComboboxResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Exten = "1";
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.RawMaterials.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxResult addIndex = new ComboboxResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Exten = (maxIndex + 1).ToString();
                    searchResult.Add(addIndex);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public object GetListTPA()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.MaterialGroupTPAs.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListNSMaterialType()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.NSMaterialTypes.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public SearchResultModel<GroupModuleResultModel> GetListModuleGroupParentChild()
        {
            SearchResultModel<GroupModuleResultModel> searchResult = new SearchResultModel<GroupModuleResultModel>();
            try
            {
                var listGroupModule = (from o in db.ModuleGroups.AsNoTracking()
                                       select new GroupModuleResultModel
                                       {
                                           Id = o.Id,
                                           Code = o.Code,
                                           Name = o.Name,
                                           ParentId = o.ParentId

                                       }).AsQueryable();

                List<GroupModuleResultModel> listResult = new List<GroupModuleResultModel>();
                var listParent = listGroupModule.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                bool isSearch = false;
                int index = 1;

                List<GroupModuleResultModel> listChild = new List<GroupModuleResultModel>();

                foreach (var parent in listParent)
                {
                    isSearch = true;


                    listChild = GetModuleGroupChild(parent.Id, listGroupModule.ToList(), index.ToString());
                    if (isSearch || listChild.Count > 0)
                    {
                        parent.Index = index.ToString();
                        listResult.Add(parent);
                        index++;
                    }

                    listResult.AddRange(listChild);
                }

                foreach (var item in listResult)
                {
                    item.IndexView = item.Index + " | " + item.Code + " | " + item.Name;
                }


                searchResult.ListResult = listResult.OrderBy(t => t.Code).ToList();
                searchResult.TotalItem = listResult.Count();
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
            }
            return searchResult;
        }

        public SearchResultModel<GroupJobResultModel> GetListJobGroupParentChild()
        {
            SearchResultModel<GroupJobResultModel> searchResult = new SearchResultModel<GroupJobResultModel>();
            try
            {
                //var listGroupJob = (from o in db.JobGroups.AsNoTracking()
                //                    select new GroupJobResultModel
                //                    {
                //                        Id = o.Id,
                //                        Code = o.Code,
                //                        Name = o.Name,
                //                    }).ToList();

                //listGroupJob.AddRange((from a in db.Jobs.AsNoTracking()
                //                       orderby a.Code
                //                       select new GroupJobResultModel()
                //                       {
                //                           Id = a.Id,
                //                           Code = a.Code,
                //                           Name = a.Name,
                //                           ParentId = a.JobGroupId
                //                       }).ToList());
                var listGroupJob = (from a in db.Jobs.AsNoTracking()
                                    orderby a.Code
                                    select new GroupJobResultModel()
                                    {
                                        Id = a.Id,
                                        Code = a.Code,
                                        Name = a.Name,
                                        // ParentId = a.JobGroupId
                                    }).ToList();


                searchResult.ListResult = listGroupJob.OrderBy(t => t.Code).ToList();
                searchResult.TotalItem = listGroupJob.Count();
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
            }
            return searchResult;
        }

        public SearchResultModel<GroupManufactureResultModel> GetListManufactureGroupParentChild()
        {
            SearchResultModel<GroupManufactureResultModel> searchResult = new SearchResultModel<GroupManufactureResultModel>();
            try
            {
                var listGroupManufacture = (from o in db.Manufactures.AsNoTracking()
                                            where o.Status != "1"
                                            select new GroupManufactureResultModel
                                    {
                                        Id = o.Id,
                                        Code = o.Code,
                                        Name = o.Name,
                                    }).ToList();


                searchResult.ListResult = listGroupManufacture.OrderBy(t => t.Code).ToList();
                searchResult.TotalItem = listGroupManufacture.Count();
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
            }
            return searchResult;
        }

        public SearchResultModel<GroupSupplierResultModel> GetListSupplierGroupParentChild()
        {
            SearchResultModel<GroupSupplierResultModel> searchResult = new SearchResultModel<GroupSupplierResultModel>();
            try
            {
                var listGroupSupplier = (from o in db.Suppliers.AsNoTracking()
                                         where o.Status != "1"
                                         select new GroupSupplierResultModel
                                    {
                                        Id = o.Id,
                                        Code = o.Code,
                                        Name = o.Name,
                                    }).ToList();

                searchResult.ListResult = listGroupSupplier.OrderBy(t => t.Code).ToList();
                searchResult.TotalItem = listGroupSupplier.Count();
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
            }
            return searchResult;
        }

        private List<GroupModuleResultModel> GetModuleGroupChild(string parentId, List<GroupModuleResultModel> listModuleGroup, string index)
        {
            List<GroupModuleResultModel> listResult = new List<GroupModuleResultModel>();
            var listChild = listModuleGroup.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            int indexChild = 1;
            List<GroupModuleResultModel> listChildChild = new List<GroupModuleResultModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetModuleGroupChild(child.Id, listModuleGroup, index + "." + indexChild);
                if (isSearch || listChildChild.Count > 0)
                {
                    child.Index = index + "." + indexChild;
                    listResult.Add(child);
                    indexChild++;
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        public SearchResultModel<ProductGroupResultModel> GetListProductGroupParentChild()
        {
            SearchResultModel<ProductGroupResultModel> searchResult = new SearchResultModel<ProductGroupResultModel>();
            try
            {
                var listProductGroup = (from o in db.ProductGroups.AsNoTracking()
                                        orderby o.Code
                                        select new ProductGroupResultModel
                                        {
                                            Id = o.Id,
                                            Code = o.Code,
                                            Name = o.Name,
                                            ParentId = o.ParentId

                                        }).AsQueryable();

                List<ProductGroupResultModel> listResult = new List<ProductGroupResultModel>();
                var listParent = listProductGroup.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                bool isSearch = false;
                int index = 1;

                List<ProductGroupResultModel> listChild = new List<ProductGroupResultModel>();

                foreach (var parent in listParent)
                {
                    isSearch = true;


                    listChild = GetProductGroupChild(parent.Id, listProductGroup.ToList(), index.ToString());
                    if (isSearch || listChild.Count > 0)
                    {
                        parent.Index = index.ToString();
                        listResult.Add(parent);
                        index++;
                    }

                    listResult.AddRange(listChild);
                }

                foreach (var item in listResult)
                {
                    item.IndexView = item.Index + " | " + item.Code + " | " + item.Name;
                }


                searchResult.ListResult = listResult;
                searchResult.TotalItem = listResult.Count();
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
            }
            return searchResult;
        }

        private List<ProductGroupResultModel> GetProductGroupChild(string parentId,
            List<ProductGroupResultModel> listProductGroup, string index)
        {
            List<ProductGroupResultModel> listResult = new List<ProductGroupResultModel>();
            var listChild = listProductGroup.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            int indexChild = 1;
            List<ProductGroupResultModel> listChildChild = new List<ProductGroupResultModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetProductGroupChild(child.Id, listProductGroup, index + "." + indexChild);
                if (isSearch || listChildChild.Count > 0)
                {
                    child.Index = index + "." + indexChild;
                    listResult.Add(child);
                    indexChild++;
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        public object GetListCriter()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.TestCriteriaGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListProductStandardGroup()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ProductStandardGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
        public object GetListSBU()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.SBUs.AsNoTracking()
                                 orderby a.Name
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
        

        public List<ComboboxResult> GetSolutionTechnologies()
        {
            var tags = (from a in db.SolutionTechnologies.AsNoTracking()
                        orderby a.Name
                        select new ComboboxResult { Name = a.Name }).ToList();

            return tags;
        }

        public object GetListCBBSBU()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.SBUs.AsNoTracking()
                                 orderby a.Name ascending
                                 where a.Status.Equals("0")
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        // Lấy combobox Stage
        public List<ComboboxResult> GetCbbStage()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Stages.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Note
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetCbbRole()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Roles.AsNoTracking()
                                 where a.IsDisable == false
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Index = a.Index,
                                 }).AsQueryable();
                var list = ListModel.OrderBy(a => a.Index).ToList();
                searchResult = list;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public List<ComboboxResult> GetListFunctionGroup()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.FunctionGroups.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
        public object GetQualification()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Qualifications.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetSuppliersByProjectId(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var plans = db.Plans.Where(a => a.ProjectId.Equals(id));
                foreach (var item in plans)
                {
                    var supplier = db.Suppliers.FirstOrDefault(a => a.Id.Equals(item.SupplierId));
                    if (supplier != null)
                    {
                        var data = searchResult.FirstOrDefault(a => a.Id.Equals(supplier.Id));
                        if (data == null)
                        {
                            ComboboxResult combobox = new ComboboxResult();
                            combobox.Id = supplier.Id;
                            combobox.Name = supplier.Name;
                            combobox.Code = supplier.Code;
                            searchResult.Add(combobox);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetGroupUser()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.GroupUsers.AsNoTracking()
                                 orderby a.Name
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListDegree()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Degrees.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListSpecialize()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Specializes.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListWorkPlace()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Workplaces.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
        public object GetListSkillGroup()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.SkillGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        /// <summary>
        /// lấy list combobox loại phòng
        /// </summary>
        /// <returns></returns>
        public object GetListRoomType()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.RoomTypes.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListPracticeGroupA()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.PracticeGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Exten = a.ParentId
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListGroupJob()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.JobGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListClassRoom()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ClassRooms.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListJob()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Jobs.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetApplication()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Applications.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();

                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public object GetListMaterial()
        {
            try
            {
                var listGroup = (from o in db.MaterialGroups.AsNoTracking()
                                 orderby o.Code
                                 select new
                                 {
                                     Id = o.Id,
                                     Code = o.Code,
                                     Name = o.Name,
                                     ParentId = o.ParentId,
                                 }).ToList();
                var data = (from a in db.Materials.AsNoTracking()
                            select new
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                ParentId = a.MaterialGroupId,
                            }).ToList();

                listGroup.AddRange(data);
                return listGroup;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
        }

        public List<ComboboxMultilevelResult> GetListCustomerType()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.CustomerTypes.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxMultilevelResult()
                                 {
                                     ParentId = a.ParentId,
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Type = a.Type
                                 }).AsQueryable();

                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListCustomer()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Customers.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxMultilevelResult> GetListErrorGroup()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.ErrorGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ParentId = a.ParentId,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListModule()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Modules.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListModuleByProjectId(string projectId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Modules.AsNoTracking()
                                 join b in db.Errors.AsNoTracking() on a.Id equals b.ObjectId
                                 join c in db.ProjectErrors.AsNoTracking() on b.Id equals c.ErrorId
                                 where c.ProjectId.Equals(projectId)
                                 orderby a.Code ascending
                                 group a by a.Id into g
                                 select new ComboboxResult()
                                 {
                                     Id = g.Key,
                                     Code = g.FirstOrDefault().Code,
                                     Name = g.FirstOrDefault().Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListManufactureGroup()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ManufactureGroups.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListSupplierGroup()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.SupplierGroups.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetDomain()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.CustomerDomains.AsNoTracking()
                                 join c in db.Jobs.AsNoTracking() on a.JobId equals c.Id into ac
                                 from c in ac.DefaultIfEmpty()
                                 group c by new { a.Id, c.Name, c.Code } into g
                                 select new ComboboxResult()
                                 {
                                     Id = g.Key.Id,
                                     Code = g.Key.Code,
                                     Name = g.Key.Name,
                                 }).ToList();
                searchResult = ListModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetSuppliers()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Suppliers.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetCbbSaleProduct()
        {
            try
            {
                var listGroup = (from o in db.SaleProductTypes.AsNoTracking()
                                 orderby o.Code
                                 select new
                                 {
                                     Id = o.Id,
                                     Code = o.Code,
                                     Name = o.Name,
                                     ParentId = o.ParentId,
                                 }).ToList();
                var data = (from a in db.SaleProducts.AsNoTracking()
                            select new
                            {
                                Id = a.Id,
                                Code = a.Model,
                                Name = a.VName,
                                ParentId = a.SaleProductTypeId,
                            }).ToList();

                listGroup.AddRange(data);
                return listGroup;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
        }
        

        public List<ComboboxResult> GetSuppliersService()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Suppliers.AsNoTracking()
                                 join g in db.SupplierInGroups.AsNoTracking() on a.Id equals g.SupplierId
                                 join m in db.MaterialGroups.AsNoTracking() on g.SupplierGroupId equals m.Id
                                 where m.Code.Equals("TPAVT.Y")
                                 orderby a.Code
                                 group a by a into g
                                 select new ComboboxResult()
                                 {
                                     Id = g.Key.Id,
                                     Code = g.Key.Code,
                                     Name = g.Key.Name,
                                 }).AsQueryable();

                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListTask()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Tasks.AsNoTracking()
                                 orderby a.Name
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Index = a.Type,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListProduct()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Products.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable().ToList();
                //var lisModule = (from a in db.Modules.AsNoTracking()
                //                 orderby a.Code ascending
                //                 select new ComboboxResult()
                //                 {
                //                     Id = a.Id,
                //                     Name = a.Name,
                //                     Code = a.Code,
                //                 }).AsQueryable().ToList();
                List<ComboboxResult> list = new List<ComboboxResult>();
                //list.AddRange(lisModule);
                list.AddRange(ListModel);
                searchResult = list.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListProjectProduct(string ProjectId)
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.ProjectProducts.AsNoTracking()
                                 where a.ProjectId.Equals(ProjectId)
                                 orderby a.ContractName ascending
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.ContractName,
                                     Code = a.ContractCode,
                                     ParentId = a.ParentId
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetListSolutionGroup()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();

            var ListModel = (from a in db.SolutionGroups.AsNoTracking()
                             orderby a.Code
                             select new ComboboxMultilevelResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 ParentId = a.ParentId
                             }).AsQueryable();
            searchResult = ListModel.ToList();
            return searchResult;
        }

        public object GetListUserWithDepartment(string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();

            var ListModel = (from a in db.Departments.AsNoTracking()
                             join b in db.Employees.AsNoTracking() on a.Id equals b.DepartmentId
                             where a.Id.Equals(departmentId)
                             orderby a.Name
                             select new ComboboxResult()
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                             }).AsQueryable();
            searchResult = ListModel.ToList();
            return searchResult;
        }

        public object GetDepartmentIdWithUserId(string UserId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();

            var ListModel = (from a in db.Departments.AsNoTracking()
                             join b in db.Employees.AsNoTracking() on a.Id equals b.DepartmentId
                             where b.Id.Equals(UserId)
                             orderby a.Name
                             select new ComboboxResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                             }).AsQueryable();
            searchResult = ListModel.ToList();
            return searchResult;
        }

        public object GetListProjectProductByProjectId(string projectId)
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.ProjectProducts.AsNoTracking()
                                 join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                                 where a.ProjectId.Equals(projectId)
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.ContractName,
                                     Code = a.ContractCode,
                                     ParentId = a.ParentId
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListSkill()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Skills.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetCBBMarialTPA(string marialgroupId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            var ListModel = (from a in db.MaterialGroups.AsNoTracking()
                             join b in db.MaterialGroupTPAs.AsNoTracking() on a.MaterialGroupTPAId equals b.Id
                             where b.Id.Equals(marialgroupId)
                             orderby a.Name
                             select new ComboboxResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                             }).AsQueryable();
            searchResult = ListModel.ToList();
            return searchResult;
        }

        public object GetCodeAutoEmPloyee()
        {
            string employeeCode = "";
            var code = (from a in db.Employees.AsNoTracking()
                        orderby a.Name
                        select new { a.Index }).Max(i => i.Index);
            code++;
            var codes = code.ToString();
            if (codes.Length < 5)
            {
                var length = 5 - codes.Length;
                if (length == 1)
                {
                    employeeCode = "0" + codes;
                }
                else if (length == 2)
                {
                    employeeCode = "00" + codes;
                }
                else if (length == 3)
                {
                    employeeCode = "000" + codes;
                }
                else if (length == 4)
                {
                    employeeCode = "0000" + codes;
                }
            }
            return new
            {
                EmployeeCode = employeeCode,
                Index = code
            };
        }

        public string GetCodeError()
        {
            var yearNow = DateTime.Now.Year;
            var code = (from a in db.Errors.AsNoTracking()
                        where a.CreateDate.Year == yearNow && a.Type == 1
                        select a.Index).DefaultIfEmpty(0).Max();
            code++;
            var trueNumberYear = DateTime.Now.ToString("yy");

            return trueNumberYear + "V." + string.Format("{0:0000}", code);
        }

        public object GetCodeProblem(int type)
        {
            var yearNow = DateTime.Now.Year;
            var code = db.Errors.AsNoTracking().Where(a => a.CreateDate.Year == yearNow && a.Type == type).Select(s => s.Index).DefaultIfEmpty(0).Max();
            code++;
            var trueNumberYear = DateTime.Now.ToString("yy");
            var codeError = "";
            if (type == 1)
            {
                codeError = trueNumberYear + "L." + string.Format("{0:0000}", code);
            }
            if (type == 2)
            {
                codeError = trueNumberYear + "V." + string.Format("{0:0000}", code);
            }
            return new
            {
                Code = codeError,
                Index = code
            };
        }

        public Model.Combobox.SearchResultModel<ComboboxResult> GetListErrorGroupMobile()
        {
            Model.Combobox.SearchResultModel<ComboboxResult> searchResultModel = new Model.Combobox.SearchResultModel<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ErrorGroups.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }

        public Model.Combobox.SearchResultModel<ComboboxResult> GetListProjectMobile()
        {
            Model.Combobox.SearchResultModel<ComboboxResult> searchResultModel = new Model.Combobox.SearchResultModel<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Projects.AsNoTracking()
                                 orderby a.Code ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }
        public Model.Combobox.SearchResultModel<ComboboxResult> GetListDepartmentMobile()
        {
            Model.Combobox.SearchResultModel<ComboboxResult> searchResultModel = new Model.Combobox.SearchResultModel<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Departments.AsNoTracking()
                                 orderby a.Name
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }
        public Model.Combobox.SearchResultModel<ComboboxResult> GetCbbEmployeesMobile()
        {
            Model.Combobox.SearchResultModel<ComboboxResult> searchResultModel = new Model.Combobox.SearchResultModel<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 where a.Status == Constants.Employee_Status_Use
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }

        public Model.Combobox.SearchResultModel<ComboboxResult> GetCbbWorkTime()
        {
            Model.Combobox.SearchResultModel<ComboboxResult> searchResultModel = new Model.Combobox.SearchResultModel<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.WorkTimes.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }

        public Model.Combobox.SearchResultModel<ComboboxEmployee> GetCbbEmployee()
        {
            Model.Combobox.SearchResultModel<ComboboxEmployee> searchResultModel = new Model.Combobox.SearchResultModel<ComboboxEmployee>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                 join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                 where a.Status == Constants.Employee_Status_Use
                                 orderby a.Name ascending
                                 select new ComboboxEmployee()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     DepartmentId = b.Id,
                                     Code = a.Code,
                                     SBUId = c.Id,
                                     SBUName = c.Name,
                                     DepartmentName = b.Name,
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }


        public List<ComboboxWorkType> GetListWorkType()
        {
            List<ComboboxWorkType> searchResult = new List<ComboboxWorkType>();

            var ListModel = (from a in db.WorkTypes.AsNoTracking()
                             orderby a.Name ascending
                             select new ComboboxWorkType()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code
                             }).AsQueryable();
            searchResult = ListModel.ToList();
            return searchResult;
        }

        public List<ComboboxMultilevelResult> GetListWorkSkill()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();

            var ListModel = (from a in db.WorkSkills.AsNoTracking()
                             orderby a.Name ascending
                             select new ComboboxMultilevelResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 //ParentId = a.ParentId,
                             }).AsQueryable();
            searchResult = ListModel.ToList();

            return searchResult;
        }

        public List<ComboboxResult> GetListProjectByUser(string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            var ListModel = (from a in db.Projects.AsNoTracking()
                             where string.IsNullOrEmpty(departmentId) || a.DepartmentId.Equals(departmentId)
                             orderby a.Code ascending
                             select new ComboboxResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code
                             }).AsQueryable();

            searchResult = ListModel.ToList();

            return searchResult;
        }

        public List<ComboboxResult> GetListUser()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            var ListModel = (from a in db.Users.AsNoTracking()
                             orderby a.UserName ascending
                             select new ComboboxResult()
                             {
                                 Id = a.Id,
                                 Name = a.UserName,
                             }).AsQueryable();

            searchResult = ListModel.ToList();

            return searchResult;
        }

        public List<ComboboxResult> GetListProjectDownloadDocumentDesign(string sbuId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             orderby a.Code ascending
                             select new
                             {
                                 a.Id,
                                 a.Name,
                                 a.Code,
                                 a.DateFrom,
                                 a.KickOffDate,
                                 a.SBUId,
                                 a.DepartmentId
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(sbuId))
            {
                dataQuery = dataQuery.Where(i => i.SBUId.Equals(sbuId));
            }

            searchResult = dataQuery.Select(s => new ComboboxResult
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code
            }).ToList();
            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách dự án theo User và thời gian
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public List<ComboboxResult> GetListProjectByUserAndDate(SearchCommonModel searchModel, string departmentId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();

            var dataQuery = (from a in db.Projects.AsNoTracking()
                                 //where a.DepartmentId.Equals(departmentId)
                             orderby a.Code ascending
                             select new
                             {
                                 a.Id,
                                 a.Name,
                                 a.Code,
                                 a.DateFrom,
                                 a.KickOffDate,
                                 a.SBUId,
                                 a.DepartmentId
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(departmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(departmentId));
            }

            if (searchModel.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom != null ? u.DateFrom >= searchModel.DateFrom : u.KickOffDate >= searchModel.DateFrom);
            }

            if (searchModel.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom != null ? u.DateFrom <= searchModel.DateTo : u.KickOffDate <= searchModel.DateTo);
            }

            searchResult = dataQuery.Select(s => new ComboboxResult
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code
            }).ToList();

            return searchResult;
        }

        public List<ComboboxResult> GetListClassIfication()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Classifications.AsNoTracking()
                                 orderby a.Index ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Index.ToString(),
                                     Index = a.Index
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxResult addFirstIndex = new ComboboxResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Exten = "1";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.Classifications.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxResult addIndex = new ComboboxResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Exten = (maxIndex + 1).ToString();
                    addIndex.Index = maxIndex + 1;
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetCBBClassIfication()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            var ListModel = (from a in db.Classifications.AsNoTracking()
                             orderby a.Index
                             select new ComboboxResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code
                             }).AsQueryable();

            searchResult = ListModel.ToList();

            return searchResult;
        }
        public List<ComboboxIndexResult> GetCBBApplication()
        {
            List<ComboboxIndexResult> searchResult = new List<ComboboxIndexResult>();
            try
            {
                var ListModel = (from a in db.Applications.AsNoTracking()
                                 orderby a.Index
                                 select new ComboboxIndexResult()
                                 {
                                     Id = a.Id,
                                     Index = a.Index,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxIndexResult addFirstIndex = new ComboboxIndexResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.Applications.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxIndexResult addIndex = new ComboboxIndexResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Index = (maxIndex + 1);
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }

            return searchResult;
        }
        public List<ComboboxMultilevelResult> GetCBBProductStandardTPAType()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            var ListModel = (from a in db.ProductStandardTPATypes.AsNoTracking()
                             orderby a.Name
                             select new ComboboxMultilevelResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Exten = a.Index.ToString(),
                                 ParentId = a.ParentId,
                                 Index = a.Index
                             }).AsQueryable();

            searchResult = ListModel.ToList();


            return searchResult;
        }

        public List<ComboboxMultilevelResult> GetCBBSaleProductType()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            var ListModel = (from a in db.SaleProductTypes.AsNoTracking()
                             orderby a.Name
                             select new ComboboxMultilevelResult()
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Exten = a.Index.ToString(),
                                 ParentId = a.ParentId,
                                 Index = a.Index
                             }).AsQueryable();

            searchResult = ListModel.ToList();

            return searchResult;
        }

        public List<ComboboxMultilevelResult> GetCBBSaleProductTypes()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            List<ComboboxMultilevelResult> ListModel = (from a in db.SaleProductTypes.AsNoTracking()
                                                        orderby a.Name
                                                        select new ComboboxMultilevelResult()
                                                        {
                                                            Id = a.Id,
                                                            Code = a.Code,
                                                            Name = a.Name,
                                                            Exten = a.Index.ToString(),
                                                            ParentId = a.ParentId,
                                                            Index = a.Index,
                                                            Type = 2,
                                                            IsPending = db.SaleProducts.Where(r => r.Status == false && r.SaleProductTypeId.Equals(a.Id)).Any()
                                                        }).ToList();


            ComboboxMultilevelResult pendingParentGroup = new ComboboxMultilevelResult()
            {
                Id = "SPB",
                Name = "Tạm dừng kinh doanh",
                Code = "",
                Type = 1,
                Index = 0,
            };

            searchResult.Add(pendingParentGroup);

            ComboboxMultilevelResult group = new ComboboxMultilevelResult();

            foreach (var item in ListModel)
            {
                group = new ComboboxMultilevelResult()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Exten = item.Index.ToString(),
                    ParentId = item.ParentId,
                    Index = item.Index,
                    Type = item.Type,
                };
                searchResult.Add(group);

                if (IsPendingGroup(ListModel, item))
                {
                    group = new ComboboxMultilevelResult()
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Name = item.Name,
                        Exten = item.Index.ToString(),
                        ParentId = item.ParentId,
                        Index = item.Index,
                        Type = item.Type,
                    };

                    if (string.IsNullOrEmpty(item.ParentId))
                    {
                        group.ParentId = "SPB";
                    }
                    else
                    {
                        group.ParentId = item.ParentId + "_pending";
                    }
                    group.Id = item.Id + "_pending";
                    group.Type = 1;
                    searchResult.Add(group);
                }
            }
            return searchResult;
        }


        private bool IsPendingGroup(List<ComboboxMultilevelResult> listGroup, ComboboxMultilevelResult group)
        {
            var listChild = listGroup.Where(r => r.ParentId != null && r.ParentId.Equals(group.Id)).ToList();
            if (group.IsPending)
            {
                return true;
            }
            foreach (var item in listChild)
            {
                if (IsPendingGroup(listGroup, item))
                {
                    return true;
                }
            }
            return false;
        }

        public List<ComboboxResult> GetCBBProductStandardTPATypeIndex(ComboboxResult model)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();

            //if (!string.IsNullOrEmpty(model.Id))
            //{
            var ListModel = (from a in db.ProductStandardTPATypes.AsNoTracking()
                             where a.ParentId.Equals(model.Id)
                             orderby a.Index
                             select new ComboboxResult()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Exten = a.Index.ToString(),
                                 Index = a.Index
                             }).AsQueryable();

            searchResult = ListModel.ToList();
            //}
            //else
            //{
            //    var ListModel = (from a in db.ProductStandardTPATypes.AsNoTracking()
            //                     orderby a.Index
            //                     select new ComboboxResult()
            //                     {
            //                         Id = a.Id,
            //                         Name = a.Name,
            //                         Exten = a.Index.ToString(),
            //                         Index = a.Index
            //                     }).AsQueryable();

            //    searchResult = ListModel.ToList();
            //}



            if (searchResult.Count() == 0)
            {
                ComboboxResult addFirstIndex = new ComboboxResult();
                addFirstIndex.Id = "";
                addFirstIndex.Name = "";
                addFirstIndex.Exten = "1";
                addFirstIndex.Index = 1;
                searchResult.Add(addFirstIndex);
            }
            else
            {
                var maxIndex = db.ProductStandardTPATypes.AsNoTracking().Where(b => b.ParentId.Equals(model.Id)).Select(b => b.Index).Max();
                ComboboxResult addIndex = new ComboboxResult();
                addIndex.Id = "";
                addIndex.Name = "";
                addIndex.Exten = (maxIndex + 1).ToString();
                addIndex.Index = maxIndex + 1;
                searchResult.Add(addIndex);
            }

            return searchResult;
        }

        public List<CustomersModel> GetListCustomers()
        {
            var listCustomer = (from a in db.Customers.AsNoTracking()
                                join b in db.CustomerTypes.AsNoTracking() on a.CustomerTypeId equals b.Id
                                join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id
                                select new CustomersModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Address = a.Address,
                                    Code = a.Code,
                                    PhoneNumber = a.PhoneNumber,
                                    SBUCode = c.Code,
                                    CustomerTypeName = b.Name

                                }).ToList();

            return listCustomer;
        }
        public List<string> ListCountry()
        {
            var listContry = (from a in db.Countries.AsNoTracking()
                              select new
                              {
                                  Name = a.CountryName,

                              }).Select(s => s.Name).ToList();

            return listContry;
        }

        public List<ComboboxResult> ListCbbCountry()
        {
            var listContry = (from a in db.Countries.AsNoTracking()
                              select new ComboboxResult
                              {
                                  Id = a.CountryName,
                                  Name = a.CountryName,

                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbLanguage()
        {
            var listContry = (from a in db.Languages.AsNoTracking()
                              orderby a.Index
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,

                              });

            return listContry.ToList();
        }


        public List<ComboboxResult> ListCbbProvince()
        {
            var listContry = (from a in db.Provinces.AsNoTracking()
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,

                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbDistrict(string provinceId)
        {
            var listContry = (from a in db.Districts.AsNoTracking()
                              where a.ProvinceId.Equals(provinceId)
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbWard(string districtId)
        {
            var listContry = (from a in db.Wards.AsNoTracking()
                              where a.DistrictId.Equals(districtId)
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,

                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbInsuranceLevel()
        {
            var listContry = (from a in db.InsuranceLevels.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Exten = a.Money.ToString()
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbBank()
        {
            var listContry = (from a in db.BankAccounts.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                              });

            return listContry.ToList();
        }
        public List<ComboboxResult> ListCbbReasonEndWorking()
        {
            var listContry = (from a in db.ReasonEndWorkings.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbPosition()
        {
            var listContry = (from a in db.JobPositions.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbWorkLocation()
        {
            var listContry = (from a in db.WorkLocations.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbReasonChangeIncome()
        {
            var listContry = (from a in db.ReasonChangeIncomes.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              }).ToList();

            return listContry;
        }

        public List<ComboboxResult> ListCbbReasonChangeInsurance()
        {
            var listContry = (from a in db.ReasonChangeInsurances.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              }).ToList();

            return listContry;
        }

        public List<ComboboxResult> ListCbbLaborContract()
        {
            var listContry = (from a in db.LaborContracts.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              }).ToList();

            return listContry;
        }

        public List<ComboboxResult> ListCbbEmployeeGroup()
        {
            var listContry = (from a in db.EmployeeGroups.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.EmployeeGroupId,
                                  Name = a.Name,
                                  Code = a.Code,
                              }).ToList();

            return listContry;
        }

        public List<QuestionGroupModel> ListCbbQuestionGroup()
        {
            var listContry = (from a in db.QuestionGroups.AsNoTracking()
                              orderby a.Name
                              select new QuestionGroupModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  ParentId = a.ParentId
                              }).ToList();

            return listContry;
        }

        public List<DocumentGroupModel> ListCbbDocumentGroup()
        {
            var listContry = (from a in db.DocumentGroups.AsNoTracking()
                              orderby a.Name
                              select new DocumentGroupModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  ParentId = a.ParentId
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbDocumentType()
        {
            var listContry = (from a in db.DocumentTypes.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                              });

            return listContry.ToList();
        }

        public object GetListDocumentGroup()
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var ListModel = (from a in db.DocumentGroups.AsNoTracking()
                                 orderby a.Code
                                 select new ComboboxMultilevelResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     ParentId = a.ParentId,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<FlowStageModel> ListCbbFlowStage()
        {
            var listContry = (from a in db.FlowStages.AsNoTracking()
                              orderby a.Name
                              select new FlowStageModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  ParentId = a.ParentId
                              }).ToList();

            return listContry;
        }

        public List<ComboboxResult> ListCbbSalaryLevel()
        {
            var listContry = (from a in db.SalaryLevels.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  Exten = a.Salary.ToString(),
                              });

            return listContry.ToList();
        }

        public List<SalaryLevelModel> ListCbbSalaryLevel(SalaryLevelModel salaryLevelModel)
        {
            var listContry = (from a in db.SalaryLevels.AsNoTracking()
                              orderby a.Name
                              select new SalaryLevelModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  SalaryGroupId = a.SalaryGroupId,
                                  SalaryTypeId = a.SalaryTypeId,
                                  Salary = a.Salary
                              });

            if (!string.IsNullOrEmpty(salaryLevelModel.SalaryGroupId))
            {
                listContry = listContry.Where(r => salaryLevelModel.SalaryGroupId.Equals(r.SalaryGroupId));
            }

            if (!string.IsNullOrEmpty(salaryLevelModel.SalaryTypeId))
            {
                listContry = listContry.Where(r => salaryLevelModel.SalaryTypeId.Equals(r.SalaryTypeId));
            }

            return listContry.ToList();
        }

        public List<ComboboxResult> GetCbbRecruitmentChannels()
        {
            var listContry = (from a in db.RecruitmentChannels.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> ListCbbLaborContractSupplier()
        {
            var listContry = (from a in db.LaborContracts.AsNoTracking()
                              orderby a.Name
                              where a.Type == Constants.Contract_Rule
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                              }).ToList();

            return listContry;
        }

        public List<ComboboxResult> GetCbbSalaryGroups()
        {
            var listContry = (from a in db.SalaryGroups.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Code = a.Code,
                                  Name = a.Name,
                              });

            return listContry.ToList();
        }

        public List<ComboboxResult> GetCbbSalarytypes()
        {
            var listContry = (from a in db.SalaryTypes.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult
                              {
                                  Id = a.Id,
                                  Code = a.Code,
                                  Name = a.Name,
                              });

            return listContry.ToList();
        }

        public List<ComboboxResultModel> GetCbbErrorAffect()
        {
            var errorAffects = (from a in db.ErrorAffects.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResultModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                });

            return errorAffects.ToList();
        }

        public List<ComboboxResultModel> GetCbbChangePlan()
        {
            var changePlans = (from a in db.ErrorChangedPlans.AsNoTracking()
                               select new ComboboxResultModel
                               {

                               });

            return changePlans.ToList();
        }

        public List<ComboboxResult> GetCbbRecruitmentRequest()
        {
            var requests = (from a in db.RecruitmentRequests.AsNoTracking()
                            where a.Status == 0
                            orderby a.Code
                            select new ComboboxResult
                            {
                                Id = a.Id.ToString(),
                                Name = a.Code,
                            });

            return requests.ToList();
        }

        public List<ComboboxResult> GetAllCbbRecruitmentRequest()
        {
            var requests = (from a in db.RecruitmentRequests.AsNoTracking()
                            orderby a.Code
                            select new ComboboxResult
                            {
                                Id = a.Id.ToString(),
                                Name = a.Code,
                            });

            return requests.ToList();
        }

        public List<CustomerContactModel> GetCustomerContact(string id)
        {
            var requests = (from a in db.CustomerContacts.AsNoTracking()
                            where a.CustomerId.Equals(id)
                            orderby a.Name
                            select new CustomerContactModel
                            {
                                Id = a.Id.ToString(),
                                Name = a.Name,
                                Address = a.Address,
                                PhoneNumber = a.PhoneNumber,
                                Position = a.Position,
                                Email = a.Email,
                                Avatar = a.Avatar
                            }).ToList();

            return requests;
        }

        public List<ComboboxResult> GetMeetingType()
        {
            var meetingTypes = (from a in db.MeetingTypes.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult
                                {
                                    Id = a.Id.ToString(),
                                    Name = a.Name,
                                    Code = a.Code,
                                    Exten = a.ParentId,
                                    Index = a.Meetings.Count() == 0 ? 0 : a.Meetings.Max(m => m.Index)
                                }).ToList();

            return meetingTypes;
        }

        public List<ComboboxEmployee> GetListDepartmentRequestEmployees(string id)
        {
            var requests = (from a in db.Employees.AsNoTracking()
                            where a.DepartmentId.Equals(id)
                            orderby a.Name
                            select new ComboboxEmployee
                            {
                                Id = a.Id.ToString(),
                                Name = a.Name,
                                Code = a.Code,
                                DepartmentId = a.DepartmentId,
                            });

            return requests.ToList();
        }

        public List<ComboboxEmployee> getListDepartmentReceiveEmployees(string id)
        {
            var requests = (from a in db.Employees.AsNoTracking()
                            where a.DepartmentId.Equals(id)
                            orderby a.Name
                            select new ComboboxEmployee
                            {
                                Id = a.Id.ToString(),
                                Name = a.Name,
                                Code = a.Code,
                                DepartmentId = a.DepartmentId,
                            });

            return requests.ToList();
        }
        public List<ProjectPhaseModel> GetProjectPhaseType()
        {
            var projectPhases = (from a in db.ProjectPhases.AsNoTracking()
                                 orderby a.Name
                                 select new ProjectPhaseModel
                                 {
                                     Id = a.Id.ToString(),
                                     Name = a.Name,
                                     Code = a.Code,
                                     ParentId = a.ParentId
                                 }).ToList();

            return projectPhases;
        }

        public List<ComboboxResult> GetListProjectAttach()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.ProjectAttaches.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxMultilevelResult> GetProjectAttachTabType(string projectId)
        {
            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            searchResult = (from a in db.ProjectAttachTabTypes.AsNoTracking()
                            orderby a.Index
                            select new ComboboxMultilevelResult()
                            {
                                Id = a.Id,
                                //Code = a.Code,
                                Name = a.Name,
                                ParentId = a.ParentId,
                                Type = 0
                            }).ToList();

            var ListModel2 = db.ProjectDocumentGroups.Where(a => a.ProjectId.Equals(projectId)).OrderBy(r => r.Name).ToList();
            ComboboxMultilevelResult dataGroup;
            foreach (var item in ListModel2)
            {
                dataGroup = new ComboboxMultilevelResult();
                dataGroup.Id = item.Id;
                dataGroup.Name = item.Name;
                dataGroup.Type = 1;
                if (string.IsNullOrEmpty(item.ParentId))
                {
                    dataGroup.ParentId = item.ProjectDocumentTabTypeId;
                }
                else
                {
                    dataGroup.ParentId = item.ParentId;

                }

                searchResult.Add(dataGroup);
            }

            return searchResult;
        }

        public List<ComboboxResult> GetCbbSBUId(string interviewBy)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                if (interviewBy == "null" || string.IsNullOrEmpty(interviewBy))
                {
                    var ListModel = (from a in db.SBUs.AsNoTracking()
                                     orderby a.Name
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }
                else
                {
                    var ListModel = (from a in db.SBUs.AsNoTracking()
                                     where string.IsNullOrEmpty(interviewBy) || a.Id.Equals(interviewBy)
                                     orderby a.Name
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public InterviewSearchResultsModel GetInterview(string id)
        {
            //List<InterviewSearchResultsModel> searchResult = new List<InterviewSearchResultsModel>();

            var ListModel = (from a in db.Interviews.AsNoTracking()
                             orderby a.Name ascending
                             where a.Id.Equals(id)
                             select new InterviewSearchResultsModel()
                             {
                                 Id = a.Id,
                                 EmployeeId = a.InterviewBy,
                                 SBUId = a.SBUId,
                                 DepartmentId = a.DepartmentId,
                                 CandidateApplyId = a.CandidateApplyId,
                                 InterviewTime = a.InterviewTime,
                                 InterviewDate = a.InterviewDate,
                                 Status = a.Status,
                             }).FirstOrDefault();
            return ListModel;
        }

        public EmployeeModel getSBU(string id)
        {

            var ListModel = (from a in db.Employees.AsNoTracking()
                             where a.Id.Equals(id)
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                             select new EmployeeModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 DepartmentId = b.Id,
                                 Code = a.Code,
                                 SBUID = c.Id,
                                 SBUName = c.Name,
                                 DepartmentName = b.Name,
                             }).FirstOrDefault();

            return ListModel;
        }

        public List<ComboboxResult> GetListSuppliers()
        {
            var listSupplier = (from a in db.Suppliers.AsNoTracking()
                                orderby a.Code
                                select new ComboboxResult
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Code = a.Code
                                }).ToList();

            foreach (var item in listSupplier)
            {
                item.Code = $"{item.Code} - {item.Name}";
            }

            return listSupplier;
        }

        public List<ComboboxResult> GetListEmployees()
        {
            var listEmployees = (from a in db.Employees.AsNoTracking()
                                orderby a.Code
                                select new ComboboxResult
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Code = a.Code
                                }).ToList();

            return listEmployees;
        }

        public List<ComboboxResult> GetCustomerRequirement(string customerId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                if (customerId == "null" || string.IsNullOrEmpty(customerId))
                {
                    var ListModel = (from a in db.CustomerRequirements.AsNoTracking()
                                     orderby a.Name
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }
                else
                {
                    var ListModel = (from a in db.CustomerRequirements.AsNoTracking()
                                     where string.IsNullOrEmpty(customerId) || a.CustomerId.Equals(customerId)
                                     orderby a.Name
                                     select new ComboboxResult()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                     }).AsQueryable();

                    searchResult = ListModel.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetCustomerRequirementById(string customerRequirementId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            var customerRequirement = (from a in db.CustomerRequirements.AsNoTracking()
                                where string.IsNullOrEmpty(customerRequirementId) || a.Id.Equals(customerRequirementId)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Code = a.Code,
                                    Exten = a.DomainId
                                }).AsQueryable();
            searchResult = customerRequirement.ToList();
            return searchResult;
        }

        public List<ComboboxResult> GetCustomerContact()
        {
            var listEmployees = (from a in db.CustomerContacts.AsNoTracking()
                                 select new ComboboxResult
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).ToList();
            return listEmployees;
        }
    }
}
