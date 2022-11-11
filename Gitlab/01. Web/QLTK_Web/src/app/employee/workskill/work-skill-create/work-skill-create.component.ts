import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { WorkSkillService } from '../../service/work-skill.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-work-skill-create',
  templateUrl: './work-skill-create.component.html',
  styleUrls: ['./work-skill-create.component.scss']
})
export class WorkSkillCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private workSkillService: WorkSkillService,
    private comboboxService: ComboboxService,

  ) { }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  ModalInfo = {//m
    Title: 'Thêm mới kỹ năng/kiến thức',
    SaveText: 'Lưu',
  };
  parentId: string;
  listWorkSkill: any[] = [];
  listWorkSkillId: any[] = [];
  isAction: boolean = false;
  Id: string;
  WorkSkillGroupId: string;

  model: any = {
    Id: '',
    Name: '',
    Description: '',
    WorkSkillGroupId: ''
  }
  ListWorkSkillGroup: any = [];
  ngOnInit() {
    this.getCBBWorkSkillGroup();
    this.getCBBWorkSkillGroup();
      if (this.Id) {
        this.ModalInfo.Title = 'Chỉnh sửa kỹ năng/kiến thức';
        this.ModalInfo.SaveText = 'Lưu';
        this.getWorkSkillInfo();
      }
      else {
        this.ModalInfo.Title = "Thêm mới kỹ năng/kiến thức";
        this.model.WorkSkillGroupId=this.WorkSkillGroupId;
      }

  }

  getCbbWorkSkill() {
    this.comboboxService.getListWorkSkill().subscribe((data: any) => {
      this.listWorkSkill = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listWorkSkill) {
        this.listWorkSkillId.push(item.Id);
        if (this.parentId == item.Id) {
          this.model.Name = item.Name;
        }
      }
    });
  }

  getCBBWorkSkillGroup() {
    this.workSkillService.searchWorkSkillGroup({}).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListWorkSkillGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getWorkSkillInfo() {
    this.workSkillService.getInforWorkSkill({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createWorkSkill(isContinue, workSkillGroupId) {
    this.workSkillService.createWorkSkill(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Description: '',
          };
          this.model.WorkSkillGroupId = workSkillGroupId;
          this.messageService.showSuccess('Thêm mới kỹ năng/kiến thức thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới kỹ năng/kiến thức thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateWorkSkill() {
    this.workSkillService.updateWorkSkill(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật kỹ năng/kiến thức thành công!');
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
    if (this.Id) {
      this.updateWorkSkill();
    }
    else {
      let workSkillGroupId = this.model.WorkSkillGroupId;
      this.createWorkSkill(isContinue, workSkillGroupId);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

}
