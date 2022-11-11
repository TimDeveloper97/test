import { Component, OnInit, Input, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChooseFolderFileComponent } from '../choose-folder-file/choose-folder-file.component';
import { Constants, MessageService, Configuration, ComponentService } from 'src/app/shared';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { TestDesignStructureService } from '../services/test-designstructure-service';
import { Subscription } from 'rxjs';
import { TestDesignService } from '../services/test-design-service';
import { MaterialService } from 'src/app/material/services/material-service';

@Component({
  selector: 'app-test-file-dmvt',
  templateUrl: './test-file-dmvt.component.html',
  styleUrls: ['./test-file-dmvt.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestFileDmvtComponent implements OnInit {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @BlockUI() blockUI: NgBlockUI;
  constructor(private modalService: NgbModal,
    public constant: Constants,
    private signalRService: SignalRService,
    private testDesignStructureService: TestDesignStructureService,
    private messageService: MessageService,
    private config: Configuration,
    private testDesignService: TestDesignService,
    private componentService: ComponentService,
    private materialService: MaterialService,

  ) { }

  pathFile: string;
  @Input() listDMVT: any;
  @Input() listDMVTNotDB : any;
  modelTest: any = {
    PathFileMaterial: '',
    List3D: [],
    ListModuleDesignDocument: [],
    ListRawMaterial: [],
    ListMaterialDB: [],
    ListModuleError: [],
    ListManufacture: [],
    ListConvertUnit: [],
    ListDesignStructure: [],
    ListDesignStructureFile: [],
    Module: '',
    ListModule: [],
    ListUnit: [],
    SelectedPath: '',
    ApiUrl: '',
    PathDownload: '',
    ModuleCode: ''
  }
  private notiSub: Subscription;
  private listDMVTError = new BroadcastEventListener<any>('listDMVTResult');
  private listDMVTExport = new BroadcastEventListener<any>('exportResultDMVT');
  isOk = false;
  listManuError: any;
  listPartError: any;
  //listDMVTNotDB: any;
  listPartManuError: any;
  pathExport: any;
  fileName: any;
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  isDMVTError = false;
  ngOnInit() {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.signalRService.listen(this.listDMVTError, false);
    this.signalRService.listen(this.listDMVTExport, false);
    this.notiSub = this.listDMVTError.subscribe((data: any) => {
      if (data) {
        this.listDMVT = data.Data.ListResult;
        // this.isOk = data.Data.isOk;
        this.listManuError = data.Data.ListManuError;
        this.listPartError = data.Data.ListPartError;
        this.listPartManuError = data.Data.ListPartManuError;
        this.isDMVTError = data.Data.IsOK;
        this.listDMVTNotDB = data.Data.ListMaterialNotDB;
        if (this.isDMVTError) {
          this.messageService.showMessage("Danh mục vật tư chưa chuẩn");
        }
        if (this.listManuError) {
          this.messageService.showMessage(this.listManuError);
        }
      }
      this.blockUI.stop();
    });

    this.notiSub = this.listDMVTExport.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showMessage('Xuất excel thành công!');
        } else {
          this.messageService.showMessage(data.Message);
        }
      }
      this.blockUI.stop();
    });

  }

  showChooseFolderWindow() {
    let activeModal = this.modalService.open(ChooseFolderFileComponent, { container: 'body', windowClass: 'choose-folder-file-modal', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
        this.pathFile = result;
      }
    }, (reason) => {

    });
  }

  checkFileDMVT() {
    this.isDMVTError = false;
    if (this.pathFile) {
      this.modelTest.PathFileMaterial = this.pathFile;
      this.modelTest.ApiUrl = this.config.ServerApi;
      let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
      if (currentUser && currentUser.access_token) {
        this.modelTest.Token = currentUser.access_token;
      }
      this.signalRService.invoke('CheckFileDMVT', this.modelTest).subscribe(data => {
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

  exportDMVTError() {
    if (this.pathFile && this.fileName) {
      this.testDesignService.exportResultDMVT({ FileName: this.fileName }).subscribe(d => {
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.SelectedPath = this.pathExport;
        this.modelTest.PathFile = this.pathFile;
        this.modelTest.PathDownload = d;
        this.modelTest.ModuleCode = this.fileName;
        this.signalRService.invoke('ExportResuleDMVT', this.modelTest).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }, e => {
        this.messageService.showError(e);
      });
    } else {
      this.messageService.showMessage("Bạn chưa nhập tên!");
    }
  }

  showChooseFolderExport() {   
    this.componentService.showChooseFolder(0, 0, '').subscribe(result => {
      if (result) {
        this.pathExport = result;
        this.exportDMVTError();
      }
    }, (reason) => {

    });
  }

  ExportDMVT() {
    this.materialService.exportDMVTNotDB(this.listDMVTNotDB).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

}
