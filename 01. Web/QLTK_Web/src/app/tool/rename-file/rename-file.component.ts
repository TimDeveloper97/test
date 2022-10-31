import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { NgBlockUI, BlockUI } from 'ng-block-ui';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { SignalRService } from 'src/app/signalR/signal-r.service';

@Component({
  selector: 'app-rename-file',
  templateUrl: './rename-file.component.html',
  styleUrls: ['./rename-file.component.scss']
})
export class RenameFileComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    private activeModal: NgbActiveModal,
    private signalRService: SignalRService,
    private messageService: MessageService
  ) { }

  name: string;
  path: string;
  pathForder: string;
  private onMove = new BroadcastEventListener<any>('getMoveFile');
  private getMove: Subscription;

  model: any = {
    Name: '',
    Path: '',
    PathForder: ''
  }

  ngOnInit() {
    this.signalRService.listen(this.onMove, false);
    this.getMove = this.onMove.subscribe((data: any) => {
      if (data) {
        this.messageService.showSuccess('Di chuyển file thành công!'); 
      }
      else {
        this.messageService.showWarning('Di chuyển file thất bại!');
      }
      this.blockUI.stop();
      this.closeModal();
    });
  }

  move() {
    if (!this.pathForder) {
      this.messageService.showMessage("Bạn chưa chọn thư mục di chuyển đến");
      return;
    }
    this.model.Name = this.name;
    this.model.Path = this.path;
    this.model.PathForder = this.pathForder;
    this.signalRService.invoke('MoveFile', this.model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  closeModal() {
    this.activeModal.close(true);
  }
}
