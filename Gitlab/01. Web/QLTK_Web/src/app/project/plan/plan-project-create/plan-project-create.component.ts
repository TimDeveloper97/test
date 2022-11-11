import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService, DateUtils, Constants } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { PlanService } from '../../service/plan.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-plan-project-create',
  templateUrl: './plan-project-create.component.html',
  styleUrls: ['./plan-project-create.component.scss']
})
export class PlanProjectCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private planService: PlanService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private comboboxService: ComboboxService,
    private dateUtils: DateUtils,
    private scheduleProjectService: ScheduleProjectService,
    public constants: Constants,
  ) { }

  modalInfo: any = {
    title: 'Thêm mới công việc',
    saveText: 'Lưu',
  };

  Id: string;
  projectId: string;
  stageId: string;
  projectProductId: string;
  parentId: string;

  Types: number;
  StartIndex = 0;
  status = 0;

  model: any = {
    Id: '',
    ProjectId: '',
    ProjectProductId: '',
    StageId: '',
    Name: '',
    Description: '',
    Index: null
  }

  listTask: any = [];

  listDepartment: any = [];
  listOrder
  departmentId: string = '';

  type: number;
  date = new Date();

  tasks: any = [];
  columnNameDepartment: any[] = [{ Name: 'Code', Title: 'Mã Phòng ban' }, { Name: 'Name', Title: 'Tên Phòng ban' }];


  ngOnInit() {
    this.getListOder();

    this.model = {
      Id: this.Id,
      ProjectId: this.projectId,
      ProjectProductId: this.projectProductId,
      StageId: this.stageId,
      ParentId: this.parentId,
      Name: '',
      Description: '',
      IsScheduleProject: true
    }

    if (this.type == 1) {
      this.modalInfo.title = 'Thêm mới công việc';
    } else {
      this.modalInfo.title = 'Chỉnh sửa công việc';
      this.model.Types = 1;
      this.getPlanInfo();
    }
  }

  getPlanInfo() {
    this.planService.getPlanInfo(this.model).subscribe((data: any) => {
      if (data) {
        this.model = data;
        this.model.IsScheduleProject = true;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListTask() {
    this.listTask = [];
    if(this.departmentId !==''){
      this.tasks.forEach((element: any) => {
        if (element.ObjectId == this.departmentId) {
          this.listTask.push(element);
        }
      });
    }    
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  save() {
    if (this.Id && this.type == 2) {
      this.update();
    } else {
      this.create();
    }
  }

  create() {
    this.scheduleProjectService.createPlan(this.model).subscribe(
      (data) => {
        this.activeModal.close(data);
        this.messageService.showSuccess('Thêm mới kế hoạch thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.scheduleProjectService.updatePlan(this.model).subscribe(
      (data) => {
        this.activeModal.close(data);
        this.messageService.showSuccess('Cập nhật kế hoạch thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListOder() {
    this.scheduleProjectService.getListOrder(this.projectProductId, this.stageId, this.type).subscribe(
      (data) => {
        this.listOrder = data;

        if (this.type == 1 && this.listOrder.length > 0) {
          this.model.Index = this.listOrder[this.listOrder.length - 1];
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
