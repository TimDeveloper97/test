import { Component, OnInit, Input } from '@angular/core';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { NgBlockUI, BlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-test-folder-igs',
  templateUrl: './test-folder-igs.component.html',
  styleUrls: ['./test-folder-igs.component.scss']
})
export class TestFolderIgsComponent implements OnInit {

  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @BlockUI() blockUI: NgBlockUI;

  constructor(
    public constant: Constants,
    private signalRService: SignalRService,
    private messageService: MessageService,
    private config: Configuration,
  ) { }

  private notiSub: Subscription;
  private checkFolderIGS = new BroadcastEventListener<any>('listErrorIGS');

  modelTest: any = {
    ApiUrl: '',
    PathFile: '',
    SelectedPath: '',
    ModuleCode: '',
  }
  @Input() listFileIGS: any = [];
  ngOnInit() {
  }

  checkIGS() {
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('CheckIGS', this.modelTest).subscribe(data => {
      if (data) {
        // this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });  
  }


}
