import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Constants, DateUtils, FileProcess, MessageService, Configuration } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { timeStamp } from 'console';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CandidateService } from '../../services/candidate.service';
import { ApplyService } from '../../services/apply.service';

@Component({
  selector: 'app-candidate-create',
  templateUrl: './candidate-create.component.html',
  styleUrls: ['./candidate-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CandidateCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private router: Router,
    public fileProcess: FileProcess,
    public constant: Constants,
    private comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private routeA: ActivatedRoute,
    private uploadService: UploadfileService,
    private candidateService: CandidateService,
    public config: Configuration,
    private applyService: ApplyService,
  ) { }
  minDateNotificationV: NgbDateStruct;
  dateOfBirth: any;
  dateOfApply: any;
  startDegreesIndex: number = 1;
  startExperienceIndex: number = 1;
  totalYearExperience: number = 0;
  dateStartOfCertificate: any;
  dateEndOfCertificate: any;
  dateOfCall: any;
  recruitmentRequests: any[]=[];
  Id: string;

  applyModel: any = {
    RecruitmentRequestId:'',
    WorkTypeId: null,
    Salary: 0,
    ApplyDate: null,
    ProfileStatus: 0,
    InterviewStatus: 0,
    Status: 0,

    Candidate: {
      Id:'',
      Code: '',
      Name: '',
      Gender: 1,
      Email: '',
      DateOfBirth: null,
      Address: '',
      ProvinceId: null,
      DistrictId: null,
      PhoneNumber: '',
      Languages: [],
      Educations: [],
      WorkHistories: [],
      RecruitmentChannelId: null,
      AcquaintanceStatus: false,
      AcquaintanceName: null,
      SBUId: null,
      DepartmentId: null,
      AcquaintanceNote: null,
      FollowStatus: false,
      WorkTypes: [],
      Attachs: [],
    }
  };

  languageModel: any = {
    Name: '',
    Level: ''
  };

  educationModel: any = {
    Name: '',
    Major: null,
    QualificationId: null,
    Type: null,
    ClassificationId: null,
    Time: null
  };

  classifications: any[] = [  ];

  workModel: any = {
    CompanyName: '',
    TotalTime: null,
    Status: null,
    Position: null,
    Income: null,
    ReferencePersonPhone: null,
    WorkTypeId: null,
  };

  workTypeModel: any = {
    WorktypeId: null
  }

  columnNameAddress: any[] = [{ Name: 'Name', Title: 'Tên' }];
  columnNameWorkType: any[] = [{ Name: 'Name', Title: 'Tên vị trí' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];
  districts: any[] = [];
  wards: any[] = [];
  provinces: any[] = [];
  countries: any[] = [];
  qualifications: any[] = [];
  channels: any[] = [];
  departments: any[] = [];
  sbus: any[] = [];
  workTypes: any[] = [];
  id: any = null;
  identifyDate:any = null;

  fileTemplate: any = {
    Id: '',
    Note: null,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  imageFile: any = null;
  interviewDate: any = null;

  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('id');
    if (this.id) {
      this.appSetting.PageTitle = "Chỉnh sửa Hồ sơ ứng viên";
      // this.getCandidateById();
      this.getAllRecruitmentRequests();
      this.getApplyById();
    }
    else {

      this.appSetting.PageTitle = "Thêm mới Hồ sơ ứng viên";
  
    }
    this.getCbbData();
  }

  getCbbData() {
    let cbbCountry = this.comboboxService.getCbbCountry();
    let cbbProvinces = this.comboboxService.getCbbProvince();
    let cbbQualifications = this.comboboxService.getCBBQualification();
    let cbbClassIfications = this.comboboxService.getCbbClassIfication();
    let cbbRecruitmentChannels = this.comboboxService.getCbbRecruitmentChannels();
    let cbbSBU = this.comboboxService.getCbbSBU();
    let cbbWorkType = this.comboboxService.getListWorkType();

    forkJoin([cbbProvinces, cbbCountry, cbbQualifications, cbbClassIfications, cbbRecruitmentChannels, cbbSBU, cbbWorkType]).subscribe(results => {
      this.provinces = results[0];
      this.countries = results[1];
      this.qualifications = results[2];
      this.classifications = results[3];
      this.channels = results[4];
      this.sbus = results[5];
      this.workTypes = results[6];
    });
  }

  getAllRecruitmentRequests() {
    this.comboboxService.getCbbRecruitmentRequest().subscribe(
      data => {
        this.recruitmentRequests = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getDistricts() {
    this.comboboxService.getCbbDistrict(this.applyModel.Candidate.ProvinceId).subscribe(
      data => {
        this.districts = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getWards() {
    this.comboboxService.getCbbWard(this.applyModel.Candidate.DistrictId).subscribe(
      data => {
        this.wards = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getDepartments() {
    this.comboboxService.getCbbDepartmentBySBU(this.applyModel.Candidate.SBUId).subscribe(
      data => {
        this.departments = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getApplyById() {
    this.applyService.getApplyById(this.id).subscribe(
      data => {
        this.applyModel = data;
        //this.getSalaryMaxMin();
        
        if (this.applyModel.Candidate.DateOfBirth) {
          this.dateOfBirth = this.dateUtils.convertDateToObject(this.applyModel.Candidate.DateOfBirth);
        }

        if (this.applyModel.Candidate.IdentifyDate) {
          this.identifyDate = this.dateUtils.convertDateToObject(this.applyModel.Candidate.IdentifyDate);
        }

        if (this.applyModel.ApplyDate) {
          this.applyModel.ApplyDate = this.dateUtils.convertDateToObject(this.applyModel.ApplyDate);
        }

        if (this.applyModel.InterviewDate) {
          this.interviewDate = this.dateUtils.convertDateToObject(this.applyModel.InterviewDate);
        }

        if (this.applyModel.Candidate.ProvinceId) {
          this.getDistricts();
        }

        if (this.applyModel.Candidate.DistrictId) {
          this.getWards();
        }

        if (this.applyModel.Candidate.SBUId) {
          this.getDepartments();
        }
        this.applyModel.Candidate.WorkHistories.forEach(element => {
          this.totalYearExperience = this.totalYearExperience + element.TotalTime;
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getSalaryRecruitmentRequest() {
    this.applyService.getSalaryByRequestId(this.applyModel.RecruitmentRequestId).subscribe(
      data => {
        this.applyModel.MinSalary = data.MinSalary;
        this.applyModel.MaxSalary = data.MaxSalary;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getSalaryMaxMin() {
    this.applyService.getSalaryLevelById(this.applyModel.WorkTypeId).subscribe(
      data => {
        this.applyModel.MinCompanySalary = data.SalaryLevelMin;
        this.applyModel.MaxCompanySalary = data.SalaryLevelMax;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCandidateById() {
    this.candidateService.getById(this.id).subscribe(
      data => {
        this.applyModel.Candidate = data;
        this.appSetting.PageTitle = "Chỉnh sửa hồ sơ ứng viên - " + this.applyModel.Candidate.Code + ' - ' + this.applyModel.Candidate.Name;
        if (this.applyModel.Candidate.DateOfBirth) {
          this.dateOfBirth = this.dateUtils.convertDateToObject(this.applyModel.Candidate.DateOfBirth);
        }

        if (this.applyModel.Candidate.IdentifyDate) {
          this.identifyDate = this.dateUtils.convertDateToObject(this.applyModel.Candidate.IdentifyDate);
        }

        if (this.applyModel.Candidate.ProvinceId) {
          this.getDistricts();
        }

        if (this.applyModel.Candidate.DistrictId) {
          this.getWards();
        }

        if (this.applyModel.Candidate.SBUId) {
          this.getDepartments();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  addLanguage() {
    if (!this.languageModel.Name) {
      this.messageService.showMessage('Bạn chưa chọn ngôn ngữ');
      return;
    }

    this.applyModel.Candidate.Languages.push(Object.assign({}, this.languageModel));

    this.languageModel = {
      Name: '',
      Level: ''
    };
  }

  showConfirmDeleteLanguage(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Ngoại ngữ này không?").then(
      data => {
        this.deleteLanguage(index);
      },
      error => {

      }
    );
  }

  deleteLanguage(index) {
    this.applyModel.Candidate.Languages.splice(index, 1);
  }

  showConfirmDeleteEducation(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Trình độ này không?").then(
      data => {
        this.deleteEducation(index);
      },
      error => {

      }
    );
  }

  deleteEducation(index) {
    this.applyModel.Candidate.Educations.splice(index, 1);
  }

  addEducation() {
    if (!this.educationModel.Name) {
      this.messageService.showMessage('Bạn chưa nhập Tên Trường/đơn vị đào tạo');
      return;
    }

    this.applyModel.Candidate.Educations.push(Object.assign({}, this.educationModel));

    this.educationModel = {
      Name: '',
      Major: null,
      QualificationId: null,
      Type: null,
      ClassificationId: null,
      Time: null
    };
  }

  showConfirmDeleteWork(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Quá trình Công tác này không?").then(
      data => {
        this.deleteEducation(index);
      },
      error => {

      }
    );
  }

  deleteWork(index) {
    this.applyModel.Candidate.WorkHistories.splice(index, 1);
  }

  addWork() {
    if (!this.workModel.CompanyName) {
      this.messageService.showMessage('Bạn chưa nhập Tên công ty công tác');
      return;
    }

    this.applyModel.Candidate.WorkHistories.push(Object.assign({}, this.workModel));

    this.workModel = {
      CompanyName: '',
      TotalTime: null,
      Status: null,
      Position: null,
      Income: null,
      ReferencePersonPhone: null
    };
  }

  uploadImage($event) {
    this.fileProcess.onAFileChange($event);
  }

  uploadFile($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.applyModel.Candidate.Attachs) {
        if (ite.Id != null) {
          if (file.name == ite.FileName) {
            isExist = true;
          }
        }
        else {
          if (file.name == ite.name) {
            isExist = true;
          }
        }
      }
    }
    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;
    let docuemntTemplate;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.applyModel.Candidate.Attachs.length; index++) {

        if (this.applyModel.Candidate.Attachs[index].Id != null) {
          if (file.name == this.applyModel.Candidate.Attachs[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.applyModel.Candidate.Attachs.splice(index, 1);
            }
          }
        }
        else if (file.name == this.applyModel.Candidate.Attachs[index].name) {
          isExist = true;
          if (isReplace) {
            this.applyModel.Candidate.Attachs.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        this.applyModel.Candidate.Attachs.push(docuemntTemplate);
      }
    }
  }

  updateFileManualDocument(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      this.applyModel.Candidate.Attachs.push(docuemntTemplate);
    }
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  showConfirmDeleteFile(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.deleteFile(document, index);
      },
      error => {

      }
    );
  }

  deleteFile(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.applyModel.Candidate.Attachs.splice(index, 1);
    }
  }

  showConfirmDeleteWorkType(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Vị trí phù hợp này không?").then(
      data => {
        this.deleteWorkType(index);
      },
      error => {

      }
    );
  }

  deleteWorkType(index) {
    this.applyModel.Candidate.WorkTypes.splice(index, 1);
  }

  addWorkType() {
    if (!this.workTypeModel.WorkTypeId) {
      this.messageService.showMessage('Bạn chưa chọn vị trí phù hợp');
      return;
    }

    this.applyModel.Candidate.WorkTypes.push(Object.assign({}, this.workTypeModel));

    this.workTypeModel = {
      WorkTypeId: null
    };
  }

  save(isContinue: boolean) {
    var listFileUpload = [];
    this.applyModel.Candidate.Attachs.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        listFileUpload.push(document.File);
      }
    });

    this.applyModel.Candidate.DateOfBirth = null;
    if (this.dateOfBirth) {
      this.applyModel.Candidate.DateOfBirth = this.dateUtils.convertObjectToDate(this.dateOfBirth);
    }

    this.applyModel.Candidate.IdentifyDate = null;
    if (this.identifyDate) {
      this.applyModel.Candidate.IdentifyDate = this.dateUtils.convertObjectToDate(this.identifyDate);
    }

    if (this.applyModel.ApplyDate) {
      this.applyModel.ApplyDate = this.dateUtils.convertObjectToDate(this.applyModel.ApplyDate);
    }

    this.applyModel.InterviewDate = null;
    if (this.interviewDate) {
      this.applyModel.InterviewDate = this.dateUtils.convertObjectToDate(this.interviewDate);
    }

    if (listFileUpload.length > 0 || this.fileProcess.FilesDataBase) {

      let uploadFiles = this.uploadService.uploadListFile(listFileUpload, 'CandidateDocument/');
      let modelFile = {
        FolderName: 'Candidate/'
      };
      let uploadImages = this.uploadService.uploadImage(this.fileProcess.FileDataBase, modelFile)

      forkJoin([uploadFiles, uploadImages]).subscribe(results => {
        if (results[0] && results[0].length > 0) {
          listFileUpload.forEach((item, index) => {
            this.applyModel.Candidate.Attachs[item.index].FilePath = results[0][index].FileUrl;
          });
        }

        if (results[1]) {
          this.applyModel.Candidate.ImagePath = results[1].FileUrl;
        }

        if (this.id) {
          this.update()
        }
        else {
          this.create(isContinue);
        }
      }, error => {
        this.messageService.showError(error);
      });
    }
    else {

      if (this.id) {
        this.update()
      }
      else {
        this.create(isContinue);
      }
    }
  }

  create(isContinue: boolean) {
    this.candidateService.create(this.applyModel.Candidate).subscribe(
      data => {
        this.messageService.showSuccess('Thêm hồ sơ ứng viên thành công!');

        if (!isContinue) {
          this.closeModal();
        }
        else {
          this.applyModel.Candidate = {
            Code: '',
            Name: '',
            Gender: 1,
            Email: '',
            DateOfBirth: null,
            Address: '',
            ProvinceId: null,
            DistrictId: null,
            PhoneNumber: '',
            Languages: [],
            Educations: [],
            WorkHistories: [],
            RecruitmentChannelId: null,
            AcquaintanceStatus: false,
            AcquaintanceName: null,
            SBUId: null,
            DepartmentId: null,
            AcquaintanceNote: null,
            ProfileStatus: 0,
            InterviewStatus: 0,
            ApplyStatus: 0,
            FollowStatus: false,
            WorkTypes: [],
            Attachs: []
          };
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.applyService.update(this.id, this.applyModel).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật hồ sơ ứng viên thành công!');
        this.close();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['tuyen-dung/ho-so-ung-vien']);
  }

  cancel(){
    this.candidateService.cancel(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật hồ sơ ứng viên thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  totalExp(){
    let totalExp =0;
    this.applyModel.Candidate.WorkHistories.forEach(element => {
      if(element.TotalTime!=''){
        totalExp = totalExp+ parseFloat(element.TotalTime) ;
      }
    });
    this.totalYearExperience = totalExp;
  }
  close() {
    this.router.navigate(['tuyen-dung/ho-so-ung-vien']);
  }
}
