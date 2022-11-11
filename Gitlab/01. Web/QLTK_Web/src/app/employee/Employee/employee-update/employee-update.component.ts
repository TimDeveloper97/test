import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';

import { DatePickerComponent, CalendarView } from '@syncfusion/ej2-angular-calendars';
import { forkJoin } from 'rxjs';
import * as moment from 'moment';
import { loadCldr, L10n } from '@syncfusion/ej2-base';

import { AppSetting, MessageService, Configuration, FileProcess, Constants, DateUtils, ComboboxService, PermissionService } from 'src/app/shared';
import { EmployeeServiceService } from '../../service/employee-service.service';
import { EmployeeUpdateService } from '../../service/employee-update.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';



@Component({
  selector: 'app-employee-update',
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.scss'],
})
export class EmployeeUpdateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private router: Router,
    public fileProcess: FileProcess,
    public constants: Constants,
    private comboboxService: ComboboxService,
    private employeeService: EmployeeServiceService,
    private employeeUpdateService: EmployeeUpdateService,
    public fileProcessImage: FileProcess,
    private routeA: ActivatedRoute,
    public fileProcessDataSheet: FileProcess,
    public dateUtils: DateUtils,
    private uploadService: UploadfileService,
    public permissionService: PermissionService
  ) { }

  ModalInfo = {
    Title: 'Cập nhật nhân viên',
    SaveText: 'Lưu',

  };

  public start: CalendarView = 'Year';
  public depth: CalendarView = 'Year';
  public format: string = 'MM/yyyy';



  isAction: boolean = false;
  Id: string;
  listData: any[] = [];
  listSBU: any[] = [];
  listDepartment: any[] = [];
  listJobPosition: any[] = [];
  userId: string;

  minDateNotificationV: NgbDateStruct;
  CodeSBU: string;
  CodeDepartment: string;
  startWorking: any;
  endWorking: any;

  listRelationship: any[] = [
    { Id: "1", Name: "Ông" },
    { Id: "2", Name: "Bà" },
    { Id: "3", Name: "Bố" },
    { Id: "4", Name: "Mẹ" },
    { Id: "5", Name: "Anh" },
    { Id: "6", Name: "Chị" },
    { Id: "7", Name: "Em" },
    { Id: "8", Name: "Vợ" },
    { Id: "10", Name: "Chồng" },
    { Id: "9", Name: "Con" }
  ]

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Email: '',
    EmployeeId: '',
    ImagePath: '',
    IdentifyNum: '',
    DepartmentId: '',
    DateOfBirth: '',
    StartWorking: '',
    PhoneNumber: '',
    Gender: 1,
    Status: '1',
    EndWorking: '',
    JobPositionId: '',
    SBUId: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: '',
    ListSupplierContact: '',
    WorkType: '',
    Index: 0,
    ListFamilies: [],
    IsOfficial: 1,
    AppliedPositionId: '',
    Seniority: '',               //Thâm niên
    PersonalEmail: '',
    MaritalStatus: '1',
    Forte: '',
    IdentifyAddress: '',
    IdentifyDate: '',
    TaxCode: '',
    ListBankAccount: [],
    StartInsurance: null,
    BookNumberInsurance: '',
    CardNumberInsurance: '',
    WorkCode: '',
    Kcb: '',
    Avatar: null,

    Address: '',
    AddressProvinceId: '',
    AddressDistrictId: '',
    AddressWardId: '',
    PermanentAddress: '',
    PermanentAddressProvinceId: '',
    PermanentAddressDistrictId: '',
    PermanentAddressWardId: '',
    CurrentAddress: '',
    CurrentAddressProvinceId: '',
    CurrentAddressDistrictId: '',
    CurrentAddressWardId: '',
    InsuranceLevelId: '',
    ReasonEndWorkingId: '',
    WorkLocationId: '',
    EmployeeGroupId: '',


    BankAccounts: [],
    WorkHistories: [],
    HistoryJobTranfer: [],
    HistoryAppoint: [],
    HistoriesIncome: [],
    HistoriesInsurance: [],
    HistoriesLaborContract: [],
  }

  familyModel: any = {
    Id: '',
    EmployeeId: '',
    Name: '',
    DateOfBirth: null,
    Relationship: '',
    Gender: '',
    Job: '',
    Workplace: '',
    PhoneNumber: ''
  }

  public minDate: Date;

  jobTranferModel: any = {
    Id: '',
    EmployeeId: '',
    DateJobTranfer: null,
    DateJobTranferV: null,
    WorkTypeId: '',
    DepartmentId: '',
    FileName: '',
    FileSize: '',
    FilePath: ''
  };

  appointModel: any = {
    Id: '',
    EmployeeId: '',
    DateAppoint: null,
    DateAppointV: null,
    PositionId: '',
    DepartmentId: '',
    FileName: '',
    FileSize: '',
    FilePath: ''
  };

  workHistoryModel: any = {
    CompanyName: '',
    Position: '',
    TotalTime: '',
    WorkTypeId: '',
    ReferencePerson: '',
    ReferencePersonPhone: '',
    NumberOfManage: '',
    Income: '',
  };

  incomeModel: any = {
    Id: '',
    EmployeeId: '',
    NewIncome: '',
    DateChange: null,
    DateChangeV: null,
    ReasonChangeIncomeId: ''
  };


  insuranceModel: any = {
    Id: '',
    EmployeeId: '',
    InsuranceLevelId: '',
    InsuranceMoney: '',
    ReasonChangeInsuranceId: '',
    DateChange: null,
    DateChangeV: null
  }

  historiesContract: any[] = [];
  contractModel: any = {
    Id: '',
    EmployeeId: '',
    LaborContractId: '',
    ContractFrom: null,
    ContractFromV: null,
    ContractTo: null,
    ContractToV: null,
    FileName: '',
    FileSize: '',
    FilePath: ''
  };

  provinces: any[] = [];
  districts: any[] = [];
  permanentDistricts: any[] = [];
  currentDistricts: any[] = [];
  wards: any[] = [];
  permanentWards: any[] = [];
  currentWards: any[] = [];
  columnNameAddress: any[] = [{ Name: 'Name', Title: 'Tên' }];
  insuranceLevels: any[] = [];
  banks: any[] = [];
  reasonsEndWorking: any[] = [];
  workLocations: any[] = [];
  departments: any[] = [];
  reasonsChangeIncome: any[] = [];
  reasonsChangeInsurance: any[] = [];
  laborContracts: any[] = [];
  cmtndDate: any = null
  positionCode: string;
  startInsurance = null;

  maritalStatus: any[] = [
    { Id: 1, Name: "Độc thân" },
    { Id: 2, Name: "Đã có gia đình" },
    { Id: 3, Name: "Đã ly hôn" },
  ]

  startBankAccountIndex = 1;
  bankAccModel: any = {
    Id: '',
    EmployeeId: '',
    BankAccountId: '',
    AccountNumber: '',
    BankCode: ''
  }

  employeeGroups: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];

  ngOnInit() {

    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCbbData();
    this.fileProcess.removeFile();
    this.minDateNotificationV = this.dateUtils.getNgbDateStructNow();

    L10n.load({
      'vi': {
        'datepicker': {
          placeholder: '',
          today: 'Tháng này'
        }
      }
    });
  }

  getCbbData() {
    let cbbJobPosition = this.comboboxService.getCbbPosition();
    let cbbSBU = this.comboboxService.getCBBSBU();
    let cbbWorkType = this.comboboxService.getListWorkType();
    let cbbProvinces = this.comboboxService.getCbbProvince();
    let cbbInsuranceLevels = this.comboboxService.getInsuranceLevel();
    let cbbBank = this.comboboxService.getBanks();
    let cbbReasonsEndWorking = this.comboboxService.getReasonsEndWorking();
    let cbbWorkLocation = this.comboboxService.getCbbWorkLocation();
    let cbbDepartment = this.comboboxService.getCbbDepartment();
    let cbbReasonsChangeIncome = this.comboboxService.getReasonsChangeIncome();
    let cbbReasonsChangeInsurance = this.comboboxService.getReasonsChangeInsurance();
    let cbbLaborContract = this.comboboxService.getLabroContract();
    let cbbEmployeeGroup = this.comboboxService.getEmployeeGroup();

    forkJoin([cbbJobPosition, cbbSBU, cbbWorkType, cbbProvinces, cbbInsuranceLevels, cbbBank, cbbReasonsEndWorking, cbbWorkLocation, cbbDepartment, cbbReasonsChangeIncome, cbbReasonsChangeInsurance, cbbLaborContract, cbbEmployeeGroup]).subscribe(results => {
      this.listJobPosition = results[0];
      this.listSBU = results[1];
      this.listWorkType = results[2];
      this.provinces = results[3];
      this.insuranceLevels = results[4];
      this.banks = results[5];
      this.reasonsEndWorking = results[6];
      this.workLocations = results[7];
      this.departments = results[8];
      this.reasonsChangeIncome = results[9];
      this.reasonsChangeInsurance = results[10];
      this.laborContracts = results[11];
      this.employeeGroups = results[12];

      if (this.model.Id) {
        this.getById();
      }
    });
  }

  calculateSeniority() {
    let oneday = 24 * 60 * 60 * 1000;
    let currentDay = new Date();
    if (this.startWorking != null) {
      let startWorkingDay = moment([this.startWorking.year, this.startWorking.month, this.startWorking.day]);
      let current = moment([currentDay.getFullYear(), currentDay.getMonth() + 1, currentDay.getDay()]);
      let diffDays = current.diff(startWorkingDay, 'days', true);
      if (diffDays > 0) {
        this.model.Seniority = Math.round(diffDays / 365 * 100) / 100;
      } else {
        this.model.Seniority = 0;
      }
    }
  }

  getDistricts(provinceId: string, type: number) {
    this.comboboxService.getCbbDistrict(provinceId).subscribe(
      data => {
        if (type == 1) {                    //type=1: nguyên quán
          this.districts = data;
        } else if (type == 2) {             //type=2: thường trú
          this.permanentDistricts = data;
        } else if (type == 3) {             //type=3: địa chỉ hiện tại
          this.currentDistricts = data;
        }

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getWards(districtId: string, type: number) {
    this.comboboxService.getCbbWard(districtId).subscribe(
      data => {
        if (type == 1) {                    //type=1: nguyên quán
          this.wards = data;
        } else if (type == 2) {             //type=2: thường trú
          this.permanentWards = data;
        } else if (type == 3) {             //type=3: địa chỉ hiện tại
          this.currentWards = data;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  insuranceMoney: any;
  getInsuranceMoney() {
    if (this.model.InsuranceLevelId != null && this.model.InsuranceLevelId != "") {
      this.insuranceMoney = this.insuranceLevels.find(a => a.Id == this.model.InsuranceLevelId).Exten;
    }
  }

  getInsuranceChangeMoney(row: any, isRow: boolean) {
    if (isRow) {
      row.InsuranceMoney = this.insuranceLevels.find(a => a.Id == row.InsuranceLevelId).Exten;
    } else {
      this.insuranceModel.InsuranceMoney = this.insuranceLevels.find(a => a.Id == this.insuranceModel.InsuranceLevelId).Exten;
    }
  }

  deleteBankAccount(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá tài khoản ngân hàng này không?").then(
      data => {
        if (this.model.BankAccounts.length > 0) {
          this.model.BankAccounts.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  validateABankAccount() {
    if (this.bankAccModel.BankAccountId == null || this.bankAccModel.BankAccountId == "" || this.bankAccModel.AccountNumber == null || this.bankAccModel.AccountNumber == "") {
      this.messageService.showMessage("Vui lòng nhập đủ thông tin ngân hàng");
    } else {
      let newBankAccModel = Object.assign({}, this.bankAccModel);
      this.model.BankAccounts.push(newBankAccModel);
      this.bankAccModel = {
        Id: '',
        EmployeeId: '',
        BankAccountId: '',
        AccountNumber: '',
        BankCode: ''
      }
    }
  }

  deleteJobTranfer(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn lịch sử điều chuyển này không?").then(
      data => {
        if (this.model.HistoryJobTranfer.length > 0) {
          this.model.HistoryJobTranfer.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  validateJobTranfer() {
    if (this.jobTranferModel.DateJobTranferV == null || this.jobTranferModel.WorkTypeId == "" || this.jobTranferModel.DepartmentId == "") {
      this.messageService.showMessage("Vui lòng nhập đủ thông tin điều chuyển!");
    } else {
      let newJobTranferModel = Object.assign({}, this.jobTranferModel);
      this.model.HistoryJobTranfer.push(newJobTranferModel);
      this.jobTranferModel = {
        Id: '',
        EmployeeId: '',
        DateJobTranfer: null,
        DateJobTranferV: null,
        WorkTypeId: '',
        DepartmentId: '',
        FileName: '',
        FileSize: '',
        FilePath: ''
      }
    }
  }

  deleteWorkHistoty(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá lịch sử công tác này không?").then(
      data => {
        if (this.model.WorkHistories.length > 0) {
          this.model.WorkHistories.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  validateWorkHistory() {
    if (this.workHistoryModel.CompanyName == "") {
      this.messageService.showMessage("Vui lòng nhập tên công ty!");
    }
    else {
      let newWorkHistoryModel = Object.assign({}, this.workHistoryModel);
      this.model.WorkHistories.push(newWorkHistoryModel);
      this.workHistoryModel = {
        CompanyName: '',
        Position: '',
        TotalTime: '',
        WorkTypeId: '',
        ReferencePerson: '',
        ReferencePersonPhone: '',
        NumberOfManage: '',
        Income: '',
      }
    }
  }

  deleteAppoint(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá lịch sử bổ nhiệm này không?").then(
      data => {
        if (this.model.HistoryAppoint.length > 0) {
          this.model.HistoryAppoint.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  validateAppoint() {
    if (this.appointModel.DateAppointV == "" || this.appointModel.PositionId == "" || this.appointModel.DepartmentId == "") {
      this.messageService.showMessage("Vui lòng nhập đầy đủ thông tin!");
    } else {
      let newAppointModel = Object.assign({}, this.appointModel);
      this.model.HistoryAppoint.push(newAppointModel);
      this.appointModel = {
        Id: '',
        EmployeeId: '',
        DateAppoint: null,
        DateAppointV: null,
        PositionId: '',
        DepartmentId: '',
        FileName: '',
        FileSize: '',
        FilePath: ''
      }
    }
  }

  deleteInsuranceHistoty(index: any) {
    if (this.model.HistoriesInsurance.length > 0) {
      this.model.HistoriesInsurance.splice(index, 1);
    }
  }

  validateInsuranceHistory() {

    if (this.insuranceModel.DateChangeV == null || this.insuranceModel.InsuranceLevelId == "" || this.insuranceModel.ReasonChangeInsuranceId == "") {
      this.messageService.showMessage("Vui lòng nhập đủ thông tin điều chỉnh mức đóng bảo hiểm");
    } else {
      let newInsuranceModel = Object.assign({}, this.insuranceModel);
      this.model.HistoriesInsurance.push(newInsuranceModel);
      this.insuranceModel = {
        Id: '',
        EmployeeId: '',
        InsuranceLevelId: '',
        InsuranceMoney: '',
        ReasonChangeInsuranceId: '',
        DateChange: null,
        DateChangeV: null
      }
    }
  }

  deleteContractHistoty(index: any) {
    if (this.model.HistoriesLaborContract.length > 0) {
      this.model.HistoriesLaborContract.splice(index, 1);
    }
  }

  validateContracteHistory() {
    let isOk = true;
    if (this.contractModel.LaborContractId == "" || this.contractModel.ContractFromV == null) {
      this.messageService.showMessage("Vui lòng nhập đủ thông tin hợp đồng!");
      isOk = false;
    } else {
      let contractFrom = this.contractModel.ContractFromV.year + "-" + this.contractModel.ContractFromV.month + "-" + this.contractModel.ContractFromV.day;
      let contractDateFrom = new Date(contractFrom);

      if (this.contractModel.ContractToV != null) {
        let contractTo = this.contractModel.ContractToV.year + "-" + this.contractModel.ContractToV.month + "-" + this.contractModel.ContractToV.day;
        let contractDateTo = new Date(contractTo);
        if (contractDateFrom > contractDateTo) {
          this.messageService.showMessage("Ngày bắt đầu hợp đồng lớn hơn ngày kết thúc hợp đồng!");
          isOk = false;
        }
      }
    }

    if (isOk) {
      let newContractModel = Object.assign({}, this.contractModel);
      this.model.HistoriesLaborContract.push(newContractModel);
      this.contractModel = {
        Id: '',
        EmployeeId: '',
        LaborContractId: '',
        ContractFrom: null,
        ContractFromV: null,
        ContractTo: null,
        ContractToV: null,
        FileName: '',
        FileSize: '',
        FilePath: ''
      }
    }

  }

  deleteIncomeHistoty(index: any) {
    if (this.model.HistoriesIncome.length > 0) {
      this.model.HistoriesIncome.splice(index, 1);
    }
  }

  validateIncomeHistory() {

    if (this.incomeModel.NewIncome == "" || this.incomeModel.DateChangeV == null || this.incomeModel.ReasonChangeIncomeId == "") {
      this.messageService.showMessage("Vui lòng nhập đầy đủ thông tin điều chỉnh thu nhập!");
    } else {
      let newIncomeModel = Object.assign({}, this.incomeModel);
      this.model.HistoriesIncome.push(newIncomeModel);
      this.incomeModel = {
        Id: '',
        EmployeeId: '',
        NewIncome: '',
        DateChange: null,
        DateChangeV: null,
        ReasonChangeIncomeId: ''
      }
    }
  }

  filedata: any;
  dateOfBirth: any;
  public async onFileChange($event) {
    this.fileProcess.onAFileChange($event);
    if ($event.target.files && $event.target.files.length > 0) {
      let files = $event.target.files;
      let reader = new FileReader();
      reader.readAsDataURL(files[0]);
      
          reader.onload = () => {
              var filer = {
                  Name: files[0].name,
                  DataURL: reader.result,
                  Size: files[0].size
              }
              this.model.Avatar = filer.DataURL;
          };
    }
  }

  filesDecisionTranfer: any[] = [];
  uploadDecisionTranfer($event, row: any) {
    var file = this.fileProcess.getFileOnFileChange($event);
    if (file[0].size > 5000000) {
      this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
      this.fileProcess.totalbyte = 0;
    } else {
      row.FileName = file[0].name;
      row.FileSize = file[0].size;
      this.filesDecisionTranfer.push(file[0]);
    }
  }

  filesDecisionAppoint: any[] = [];

  uploadDecisionAppoint($event, row: any) {
    var file = this.fileProcess.getFileOnFileChange($event);
    if (file[0].size > 5000000) {
      this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
      this.fileProcess.totalbyte = 0;
    } else {
      row.FileName = file[0].name;
      row.FileSize = file[0].size;
      this.filesDecisionAppoint.push(file[0]);
    }
  }

  filesContract: any[] = [];
  uploadContract($event, row: any) {
    var file = this.fileProcess.getFileOnFileChange($event);
    if (file[0].size > 5000000) {
      this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
      this.fileProcess.totalbyte = 0;
    } else {
      row.FileName = file[0].name;
      row.FileSize = file[0].size;
      this.filesContract.push(file[0]);
    }
  }

  downloadAFile(row: any) {
    this.fileProcess.downloadFileBlob(row.FilePath, row.FileName);
  }

  getById() {
    this.employeeService.getById(this.model).subscribe(data => {
      this.model = data;
      this.appSetting.PageTitle = "Chỉnh sửa nhân viên - " + this.model.Code + " - " + this.model.Name;
      this.model.SBUId = data.SBUID;
      this.CodeDepartment = data.DepartmentCode;
      if (data.EndWorking != null) {
        this.endWorking = this.dateUtils.convertDateToObject(data.EndWorking);
      }
      if (data.StartWorking != null) {
        this.startWorking = this.dateUtils.convertDateToObject(data.StartWorking);
      }
      if (data.DateOfBirth != null) {
        this.dateOfBirth = this.dateUtils.convertDateToObject(data.DateOfBirth);
      }

      if (this.model.StartInsurance) {
        this.startInsurance = this.model.StartInsurance;
      }

      this.GetCbbDepartmentById();
      if (data.DepartmentId == null || data.DepartmentId == '') {
        this.getCbbSBUById();
      }
      else {
        this.getCbbSBU();
      }

      if (data.ImagePath) {
        this.model.Avatar = this.config.ServerFileApi + data.ImagePath;
      }
      this.model.ListFamilies.forEach(element => {
        if (element.DateOfBirth != null) {
          element.DateOfBirthV = this.dateUtils.convertDateToObject(element.DateOfBirth)
        }
      });

      if (data.EndWorking != null) {
        this.model.EndWorking = this.dateUtils.convertDateToObject(data.EndWorking);
      }
      if (data.DateOfBirth != null) {
        this.model.DateOfBirth = this.dateUtils.convertDateToObject(data.DateOfBirth);
      }

      if (data.StartWorking != null) {
        this.model.StartWorking = this.dateUtils.convertDateToObject(data.StartWorking);;
      }

      if (data.IdentifyDate != null) {
        this.cmtndDate = this.dateUtils.convertDateToObject(data.IdentifyDate);;
      }

      this.model.HistoryJobTranfer.forEach(element => {
        if (element.DateJobTranfer != null && element.DateJobTranfer != "") {
          element.DateJobTranferV = this.dateUtils.convertDateToObject(element.DateJobTranfer);;
        }
      });

      this.model.HistoryAppoint.forEach(element => {
        if (element.DateAppoint != null && element.DateAppoint != "") {
          element.DateAppointV = this.dateUtils.convertDateToObject(element.DateAppoint);;
        }
      });

      this.model.HistoriesIncome.forEach(element => {
        if (element.DateChange != null && element.DateChange != "") {
          element.DateChangeV = this.dateUtils.convertDateToObject(element.DateChange);;
        }
      });

      this.model.HistoriesInsurance.forEach(element => {
        if (element.DateChange != null && element.DateChange != "") {
          element.DateChangeV = this.dateUtils.convertDateToObject(element.DateChange);;
        }
      });

      this.model.HistoriesLaborContract.forEach(element => {
        if (element.ContractFrom != null && element.ContractFrom != "") {
          element.ContractFromV = this.dateUtils.convertDateToObject(element.ContractFrom);;
        }
        if (element.ContractTo != null && element.ContractTo != "") {
          element.ContractToV = this.dateUtils.convertDateToObject(element.ContractTo);;
        }
      });

      this.getWorkTypeCode();
      this.getPositionCode();
      this.getInsuranceMoney();
      this.calculateSeniority();
      this.getDistricts(this.model.AddressProvinceId, 1);
      this.getDistricts(this.model.PermanentAddressProvinceId, 2);
      this.getDistricts(this.model.CurrentAddressProvinceId, 3);
      this.getWards(this.model.AddressDistrictId, 1);
      this.getWards(this.model.PermanentAddressDistrictId, 2);
      this.getWards(this.model.CurrentAddressDistrictId, 3)
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getBankCode(row: any, isRow: boolean) {
    if (isRow) {
      row.BankCode = this.banks.find(a => a.Id == row.BankAccountId).Code;
    } else {
      this.bankAccModel.BankCode = this.banks.find(a => a.Id == row.BankAccountId).Code;
    }
  }

  getName(SBUID) {
    for (var item of this.listSBU) {
      if (item.Id == SBUID) {
        this.CodeSBU = item.Code;
        this.CodeDepartment = null;
      }
    }
  }

  getPositionCode() {
    this.positionCode = this.listJobPosition.find(a => a.Id == this.model.JobPositionId).Code;
  }

  getWorkTypeCode() {
    this.model.WorkCode = this.listWorkType.find(a => a.Id == this.model.WorkType).Code;
  }

  listWorkType: any[] = [];

  getListWorkType() {
    this.comboboxService.getListWorkType().subscribe(
      data => {
        this.listWorkType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getNameDepartment(DepartmentId) {
    for (var item of this.listDepartment) {
      if (item.Id == DepartmentId) {
        this.CodeDepartment = item.Code;
      }
    }

  }

  getCbbSBUById() {
    this.model.SBUID = null;
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
        this.model.SBUID = '';
        // for (var item of this.listSBU) {
        //   if (item.Id == this.model.SBUID) {
        //     this.CodeSBU = item.Code;
        //   }

        // }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


  getCbbSBU() {
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
        for (var item of this.listSBU) {
          if (item.Id == this.model.SBUID) {
            this.CodeSBU = item.Code;
          }

        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  GetCbbDepartmentById() {
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );


  }


  GetCbbDepartment() {
    this.model.DepartmentId = null;
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
        this.model.DepartmentId = '';
        this.getName(this.model.SBUId);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCBBJobPosition() {
    this.comboboxService.getCbbJobPositions().subscribe(
      data => {
        this.listJobPosition = data;
        for (var i of this.listJobPosition) {
          if (i.Id == "" || i.Id == null) {
            this.listJobPosition.splice(this.listJobPosition.indexOf(i), 1);
          }
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  checkStage: boolean;
  save(isContinue: boolean) {
    this.checkStage = true;

    let filesDecisionTranfer = this.uploadService.uploadListFile(this.filesDecisionTranfer, 'DecisionJobTranfer/');
    let filesDecisionAppoint = this.uploadService.uploadListFile(this.filesDecisionAppoint, 'DecisionAppoint/');
    let filesContract = this.uploadService.uploadListFile(this.filesContract, 'LaborContract/');

    forkJoin([filesDecisionTranfer, filesDecisionAppoint, filesContract]).subscribe(results => {
      if (results[0].length > 0) {
        results[0].forEach(item => {
          var jobTranfer = this.model.HistoryJobTranfer.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
          jobTranfer.FilePath = item.FileUrl;
        });
      }

      if (results[1].length > 0) {
        results[1].forEach(item => {
          var appoint = this.model.HistoryAppoint.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
          appoint.FilePath = item.FileUrl;
        });
      }

      if (results[2].length > 0) {
        results[2].forEach(item => {
          var contract = this.model.HistoriesLaborContract.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
          contract.FilePath = item.FileUrl;
        });
      }

      if (this.fileProcess.FileDataBase == null) {
        this.updateEmployee();
      }
      else {
        let modelFile = {
          FolderName: 'Employee/'
        };
        this.employeeUpdateService.uploadImage(this.fileProcess.FileDataBase, modelFile).subscribe(
          data => {
            this.model.ImagePath = data.FileUrl;
            this.updateEmployee();
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  updateEmployee() {
    var validEmail = true;
    var regex = this.constants.validEmailRegEx;
    if (this.model.Email != '') {
      if (!regex.test(this.model.Email)) {
        validEmail = false;
      }
    }
    if (this.endWorking != null && this.endWorking != '') {
      this.model.EndWorking = this.dateUtils.convertObjectToDate(this.endWorking)
    }
    if (this.startWorking != null && this.startWorking != '') {
      this.model.StartWorking = this.dateUtils.convertObjectToDate(this.startWorking)
    }
    if (this.dateOfBirth != null && this.dateOfBirth != '') {
      this.model.DateOfBirth = this.dateUtils.convertObjectToDate(this.dateOfBirth)
    }
    this.model.ListFamilies.forEach(element => {
      if (element.DateOfBirthV != null && element.DateOfBirthV != '') {
        element.DateOfBirth = this.dateUtils.convertObjectToDate(element.DateOfBirthV);
      }
    });

    this.model.HistoryJobTranfer.forEach(element => {
      if (element.DateJobTranferV != null && element.DateJobTranferV != "") {
        element.DateJobTranfer = this.dateUtils.convertObjectToDate(element.DateJobTranferV);
      }
    });

    this.model.HistoryAppoint.forEach(element => {
      if (element.DateAppointV != null && element.DateAppointV != "") {
        element.DateAppoint = this.dateUtils.convertObjectToDate(element.DateAppointV);
      }
    });

    this.model.HistoriesIncome.forEach(element => {
      if (element.DateChangeV != null && element.DateChangeV != "") {
        element.DateChange = this.dateUtils.convertObjectToDate(element.DateChangeV);
      }
    });

    this.model.HistoriesInsurance.forEach(element => {
      if (element.DateChangeV != null && element.DateChangeV != "") {
        element.DateChange = this.dateUtils.convertObjectToDate(element.DateChangeV);
      }
    });

    this.model.HistoriesLaborContract.forEach(element => {
      if (element.ContractFromV != null && element.ContractFromV != "") {
        element.ContractFrom = this.dateUtils.convertObjectToDate(element.ContractFromV);
      }
      if (element.ContractToV != null && element.ContractToV != "") {
        element.ContractTo = this.dateUtils.convertObjectToDate(element.ContractToV);
      }
    });

    if (this.cmtndDate != null) {
      this.model.IdentifyDate = this.dateUtils.convertObjectToDate(this.cmtndDate)
    }

    this.model.StartInsurance = null;
    if (this.startInsurance) {
      this.model.StartInsurance = moment(this.startInsurance).format('YYYY-MM-DD');
    }

    this.employeeUpdateService.updateEmployee(this.model).subscribe(
      data => {
        if (validEmail) {
          this.messageService.showSuccess('Cập nhật nhân viên thành công!');
          this.closeModal(true);
        }
        else {
          this.messageService.showSuccess('Cập nhật nhân viên thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
        // this.getById();
      }
    );
  }
  // Thêm người liên hệ
  Index = 1;

  addRow() {
    if (this.familyModel.Name != "") {
      var addFamily = Object.assign({}, this.familyModel);
      this.model.ListFamilies.push(addFamily);
      this.familyModel = {
        Id: '',
        EmployeeId: '',
        Name: '',
        DateOfBirth: null,
        Relationship: '',
        Gender: '',
        Job: '',
        Workplace: '',
        PhoneNumber: ''
      };
    }
    else {
      this.messageService.showMessage("Tên không được để trống!");
    }
  }

  deleteRow(id, index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá người liên hệ này không?").then(
      data => {
        this.model.ListFamilies.splice(index, 1);
      },
      error => {

      }
    );
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['nhan-vien/quan-ly-nhan-vien']);
  }
}
