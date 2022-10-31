import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbTimeAdapter, NgbTimepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService, DateUtils, AppSetting, Constants } from 'src/app/shared';
import * as moment from 'moment';
import { WorkDiaryService } from '../../service/work-diary.service';
import { NtsTimeStringAdapter } from 'src/app/shared/common/nts-time-string-adapter';
import { Router, ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-work-diary-create',
  templateUrl: './work-diary-create.component.html',
  styleUrls: ['./work-diary-create.component.scss'],
  providers: [{ provide: NgbTimeAdapter, useClass: NtsTimeStringAdapter }],
  encapsulation: ViewEncapsulation.None
})
export class WorkDiaryCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private workDiaryService: WorkDiaryService,
    private router: Router,
    private routeA: ActivatedRoute,
    private appSetting: AppSetting,
    public contant: Constants,
    private ngbTimepickerConfig: NgbTimepickerConfig
  ) {
    ngbTimepickerConfig.spinners = false;
  }

  modalInfo = {
    Title: 'Thêm mới nhật kí công việc',
    SaveText: 'Lưu',
  };
  columnName: any[] = [{ Name: 'Name', Title: 'Tên nhật kí công việc' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnModule: any[] = [{ Name: 'Code', Title: 'Mã module' }, { Name: 'Name', Title: 'Tên module' }];
  listEmployee: any[] = [];
  listProject: any[] = [];
  listModule: any[] = [];
  isAction: boolean = false;
  codeEmployee: string;
  nameEmployee: string;
  nameDepartment: string;
  nameSBU: string;
  codeProject: string;
  nameProject: string;
  startWorking: any;
  Id: string;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    ObjectId: null,
    ProjectId: null,
    EmployeeId: null,
    WorkDate: null,
    TotalTime: '',
    Done: '',
    Address: 'TPA',
    SBUId: '',
    DepartmentId: '',
    ObjectType: 0,
    CreateBy: '',
    StartTime: null,
    EndTime: null
  }
  project: any = { Id: '-', Name: 'Không thuộc dự án', Code: 'khac' };

  ngOnInit() {
    this.model.Id = this.Id;
    forkJoin([
      this.workDiaryService.GetCbbEmployeeByUser(),
      this.workDiaryService.GetCbbprojectByUser(),
      this.comboboxService.getListModule()
    ]).subscribe(([res1, res2, res3]) => {
      this.listEmployee = res1.ListResult;
      this.listProject = res2.ListResult;
      this.listProject.push(this.project);
      this.listModule = res3;
      if (this.model.Id) {
        this.getWorkDiary();
      }
      else {
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser) {
          this.model.DepartmentId = currentUser.departmentId;
          this.model.SBUId = currentUser.sbuId;

          this.model.EmployeeName = currentUser.employeeName;
          this.model.EmployeeCode = currentUser.employeeCode;
          this.model.DepartmentName = currentUser.departmentName;
          this.model.SBUname = currentUser.sbuName;
        }
      }
    });
    if (this.model.Id) {
      this.modalInfo.Title = 'Chỉnh sửa nhật kí công việc';
      this.modalInfo.SaveText = 'Lưu';
    }
    else {
      this.modalInfo.Title = 'Thêm mới nhật kí công việc';
    }

  }

  getWorkDiary() {
    this.workDiaryService.getByIdWorkDiary(this.model).subscribe(data => {
      this.model = data;
      if (data.WorkDate != null && data.WorkDate != '') {
        this.startWorking = this.dateUtils.convertDateToObject(data.WorkDate);
      }
    });
  }

  createWorkDiary(isContinue) {

    if (this.startWorking != null) {
      this.model.WorkDate = this.dateUtils.convertObjectToDate(this.startWorking);
    }
    else {
      this.model.WorkDate = null;
    }
    this.workDiaryService.addWorkDiary(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            ObjectId: this.model.ObjectId,
            ProjectId: this.model.ProjectId,
            WorkDate: this.model.WorkDate,
            TotalTime: '',
            Done: '',
            Address: 'TPA',
            SBUId: this.model.SBUId,
            DepartmentId: this.model.DepartmentId,
            ObjectType: 0,
            CreateBy: this.model.CreateBy
          };
          this.messageService.showSuccess('Thêm mới thời gian làm việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới thời gian làm việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateWorkDiary() {
    if (this.startWorking != null) {
      this.model.WorkDate = this.dateUtils.convertObjectToDate(this.startWorking);
    }
    else {
      this.model.WorkDate = null;
    }

    this.workDiaryService.updateWorkDiary(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật thời gian làm việc thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.model.Id) {
      this.updateWorkDiary();
    }
    else {
      this.createWorkDiary(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
