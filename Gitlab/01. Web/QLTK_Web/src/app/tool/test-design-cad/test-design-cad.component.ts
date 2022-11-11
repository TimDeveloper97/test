import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-test-design-cad',
  templateUrl: './test-design-cad.component.html',
  styleUrls: ['./test-design-cad.component.scss']
})
export class TestDesignCadComponent implements OnInit {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @BlockUI() blockUI: NgBlockUI;
  @Input() isDesignCADError: boolean;
  constructor(public constant: Constants,
    private signalRService: SignalRService,
    private messageService: MessageService,
    private config: Configuration,) {
    this.items = [
      { Id: 1, text: 'Xóa file thừa', icon: 'fas fa-times text-danger' }
    ];
  }

  items: any[] = [];
  modelTest: any = {
    ApiUrl: '',
    PathFile: '',
    SelectedPath: '',
    ModuleCode: '',
    List3D: [],
    ListMaterialDB: [],
    ListModuleDesignDocument: [],
    ListRawMaterial: [],
    ListModuleError: [],
    ListConvertUnit: [],
    ListDesignStructure: [],
    ListDesignStructureFile: [],
    Module: []
  }
  private notiSub: Subscription;
  private errorCAD = new BroadcastEventListener<any>('listErrorCAD');
  private downloadAllCAD = new BroadcastEventListener<any>('downloadAllCAD');
  @Input() listErrorCAD: any = [];
  @Input() listErrorCheckCAD: any = [];
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
  }

  listSuccess: any = [];

  ngOnInit() {
    // if (this.listErrorCheckCAD.length == 0) {
    //   // this.messageService.showMessage("Thư mục CAD đã chuẩn!");
    //   this.listSuccess.push("Thư mục CAD đã chuẩn");
    // }
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;

    this.signalRService.listen(this.errorCAD, false);
    this.notiSub = this.errorCAD.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.listErrorCAD = data.Data;
          if (this.listErrorCAD.length == 0) {
            this.messageService.showMessage("Thư mục CAD thiết kế đã chuẩn");
          }
        } else {
          this.messageService.showMessage(data.Message);
        }
      }

      this.blockUI.stop();
    });


    this.signalRService.listen(this.downloadAllCAD, false);
    this.notiSub = this.downloadAllCAD.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Tải dữ liệu thành công!');
        } else {
          this.messageService.showMessage(data.Message);
        }
      }
      this.blockUI.stop();
    });


  }

  checkDesignCAD() {
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('CheckDesignCAD', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  downloadAll() {

    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }

    this.signalRService.invoke('DownLoadAllCAD', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  listFile: any[] = [];
  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.deleteLists();
    }
  }

  deleteLists() {
    this.messageService.showConfirm("Bạn có chắc muốn xoá các file thừa này không?").then(
      data => {
        this.listErrorCAD.forEach(element => {
          if (element.Type == "Thừa file CAD") {
            this.listFile.push(element.PathLocal);
          }
        });
        this.signalRService.invoke('DeleteListFile', this.listFile).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      },
      error => {
        
      }
    );
  }
}
