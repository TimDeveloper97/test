import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-choose-file-modal',
  templateUrl: './choose-file-modal.component.html',
  styleUrls: ['./choose-file-modal.component.scss']
})
export class ChooseFileModalComponent implements OnInit {
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
  templatePath: string = '';

  ngOnInit() {
    this.getFolderFile();
    this.signalRService.listen(this.onSendListFolder, true);
    this.notiSub = this.onSendListFolder.subscribe((data: any) => {
      if (data) {
        this.dataModel.ListFolder = data;
      }
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
    this.activeModal.close(false);
  }

  choose() {
    this.activeModal.close(this.dataModel.Path);
  }

  dowload() {
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    link.href = this.templatePath;
    link.download = 'Download.zip';
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);
  }

}
