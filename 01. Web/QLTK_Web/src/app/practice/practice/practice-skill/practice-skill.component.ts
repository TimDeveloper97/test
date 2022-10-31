import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService, Configuration, Constants, AppSetting, PermissionService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';
import { PracticeSkillChooseComponent } from '../practice-skill-choose/practice-skill-choose.component';
import { SkillService } from '../../skills/service/skill.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-practice-skill',
  templateUrl: './practice-skill.component.html',
  styleUrls: ['./practice-skill.component.scss']
})
export class PracticeSkillComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private planService: PracticeService,
    public constants: Constants,
    public appSetting: AppSetting,
    public permissionService: PermissionService,
    private serviceHistory: HistoryVersionService
  ) { }

  listData: any = [];
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    Quantity: '',
    TotalPrice: '',
    PracticeId: '',
    listSelect: []
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành/công đoạn";
    this.model.PracticeId = this.Id;
    this.getskillInPractice();
  }

  showClick() {
    let activeModal = this.modalService.open(PracticeSkillChooseComponent, { container: 'body', windowClass: 'practice-skill-choose-model', backdrop: 'static' });
    activeModal.componentInstance.PracticeId = this.model.PracticeId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {
    });
  }

  getskillInPractice() {
    if (!this.permissionService.checkPermission(['F040727'])) {
      this.planService.getskillInPractice(this.model).subscribe(data => {
        this.listData = data.ListResult;
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }
  save() {
    this.model.listSelect = this.listData;
    this.model.PracticeId = this.Id;
    this.planService.CreateSkillInPractice(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Lưu thông tin kỹ năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng này không?").then(
      data => {
        this.delete(model);
      },
      error => {
        
      }
    );
  }

  delete(model:any) {
    var index = this.listData.indexOf(model);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  closeModal() {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Practice;
    activeModal.result.then(async (result) => {
      if (result) {
        this.model.CurentVersion = result.Version + 1;
        await this.save();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
