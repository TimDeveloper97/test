import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants } from 'src/app/shared';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-plan-employee',
  templateUrl: './plan-employee.component.html',
  styleUrls: ['./plan-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PlanEmployeeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    private scheduleProjectService: ScheduleProjectService,

  ) { }

  planId: string;
  listIdUserId: any[] = [];
  listEmployee: any[] = [];
  listData: any[] = [];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];

  id: string;
  userId: string;
  isMain: boolean = false;
  isNeedReload: boolean = false;

  model: any = {
    PlanId: '',
    ListPlanAdjustment: []
  }

  modelEmployee: any = {
    UserId: null,
    IsMain: false
  }

  ngOnInit() {
    this.model.PlanId = this.planId;

    this.getListPlanAdjustment();
    this.getListEmployee();
  }

  getListPlanAdjustment() {
    this.scheduleProjectService.getListPlanAdjustment(this.planId).subscribe((data: any) => {
      if (data) {
        this.listData = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListEmployee() {
    this.scheduleProjectService.getListEmployee(this.listIdUserId).subscribe((data: any) => {
      if (data) {
        this.listEmployee = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    this.model.ListPlanAdjustment = this.listData;
    this.scheduleProjectService.updatePlanAssignment(this.model).subscribe(
      data => {
        this.isNeedReload = true;
        this.activeModal.close(this.isNeedReload);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  selectIndex = 1;
  value: string;
  loadValue(param, index) {
    this.selectIndex = index;
    this.value = '';
  }

  addRowParameter() {
    var employee = Object.assign({}, this.modelEmployee);
    employee.UserId = this.userId;
    employee.IsMain = this.isMain;

    if (this.listData.length == 0) {
      employee.IsMain = true;
    }

    this.listData.push(employee);

    //refresh dòng trống
    this.userId = null;
    this.isMain = false
  }

  deleteRow(id, index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá nhân viên này không?").then(
      data => {
        this.listData.splice(index, 1);
        this.model.ListPlanAdjustment = this.listData;
      this.scheduleProjectService.updatePlanAssignment(this.model).subscribe(
        data => {
          this.isNeedReload = true;
        }, error => {
          this.messageService.showError(error);
        }
    );
      },
      error => {

      }
    );
  }

  closeModal() {
    this.activeModal.close(this.isNeedReload);
  }

  check(row: any, index: number) {
    if (row.IsMain) {
      for (let a = 0; a < this.listData.length; a++) {
        if (index != a) {
          this.listData[a].IsMain = false;
        }
      }
    }
  }
}
