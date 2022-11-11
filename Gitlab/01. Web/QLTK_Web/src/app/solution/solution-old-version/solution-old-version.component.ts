import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { SolutionService } from '../service/solution.service';
import { Router } from '@angular/router';
import { AppSetting, Constants, MessageService, Configuration, ComponentService } from 'src/app/shared';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { SignalRService } from 'src/app/signalR/signal-r.service';

@Component({
  selector: 'app-solution-old-version',
  templateUrl: './solution-old-version.component.html',
  styleUrls: ['./solution-old-version.component.scss']
})
export class SolutionOldVersionComponent implements OnInit, OnDestroy {
  private onDownloadFolder = new BroadcastEventListener<any>('downloadFolderSolutionDesignDocument');
  private downloadFolderSub: Subscription;
  @BlockUI() blockUI: NgBlockUI;
  @Input() Id: string;
  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private service: SolutionService,
    public constants: Constants,
    public messageService: MessageService,
    private signalRService: SignalRService,
    private config: Configuration,
    private componentService: ComponentService
  ) { }

  ngOnInit() {
    this.getSolutionOldVersion();
    this.signalRService.listen(this.onDownloadFolder, true);
    this.downloadFolderSub = this.onDownloadFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 2) {
          this.messageService.showMessage(data.Message);
        }
        else {
          this.messageService.showMessage('Download thành công!');
        }
      }
    });
  }

  ngOnDestroy() {
    this.downloadFolderSub.unsubscribe();
    this.signalRService.stopListening(this.onDownloadFolder);
  }

  listHistory: any[] = [];

  getSolutionOldVersion() {
    this.service.getSolutionOldVersion(this.Id).subscribe(data => {
      this.listHistory = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  downloadFolder(folderId: string, designTypes: number) {
    this.componentService.showChooseFolder(3, designTypes, this.Id).subscribe(result => {
      if (result) {
        var file = {
          Id: folderId,
          DownloadPath: result,
          ObjectId: this.Id,
          Token: '',
          ApiUrl: this.config.ServerApi
        };
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser && currentUser.access_token) {
          file.Token = currentUser.access_token;
        }

        this.signalRService.invoke('DownloadFolderSolutionDesignDocument', file).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }
    });
  }
}
