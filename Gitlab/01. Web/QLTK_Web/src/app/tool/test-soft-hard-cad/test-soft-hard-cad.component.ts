import { Component, OnInit, Input, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { SignalRService } from 'src/app/signalR/signal-r.service';

@Component({
  selector: 'app-test-soft-hard-cad',
  templateUrl: './test-soft-hard-cad.component.html',
  styleUrls: ['./test-soft-hard-cad.component.scss']
})
export class TestSoftHardCadComponent implements OnInit, OnDestroy {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @Input() cadResultModel: any;
  @BlockUI() blockUI: NgBlockUI;
  constructor(private config: Configuration,
    private signalRService: SignalRService,
    public constant: Constants,
    private messageService: MessageService,) {
    this.items = [
      { Id: 1, text: 'Xóa file', icon: 'fas fa-times text-danger' }
    ];
  }

  items: any;
  modelTest: any = {
    ApiUrl: '',
    PathFile: '',
    SelectedPath: '',
    ModuleCode: '',
  }

  @Input() listHardCAD: any = [];
  @Input() listSoftCAD: any = [];
  @Input() isSuccess: boolean;

  private notiSub: Subscription;
  private checkSoftHard = new BroadcastEventListener<any>('checkSoftHardCAD');
  private download = new BroadcastEventListener<any>('downloadCAD');
  private deleteList: Subscription;
  private onDeleteList = new BroadcastEventListener<any>('getDeleteList');
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  @ViewChild('scrollHeaderTwo',{static:false}) scrollHeaderTwo: ElementRef;

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
    //this.deleteList.unsubscribe();
    //this.signalRService.stopListening(this.onDeleteList);
  }

  ngOnInit() {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderTwo.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;
    // this.signalRService.listen(this.checkSoftHard);
    // this.signalRService.listen(this.download);


    this.signalRService.listen(this.checkSoftHard, false);
    this.notiSub = this.checkSoftHard.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Tải dữ liệu thành công!');
          this.listHardCAD = data.Data.ListHardCAD;
          this.listSoftCAD = data.Data.ListSoftCAD;
          this.isSuccess = data.Data.IsSuccess;
          if (this.isSuccess) {
            this.messageService.showMessage("Đã chuẩn!");
          } else {
            this.messageService.showMessage("Chưa chuẩn!");
          }
        } else {
          this.messageService.showMessage(data.Message);
        }

      }
      this.blockUI.stop();
    });

    this.signalRService.listen(this.download, false);
    this.notiSub = this.download.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Tải dữ liệu thành công!');
        } else {
          this.messageService.showMessage(data.Message);
        }
      }
      this.blockUI.stop();
    });

    // Delete danh sách file
    // this.signalRService.listen(this.onDeleteList, true);
    // this.deleteList = this.onDeleteList.subscribe((data: any) => {
    //   this.blockUI.stop();
    //   if (data) {
    //     //this.check();
    //     this.messageService.showSuccess('Xóa file thành công!');
    //   }
    // });
  }

  checkSoftHardCAD() {
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('CheckSoftHardCAD', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  downloadAllCADHard() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('DownloadHardCAD', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  itemClickSoftCAD(e) {
    if (e.itemData.Id == 1) {
      this.deleteLists(this.listSoftCAD);
    }
  }

  itemClickHardCAD(e) {
    if (e.itemData.Id == 1) {
      this.deleteLists(this.listHardCAD);
    }
  }

  ListPath: any[] = [];
  listItem: any[] = [];
  deleteLists(list) {
    this.ListPath = [];
    for (var item of list) {
      if (!item.IsExist) {
        this.ListPath.push(item.FilePath);
        this.listItem.push(item);
      }
    }
    if (this.ListPath.length > 0) {
      this.signalRService.invoke('DeleteListFile', this.ListPath).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
  }
}
