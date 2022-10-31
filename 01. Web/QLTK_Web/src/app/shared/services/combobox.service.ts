import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../config/configuration';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})
export class ComboboxService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCbbMaterialGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListMaterialGroup', httpOptions);
    return tr
  }

  getCbbMaterialGroupParent(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListMaterialGroupParent', httpOptions);
    return tr
  }

  getCbbManufacture(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListManufacture', httpOptions);
    return tr
  }
  getCbbIndustry(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListIndustry', httpOptions);
    return tr
  }
  getCbbNSMaterialType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListNSMaterialType', httpOptions);
    return tr
  }

  getCbbMaterialGroupTPA(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListTPA', httpOptions);
    return tr
  }

  getCbbRawMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListRawMaterial', httpOptions);
    return tr
  }

  getCbbUnit(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListUnit', httpOptions);
    return tr
  }

  getCbbModule(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbModule', httpOptions);
    return tr
  }

  getCbbProduct(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbProduct', httpOptions);
    return tr
  }

  getCbbModuleGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupModuleParentChild', httpOptions);
    return tr
  }

  getCbbJobGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupJobParentChild', httpOptions);
    return tr
  }

  getCbbSupplierGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupSupplierParentChild', httpOptions);
    return tr
  }

  getCbbManufactureGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupManufactureParentChild', httpOptions);
    return tr
  }

  getCbbCriter(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListCriter', httpOptions);
    return tr
  }

  getCbbStage(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbStage', httpOptions);
    return tr
  }
  
  getCbbEmployee(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbEmployees', httpOptions);
    return tr
  }
  getCbbSBU(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSBU', httpOptions);
    return tr
  }

  getSolutionTechnologies(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetSolutionTechnologies', httpOptions);
    return tr
  }

  getCBBSupplier(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbSupplier', httpOptions);
    return tr
  }

  getApplication(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetApplication', httpOptions);
    return tr
  }

  getCBBSupplierService(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbSupplierService', httpOptions);
    return tr
  }

  getCBBSBU(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListCBBSBU', httpOptions);
    return tr
  }

  getCbbManager(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListManagers', httpOptions);
    return tr
  }

  getCbbDepartmentBySBU(sbuId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbDepartmentBySBUId?sbuId=' + sbuId, httpOptions);
    return tr
  }
  
  getCustomerRequirement(customerId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCustomerRequirement?customerId=' + customerId, httpOptions);
    return tr
  }

  getCustomerRequirementById(customerRequirementId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCustomerRequirementById?customerRequirementId=' + customerRequirementId, httpOptions);
    return tr
  }

  getListJobPosition(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBJobPosition', httpOptions);
    return tr
  }

  getListGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListGroupFunction', httpOptions);
    return tr
  }
  getCBBGroupUser(departmentId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListGroupUsers?departmentId=' + departmentId, httpOptions);
    return tr
  }
  getListDegree(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListDegree', httpOptions);
    return tr
  }
  getListSpecialize(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSpecialize', httpOptions);
    return tr
  }
  getListWorkPlace(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListWorkPlace', httpOptions);
    return tr
  }

  getListRoomType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListRoomType', httpOptions);
    return tr
  }

  getCbbProductGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetProductGroupParentChild', httpOptions);
    return tr
  }

  getCbbDegree(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListDegree', httpOptions);
    return tr
  }

  getCbbSkillGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSkillGroup', httpOptions);
    return tr
  }

  getCbbProductStandard(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProductStandard', httpOptions);
    return tr
  }

  getCbbProductStandardGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProductStandardGroup', httpOptions);
    return tr
  }

  getCbbDepartment(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListDepartment', httpOptions);
    return tr
  }

  getCbbDepartmentUse(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListDepartmentUse', httpOptions);
    return tr
  }

  getCbbJobPositions(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListJobPosition', httpOptions);
    return tr
  }

  getCBBGroupUsers(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupUser', httpOptions);
    return tr
  }

  getCBBQualification(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetQualification', httpOptions);
    return tr
  }

  getCbbPracticeGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListPracticeGroupA', httpOptions);
    return tr
  }

  getCBBListGroupJob(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListGroupJob', httpOptions);
    return tr
  }

  getListClassRoom(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListClassRoom', httpOptions);
    return tr
  }

  getListJob(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListJob', httpOptions);
    return tr
  }
  getListCustomer(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListCustomer', httpOptions);
    return tr
  }

  getListMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListMaterial', httpOptions);
    return tr
  }

  getListCustomerType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListCustomerType', httpOptions);
    return tr
  }

  getListErrorGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListErrorGroup', httpOptions);
    return tr
  }

  getListModule(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListModule', httpOptions);
    return tr
  }

  GetListModuleByProjectId(id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListModuleByProjectId/'+id, httpOptions);
    return tr
  }

  getListProject(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProject', httpOptions);
    return tr
  }

  getEmployeeByDepartment(DepartmentId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetEmployeeByDepartment?departmentId=' + DepartmentId, httpOptions);
    return tr
  }

  getEmployeeByDepartmentWithStatus(DepartmentId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetEmployeeByDepartmentWithStatus?departmentId=' + DepartmentId, httpOptions);
    return tr
  }

  getListManufactureGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListManufactureGroup', httpOptions);
    return tr
  }

  getListSupplierGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSupplierGroup', httpOptions);
    return tr
  }

  getListTask(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListTask', httpOptions);
    return tr
  }

  getListProjectProduct(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProjectProduct?ProjectId=' + projectId, httpOptions);
    return tr
  }

  getListProduct(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProduct', httpOptions);
    return tr
  }

  getListUser(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListUser', httpOptions);
    return tr
  }

  getListSolutionGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSolutionGroup', httpOptions);
    return tr
  }
  getListUserWithDepartment(departmentId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListUserWithDepartment?departmentId=' + departmentId, httpOptions);
    return tr
  }

  getDepartmentIdWithUserId(UserId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetDepartmentIdWithUserId?UserId=' + UserId, httpOptions);
    return tr
  }

  getListProjectProductByProjectId(ProjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProjectProductByProjectId?ProjectId=' + ProjectId, httpOptions);
    return tr
  }
  getListEmployeesStatus(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbEmployeesStatus', httpOptions);
    return tr
  }

  getListSkill(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getListSkill', httpOptions);
    return tr
  }

  getCBBMarialTPA(marialgroupId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getListSkill=' + marialgroupId, httpOptions);
    return tr
  }

  getCodeAutoEmPloyee(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCodeAutoEmPloyee', httpOptions);
    return tr
  }

  getListModuleGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupModule', httpOptions);
    return tr
  }

  getListProductGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupProduct', httpOptions);
    return tr
  }

  GetCbbWorkTime(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbWorkTime', httpOptions);
    return tr
  }

  GetCbbEmployee(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbEmployee', httpOptions);
    return tr
  }

  getCodeError(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCodeError', httpOptions);
    return tr
  }

  getCodeProblem(type): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCodeProblem?Type=' + type, httpOptions);
    return tr
  }

  GetCbbEmployeeByDepartmentId(departmentId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbEmployeeByDepartmentId?departmentId=' + departmentId, httpOptions);
    return tr
  }

  getListWorkType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListWorkType', httpOptions);
    return tr
  }

  getListWorkSkill(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListWorkSkill', httpOptions);
    return tr
  }

  getListProjectByUser(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProjectByUser', httpOptions);
    return tr
  }

  getListProjectByUserAndDate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProjectByUserAndDate', model, httpOptions);
    return tr
  }

  getListProjectDownloadDocumentDesign(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProjectDownloadDocumentDesign', httpOptions);
    return tr
  }

  getCbbClassIfication(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListClassIfication', httpOptions);
    return tr
  }

  GetCBBClassIfication(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBClassIfication', httpOptions);
    return tr
  }

  getCBBProductStandardTPAType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBProductStandardTPAType', httpOptions);
    return tr
  }

  
  getCbbRecruitmentChannels(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbRecruitmentChannels', httpOptions);
    return tr
  }


  getCBBSaleProductType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBSaleProductType', httpOptions);
    return tr
  }

  getCBBApplication(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBApplication', httpOptions);
    return tr
  }

  GetListCustomers(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListCustomers', httpOptions);
    return tr
  }
  getListCountry(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListCountry', httpOptions);
    return tr
  }
  getCbbCountry(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getCbbCountry', httpOptions);
    return tr
  }

  getCbbSaleProduct(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbSaleProduct', httpOptions);
    return tr
  }

  getCbbLanguage(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getCbbLanguage', httpOptions);
    return tr
  }


  getCbbProvince(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetProvince', httpOptions);
    return tr
  }

  getCbbDistrict(provinceId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetDistrict?provinceId=' + provinceId, httpOptions);
    return tr
  }

  getCbbWard(districtId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetWard?districtId=' + districtId, httpOptions);
    return tr
  }

  getInsuranceLevel(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetInsuranceLevel', httpOptions);
    return tr
  }

  getBanks(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetBanks', httpOptions);
    return tr
  }

  getReasonsEndWorking(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetReasonsEndWorking', httpOptions);
    return tr
  }

  getCbbPosition(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetPosition', httpOptions);
    return tr
  }

  getCbbWorkLocation(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetWorkLocation', httpOptions);
    return tr
  }

  getReasonsChangeIncome(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetReasonChangeIncome', httpOptions);
    return tr
  }

  getReasonsChangeInsurance(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetReasonChangeInsurance', httpOptions);
    return tr
  }

  getLabroContract(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetLabroContract', httpOptions);
    return tr
  }

  getEmployeeGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/EmployeeGroup', httpOptions);
    return tr
  }

  getQuestionGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/QuestionGroup', httpOptions);
    return tr
  }

  getDocumentGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/DocumentGroup', httpOptions);
    return tr
  }

  getDocumentType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/DocumentType', httpOptions);
    return tr
  }

  getCbbDocumentGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListDocumentGroup', httpOptions);
    return tr
  }

  getCbbFlowStage(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListFlowStage', httpOptions);
    return tr
  }

  getCbbSalaryLevel(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSalaryLevel', httpOptions);
    return tr
  }

  getCbbSalaryLevelByGroupType(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSalaryLevelbyGroupType', model,httpOptions);
    return tr
  }

  getCbbSalaryGroups(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbSalaryGroups', httpOptions);
    return tr
  }

  getCbbSalarytypes(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbSalarytypes', httpOptions);
    return tr
  }

  getLabroContractSupplier(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetSupplierContract', httpOptions);
    return tr
  }

  getCbbErrorAffect(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbErrorAffect', httpOptions);
    return tr
  }

  getCbbChangePlan(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbChangePlan', httpOptions);
    return tr
  }

  getCbbRecruitmentRequest(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbRecruitmentRequest', httpOptions);
    return tr
  }

  getAllCbbRecruitmentRequest(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetAllCbbRecruitmentRequest', httpOptions);
    return tr
  }
  getCustomerContact(customerId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCustomerContactByCustomerId?customerId=' + customerId, httpOptions);
    return tr
  }

  getListCustomerContact(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCustomerContact', httpOptions);
    return tr
  }

  getMeetingType(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetMeetingType', httpOptions);
    return tr
  }

  getListEmployees(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListEmployees', httpOptions);
    return tr
  }

  getListDepartmentRequestEmployees(departmentRequest): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListDepartmentRequestEmployees?departmentRequest=' + departmentRequest, httpOptions);
    return tr
  }

  getListDepartmentReceiveEmployees(departmentReceive): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getListDepartmentReceiveEmployees?departmentReceive=' + departmentReceive, httpOptions);
    return tr
  }

  getProjectPhaseType(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetProjectPhaseType', httpOptions);
    return tr
  }

  getListProjectAttach(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListProjectAttach', httpOptions);
    return tr
  }

  GetProjectAttachTabType(Id : any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetProjectAttachTabType?projectId='+Id, httpOptions);
    return tr
  }

  getCbbBySBU(interviewBy): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbSBUId?interviewBy=' + interviewBy, httpOptions);
    return tr
  }

  getInterview(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetInterview?id=' + id, httpOptions);
    return tr
  }

  getSBU(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getSBU?id=' + id, httpOptions);
    return tr
  }

  getCBBSaleProductTypes(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBSaleProductTypes', httpOptions);
    return tr
  }

  getSupplierbyProject(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/getSupplierbyProject?id='+projectId, httpOptions);
    return tr
  }

  getRoleType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCbbRole', httpOptions);
    return tr
  }

  getListSupplier(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSupplier', httpOptions);
    return tr
  }

  getListDomain(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetDomain', httpOptions);
    return tr
  }
}
