import { Component, OnInit, Input } from '@angular/core';
import { PracticeExpertsChooseComponent } from '../practice-experts-choose/practice-experts-choose.component';
import { Router } from '@angular/router';
import { MessageService, Configuration, Constants, PermissionService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';
import { PracticeExpertService } from '../service/practice-expert.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-practice-experts',
  templateUrl: './practice-experts.component.html',
  styleUrls: ['./practice-experts.component.scss']
})
export class PracticeExpertsComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private service: PracticeExpertService,
    public constants: Constants,
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
    ExpertId: '',
    PracticeId: '',
    listSelect: []
  }

  ngOnInit() {
    this.model.PracticeId = this.Id;
    this.searchPracticeExpert();
  }

  showClick() {
    let activeModal = this.modalService.open(PracticeExpertsChooseComponent, { container: 'body', windowClass: 'practice-experts-model', backdrop: 'static' });
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

  searchPracticeExpert() {
    if (!this.permissionService.checkPermission(['F040719'])) {
      this.service.searchPracticeExpert(this.model).subscribe(data => {
        this.listData = data.ListResult;
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  save() {
    this.model.listSelect = this.listData;
    this.service.createPracticeExpert(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Lưu thông tin chuyên gia thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chuyên gia này không?").then(
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
