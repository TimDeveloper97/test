import { Component, OnInit, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { Constants, MessageService, Configuration, AppSetting, ComponentService } from 'src/app/shared';
import { TestDesignService } from '../services/test-design-service';
import { GeneralTemplateService } from '../services/general-template.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-test-design-manager',
  templateUrl: './test-design-manager.component.html',
  styleUrls: ['./test-design-manager.component.scss']
})
export class TestDesignManagerComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    private signalRService: SignalRService,
    private modalService: NgbModal,
    public constant: Constants,
    private messageService: MessageService,
    private testDesignService: TestDesignService,
    private config: Configuration,
    private serviceGeneral: GeneralTemplateService,
    public appSetting: AppSetting,
    private componentService: ComponentService
  ) {
    this.items = [
      { Id: 1, text: 'Xóa file', icon: 'fas fa-times text-danger' }
    ];
  }

  items: any;
  path: string
  txtPathSC: string;
  pathDMVT: string;
  private softDesign = new BroadcastEventListener<any>('listSoftDesgin');
  private hardDesgin = new BroadcastEventListener<any>('listHardDesgin');
  private softSub: Subscription;
  private hardSub: Subscription;
  private checkElectricSub: Subscription;
  private onCheckElectric = new BroadcastEventListener<any>('listFoderElec');
  private checkElectronicSub: Subscription;
  private onCheckElectronic = new BroadcastEventListener<any>('checkElectronic');
  private deleteList: Subscription;
  private onDeleteList = new BroadcastEventListener<any>('getDeleteList');
  listSoftDesign: any = [];
  listHardDesgin: any = [];
  isCheckDesign = false;

  moduleCode: string;
  selectedPath: string;

  modelTest: any = {
    ApiUrl: '',
    SelectedPath: '',
    Type: 1,
    ModuleCode: '',
    Token: '',
    CheckModel: {}
  }

  checkModel: any = {
    IsCheckDMVT: true,
    IsCheckDesignCAD: true,
    IsCheckSoftHardCAD: true,
    IsCheckMatch: true,
    IsCheckDesignFolder: true,
    IsCheckElectric: false,
    IsCheckElectronic: false,
    IsCheckMAT: true,
    IsCheckIGS: true,
  }

  listCheckFile: any = [];
  listError: any = [];
  listErrorDownload: any[] = [];
  listFileRedundant: any[] = [];
  listDocumentFileSize: any[] = [];
  listErrorCheckCAD = [];
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  @ViewChild('scrollHeaderTwo',{static:false}) scrollHeaderTwo: ElementRef;

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
    this.softSub.unsubscribe();
    this.checkElectricSub.unsubscribe();
    this.checkElectronicSub.unsubscribe();
    this.hardSub.unsubscribe();
    this.deleteList.unsubscribe();
    this.signalRService.stopListening(this.softDesign);
    this.signalRService.stopListening(this.hardDesgin);
    this.signalRService.stopListening(this.onCheckElectric);
    this.signalRService.stopListening(this.onCheckElectronic);
    this.signalRService.stopListening(this.onDeleteList);
  }

  listErrorDesignCAD: any = [];
  cadResultModel: any;
  listFileMAT: any = [];
  listHardCAD: any = [];
  listSoftCAD: any = [];
  listDMVTError: any = [];
  listDMVTNotDB: any = [];

  listMaterial: any = [];
  listFileIGS: any = [];
  resultCheckDMVTModel = {
    ListResult: []
  };
  isSoftHardCADError = false;
  isMatch = true;
  isDesignFolderError = false;
  isDesignCADError = false;
  isMATError = false;
  isDMVTError = false;
  isMaterial = false;
  isCheckElectric = false;
  isCheckElectronic = false;
  isIGSError = false;

  ngOnInit() {
    this.appSetting.PageTitle = "Kiểm tra dữ liệu thiết kế";
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderTwo.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.signalRService.listen(this.softDesign, true);
    this.softSub = this.softDesign.subscribe((data: any) => {
      this.blockUI.stop();
      this.checkFolder(data);
    });

    this.signalRService.listen(this.onCheckElectric, true);
    this.checkElectricSub = this.onCheckElectric.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.listCheckFile = data.ListCheckFile;
        this.listError = data.LstError;
        if (this.listError.length > 0) {
          this.isDesignFolderError = true;
        }
        else {
          this.messageService.showMessage('Thiết kế chuẩn');
        }
      }
    });

    this.signalRService.listen(this.onCheckElectronic, true);
    this.checkElectronicSub = this.onCheckElectronic.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.listError = data.LstError;
        this.listDMVTError = data.resultCheckDMVTModel.ListResult;
        this.isDMVTError = data.resultCheckDMVTModel.IsOK;
        this.listDMVTNotDB = data.resultCheckDMVTModel.ListMaterialNotDB;
        if (this.listError && this.listError.length > 0) {
          this.isCheckElectronic = true;
        }

        if (!this.isDMVTError && !this.isCheckElectronic) {
          this.messageService.showMessage('Thiết kế chuẩn');
        }
      }
    });

    this.signalRService.listen(this.hardDesgin, true);
    this.hardSub = this.hardDesgin.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Tải dữ liệu thành công!');
          this.listHardDesgin = data.Data;
        } else {
          this.messageService.showMessage(data.Message);
        }
      }
    });

    this.signalRService.listen(this.onDeleteList, true);
    this.deleteList = this.onDeleteList.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        // this.check();
        this.messageService.showSuccess('Xóa file thành công!');
      } else {
        this.messageService.showMessage(data.Message);
      }
    });
  }

  itemClickHardDesgin(e) {
    if (e.itemData.Id == 1) {
      this.deleteListsHardDesgin();
    }
  }
  itemClickSoftDesign(e) {
    if (e.itemData.Id == 1) {
      this.deleteListsSoftDesign();
    }
  }

  ListPath: any[] = [];
  deleteListsHardDesgin() {
    this.ListPath = [];
    for (var item of this.listHardDesgin) {
      if (this.isCheckDesign == true && item.IsExistName == false || item.IsDifferentSize == true || item.IsDifferentDate == true) {
        this.ListPath.push(item.FilePath)
      }
    }
    if (this.ListPath.length > 0) {
      this.blockUI.start();
      this.signalRService.invoke('DeleteListFile', this.ListPath).subscribe(data => {
        if (data) {
        }
        else {
          this.blockUI.stop();
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
  }

  deleteListsSoftDesign() {
    this.ListPath = [];
    for (var item of this.listSoftDesign) {
      if (this.isCheckDesign == true && item.IsExistName == false) {
        this.ListPath.push(item.FilePath)
      }
    }
    if (this.ListPath.length > 0) {
      this.blockUI.start();
      this.signalRService.invoke('DeleteListFile', this.ListPath).subscribe(data => {
        if (data) {
        }
        else {
          this.blockUI.stop();
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
  }

  // Kiểm tra cơ khí
  checkMechanical() {
    this.modelTest.CheckModel = this.checkModel;
    this.isDMVTError = false;
    this.isDesignCADError = false;
    this.isSoftHardCADError = false;
    this.isMatch = true;
    this.isDesignFolderError = false;
    this.isMATError = false;
    this.modelTest.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }

    this.blockUI.start();
    this.signalRService.invoke('CheckMechanical', this.modelTest).subscribe(data => {
      if (data) {
      }
      else {
        this.blockUI.stop()
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  checkFolder(data) {
    this.blockUI.stop();
    if (data) {
      if (data.StatusCode == 1) {
        this.listError = data.Data.listError;
        this.listErrorDownload = data.Data.listFileSize;
        this.listFileRedundant = data.Data.listFileRedundant;
        this.listDocumentFileSize = data.Data.listDocumentFileSize;
        this.listErrorCheckCAD = data.Data.listErrorCheckCAD;
        if (this.listError.length > 0) {
          // this.messageService.showMessage("Cấu trúc thiết kế chưa chuẩn!");
          this.isDesignFolderError = true;
        }
        // } else {
        this.listSoftDesign = data.Data.listSoftĐesign;
        if (this.listSoftDesign && this.listSoftDesign.length > 0) {
          this.txtPathSC = this.listSoftDesign[0].TxtPathSC;
          this.pathDMVT = this.listSoftDesign[0].PathDMVT;
          this.moduleCode = this.listSoftDesign[0].ModuleCode;
          this.selectedPath = this.listSoftDesign[0].SelectedPath;
        }

        this.listHardDesgin = data.Data.listHardĐesign;
        this.checkDesign();
        this.listErrorDesignCAD = data.Data.listErrorDesignCAD;
        if (this.listErrorDesignCAD && this.listErrorDesignCAD.length > 0) {
          if (this.listErrorDesignCAD.length > 0 || this.listErrorCheckCAD.length > 0) {
            this.isDesignCADError = true;
          }
        }

        this.cadResultModel = data.Data.cadResultModel;
        this.listFileMAT = data.Data.listFileMAT;
        if (this.listFileMAT && this.listFileMAT.length > 0) {
          this.listFileMAT.forEach(element => {
            if (element.Status == "2") {
              this.isMATError = true;
            }
          });

        }
        this.cadResultModel = data.Data.cadResultModel;
        this.listFileIGS = data.Data.listFileIGS;
        if (this.listFileIGS && this.listFileIGS.length > 0) {
          this.listFileIGS.forEach(element => {
            if (element.Status == "2") {
              this.isIGSError = true;
            }
          });

        }
        this.listHardCAD = this.cadResultModel.ListHardCAD;
        this.listSoftCAD = this.cadResultModel.ListSoftCAD;
        this.isSoftHardCADError = this.cadResultModel.IsSuccess;
        this.listDMVTError = data.Data.checkDMVTModel.ListResult;
        this.isDMVTError = data.Data.checkDMVTModel.IsOK;
        this.listDMVTNotDB = data.Data.checkDMVTModel.ListMaterialNotDB;
        // if (this.listDMVTError && this.listDMVTError.length > 0) {
        //   this.isDMVTError = true;
        // }

        this.listMaterial = data.Data.listMaterial;
        if (this.listMaterial && this.listMaterial.length > 0) {
          this.isMaterial = true;
        }

        if (!this.isDMVTError && !this.isMATError && !this.isIGSError && !this.isDesignCADError && !this.isDesignFolderError
          && !this.isSoftHardCADError && this.isMatch) {
          this.messageService.showMessage('Thiết kế chuẩn');
        }

      } else {
        this.messageService.showMessage(data.Message);
      }
    }
  }

  showChooseFolderWindow() {
    this.componentService.showChooseFolder(0, 0, '').subscribe(result => {
      if (result) {
        if (this.modelTest.Type) {
          if (this.modelTest.Type == 1) {
            this.modelTest.SelectedPath = result;
            this.checkMechanical();
          }
          else if (this.modelTest.Type == 2) {
            this.checkFolderModel.Path = result;
            this.checkFolderModel.DesignType = 2;
            this.checkElectric();
          }
          else if (this.modelTest.Type == 3) {
            this.checkFolderModel.Path = result;
            this.checkFolderModel.DesignType = 3;
            this.checkElectronic();
          }
        }
        this.isCheckDesign = false;
      }
    }, (reason) => {

    });
  }

  loadHardDesign() {
    this.blockUI.start();
    this.signalRService.invoke('LoadHardDesign', this.path).subscribe(data => {
      if (data) {
      }
      else {
        this.blockUI.stop();
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  checkDesign() {
    this.isCheckDesign = true;
    this.listHardDesgin.forEach(hardDesign => {
      var item = this.listSoftDesign.find(t => t.NameCompare === hardDesign.NameCompare);
      if (item) {
        if (item.IValue != hardDesign.SizeCompare) {
          hardDesign.IsDifferentSize = true;
        }
        if (item.WValue != hardDesign.DateCompare) {
          hardDesign.IsDifferentDate = true;
        }
        hardDesign.IsExistName = true;
        item.IsExistName = true;
      }
    });

    for (var i = 0; i < this.listHardDesgin.length; i++) {
      if (this.listHardDesgin[i].IsDifferentSize == true || this.listHardDesgin[i].IsDifferentDate == true || this.listHardDesgin[i].IsExistName == false) {
        this.isMatch = false;
        i = this.listHardDesgin.length;
      }
    }

    for (var i = 0; i < this.listSoftDesign.length; i++) {
      if (this.listSoftDesign[i].IsExistName == false) {
        this.isMatch = false;
        i = this.listSoftDesign.length;
      }
    }

    // if (this.isMatch == false) {
    //   this.messageService.showMessage('Dữ liệu Bản mềm IPT và Bản cứng JPG chưa chuẩn!');
    //   this.isMatch = true;
    // } else {
    //   this.messageService.showMessage('Dữ liệu Dữ liệu Bản mềm IPT và Bản cứng JPG đã chuẩn!');
    // }
  }

  model: any = {
    ModuleCode: '',
    ListSoftDesign: [],
    ListHardDesign: [],
  }

  ExportExcel() {
    this.model.ListSoftDesign = this.listSoftDesign;
    this.model.ListHardDesign = this.listHardDesgin;
    this.model.ModuleCode = this.listHardDesgin[0].ModuleCode;
    this.testDesignService.exportExcel(this.model).subscribe(d => {
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

  modelCheck: any = {
    ProductCode: '',
    Name: '',
    UserCheck: '',
  }

  checkFolderModel = {
    ApiUrl: '',
    ModuleCode: '',
    Path: '',
    DesignType: 0,
    Token: '',
    CheckModel: {}
  }

  // Tạo danh mục kiểm tra
  GeneralCheck() {
    this.serviceGeneral.GeneralCheckList(this.modelCheck).subscribe(d => {
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

  check() {
    if (this.modelCheck.ProductCode == '') {
      this.messageService.showMessage("Mã module không được để trống");
    }
    else if (this.modelTest.Type == 0) {
      this.messageService.showMessage("Bạn chưa chọn loại thiết kế");
    }
    else {
      if (this.modelTest.Type == 1) {
        this.modelTest.SelectedPath = this.path;
        this.modelTest.ModuleCode = this.modelCheck.ProductCode;
        this.checkMechanical();
      }
      else if (this.modelTest.Type == 2) {
        this.checkFolderModel.ModuleCode = this.modelCheck.ProductCode;
        this.checkFolderModel.DesignType = 2;
        this.checkElectric();
      }
      else if (this.modelTest.Type == 3) {
        this.checkFolderModel.ModuleCode = this.modelCheck.ProductCode;
        this.checkFolderModel.DesignType = 3;
        this.checkElectronic();
      }
    }
  }

  checkElectric() {
    this.checkFolderModel.CheckModel = this.checkModel;
    this.isCheckElectric = false;
    this.isDesignFolderError = false;
    this.checkFolderModel.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.checkFolderModel.Token = currentUser.access_token;
    }

    this.blockUI.start();
    this.signalRService.invoke('CheckElectric', this.checkFolderModel).subscribe(data => {
      if (data) {
      }
      else {
        this.blockUI.stop();
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  checkElectronic() {
    this.checkFolderModel.CheckModel = this.checkModel;
    this.isCheckElectronic = false;
    this.isDMVTError = false;
    this.checkFolderModel.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.checkFolderModel.Token = currentUser.access_token;
    }
    this.blockUI.start();
    this.signalRService.invoke('CheckElectronic', this.checkFolderModel).subscribe(data => {
      if (data) {
      }
      else {
        this.blockUI.stop();
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  changeCheck() {
    if (this.modelTest.Type == 1) {
      this.checkModel = {
        IsCheckDMVT: true,
        IsCheckDesignCAD: true,
        IsCheckSoftHardCAD: true,
        IsCheckMatch: true,
        IsCheckDesignFolder: true,
        IsCheckElectric: false,
        IsCheckElectronic: false,
        IsCheckMAT: true,
        IsCheckIGS: true,
      }
    }
    else if (this.modelTest.Type == 2) {
      this.checkModel = {
        IsCheckDMVT: false,
        IsCheckDesignCAD: false,
        IsCheckSoftHardCAD: false,
        IsCheckMatch: false,
        IsCheckDesignFolder: true,
        IsCheckElectric: true,
        IsCheckElectronic: false,
        IsCheckMAT: false,
        IsCheckIGS: false,
      }
    }
    else if (this.modelTest.Type == 3) {
      this.checkModel = {
        IsCheckDMVT: true,
        IsCheckDesignCAD: false,
        IsCheckSoftHardCAD: false,
        IsCheckMatch: false,
        IsCheckDesignFolder: false,
        IsCheckElectric: false,
        IsCheckElectronic: true,
        IsCheckMAT: false,
        IsCheckIGS: false,
      }
    }
  }
}
