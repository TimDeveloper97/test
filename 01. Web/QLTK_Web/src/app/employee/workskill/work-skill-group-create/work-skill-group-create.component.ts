import { Component, OnInit } from '@angular/core';
import { MessageService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { WorkSkillService } from '../../service/work-skill.service';

@Component({
  selector: 'app-work-skill-group-create',
  templateUrl: './work-skill-group-create.component.html',
  styleUrls: ['./work-skill-group-create.component.scss']
})
export class WorkSkillGroupCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private workSkillService: WorkSkillService, ) { }

  Id: string;
  ParentId: string;
  ModalInfo = {
    Title: 'Thêm mới nhóm kỹ năng/kiến thức',
    SaveText: 'Lưu',

  };
  isAction: boolean = false;
  WorkSkillGroupModel: any = {
    Code: '',
    Name: '',
    ParentId: null,
    Id: '',
  }

  ListWorkSkillGroup: any = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  ngOnInit() {
    this.getCBBWorkSkillGroup();
    if (this.Id) {
      this.ModalInfo.Title = "Chỉnh sửa nhóm kỹ năng/kiến thức";
      this.WorkSkillGroupModel.Id = this.Id;
      this.getWorkSkillGroupInfo();
    }
    if (this.ParentId) {
      this.WorkSkillGroupModel.ParentId = this.ParentId;
    }
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

  getWorkSkillGroupInfo() {
    this.workSkillService.getInforWorkSkillGroup({ Id: this.Id }).subscribe(data => {
      this.WorkSkillGroupModel = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  updateWorkSkillGroup() {
    this.workSkillService.updateWorkSkillGroup(this.WorkSkillGroupModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm kỹ năng/kiến thức thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createWorkSkillGroup(isContinue, parentId) {
    this.workSkillService.createWorkSkillGroup(this.WorkSkillGroupModel).subscribe(
      () => {
        if (isContinue) {
          this.isAction = true;
          this.WorkSkillGroupModel = {};
          this.messageService.showSuccess("Thêm mới nhóm kỹ năng/kiến thức thành công!");
        } else {
          this.activeModal.close(true);
          this.messageService.showSuccess("Thêm mới nhóm kỹ năng/kiến thức thành công!");
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateWorkSkillGroup();
    }
    else {
      let parentId = this.WorkSkillGroupModel.ParentId;
      this.createWorkSkillGroup(isContinue, parentId);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
