import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { TestDesignService } from '../services/test-design-service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-test-standard-supplies',
  templateUrl: './test-standard-supplies.component.html',
  styleUrls: ['./test-standard-supplies.component.scss']
})
export class TestStandardSuppliesComponent implements OnInit {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    public constant: Constants,
    private messageService: MessageService,
    private signalRService: SignalRService,
    private testDesignService: TestDesignService,
    private config: Configuration,
  ) { }

  @Input() listDMVT: any = [];
  private notiSub: Subscription;
  private loadListDMVT = new BroadcastEventListener<any>('listDMVT');
  model: any = {
    ModuleCode: '',
    ListMaterial: [],
    CreateBy: ''
  }

  modelTest: any = {
    PathFileMaterial: '',
    ApiUrl: '',
    PathDownload: '',
    ModuleCode: '',
    FileName: '',
    Type: '',
  }
  private getDownload: Subscription;
  private onDownload = new BroadcastEventListener<any>('generalDownload');
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  ngOnInit() {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.signalRService.listen(this.loadListDMVT, false);

    this.getDownload = this.onDownload.subscribe((data: any) => {
      if (data) {
        this.messageService.showSuccess('Tạo biểu mẫu thành công!');       
      }
      else {
        this.messageService.showMessage('Tạo biểu mẫu không thành công!');
      }
      this.blockUI.stop();
    });

    this.notiSub = this.loadListDMVT.subscribe((data: any) => {
      if (data) {
        this.listDMVT = data.Data;
        this.model.ModuleCode = this.moduleCode;
        this.model.CreateBy = data.Data[0].CreateBy;
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Tải dữ liệu thành công!');
        } else {
          this.messageService.showMessage('Tải dữ liệu không thành công!');
        }
      }
      this.blockUI.stop();
    });
  }

  loadDMVT() {
    if (this.pathDMVT) {
      this.signalRService.invoke('LoadDMVT', this.pathDMVT).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });  
     
    } else {
      this.messageService.showMessage('Chưa chọn thư mục!');
    }
  }

  exportReportDMVT() {
    this.model.ModuleCode = this.moduleCode;
    this.model.CreateBy = this.listDMVT[0].CreateBy;
    this.model.ListMaterial = this.listDMVT;
    this.testDesignService.exportReportDMVT(this.model).subscribe(d => {
      this.modelTest.PathFileMaterial = d;
      this.modelTest.ApiUrl = this.config.ServerApi;
      this.modelTest.ModuleCode = this.moduleCode;
      this.modelTest.Type = 1;
      this.modelTest.FileName = "XNVT.";
      this.DowloadTemplateToFolder();
    }, e => {
      this.messageService.showError(e);
    });
  }

  DowloadTemplateToFolder() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('DowloadTemplateToFolder', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });  
  }
}
