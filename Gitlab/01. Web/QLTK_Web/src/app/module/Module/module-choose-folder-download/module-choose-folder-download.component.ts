import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ChooseFolderModalComponent } from '../../choose-folder-modal/choose-folder-modal.component';
import { MessageService } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DowloadFileModuleService } from '../../services/dowload-file-module.service';

@Component({
  selector: 'app-module-choose-folder-download',
  templateUrl: './module-choose-folder-download.component.html',
  styleUrls: ['./module-choose-folder-download.component.scss']
})
export class ModuleChooseFolderDownloadComponent implements OnInit, OnDestroy {
  private onDownload = new BroadcastEventListener<any>('download');
  private downloadSub: Subscription;
  @BlockUI() blockUI: NgBlockUI;
  FolderId: string;
  ModuleId: string;
  listData: any[] = [];
  listModuleDesignDocument: any[] = [];
  listModuleMaterial: any[] = [];
  id: string;
  downloadModel = {
    Id: '',
    Type: 1,
    ModuleId: '',
    Path: ''
  }

  model: any = {
    ModuleId: '',
    Path: '',
    Type: 1,
    ListModuleDesignDocument: [],
    ListStrucFile: [],
    ListModuleMaterial: []
  }

  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private service: DowloadFileModuleService,
    private messageService: MessageService,
    private signalRService: SignalRService,
  ) { }

  ngOnInit() {
    this.downloadModel.ModuleId = this.ModuleId;
    this.getlistModuleDesignDocument();
    //this.getListFile();
    this.getListModuleMaterial();
    this.signalRService.listen(this.onDownload, false);
    this.downloadSub = this.onDownload.subscribe((data: any) => {
      if (data) {
        if (data.length > 0) {
          this.messageService.showMessage("Không có file trên nguồn");
        } else {
          this.messageService.showSuccess('Download file thành công!');
        }
      }
      else {
        this.messageService.showSuccess('Download file không thành công!');    
      }
      this.blockUI.stop();
      this.closeModal();
    });
  }

  ngOnDestroy(){
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownload);
  }

  showChooseFolderWindow() {
    let activeModal = this.modalService.open(ChooseFolderModalComponent, { container: 'body', windowClass: 'choose-folder-modal', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
        this.downloadModel.Path = result;
      }
    }, (reason) => {

    });
  }

  getlistModuleDesignDocument() {
    this.service.GetListModuleDesignDocument().subscribe((data: any) => {
      if (data) {
        this.listModuleDesignDocument = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListFile() {
    this.service.ListFileDowload(this.downloadModel).subscribe((data: any) => {
      if (data) {
        this.listData = data;
        this.download();
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListModuleMaterial() {
    this.service.GetListModuleMaterial(this.downloadModel).subscribe((data: any) => {
      if (data) {
        this.listModuleMaterial = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  download() {
    this.model.ModuleId = this.ModuleId;
    this.model.Path = this.downloadModel.Path;
    this.model.Type = this.downloadModel.Type;
    this.model.ListModuleDesignDocument = this.listModuleDesignDocument;
    this.model.ListStrucFile = this.listData;
    this.model.ListModuleMaterial = this.listModuleMaterial;
    this.signalRService.invoke('DownloadFolderModuleDesignDocument', this.model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  downloadFile() {
    this.getListFile();
    //this.download();
  }

  closeModal() {
    this.activeModal.close();
  }
}
