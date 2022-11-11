import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Constants, DateUtils, FileProcess, MessageService, Configuration } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { InterviewService } from '../../services/interview.service';
import { QuestionInterviewCreateComponent } from '../question-interview-create/question-interview-create.component';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';

@Component({
  selector: 'app-interview-create',
  templateUrl: './interview-create.component.html',
  styleUrls: ['./interview-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InterviewCreateComponent implements OnInit {

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
    private interviewService: InterviewService,
    public config: Configuration,
    private modalService: NgbModal,
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
  EmployeeId: any;
  SBUId: any;
  DepartmentId: any;
  InterviewTime: any;

  user: any = {};
  listUserId: any = [];
  employees: any[] = [];
  listQuestions: any[] = [];
  interviewModel: any = {
    Score: 0,
    Candidate: {},
    Apply: {},
    ListUser: [],
    Questions: [],
    EmployeeId: '',
    SBUId: '',
    DepartmentId: '',
    InterviewId: '',
  };

  listInterviewModel: any = {
    Score: 0,
    Candidate: {},
    Apply: {},
    ListUser: [],
    Questions: [],
    EmployeeId: '',
    SBUId: '',
    DepartmentId: '',
    CandidateApplyId: '',
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

  classifications: any[] = [];

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
  employeeColumnName: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  columnQuestion: any[] = [{ Name: 'QuestionContent', Title: 'Nội dung câu hỏi' }];
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
  interviewId: any = null;
  identifyDate: any = null;
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
  applyDate: any = null;
  candidateId: any = null;
  interviewDate: any = null;
  totalItems: any = null;
  status: any = null;

  selectIndex = -1;
  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('id');
    this.getEmployee();
    this.selectQuestion(0);

    this.appSetting.PageTitle = "Quản lý ứng tuyển";
    this.getInterview();
    //this.user = JSON.parse(localStorage.getItem('qltkcurrentUser'));

    this.getCbbData();
    this.getEmployees();

    // if (!this.EmployeeId){
    //   this.EmployeeId = this.listInterviewModel.EmployeeId;
    //     this.SBUId = this.listInterviewModel.SBUId;
    //     this.DepartmentId = this.listInterviewModel.DepartmentId;
    // }

    //this.getQuestions(this.id);
  }

  getInterview() {
    this.comboboxService.getInterview(this.id).subscribe(
      data => {
        this.listInterviewModel = data;
        if (this.listInterviewModel) {

          this.EmployeeId = this.listInterviewModel.EmployeeId;
          this.SBUId = this.listInterviewModel.SBUId;
          this.DepartmentId = this.listInterviewModel.DepartmentId;
          this.interviewId = this.listInterviewModel.CandidateApplyId;
          this.status = this.listInterviewModel.Status;
          if (this.listInterviewModel.InterviewTime != null) {
            this.InterviewTime = this.dateUtils.convertTimeToObject(this.listInterviewModel.InterviewTime);
          }

          if (this.listInterviewModel.InterviewDate) {
            this.interviewDate = this.dateUtils.convertDateToObject(this.listInterviewModel.InterviewDate);
          }

          this.getInterviewInfo();
          // this.getEmployee();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getEmployee() {
    this.interviewService.getEmployee(this.id).subscribe(
      data => {
        this.interviewModel.ListUser = data;
        this.interviewModel.ListUser.forEach((element, index) => {
          element.Index = index + 1;
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }



  getCbbData() {
    let cbbCountry = this.comboboxService.getCbbCountry();
    let cbbProvinces = this.comboboxService.getCbbProvince();
    let cbbQualifications = this.comboboxService.getCBBQualification();
    let cbbClassIfications = this.comboboxService.getCbbClassIfication();
    let cbbRecruitmentChannels = this.comboboxService.getCbbRecruitmentChannels();
    let cbbSBU = this.comboboxService.getCbbSBU();
    let cbbWorkType = this.comboboxService.getListWorkType();

    //let cbbDepartments = this.comboboxService.getCbbDepartmentBySBU(this.SBUId);
    let cbbDepartments = this.comboboxService.getCbbDepartment();

    forkJoin([cbbProvinces, cbbCountry, cbbQualifications, cbbClassIfications, cbbRecruitmentChannels, cbbSBU, cbbWorkType, cbbDepartments]).subscribe(results => {
      this.provinces = results[0];
      this.countries = results[1];
      this.qualifications = results[2];
      this.classifications = results[3];
      this.channels = results[4];
      this.sbus = results[5];
      this.workTypes = results[6];
      this.departments = results[7];
    });
  }

  getDistricts() {
    this.comboboxService.getCbbDistrict(this.interviewModel.Candidate.ProvinceId).subscribe(
      data => {
        this.districts = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getWards() {
    this.comboboxService.getCbbWard(this.interviewModel.Candidate.DistrictId).subscribe(
      data => {
        this.wards = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getDepartments() {
    this.comboboxService.getCbbDepartmentBySBU(this.user.sbuId).subscribe(
      data => {
        this.departments = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getInterviewInfo() {
    this.interviewService.getIntervewInfo({ ApplyId: this.interviewId, DepartmentId: this.listInterviewModel.departmentId, IdInterview: this.id }).subscribe(
      data => {
        this.interviewModel = data;
        if (this.interviewModel.Candidate.DateOfBirth) {
          this.dateOfBirth = this.dateUtils.convertDateToObject(this.interviewModel.Candidate.DateOfBirth);
        }

        if (this.interviewModel.Candidate.IdentifyDate) {
          this.identifyDate = this.dateUtils.convertDateToObject(this.interviewModel.Candidate.IdentifyDate);
        }

        if (this.interviewModel.Apply.ApplyDate) {
          this.applyDate = this.dateUtils.convertDateToObject(this.interviewModel.Apply.ApplyDate);
        }

        this.totalItems = this.interviewModel.Questions.length;
        this.interviewModel.Candidate.WorkHistories.forEach(element => {
          this.totalYearExperience = this.totalYearExperience + element.TotalTime;
        });

        this.interviewModel.Questions.forEach((element, index) => {
          element.Index = index + 1;
        });
        let score = 0;
        let totalOK = 0;
        let scoresMax = 0;
        this.interviewModel.Questions.forEach(element => {
          if (element.QuestionType == 1 && element.Status == 1) {
            score += parseInt(element.QuestionScore);
            totalOK++;
          }
          else {
            if (element.Score != '') {
              score += parseInt(element.QuestionScore);
            }
          }
        });

        this.interviewModel.Questions.forEach(element => {
          scoresMax += parseInt(element.Score);
        });

        this.interviewModel.Scores = score;
        this.interviewModel.ScoresMax = scoresMax;
        this.getQuestions();
        this.getEmployee();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getQuestions() {
    this.listQuestions = this.interviewModel.Questions;
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  save() {
    this.create();
  }
  saveEmployee() {
    this.updateEmployee()
  }
  saveQuestions() {
    this.updateQuestions()
  }

  updateEmployee() {
    this.interviewService.updateEmployee(this.id, this.interviewModel.ListUser).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật người phỏng vấn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateQuestions() {
    this.interviewService.updateQuestions(this.id, this.interviewModel.Questions).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật câu hỏi thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create() {

    this.interviewModel.EmployeeId = this.EmployeeId;
    this.interviewModel.SBUId = this.SBUId;
    this.interviewModel.DepartmentId = this.DepartmentId;
    this.interviewModel.InterviewId = this.id;
    this.interviewModel.Status = this.status;
    this.interviewService.create(this.interviewModel).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật hồ sơ ứng tuyển thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  selectQuestion(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
    }
    else {
      this.selectIndex = -1
    }
  }

  closeModal() {
    this.router.navigate(['tuyen-dung/phong-van']);
  }

  answerChange() {
    this.interviewModel.Questions[this.selectIndex].Status = this.interviewModel.Questions[this.selectIndex].Answer;
    // > 0 ? this.interviewModel.Questions[this.selectIndex].Answer == this.interviewModel.Questions[this.selectIndex].QuestionAnswer ? 1 : 2 : 0;

    if (this.interviewModel.Questions[this.selectIndex].QuestionType == 1) {
      this.statusChange();
    }
  }

  statusChange() {
    let score = 0;
    let totalOK = 0;
    let scoresMax = 0;
    this.interviewModel.Questions.forEach(element => {
      if (element.QuestionType == 1 && element.Status == 1) {
        score += parseInt(element.QuestionScore);
        totalOK++;
      }
      else {
        if (element.Score != '') {
          score += parseInt(element.QuestionScore);
        }
      }
    });

    this.interviewModel.Questions.forEach(element => {
      scoresMax += parseInt(element.Score);
    });

    this.interviewModel.Scores = score;
    this.interviewModel.ScoresMax = scoresMax;
    this.interviewModel.CorrectAnswer = totalOK;
  }

  showCreateQuestion() {
    let activeModal = this.modalService.open(QuestionInterviewCreateComponent, { container: 'body', windowClass: 'question-interview-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = this.interviewModel.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.interviewModel.Questions.push(result);
      }
    }, (reason) => {
    });
  }

  showComfirmDeleteQuestions(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá câu hỏi phỏng vấn này không?").then(
      data => {
        this.interviewModel.Questions.splice(index, 1);
      },
      error => {

      }
    );
  }

  showSelectEmployee() {
    let activeModal = this.modalService.open(ShowChooseEmployeeComponent, { container: 'body', windowClass: 'select-employee-model', backdrop: 'static' });
    this.interviewModel.ListUser.forEach(element => {
      this.listUserId.push(element.Id);
    });

    activeModal.componentInstance.listEmployeeId = this.listUserId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.interviewModel.ListUser.push(element);
          this.interviewModel.ListUser.forEach((element, index) => {
            element.Index = index + 1;
          });
        });
      }
    }, (reason) => {

    });
  }

  // changeEmployee() {
  //   this.employees.forEach(item => {
  //     if (this.user.ManageId == item.Id) {
  //       this.user.PhoneNumber = item.Exten;
  //     }
  //   });
  // }

  getEmployees() {
    this.comboboxService.getCbbEmployee().subscribe(
      data => {
        this.employees = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showComfirmDeleteEmployee(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người phỏng vấn này không?").then(
      data => {
        this.interviewModel.ListUser.splice(index, 1);
      },
      error => {

      }
    );
  }
}
