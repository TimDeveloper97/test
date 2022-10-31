import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { ExpertService } from 'src/app/practice/Expert/service/expert.service';
import { ProjectTransferAttachService } from '../../service/project-transfer-attach.service';

@Component({
  selector: 'app-confirm-transfer',
  templateUrl: './confirm-transfer.component.html',
  styleUrls: ['./confirm-transfer.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class ConfirmTransferComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private transferService: ProjectTransferAttachService
  ) { }

  @Input() ProjectId: string;
  checkedTop: boolean = false;
  isAction: boolean = false;
  listData: any = [];

  ngOnInit() {
    this.getListPlanTransferByProjectId();
  }

  getListPlanTransferByProjectId() {
    this.transferService.getListPlanTransferByProjectId(this.ProjectId).subscribe(data => {
      this.listData = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  save() {
    this.transferService.updatePlanStatusByProjectId(this.listTemp).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật dữ liệu thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listData.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }
  listTemp: any[] = [];

  selectFunction(index, event) {
    this.listData.forEach(element => {
      if (event.Id == element.Id) {
        if (event.Checked) {
          element.Checked = true;
          this.listTemp.push(element.Id);
        } else {
          element.Checked = false;
          this.listTemp.splice(index, 1);
        }
      }
    });

  }

}
