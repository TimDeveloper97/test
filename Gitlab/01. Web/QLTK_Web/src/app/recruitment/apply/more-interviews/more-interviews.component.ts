import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService, DateUtils, Constants } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { forkJoin } from 'rxjs';
import { ApplyService } from '../../services/apply.service';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';
import { ChooseQuestionComponent } from '../../../employee/worktype-interview/choose-question/choose-question.component';
import { QuestionMoreInterviewsCreateComponent } from '../question-more-interviews-create/question-more-interviews-create.component';

@Component({
  selector: 'app-more-interviews',
  templateUrl: './more-interviews.component.html',
  styleUrls: ['./more-interviews.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MoreInterviewsComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private applyService: ApplyService,
    public dateUtils: DateUtils,
    public constant: Constants,
    private modalService: NgbModal,

  ) { }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  employeeColumnName: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  listSBU: any[] = [];
  departments: any[] = [];
  ModalInfo = {//m
    Title: 'Thêm mới phỏng vấn',
    SaveText: 'Lưu',
  };
  parentId: string;
  listMoreInterviews: any[] = [];
  listMoreInterviewsId: any[] = [];
  employees: any[] = [];
  listUserId: any = [];
  isAction: boolean = false;
  Id: string;
  interviewDate: any = null;
  EmployeeId : any;
  ApplyId : string;


  model: any = {
    Id: '',
    CandidateApplyId: '',
    Status: 0,
    EmployeeId: '',
    SBUId: '',
    DepartmentId: '',
    InterviewDate: '',
    InterviewTime: '',
    InterviewBy: '',
    ListUser: [],
    Questions: [],
  }
  ngOnInit() {
    this.model.CandidateApplyId = this.ApplyId;
    this.getEmployees();
    this.getCbbSBU();
    this.getCbbDepartment();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa phỏng vấn';
      this.ModalInfo.SaveText = 'Lưu';
      this.getMoreInterviewsInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới phỏng vấn";
    }

  }


  getSBU(){
    this.comboboxService.getSBU(this.EmployeeId).subscribe(
      data => {
        this.model.SBUId = data.SBUID;
        this.model.DepartmentId = data.DepartmentId;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getMoreInterviewsInfo() {
    this.applyService.getInforMoreInterviews(this.Id).subscribe(data => {
      this.model = data;
      this.model.ListUser = data.ListUser;
      var j = 1;
      this.model.ListUser.forEach(element => {
        element.index = j++;
      });
      this.model.Questions = data.Questions;
      this.EmployeeId = this.model.InterviewBy;
      if (this.model.InterviewDate) {
        this.interviewDate = this.dateUtils.convertDateToObject(this.model.InterviewDate);
      }
      if (this.model.InterviewTime){
        this.model.InterviewTime = this.dateUtils.convertTimeToObject(this.model.InterviewTime);
      }
    });
  }

  createMoreInterviews(isContinue) {
    this.applyService.createMoreInterviews(this.model).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới phỏng vấn thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới phỏng vấn thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }
  clear() {
    this.model = {
      Id: '',
      CandidateApplyId: this.ApplyId,
      Status: 0,
      EmployeeId: '',
      SBUId: '',
      DepartmentId: '',
      InterviewDate: null,
      InterviewBy: '',
      ListUser: [],
      Questions: [],
    };
    this.EmployeeId = '';
    this.interviewDate = '';

  }

  updateMoreInterviews() {
    this.model.InterviewBy =this.EmployeeId;
    this.applyService.updateMoreInterviews(this.Id, this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật phỏng vấn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {

    this.model.InterviewBy =this.EmployeeId;
    this.model.InterviewDate = null;
    if (this.interviewDate) {
      this.model.InterviewDate = this.dateUtils.convertObjectToDate(this.interviewDate);
    }
    if (this.model.InterviewTime){
      this.model.InterviewTime = this.dateUtils.convertObjectToTime(this.model.InterviewTime)
    }
    if (this.Id) {
      this.updateMoreInterviews();
    }
    else {
      this.createMoreInterviews(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

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

  getCbbSBU() {
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getCbbDepartment() {
    this.comboboxService.getCbbDepartment().subscribe(
      data => {
        this.departments = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showSelectEmployee() {
    let activeModal = this.modalService.open(ShowChooseEmployeeComponent, { container: 'body', windowClass: 'select-employee-model', backdrop: 'static' });
    this.model.ListUser.forEach(element => {
      this.listUserId.push(element.Id);
    });

    activeModal.componentInstance.listEmployeeId = this.listUserId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsNew = true;
          this.model.ListUser.push(element);
        });
        var j = 1;
        this.model.ListUser.forEach(element => {
          element.index = j++;
        });
      }
    }, (reason) => {

    });
  }

  showComfirmDeleteEmployee(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người phỏng vấn này không?").then(
      data => {
        this.model.ListUser.splice(index, 1);
      },
      error => {

      }
    );
  }

  chooseQuestion() {
    let activeModal = this.modalService.open(ChooseQuestionComponent, { container: 'body', windowClass: 'choose-question-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.model.Questions.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.model.Questions.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteQuestion(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa câu hỏi này không?").then(
      data => {
        if (this.model.Questions.length > 0) {
          this.model.Questions.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  showCreateQuestion() {
    let activeModal = this.modalService.open(QuestionMoreInterviewsCreateComponent, { container: 'body', windowClass: 'question-more-interview-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = this.model.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.model.Questions.push(result);
      }
    }, (reason) => {
    });
  }

}
