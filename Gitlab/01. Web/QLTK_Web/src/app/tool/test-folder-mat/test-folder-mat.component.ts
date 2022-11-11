import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';

@Component({
  selector: 'app-test-folder-mat',
  templateUrl: './test-folder-mat.component.html',
  styleUrls: ['./test-folder-mat.component.scss']
})
export class TestFolderMatComponent implements OnInit {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @BlockUI() blockUI: NgBlockUI;

  constructor(public constant: Constants,
    private signalRService: SignalRService,
    private messageService: MessageService,
    private config: Configuration, ) { }

  private notiSub: Subscription;
  private checkFolderMAT = new BroadcastEventListener<any>('listErrorMAT');

  modelTest: any = {
    ApiUrl: '',
    PathFile: '',
    SelectedPath: '',
    ModuleCode: '',
  }

  @Input() listFileMAT: any = [];
  ngOnInit() {
    // this.signalRService.listen(this.checkFolderMAT, false);
    // this.notiSub = this.checkFolderMAT.subscribe((data: any) => {
    //   if (data) {
    //     this.blockUI.stop();
    //     this.listFileMAT = data.Data;
    //   }
    // });
  }

  checkMAT() {
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('CheckMAT', this.modelTest).subscribe(data => {
      if (data) {
        // this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });  
  }
}
