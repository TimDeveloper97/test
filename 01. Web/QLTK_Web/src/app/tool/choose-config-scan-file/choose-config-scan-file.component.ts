import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants, Configuration } from 'src/app/shared';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { ConfigScanFileService } from '../services/config-scan-file.service';
import { ConfigScanFileComponent } from '../config-scan-file/config-scan-file.component';

@Component({
  selector: 'app-choose-config-scan-file',
  templateUrl: './choose-config-scan-file.component.html',
  styleUrls: ['./choose-config-scan-file.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseConfigScanFileComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  constructor(
    private messageService: MessageService,
    private service: ConfigScanFileService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private config: Configuration,
    private modalService: NgbModal,
    private signalRService: SignalRService
  ) { }

  private onScanFilePDF = new BroadcastEventListener<any>('scanfilePDF');
  private getScanFilePDF: Subscription;
  Id: string;
  moduleCode: string;
  isAction: boolean = false;
  listData: any[] = [];
  model: any = {
    Name: '',
    Code: '',
    Description: '',
    Type: 1
  }

  modelScanfilePDF: any = {
    ApiUrl: '',
    ModuleCode: '',
    FileId: 0,
    ListFileScan: []
  }

  ngOnInit() {
    this.modelScanfilePDF.FileId = this.Id;
    this.modelScanfilePDF.ModuleCode = this.moduleCode;
    this.searchConfigScanFile();
    this.signalRService.listen(this.onScanFilePDF, false);
    this.getScanFilePDF = this.onScanFilePDF.subscribe((data: any) => {
      if (data) {
        this.messageService.showSuccess('Đổi tên file thành công!');
        this.closeModal(true);
      }
      else {
        this.messageService.showSuccess('Đổi tên file thất bại!');
        this.closeModal(true);
      }
      this.blockUI.stop();
    });
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
    this.onScanFilePDF.unsubscribe();
    this.signalRService.stopListening(this.onScanFilePDF);
  }

  searchConfigScanFile() {
    this.service.searchConfigScanFile(this.model).subscribe(data => {
      this.listData = data.ListResult;
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDeleteConfigScanFile(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Template này không?").then(
      data => {
        this.deleteConfigScanFile(Id);
      },
      error => {
        
      }
    );
  }

  deleteConfigScanFile(Id: string) {
    this.service.deleteConfigScanFile({ Id: Id }).subscribe(
      data => {
        this.searchConfigScanFile();
        this.messageService.showSuccess('Xóa Template thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  chooseConfigScanFile() {
    for (var item of this.listData) {
      if (item.Checked == true) {
        this.modelScanfilePDF.ListFileScan.push(item);
      }
    }
    this.modelScanfilePDF.ApiUrl = this.config.ServerApi;

    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelScanfilePDF.Token = currentUser.access_token;
    }
    this.signalRService.invoke('ScanFilePDF', this.modelScanfilePDF).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  clear() {
    this.model = {
      Name: '',
      Code: '',
      Type: 1
    }
    this.searchConfigScanFile();
  }

  showCreateUpdateConfigScanFile(Id: string) {
    let activeModal = this.modalService.open(ConfigScanFileComponent, { container: 'body', windowClass: 'config-scan-file', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchConfigScanFile();
      }
    }, (reason) => {
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
