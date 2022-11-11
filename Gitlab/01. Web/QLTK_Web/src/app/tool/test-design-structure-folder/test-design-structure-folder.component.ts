import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Constants, Configuration, MessageService, ComponentService } from 'src/app/shared';
import { TestDesignStructureService } from '../services/test-designstructure-service';
import { Subscription } from 'rxjs';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TestDesignPopupReportComponent } from '../test-design-popup-report/test-design-popup-report.component';
import { TestDesignService } from '../services/test-design-service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-test-design-structure-folder',
  templateUrl: './test-design-structure-folder.component.html',
  styleUrls: ['./test-design-structure-folder.component.scss']
})
export class TestDesignStructureFolderComponent implements OnInit {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @Input() listErrorDesignFolder: [];
  @Input() listErrorDownload: any[];
  @Input() listFileRedundant: any[];
  @Input() listDocumentFileSize: any[];
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    public constant: Constants,
    private config: Configuration,
    private testDesignStructureService: TestDesignStructureService,
    private messageService: MessageService,
    private signalRService: SignalRService,
    private modalService: NgbModal,
    private testDesignService: TestDesignService,
    private componentService: ComponentService
  ) {
    this.items = [
      { Id: 1, text: 'Xóa thư mục, file thừa', icon: 'fas fa-times text-danger' }
    ];
  }

  items: any;
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
  private error = new BroadcastEventListener<any>('listError');
  private resultReportNot3d = new BroadcastEventListener<any>('exportReportNot3D');
  private exportReportExcel = new BroadcastEventListener<any>('exportExcelTestDesignStructure');
  private onDownload = new BroadcastEventListener<any>('downloadFileModuleDesignDocument');
  private downloadSub: Subscription;
  private onDownloadAFile = new BroadcastEventListener<any>('downloadAFile');
  private downloadAFileSub: Subscription;
  private deleteList: Subscription;
  private onDeleteList = new BroadcastEventListener<any>('getDeleteList');
  listError: any = [];
  listErrorMain: any = [];
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  listHeight = 0;
  datatbleCT: any = [];
  errorSpecial: string;
  htmlText: string;

  modelReport: any = {
    ApiUrl: '',
    PathDownload: '',
    PathLocal: '',
    ModuleCode: '',
    DatatbleCT: [],
  }

  ngOnInit() {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.listHeight = window.innerHeight - 390;

    // this.modelTest.PathFile = this.pathDMVT;
    // this.modelTest.ModuleCode = this.moduleCode;
    // this.modelTest.SelectedPath = this.selectedPath;
    // this.modelTest.ApiUrl = this.config.ServerApi;

    // this.searchList3D();
    // this.searchListMaterial();
    // this.searchListModuleDesignDocument();
    // this.searchListRawMaterial();
    // this.searchListModuleErrorNotDone();
    // this.searchListConvertUnit();
    // this.searchListDesignStructure();
    // this.searchListDesignStructureFile();


    this.signalRService.listen(this.error, false);
    this.signalRService.listen(this.resultReportNot3d, false);

    this.notiSub = this.resultReportNot3d.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Xuất vật tư không có 3D thành công!');
        } else {
          this.messageService.showMessage('Xuất vật tư không có 3D không thành công!');
        }
      }
      this.blockUI.stop();
    });

    this.notiSub = this.error.subscribe((data: any) => {
      if (data) {
        if (data.Data) {
          this.listError = data.Data.listError;
          this.listErrorMain = data.Data.listErrorMain;
          this.datatbleCT = data.Data.datatbleCT;
          this.htmlText = data.Data.htmlText;
          if (this.datatbleCT.length > 0 && this.listErrorMain.length > 0) {
            this.listError.forEach(error => {
              this.errorSpecial = this.errorSpecial + error;
            });
            this.messageService.showMessageTitle(this.errorSpecial, "Danh sách lỗi nghiêm trọng");
          } else {
            this.messageService.showMessage("Cấu trúc thiết kế " + this.moduleCode + " đã chuẩn! Bạn Có thể xem báo cáo?");
            // this.messageService.showSuccess('Đã in báo cáo!');
          }
        }

        if (data.StatusCode == 1) {
          this.messageService.showSuccess("Kiểm tra dữ liệu thành công!");
        } else {
          this.messageService.showMessage(data.Message);
        }
      }

      this.blockUI.stop();
    });
    this.signalRService.listen(this.onDownload, true);
    this.downloadSub = this.onDownload.subscribe((data: any) => {
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
    //
    this.signalRService.listen(this.onDownloadAFile, true);
    this.downloadAFileSub = this.onDownloadAFile.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showMessage('Download thành công!');
        } else {
          this.messageService.showMessage(data.Message);
        }
      } else {
        this.messageService.showMessage(data.Message);
      }
    });

    // this.signalRService.listen(this.onDeleteList, true);
    // this.deleteList = this.onDeleteList.subscribe((data: any) => {
    //   this.blockUI.stop();
    //   if (data) {
    //     //this.check();
    //     this.messageService.showSuccess('Xóa file thành công!');
    //   }
    // });
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownload);
    this.downloadAFileSub.unsubscribe();
    this.signalRService.stopListening(this.onDownloadAFile);
    //this.deleteList.unsubscribe();
    //this.signalRService.stopListening(this.onDeleteList);
  }

  exportExcel() {
    this.testDesignService.exportReportTestDesignStructure({ ModuleCode: this.moduleCode, DatatbleCT: this.datatbleCT }).subscribe(d => {
      this.modelReport.ApiUrl = this.config.ServerApi;
      this.modelReport.PathDownload = d;
      this.modelReport.ModuleCode = this.moduleCode
      this.signalRService.invoke('ExportExcelReportDesignStructure', this.modelReport).subscribe(data => {
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
  }

  showReport() {
    let activeModal = this.modalService.open(TestDesignPopupReportComponent, { container: 'body', windowClass: 'test-design-popup-report-modal', backdrop: 'static' })
    activeModal.componentInstance.htmlText = this.htmlText;
  }

  viewReportNot3D() {
    let isExistNot3d = false;
    for (var i = 0; i < this.datatbleCT.length; i++) {
      if (this.datatbleCT[i].Type == "Không có trong thư viện 3D") {
        isExistNot3d = true;
        i = this.datatbleCT.length - 1;
      }
    }

    if (isExistNot3d) {
      this.modelReport.DatatbleCT = this.datatbleCT.filter(element => {
        return element.Type == "Không có trong thư viện 3D";
      });

      if (this.modelReport.DatatbleCT.length > 0) {
        this.modelReport.ModuleCode = this.moduleCode;
        this.signalRService.invoke('ExportReportNot3D', this.modelReport).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });

      } else {
        this.messageService.showMessage('Không có vật tư không có 3D!');
      }

    } else {
      this.messageService.showMessage("Không có vật tư cần xuất!");
    }

  }

  showChooseFolderWindow(isExcel: boolean) {
    this.componentService.showChooseFolder(0, 0, '').subscribe(result => {
      if (result) {
        this.modelReport.PathLocal = result;
        if (!isExcel) {
          this.viewReportNot3D();
        } else {
          this.exportExcel();
        }
      }
    }, (reason) => {

    });
  }

  getListErrorDesignStructure() {
    this.modelTest.ModuleCode = this.moduleCode;
    this.modelTest.PathFile = this.pathDMVT;
    this.modelTest.SelectedPath = this.selectedPath;
    this.modelTest.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('GetListErrorDesignStructure', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  searchList3D() {
    this.testDesignStructureService.searchDesign().subscribe((data: any) => {
      if (data) {
        this.modelTest.List3D = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListMaterial() {
    this.testDesignStructureService.searchMaterial().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListMaterialDB = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListModuleDesignDocument() {
    this.testDesignStructureService.searchListModuleDesignDocument().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListModuleDesignDocument = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListRawMaterial() {
    this.testDesignStructureService.searchRawMaterial().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListRawMaterial = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListModuleErrorNotDone() {
    this.testDesignStructureService.searchListModuleErrorNotDone().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListModuleError = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListConvertUnit() {
    this.testDesignStructureService.searchListConvertUnit().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListConvertUnit = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListDesignStructure() {
    this.testDesignStructureService.searchListDesignStructure().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListDesignStructure = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchListDesignStructureFile() {
    this.testDesignStructureService.searchListDesignStructureFile().subscribe((data: any) => {
      if (data) {
        this.modelTest.ListDesignStructureFile = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchModule() {
    this.testDesignStructureService.searchModule(this.moduleCode).subscribe((data: any) => {
      if (data) {
        this.modelTest.Module.push(data);
        this.getListErrorDesignStructure();
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  download(index) {
    var data = this.listErrorDownload.filter(i => i.Index == index);
    if (data.length > 0) {
      if (data[0].Type == 1) {
        this.downloadAFile(data[0]);
      } else if (data[0].Type == 2) {
        this.downloadFile(index, data[0].FileName, data[0].FilePath)
      }
    } else {
      this.messageService.showMessage("Bạn chỉ có thể download file khác size");
    }
  }

  downloadAFile(data: any) {
    var file = {
      FileName: data.FileName,
      PathFileMaterial: data.FilePath,
      Token: '',
      ApiUrl: this.config.ServerApi,
      FileApiUrl: this.config.ServerFileApi
    };
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      file.Token = currentUser.access_token;
    }

    this.signalRService.invoke('DownloadFile3DMaterial', file).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  downloadFile(id, name, serverPath) {
    var file = {
      Id: id,
      Token: '',
      Type: 1,
      FilePath: serverPath,
      ModuleCode: this.moduleCode,
      ListDocumentFileSize: this.listDocumentFileSize,
      ApiUrl: this.config.ServerApi
    };
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      file.Token = currentUser.access_token;
    }

    this.signalRService.invoke('DownloadFileModuleDocument', file).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }


  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.deleteLists();
    }
  }

  deleteLists() {
    this.signalRService.invoke('DeleteListFileFolder', this.listFileRedundant).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }
}
