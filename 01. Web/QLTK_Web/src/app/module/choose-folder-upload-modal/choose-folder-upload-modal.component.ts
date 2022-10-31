import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Configuration, Constants } from 'src/app/shared';
import { ModuleTabDesignDocumentService } from '../services/module-tab-design-document.service';
import { NgBlockUI, BlockUI } from 'ng-block-ui';
import { ListPlanDesginComponent } from '../Module/list-plan-desgin/list-plan-desgin.component';
import { DxTreeListComponent } from 'devextreme-angular';
import { ShowChooseProductModuleUpdateComponent } from '../Module/show-choose-product-module-update/show-choose-product-module-update.component';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
  selector: 'app-choose-folder-upload-modal',
  templateUrl: './choose-folder-upload-modal.component.html',
  styleUrls: ['./choose-folder-upload-modal.component.scss']
})
export class ChooseFolderUploadModalComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI!: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView!: DxTreeListComponent;
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private onUploadFolder = new BroadcastEventListener<any>('uploadFolder');
  private onCompareFileIGS = new BroadcastEventListener<any>('compareFileIGS');
  private compareFileIGSSub!: Subscription;
  private notiSub!: Subscription;
  private uploadSub!: Subscription;

  ModuleId!: string;
  ModuleCode!: string;
  ModuleGroupId!: string;
  listData: any[] = [];
  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  listCodeRule = [];

  dataModel = {
    ApiUrl: '',
    ModuleId: '',
    DesignType: 0,
    Path: '',
    ModuleCode: '',
    ModuleGroupCode: '',
    ModuleGroupId: '',
    Token: '',
    ListError: [],
    ListMaterialModel: [],
    ListRawMaterialsModel: [],
    ListManufacturerModel: [],
    ListFolderModel: [],
    ListFileModel: [],
    ListMaterialGroupModel: [],
    ListUnitModel: [],
    LstError: [],
    ListResult: [],
    ListCodeRule: []
  }

  folderModel = {
    Id: '',
    Path: '',
    SelectPath: '',
    ListFolder: []
  }

  uploadModel = {
    ModuleId: '',
    ListFile: [],
    DesignType: 0,
    LstError: [] as any[],
    ListData: [] as any[],
    Status: false
  }

  searchCodeRuleModel = {
    Code: ''
  }
  checkSelect: boolean = false;
  currentUser: any = null;
  constructor(
    private config: Configuration,
    private signalRService: SignalRService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private serviceHistory: HistoryVersionService,
    private designDocumentService: ModuleTabDesignDocumentService,
  ) { }

  ngOnInit() {
    let userStore = localStorage.getItem('qltkcurrentUser');
    if (userStore) {
      this.currentUser = JSON.parse(userStore);
    }

    this.dataModel.ModuleId = this.ModuleId;
    this.dataModel.ModuleCode = this.ModuleCode;
    this.dataModel.ModuleGroupId = this.ModuleGroupId;
    this.getFolder();
    this.signalRService.listen(this.onSendListFolder, false);
    this.notiSub = this.onSendListFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.folderModel.ListFolder = data.ListForder;
        if (this.checkSelect) {
          this.folderModel.SelectPath = data.Path;
          this.selectPathForder();
        }
      }
    });
    this.signalRService.listen(this.onUploadFolder, true);
    this.uploadSub = this.onUploadFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.LstError && data.LstError.length > 0) {
          var errorMessage = data.LstError.join("<br/>")
          this.messageService.showMessage(errorMessage);
        }
        else {
          this.insertDatabase(data);
        }
      }
    });

    this.signalRService.listen(this.onCompareFileIGS, true);
    this.compareFileIGSSub = this.onCompareFileIGS.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.listData = data;
        if (this.listData.length > 0) {
          this.showChooseProductModule();
        } else {
          this.chooseFolder();
        }
      }
    });
  }

  ngOnDestroy() {
    this.notiSub.unsubscribe();
    this.uploadSub.unsubscribe();
    this.signalRService.stopListening(this.onSendListFolder);
    this.signalRService.stopListening(this.onUploadFolder);
    this.compareFileIGSSub.unsubscribe();
    this.signalRService.stopListening(this.onCompareFileIGS);
  }

  getFolder() {
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  getLink() {
    var model = {
      ModuleCode: this.ModuleCode,
      Token: '',
      Type: this.dataModel.DesignType,
      ApiUrl: this.config.ServerApi,
    };
    this.checkSelect = true;
    if (this.currentUser && this.currentUser.access_token) {
      model.Token = this.currentUser.access_token;
    }
    this.signalRService.invoke('GetSelectFolder', model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  selectPathForder() {
    var data: any[] = this.folderModel.ListFolder.filter((i: any) => i.Id == this.folderModel.SelectPath);
    if (data.length > 0) {
      this.selectedId = data[0].Id;
      this.dataModel.Path = data[0].Id;
      this.folderModel.Path = data[0].Id;
      this.treeView.selectedRowKeys = [this.selectedId];
    }
  }

  onSelectionChanged(e: any) {
    this.selectedId = e.selectedRowKeys[0];
    this.dataModel.Path = e.selectedRowKeys[0];
    this.folderModel.Path = e.selectedRowKeys[0];
  }

  onRowExpanding(e: any) {
    this.checkSelect = false;
    this.folderModel.Id = e.key;
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
    // this.notiSub = this.onSendListFolder.subscribe((data: any) => {
    //   this.folderModel.ListFolder = data;
    // });
  }

  closeModal() {
    this.activeModal.close(true);
  }

  chooseFolder() {
    var checkUpload = false;

    this.dataModel.ApiUrl = this.config.ServerApi;
    if (this.currentUser && this.currentUser.access_token) {
      this.dataModel.Token = this.currentUser.access_token;
    }
    if (this.dataModel.DesignType) {
      this.signalRService.invoke('UploadModuleDesignDocument', this.dataModel).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
    else {
      this.messageService.showMessage("Chưa chọn loại thiết kế. Vui lòng kiểm tra lại!")
    }
  }

  insertDatabase(data:any) {
    this.uploadModel = data;
    this.uploadModel.ModuleId = this.dataModel.ModuleId;
    this.uploadModel.DesignType = this.dataModel.DesignType;
    this.uploadModel.ListData = this.listData;
    this.designDocumentService.uploadDesignDocument(this.uploadModel).subscribe((data: any) => {
      this.messageService.showSuccess("Upload tài liệu thiết kế thành công!");
      this.closeModal();
      this.showListPlanDesgin();
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showListPlanDesgin() {
    let showPlanDesgin = this.modalService.open(ListPlanDesginComponent, { container: 'body', windowClass: 'list-plan-desgin', backdrop: 'static' });
    showPlanDesgin.componentInstance.ModuleId = this.ModuleId;
    showPlanDesgin.result.then((result) => {
    }, (reason) => {

    });
  }

  showChooseProductModule() {
    let activeModal = this.modalService.open(ShowChooseProductModuleUpdateComponent, { container: 'body', windowClass: 'show-choose-product-module', backdrop: 'static' });
    activeModal.componentInstance.listData = this.listData;
    activeModal.result.then((result) => {
      if (result) {
        this.listData = result;
        this.chooseFolder();
      } else {
        this.chooseFolder();
      }
    }, (reason) => {

    });
  }

  compareFileIGS() {
    var file = {
      ModuleCode: this.ModuleCode,
      Type: this.dataModel.DesignType,
      Token: '',
      ApiUrl: this.config.ServerApi
    };
    if (this.currentUser && this.currentUser.access_token) {
      file.Token = this.currentUser.access_token;
    }
    this.signalRService.invoke('CompareFileIGS', file).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.compareFileIGS();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.ModuleId;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Module;
    activeModal.result.then(async (result) => {
      if (result) {
        this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.compareFileIGS();
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
