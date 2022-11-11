import { Component, OnInit, ViewEncapsulation, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ProjectEmployeeService } from '../../service/project-employee.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
@Component({
  selector: 'app-project-employee-create',
  templateUrl: './project-employee-create.component.html',
  styleUrls: ['./project-employee-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectEmployeeCreateComponent implements OnInit {

  @Input() Id: string;

  constructor(
    private activeModal: NgbActiveModal,
    private comboboxService: ComboboxService,
    private modalService: NgbModal,
    public constant: Constants,
    public dateUtils: DateUtils,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    public fileProcessDataSheet: FileProcess,
    public fileProcess: FileProcess,
    private service: ProjectEmployeeService,
    private messageService: MessageService,
    private uploadService: UploadfileService,
  ) { }

  minDateNotificationV: NgbDateStruct;
  dateOfBirth: any;
  provinces: any[] = [];
  districts: any[] = [];
  wards: any[] = [];
  listRole = [];
  currentDistricts: any[] = [];
  currentWards: any[] = [];

  modalInfo = {
    Title: 'Thêm mới nhân viên ngoài tham gia dự án',
    SaveText: 'Lưu',
  };
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];
  projectId:'';
  model: any = {
    Name: '',
    Email: '',
    ImagePath: '',
    DateOfBirth: '',
    PhoneNumber: '',
    Gender: 1,
    TaxCode: '',
    Address: '',
    BankAccount:'',
    BankName: '',
    BankCode: '',
    IdentifyNum: '',
    IdentifyAddress: '',
    IdentifyDate: '',
    CurrentAddress: '',
    CurrentAddressProvinceId: '',
    CurrentAddressDistrictId: '',
    CurrentAddressWardId: '',
    JobDescription: '',
    RoleId: '',
    Status: '',
    Evaluate: '',
    StartTime: '',
    EndTime: '',
    Subsidy: '',
    SubsidyStartTime: '',
    SubsidyEndTime: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: '',
    Type: '',
    ProjectId: '',
  }
  columnNameAddress: any[] = [{ Name: 'Name', Title: 'Tên' }];
  cmtndDate: any = null;
  startTime: any = null;
  endTime: any = null;
  subsidyStartTime: any = null;
  subsidyEndTime: any = null;
  DescriptionRole = '';
  banks: any[] = [];
  IBankCode: '';

  ngOnInit(): void {
    this.getCbbData();
    this.minDateNotificationV = this.dateUtils.getNgbDateStructNow();
    this.getRole();
  }

  getCbbData() {
    let cbbProvinces = this.comboboxService.getCbbProvince();
    let cbbBank = this.comboboxService.getBanks();
    forkJoin([cbbProvinces,cbbBank]).subscribe(results => {
      this.provinces = results[0];
      this.banks = results[1];
    });
  }

  getBankCode() {
    this.IBankCode = this.banks.find(a => a.Id == this.model.BankName).Code;
  }

  closeModal() {
    this.activeModal.close(true);
  }

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
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

  getRole(){
    this.service.getRole().subscribe(data => {
      if (data) {
        this.listRole = data;
        console.log(data);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  changeRole(RoleId: any){
    this.service.getDescriptionRoleById(RoleId).subscribe((data: any) => {
      if (data) {
        this.DescriptionRole = data.Description;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  
  getDistricts(provinceId: string, type: number) {
    this.comboboxService.getCbbDistrict(provinceId).subscribe(
      data => {          
          this.currentDistricts = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getWards(districtId: string, type: number) {
    this.comboboxService.getCbbWard(districtId).subscribe(
      data => {           
          this.currentWards = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(){
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
        this.createProjectExEmployee();
      }
      else {
        let modelFile = {
          FolderName: 'Employee/'
        };
        this.service.uploadImage(this.fileProcess.FileDataBase, modelFile).subscribe(
          data => {
            this.model.ImagePath = data.FileUrl;
            this.createProjectExEmployee();
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  createProjectExEmployee(){
    var validEmail = true;
    var regex = this.constant.validEmailRegEx;
    if (this.model.Email != '') {
      if (!regex.test(this.model.Email)) {
        validEmail = false;
      }
    }
    if (this.subsidyStartTime != null && this.subsidyStartTime != '') {
      this.model.SubsidyStartTime = this.dateUtils.convertObjectToDate(this.subsidyStartTime)
    }
    if (this.subsidyEndTime != null && this.subsidyEndTime != '') {
      this.model.SubsidyEndTime = this.dateUtils.convertObjectToDate(this.subsidyEndTime)
    }
    if (this.startTime != null && this.startTime != '') {
      this.model.StartTime = this.dateUtils.convertObjectToDate(this.startTime)
    }
    if (this.endTime != null && this.endTime != '') {
      this.model.EndTime = this.dateUtils.convertObjectToDate(this.endTime)
    }
    if (this.dateOfBirth != null && this.dateOfBirth != '') {
      this.model.DateOfBirth = this.dateUtils.convertObjectToDate(this.dateOfBirth)
    }
    if (this.cmtndDate != null) {
      this.model.IdentifyDate = this.dateUtils.convertObjectToDate(this.cmtndDate)
    }
    if(!this.model.JobDescription){
      this.model.JobDescription = this.DescriptionRole; 
    }
    // if(!this.model.BankCode){
    //   this.model.BanhCode = this.IBankCode; 
    // }
    this.model.Type = 2;
    this.model.ProjectId = this.projectId;
    this.model.Status = 1;
    this.model.Evaluate = 5;
    this.model.BankCode = this.IBankCode; 
    
    if (validEmail) {
      this.service.CreateProjectEmployeeAndExternalEmployee(this.model).subscribe(
        data => {
              this.clear();
              this.messageService.showSuccess('Thêm mới nhân viên thành công!');
              this.closeModal();
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.messageService.showSuccess('Email không hợp lệ!');
    }
  }

  clear(){
    this.model = {
      Name: '',
    Email: '',
    ImagePath: '',
    DateOfBirth: '',
    PhoneNumber: '',
    Gender: 1,
    TaxCode: '',
    Address: '',
    BankAccount:'',
    BankName: '',
    BankCode: '',
    IdentifyNum: '',
    IdentifyAddress: '',
    IdentifyDate: '',
    CurrentAddress: '',
    CurrentAddressProvinceId: '',
    CurrentAddressDistrictId: '',
    CurrentAddressWardId: '',
    JobDescription: '',
    RoleId: '',
    Status: '',
    Evaluate: '',
    StartTime: '',
    EndTime: '',
    Subsidy: '',
    SubsidyStartTime: '',
    SubsidyEndTime: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: '',
    Type: '',
    ProjectId: '',
    }
    if (this.fileProcess.fileModel.DataURL != undefined) {
      this.fileProcess.fileModel.DataURL = null;
    }
  }
}
