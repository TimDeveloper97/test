using NTS.Business.Combobox;
using NTS.Model.QLTKMG;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Common;
using NTS.Model;
using QLTK.Api;
using NTS.Model.Combobox;
using NTS.Model.SalaryLevel;

namespace NTS.Api.Controllers.Combobox
{
    [RoutePrefix("api/ComboboxCommon")]
    [ApiHandleExceptionSystem]
    [Authorize]
    public class ComboboxController : BaseController
    {
        private readonly ComboboxBusiness _Business = new ComboboxBusiness();

        [Route("GetListBanking")]
        [HttpPost]
        public HttpResponseMessage GetListBanking()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListGroupModule")]
        [HttpPost]
        public HttpResponseMessage GetListGroupModule()
        {
            try
            {
                var result = _Business.GetListGroupModule();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListParentMaterialGroup")]
        [HttpPost]
        public HttpResponseMessage GetListParentMaterialGroup(QLTKMGModel model)
        {
            try
            {
                var result = _Business.GetListParentMaterialGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListMaterialGroup")]
        [HttpPost]
        public HttpResponseMessage GetListMaterialGroup()
        {
            try
            {
                var result = _Business.GetListMaterialGroup();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListMaterialGroupParent")]
        [HttpPost]
        public HttpResponseMessage GetListMaterialGroupParent()
        {
            try
            {
                var result = _Business.GetListMaterialGroupParent();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListManufacture")]
        [HttpPost]
        public HttpResponseMessage GetListManufacture()
        {
            try
            {
                var result = _Business.GetListManufacture();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListIndustry")]
        [HttpPost]
        public HttpResponseMessage GetListIndustry()
        {
            try
            {
                var result = _Business.GetListIndustry();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCbbDepartmentBySBUId")]
        [HttpPost]
        public HttpResponseMessage GetCbbDepartmentBySBUId(string sbuId)
        {
            try
            {
                var result = _Business.GetCbbDepartmentBySBUId(sbuId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCustomerRequirement")]
        [HttpPost]
        public HttpResponseMessage GetCustomerRequirement(string customerId)
        {
            try
            {
                var result = _Business.GetCustomerRequirement(customerId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCustomerRequirementById")]
        [HttpPost]
        public HttpResponseMessage GetCustomerRequirementById(string customerRequirementId)
        {
            try
            {
                var result = _Business.GetCustomerRequirementById(customerRequirementId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListDepartment")]
        [HttpPost]
        public HttpResponseMessage GetListDepartment()
        {
            try
            {
                var result = _Business.GetListDepartment();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListDepartmentUse")]
        [HttpPost]
        public HttpResponseMessage GetListDepartmentUse()
        {
            try
            {
                var result = _Business.GetListDepartmentUse();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetGroupModule")]
        [HttpPost]
        public HttpResponseMessage GetGroupModule()
        {
            try
            {
                var result = _Business.GetGroupModule();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetGroupProduct")]
        [HttpPost]
        public HttpResponseMessage GetGroupProduct()
        {
            try
            {
                var result = _Business.GetGroupProduct();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("GetListJobPosition")]
        [HttpPost]
        public HttpResponseMessage GetListJobPosition()
        {
            try
            {
                var result = _Business.GetListJobPosition();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCBBJobPosition")]
        [HttpPost]
        public HttpResponseMessage GetCBBJobPosition()
        {
            try
            {
                var result = _Business.GetCBBJobPosition();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListQualification")]
        [HttpPost]
        public HttpResponseMessage GetListQualification()
        {
            try
            {
                var result = _Business.GetListQualification();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListGroupUsers")]
        [HttpPost]
        public HttpResponseMessage GetListGroupUsers(string departmentId)
        {
            var result = _Business.GetListGroupUsers(departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetListModulesByGroup")]
        [HttpPost]
        public HttpResponseMessage GetListModulesByGroup(string groupId)
        {
            try
            {
                var result = _Business.GetListModulesByGroup(groupId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("GetListProject")]
        [HttpPost]
        public HttpResponseMessage GetListProject()
        {
            try
            {
                var result = _Business.GetListProject();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GenCodeModuleError")]
        [HttpPost]
        public HttpResponseMessage GenCodeModuleError()
        {
            try
            {
                var result = _Business.GenCodeModuleError();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListModuleErrorTypesParent")]
        [HttpPost]
        public HttpResponseMessage GetListModuleErrorTypesParent(string type)
        {
            try
            {
                var result = _Business.GetListModuleErrorTypesParent(type);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListManagers")]
        [HttpPost]
        public HttpResponseMessage GetListManagers()
        {
            try
            {
                var result = _Business.GetListManagers();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetEmployeeByDepartment")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeByDepartment(string departmentId)
        {
            try
            {
                var result = _Business.GetEmployeeByDepartment(departmentId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetEmployeeByDepartmentWithStatus")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeByDepartmentWithStatus(string departmentId)
        {
            try
            {
                var result = _Business.GetEmployeeByDepartmentWithStatus(departmentId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListDepartmentTreeNode")]
        [HttpPost]
        public HttpResponseMessage GetListDepartmentTreeNode()
        {
            try
            {
                var result = _Business.GetListDepartmentTreeNode();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListPracticeGroup")]
        [HttpPost]
        public HttpResponseMessage GetListPracticeGroup()
        {
            try
            {
                var result = _Business.GetListPracticeGroup();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListUnit")]
        [HttpPost]
        public HttpResponseMessage GetListUnit()
        {
            try
            {
                var result = _Business.GetListUnit();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("GetCbbModule")]
        [HttpPost]
        public HttpResponseMessage GetCbbModule()
        {
            try
            {
                var result = _Business.GetCbbModule();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCbbProduct")]
        [HttpPost]
        public HttpResponseMessage GetCbbProduct()
        {
            try
            {
                var result = _Business.GetCbbProduct();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        

        [Route("GetListRawMaterial")]
        [HttpPost]
        public HttpResponseMessage GetListRawMaterial()
        {
            try
            {
                var result = _Business.GetListRawMaterial();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("GetListTPA")]
        [HttpPost]
        public HttpResponseMessage GetListTPA()
        {
            try
            {
                var result = _Business.GetListTPA();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListNSMaterialType")]
        [HttpPost]
        public HttpResponseMessage GetListNSMaterialType()
        {
            try
            {
                var result = _Business.GetListNSMaterialType();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("GetGroupModuleParentChild")]
        [HttpPost]
        public HttpResponseMessage GetGroupModuleParentChild()
        {
            try
            {
                var result = _Business.GetListModuleGroupParentChild();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetGroupJobParentChild")]
        [HttpPost]
        public HttpResponseMessage GetGroupJobParentChild()
        {
            try
            {
                var result = _Business.GetListJobGroupParentChild();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [Route("GetGroupManufactureParentChild")]
        [HttpPost]
        public HttpResponseMessage GetGroupManufactureParentChild()
        {
            try
            {
                var result = _Business.GetListManufactureGroupParentChild();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetGroupSupplierParentChild")]
        [HttpPost]
        public HttpResponseMessage GetGroupSupplierParentChild()
        {
            try
            {
                var result = _Business.GetListSupplierGroupParentChild();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetProductGroupParentChild")]
        [HttpPost]
        public HttpResponseMessage GetProductGroupParentChild()
        {
            try
            {
                var result = _Business.GetListProductGroupParentChild();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListCriter")]
        [HttpPost]
        public HttpResponseMessage GetListCriter()
        {
            try
            {
                var result = _Business.GetListCriter();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListProductStandardGroup")]
        [HttpPost]
        public HttpResponseMessage GetListProductStandardGroup()
        {
            try
            {
                var result = _Business.GetListProductStandardGroup();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListSBU")]
        [HttpPost]
        public HttpResponseMessage GetListSBU()
        {
            try
            {
                var result = _Business.GetListSBU();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetSolutionTechnologies")]
        [HttpPost]
        public HttpResponseMessage GetSolutionTechnologies()
        {
            try
            {
                var result = _Business.GetSolutionTechnologies();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListCBBSBU")]
        [HttpPost]
        public HttpResponseMessage GetListCBBSBU()
        {
            try
            {
                var result = _Business.GetListCBBSBU();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCbbStage")]
        [HttpPost]
        public HttpResponseMessage GetCbbStage()
        {
            var result = _Business.GetCbbStage();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbEmployees")]
        [HttpPost]
        public HttpResponseMessage GetCbbEmployees()
        {
            var result = _Business.GetCbbEmployees();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetCbbEmployeeByDepartmentId")]
        [HttpGet]
        public HttpResponseMessage GetCbbEmployeeByDepartmentId(string departmentId)
        {
            var result = _Business.GetCbbEmployeeByDepartmentId(departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFunctionGroup")]
        [HttpPost]
        public HttpResponseMessage GetListFunctionGroup()
        {
            var result = _Business.GetListFunctionGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("GetQualification")]
        [HttpPost]
        public HttpResponseMessage GetQualification()
        {
            try
            {
                var result = _Business.GetQualification();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetGroupUser")]
        [HttpPost]
        public HttpResponseMessage GetGroupUser()
        {
            try
            {
                var result = _Business.GetGroupUser();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("GetListDegree")]
        [HttpPost]
        public HttpResponseMessage GetListDegree()
        {
            var result = _Business.GetListDegree();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSpecialize")]
        [HttpPost]
        public HttpResponseMessage GetListSpecialize()
        {
            var result = _Business.GetListSpecialize();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListWorkPlace")]
        [HttpPost]
        public HttpResponseMessage GetListWorkPlace()
        {
            var result = _Business.GetListWorkPlace();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSkillGroup")]
        [HttpPost]
        public HttpResponseMessage GetListSkillGroup()
        {
            var result = _Business.GetListSkillGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListRoomType")]
        [HttpPost]
        public HttpResponseMessage GetListRoomType()
        {
            var result = _Business.GetListRoomType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListPracticeGroupA")]
        [HttpPost]
        public HttpResponseMessage GetListPracticeGroupA()
        {
            var result = _Business.GetListPracticeGroupA();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListGroupJob")]
        [HttpPost]
        public HttpResponseMessage GetListGroupJob()
        {
            var result = _Business.GetListGroupJob();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListClassRoom")]
        [HttpPost]
        public HttpResponseMessage GetListClassRoom()
        {
            var result = _Business.GetListClassRoom();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListJob")]
        [HttpPost]
        public HttpResponseMessage GetListJob()
        {
            var result = _Business.GetListJob();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListCustomer")]
        [HttpPost]
        public HttpResponseMessage GetListCustomer()
        {
            var result = _Business.GetListCustomer();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListMaterial")]
        [HttpPost]
        public HttpResponseMessage GetListMaterial()
        {
            var result = _Business.GetListMaterial();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListCustomerType")]
        [HttpPost]
        public HttpResponseMessage GetListCustomerType()
        {
            var result = _Business.GetListCustomerType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListErrorGroup")]
        [HttpPost]
        public HttpResponseMessage GetListErrorGroup()
        {
            var result = _Business.GetListErrorGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListModule")]
        [HttpPost]
        public HttpResponseMessage GetListModule()
        {
            var result = _Business.GetListModule();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListModuleByProjectId/{id}")]
        [HttpPost]
        public HttpResponseMessage GetListModuleByProjectId(string id)
        {
            var result = _Business.GetListModuleByProjectId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListManufactureGroup")]
        [HttpPost]
        public HttpResponseMessage GetListManufactureGroup()
        {
            var result = _Business.GetListManufactureGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSupplierGroup")]
        [HttpPost]
        public HttpResponseMessage GetListSupplierGroup()
        {
            var result = _Business.GetListSupplierGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDomain")]
        [HttpPost]
        public HttpResponseMessage GetDomain()
        {
            var result = _Business.GetDomain();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListTask")]
        [HttpPost]
        public HttpResponseMessage GetListTask()
        {
            var result = _Business.GetListTask();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectProduct")]
        [HttpPost]
        public HttpResponseMessage GetListProjectProduct(string ProjectId)
        {
            var result = _Business.GetListProjectProduct(ProjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProduct")]
        [HttpPost]
        public HttpResponseMessage GetListProduct()
        {
            var result = _Business.GetListProduct();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSolutionGroup")]
        [HttpPost]
        public HttpResponseMessage GetListSolutionGroup()
        {
            var result = _Business.GetListSolutionGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListUserWithDepartment")]
        [HttpPost]
        public HttpResponseMessage GetListUserWithDepartment(string departmentId)
        {
            var result = _Business.GetListUserWithDepartment(departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDepartmentIdWithUserId")]
        [HttpPost]
        public HttpResponseMessage GetDepartmentIdWithUserId(string UserId)
        {
            var result = _Business.GetDepartmentIdWithUserId(UserId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectProductByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetListProjectProductByProjectId(string projectId)
        {
            var result = _Business.GetListProjectProductByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbEmployeesStatus")]
        [HttpPost]
        public HttpResponseMessage GetCbbEmployeesStatus()
        {
            var result = _Business.GetCbbEmployeesStatus();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSkill")]
        [HttpPost]
        public HttpResponseMessage GetListSkill()
        {
            var result = _Business.GetListSkill();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCBBMarialTPA")]
        [HttpPost]
        public HttpResponseMessage GetCBBMarialTPA(string marialgroupId)
        {
            var result = _Business.GetCBBMarialTPA(marialgroupId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCodeError")]
        [HttpPost]
        public HttpResponseMessage GetCodeError()
        {
            var result = _Business.GetCodeError();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCodeProblem")]
        [HttpPost]
        public HttpResponseMessage GetCodeProblem(int type)
        {
            var result = _Business.GetCodeProblem(type);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCodeAutoEmPloyee")]
        [HttpPost]
        public HttpResponseMessage GetCodeAutoEmPloyee()
        {
            var result = _Business.GetCodeAutoEmPloyee();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListErrorGroupMobile")]
        [HttpPost]
        public HttpResponseMessage GetListErrorGroupMobile()
        {
            var result = _Business.GetListErrorGroupMobile();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectMobile")]
        [HttpPost]
        public HttpResponseMessage GetListProjectMobile()
        {
            try
            {
                var result = _Business.GetListProjectMobile();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("GetListDepartmentMobile")]
        [HttpPost]
        public HttpResponseMessage GetListDepartmentMobile()
        {
            try
            {
                var result = _Business.GetListDepartmentMobile();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("GetCbbEmployeesMobile")]
        [HttpPost]
        public HttpResponseMessage GetCbbEmployeesMobile()
        {
            var result = _Business.GetCbbEmployeesMobile();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbWorkTime")]
        [HttpPost]
        public HttpResponseMessage GetCbbWorkTime()
        {
            var result = _Business.GetCbbWorkTime();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbEmployee")]
        [HttpPost]
        public HttpResponseMessage GetCbbEmployee()
        {
            var result = _Business.GetCbbEmployee();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListWorkType")]
        [HttpPost]
        public HttpResponseMessage GetListWorkType()
        {
            var result = _Business.GetListWorkType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListWorkSkill")]
        [HttpPost]
        public HttpResponseMessage GetListWorkSkill()
        {
            var result = _Business.GetListWorkSkill();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectByUser")]
        [HttpPost]
        public HttpResponseMessage GetListProjectByUser()
        {
            var departmentId = "";
            if (!this.CheckPermission(Constants.Permission_Code_F060705))
            {
                departmentId = this.GetDepartmentIdByRequest();
            }
            var result = _Business.GetListProjectByUser(departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListUser")]
        [HttpPost]
        public HttpResponseMessage GetListUser()
        {
            
           
            var result = _Business.GetListUser();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetListProjectByUserAndDate")]
        [HttpPost]
        public HttpResponseMessage GetListProjectByUserAndDate(SearchCommonModel searchModel)
        {
            var departmentId = "";
            if (!this.CheckPermission(Constants.Permission_Code_F060705))
            {
                departmentId = this.GetDepartmentIdByRequest();
            }
            var result = _Business.GetListProjectByUserAndDate(searchModel, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectDownloadDocumentDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090900")]
        public HttpResponseMessage GetListProjectDownloadDocumentDesign()
        {
            var sbuId = "";
            if (!this.CheckPermission(Constants.Permission_Code_F090902))
            {
                sbuId = this.GetSBUIdByRequest();
            }
            var result = _Business.GetListProjectDownloadDocumentDesign(sbuId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListClassIfication")]
        [HttpPost]
        public HttpResponseMessage GetListClassIfication()
        {
            try
            {
                var result = _Business.GetListClassIfication();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetCBBClassIfication")]
        [HttpPost]
        public HttpResponseMessage GetCBBClassIfication()
        {
            var result = _Business.GetCBBClassIfication();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCBBApplication")]
        [HttpPost]
        public HttpResponseMessage GetCBBApplication()
        {
            var result = _Business.GetCBBApplication();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCBBProductStandardTPAType")]
        [HttpPost]
        public HttpResponseMessage GetCBBProductStandardTPAType()
        {
            var result = _Business.GetCBBProductStandardTPAType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetCBBSaleProductType")]
        [HttpPost]
        public HttpResponseMessage GetCBBSaleProductType()
        {
            var result = _Business.GetCBBSaleProductType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCBBSaleProductTypes")]
        [HttpPost]
        public HttpResponseMessage GetCBBSaleProductTypes()
        {
            var result = _Business.GetCBBSaleProductTypes();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCBBProductStandardTPATypeIndex")]
        [HttpPost]
        public HttpResponseMessage GetCBBProductStandardTPATypeIndex(ComboboxResult model)
        {
            var result = _Business.GetCBBProductStandardTPATypeIndex(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListCustomers")]
        [HttpPost]
        public HttpResponseMessage GetListCustomers()
        {
            var result = _Business.GetListCustomers();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListCountry")]
        [HttpPost]
        public HttpResponseMessage GetListCountry()
        {
            var result = _Business.ListCountry();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("GetCbbCountry")]
        [HttpPost]
        public HttpResponseMessage GetCbbCountry()
        {
            var result = _Business.ListCbbCountry();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getCbbLanguage")]
        [HttpPost]
        public HttpResponseMessage getCbbLanguage()
        {
            var result = _Business.ListCbbLanguage();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbSupplier")]
        [HttpPost]
        public HttpResponseMessage GetCbbSupplier()
        {
            var result = _Business.GetSuppliers();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbSaleProduct")]
        [HttpPost]
        public HttpResponseMessage GetCbbSaleProduct()
        {
            var result = _Business.GetCbbSaleProduct();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbSupplierService")]
        [HttpPost]
        public HttpResponseMessage GetCbbSupplierService()
        {
            var result = _Business.GetSuppliersService();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetApplication")]
        [HttpPost]
        public HttpResponseMessage GetApplication()
        {
            var result = _Business.GetApplication();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProvince")]
        [HttpPost]
        public HttpResponseMessage GetProvince()
        {
            var result = _Business.ListCbbProvince();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDistrict")]
        [HttpPost]
        public HttpResponseMessage GetDistrict(string provinceId)
        {
            var result = _Business.ListCbbDistrict(provinceId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWard")]
        [HttpPost]
        public HttpResponseMessage GetWard(string districtId)
        {
            var result = _Business.ListCbbWard(districtId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetInsuranceLevel")]
        [HttpPost]
        public HttpResponseMessage GetInsuranceLevel()
        {
            var result = _Business.ListCbbInsuranceLevel();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetBanks")]
        [HttpPost]
        public HttpResponseMessage GetBanks()
        {
            var result = _Business.ListCbbBank();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetReasonsEndWorking")]
        [HttpPost]
        public HttpResponseMessage GetReasonsEndWorking()
        {
            var result = _Business.ListCbbReasonEndWorking();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetPosition")]
        [HttpPost]
        public HttpResponseMessage GetPosition()
        {
            var result = _Business.ListCbbPosition();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWorkLocation")]
        [HttpPost]
        public HttpResponseMessage GetWorkLocation()
        {
            var result = _Business.ListCbbWorkLocation();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetReasonChangeIncome")]
        [HttpPost]
        public HttpResponseMessage GetReasonChangeIncome()
        {
            var result = _Business.ListCbbReasonChangeIncome();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetReasonChangeInsurance")]
        [HttpPost]
        public HttpResponseMessage GetReasonChangeInsurance()
        {
            var result = _Business.ListCbbReasonChangeInsurance();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetLabroContract")]
        [HttpPost]
        public HttpResponseMessage GetLabroContract()
        {
            var result = _Business.ListCbbLaborContract();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSupplierContract")]
        [HttpPost]
        public HttpResponseMessage GetSupplierContract()
        {
            var result = _Business.ListCbbLaborContractSupplier();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("EmployeeGroup")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeGroup()
        {
            var result = _Business.ListCbbEmployeeGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("QuestionGroup")]
        [HttpPost]
        public HttpResponseMessage ListCbbQuestionGroup()
        {
            var result = _Business.ListCbbQuestionGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DocumentGroup")]
        [HttpPost]
        public HttpResponseMessage ListCbbDocumentGroup()
        {
            var result = _Business.ListCbbDocumentGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DocumentType")]
        [HttpPost]
        public HttpResponseMessage ListCbbDocumentType()
        {
            var result = _Business.ListCbbDocumentType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListDocumentGroup")]
        [HttpPost]
        public HttpResponseMessage GetListDocumentGroup()
        {
            var result = _Business.GetListDocumentGroup();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFlowStage")]
        [HttpPost]
        public HttpResponseMessage ListCbbFlowStage()
        {
            var result = _Business.ListCbbFlowStage();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSalaryLevel")]
        [HttpPost]
        public HttpResponseMessage ListCbbSalaryLevel()
        {
            var result = _Business.ListCbbSalaryLevel();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSalaryLevelbyGroupType")]
        [HttpPost]
        public HttpResponseMessage ListCbbSalaryLevel(SalaryLevelModel salaryLevelModel)
        {
            var result = _Business.ListCbbSalaryLevel(salaryLevelModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetCbbRecruitmentChannels")]
        [HttpPost]
        public HttpResponseMessage GetCbbRecruitmentChannels()
        {
            var result = _Business.GetCbbRecruitmentChannels();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbSalaryGroups")]
        [HttpPost]
        public HttpResponseMessage GetCbbSalaryGroups()
        {
            var result = _Business.GetCbbSalaryGroups();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbSalarytypes")]
        [HttpPost]
        public HttpResponseMessage GetCbbSalarytypes()
        {
            var result = _Business.GetCbbSalarytypes();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbErrorAffect")]
        [HttpPost]
        public HttpResponseMessage GetCbbErrorAffect()
        {
            var result = _Business.GetCbbErrorAffect();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbChangePlan")]
        [HttpPost]
        public HttpResponseMessage GetCbbChangePlan()
        {
            var result = _Business.GetCbbChangePlan();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbRecruitmentRequest")]
        [HttpPost]
        public HttpResponseMessage GetCbbRecruitmentRequest()
        {
            var result = _Business.GetCbbRecruitmentRequest();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetAllCbbRecruitmentRequest")]
        [HttpPost]
        public HttpResponseMessage GetAllCbbRecruitmentRequest()
        {
            var result = _Business.GetAllCbbRecruitmentRequest();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerContactByCustomerId")]
        [HttpPost]
        public HttpResponseMessage GetCustomerContact(string customerId)
        {
            var result = _Business.GetCustomerContact(customerId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetMeetingType")]
        [HttpGet]
        public HttpResponseMessage GetMeetingType()
        {
            var result = _Business.GetMeetingType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListDepartmentRequestEmployees")]
        [HttpPost]
        public HttpResponseMessage GetListDepartmentRequestEmployees(string departmentRequest)
        {
            var result = _Business.GetListDepartmentRequestEmployees(departmentRequest);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getListDepartmentReceiveEmployees")]
        [HttpPost]
        public HttpResponseMessage getListDepartmentReceiveEmployees(string departmentReceive)
        {
            var result = _Business.getListDepartmentReceiveEmployees(departmentReceive);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProjectPhaseType")]
        [HttpGet]
        public HttpResponseMessage GetProjectPhaseType()
        {
            var result = _Business.GetProjectPhaseType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectAttach")]
        [HttpPost]
        public HttpResponseMessage GetListProjectAttach()
        {
            var result = _Business.GetListProjectAttach();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProjectAttachTabType")]
        [HttpPost]
        public HttpResponseMessage GetProjectAttachTabType(string projectId)
        {
            var result = _Business.GetProjectAttachTabType(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbSBUId")]
        [HttpPost]
        public HttpResponseMessage GetCbbSBUId(string interviewBy)
        {
            try
            {
                var result = _Business.GetCbbSBUId(interviewBy);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetInterview")]
        [HttpPost]
        public HttpResponseMessage GetInterview(string id)
        {
            var result = _Business.GetInterview(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getSBU")]
        [HttpPost]
        public HttpResponseMessage getSBU(string id)
        {
            var result = _Business.getSBU(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getSupplierbyProject")]
        [HttpPost]
        public HttpResponseMessage getSupplierbyProject(string id)
        {
            var result = _Business.GetSuppliersByProjectId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbRole")]
        [HttpPost]
        public HttpResponseMessage GetCbbRole()
        {
            var result = _Business.GetCbbRole();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListSupplier")]
        [HttpPost]
        public HttpResponseMessage GetListSupplier()
        {
            var result = _Business.GetListSuppliers();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListEmployees")]
        [HttpPost]
        public HttpResponseMessage GetListEmployees()
        {
            var result = _Business.GetListEmployees();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerContact")]
        [HttpPost]
        public HttpResponseMessage GetCustomerContact()
        {
            var result = _Business.GetCustomerContact();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
