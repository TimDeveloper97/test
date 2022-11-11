import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MessageService, Constants, Configuration } from 'src/app/shared';

@Component({
  selector: 'app-choose-folder-file',
  templateUrl: './choose-folder-file.component.html',
  styleUrls: ['./choose-folder-file.component.scss']
})
export class ChooseFolderFileComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  constructor(private signalRService: SignalRService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService) { }
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolderFile');
  private notiSub: Subscription;

  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  dataModel = {
    Id: '',
    Path: '',
    ListFolder: []
  }
  ngOnInit() {
    this.getFolderFile();
    this.signalRService.listen(this.onSendListFolder, false);
    this.notiSub = this.onSendListFolder.subscribe((data: any) => {
      this.dataModel.ListFolder = data;
      this.blockUI.stop();
    });
  }

  getFolderFile() {
    this.signalRService.invoke('GetFolderFile', this.dataModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  onSelectionChanged(e) {
    this.selectedId = e.selectedRowKeys[0];
    this.dataModel.Path = e.selectedRowKeys[0];
  }

  onRowExpanding(e) {
    this.dataModel.Id = e.key;
    this.signalRService.invoke('GetFolderFile', this.dataModel).subscribe(data => {
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
