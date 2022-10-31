import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgBlockUI, BlockUI } from 'ng-block-ui';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-choose-folder-modal',
  templateUrl: './choose-folder-modal.component.html',
  styleUrls: ['./choose-folder-modal.component.scss']
})
export class ChooseFolderModalComponent implements OnInit, OnDestroy {
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private notiSub!: Subscription;

  @BlockUI() blockUI!: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView!: DxTreeListComponent;

  designType: number = 0;
  moduleCode: string = '';
  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  checkSelect: boolean = false;
  dataModel = {
    Id: '',
    Path: '',
    SelectPath: '',
    ListFolder: []
  }

  constructor(
    private config: Configuration,
    private signalRService: SignalRService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    if (this.moduleCode) {
      this.getLink();
    } else {
      this.getFolder();
    }
    this.signalRService.listen(this.onSendListFolder, false);
    this.notiSub = this.onSendListFolder.subscribe((data: any) => {
      this.blockUI.stop();
      this.dataModel.ListFolder = data.ListForder;
      if (this.checkSelect) {
        this.dataModel.SelectPath = data.Path;
        this.selectPathForder();
      }
    });
  }

  ngOnDestroy() {
    this.notiSub.unsubscribe();
    this.signalRService.stopListening(this.onSendListFolder);
  }

  getFolder() {
    this.checkSelect = false;
    this.signalRService.invoke('GetFolder', this.dataModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  getLink() {
    var model = {
      ModuleCode: this.moduleCode,
      Token: '',
      Type: this.designType,
      ApiUrl: this.config.ServerApi,
    };
    this.checkSelect = true;
    let userStore = localStorage.getItem('qltkcurrentUser');
    let currentUser: any = null;
    if (userStore) {
      currentUser = JSON.parse(userStore);
    }
    if (currentUser && currentUser.access_token) {
      model.Token = currentUser.access_token;
    }
    this.signalRService.invoke('GetSelectFolder', model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  selectPathForder() {
    var data: any[] = this.dataModel.ListFolder.filter((i: any) => i.Id == this.dataModel.SelectPath);
    if (data.length > 0) {
      this.selectedId = data[0].Id;
      this.dataModel.Path = data[0].Id;
      this.treeView.selectedRowKeys = [this.selectedId];
    }
  }

  onSelectionChanged(e: any) {
    this.selectedId = e.selectedRowKeys[0];
    this.dataModel.Path = e.selectedRowKeys[0];
  }

  onRowExpanding(e: any) {
    this.dataModel.Id = e.key;
    this.signalRService.invoke('GetFolder', this.dataModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  choose() {
    this.activeModal.close(this.dataModel.Path);
  }
}
