import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SkillGroupService } from '../service/skill--group.service';
import { SkillGroupCreateComponent } from '../skill-group-create/skill-group-create.component';

@Component({
  selector: 'app-skill-group-manage',
  templateUrl: './skill-group-manage.component.html',
  styleUrls: ['./skill-group-manage.component.scss']
})
export class SkillGroupManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private skillGroupService: SkillGroupService,
    public constant: Constants
  ) { }

  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm kỹ năng";
    this.searchSkillGroup();
  }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    //Note: '',
  }

  searchSkillGroup() {
    this.skillGroupService.searchSkillGroup(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      Note: '',
    }
    this.searchSkillGroup();
  }
  
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SkillGroupCreateComponent, { container: 'body', windowClass: 'SkillGroup-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSkillGroup();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteSkillGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm kỹ năng này không?").then(
      data => {
        this.deleteSkillGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteSkillGroup(Id: string) {
    this.skillGroupService.deleteSkillGroup({ Id: Id }).subscribe(
      data => {
        this.searchSkillGroup();
        this.messageService.showSuccess('Xóa nhóm kỹ năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
