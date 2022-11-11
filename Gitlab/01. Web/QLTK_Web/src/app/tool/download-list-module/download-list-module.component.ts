import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { DxTreeListComponent } from 'devextreme-angular';
import { Router } from '@angular/router';
import { Configuration, MessageService, AppSetting, Constants, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { DownloadListModuleService } from '../services/download-list-module.service';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-download-list-module',
  templateUrl: './download-list-module.component.html',
  styleUrls: ['./download-list-module.component.scss']
})
export class DownloadListModuleComponent implements OnInit, OnDestroy {
  private onDownload = new BroadcastEventListener<any>('downloadModuleDesignDocumentShare');
  private downloadSub: Subscription;
  @ViewChild(DxTreeListComponent) treeView;
  @BlockUI() blockUI: NgBlockUI;
  constructor(private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    public appSetting: AppSetting,
    private downloadModuleService: DownloadListModuleService,
    public constant: Constants,
    private signalRService: SignalRService,
    private componentService: ComponentService
  ) { this.pagination = Object.assign({}, appSetting.Pagination); }


  ModalInfo = {
    Title: 'Module',
    SaveText: 'Lưu',
  };

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  listSelect: any[] = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  listModuleGroupId = [];
  StartIndex = 0;
  pagination;
  checkProjectId: string;
  LstpageSize = [5, 10, 15, 20, 25, 30];
  ListModuleGroup: any[] = [];
  listModule: any[] = [];
  listFile: any[] = [
    { Id: 0, Name: 'TK Cơ khí' },
    { Id: 1, Name: 'TK Điện' },
    { Id: 2, Name: 'TK Điện tử' },
    { Id: 3, Name: 'In Film' },
    { Id: 4, Name: 'HMI' },
    { Id: 5, Name: 'PLC' },
    { Id: 6, Name: 'Phần mềm' },
  ]

  listProject: any[] = [];

  modelModelGroup: any = {
    Id: '',
    Code: '',
    Name: '',
    ParentId: '',
    TotalItems: 0,
  }

  modelAll: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  importModel: any = {
    Path: ''
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên module...',
    Items: [
      // {
      //   Name: 'Tình trạng dữ liệu',
      //   FieldName: 'IsEnought',
      //   Placeholder: 'Tình trạng dữ liệu',
      //   Type: 'select',
      //   Data: this.constant.ModuleIsEnought,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      // {
      //   Name: 'Tình trạng sử dụng',
      //   FieldName: 'Status',
      //   Placeholder: 'Tình trạng sử dụng',
      //   Type: 'select',
      //   Data: this.constant.ModuleStatus,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'Dự Án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.ProjectByUserSBU,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn mã dự án'
      },
    ]
  };

  modelModule: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    TotalItemExten: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    ListIdSelect: [],
  }

  moduleGroupId: '';
  selectedModelGroupId = '';
  height = 0;

  downloadModel: any = {
    ModuleIds: [],
    ProjectId: '',
    ApiUrl: '',
    Token: ''
  }


  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Download tài liệu thiết kế";
    this.searchModule();
    this.selectedModelGroupId = localStorage.getItem("selectedModelGroupId");
    localStorage.removeItem("selectedModelGroupId");

    this.signalRService.listen(this.onDownload, true);

    this.downloadSub = this.onDownload.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Download file thành công!');
        } else {
          this.messageService.showMessage(data.Message);
        }
      }
      this.blockUI.stop();
    }, error => {
      this.messageService.showMessage('Download file không thành công!');
      this.blockUI.stop();
    });
  }

  ngOnDestroy() {
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownload);
  }

  clear() {
    this.modelModule = {
      PageSize: 10,
      totalItems: 0,
      TotalItemExten: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      ModuleGroupId: '',
      Code: '',
      Name: '',
      Note: '',
      IsEnought: '',
      State: '',
      Pricing: '',
      OrderTime: '',
      CurrentVersion: '',
      UpdateDate: '',
      File: '',
      Status: '',
      FileElectric: '',
      FileElectronic: '',
      FileMechanics: '',
    }
    this.searchModule();
  }

  searchModule() {
    this.modelModule.ListIdSelect = [];
    this.listSelect.forEach(element => {
      this.modelModule.ListIdSelect.push(element.Id);
    });

    if (!this.checkProjectId) {
      this.checkProjectId = this.modelModule.ProjectId;
    } else {
      if (this.checkProjectId != this.modelModule.ProjectId) {
        this.listSelect = [];
        this.checkProjectId = this.modelModule.ProjectId;
      }
    }

    // this.listModule.forEach(element => {
    //   if (element.Checked) {
    //     this.modelModule.ListIdChecked.push(element.Id);
    //   }
    // });

    this.downloadModuleService.searchModule(this.modelModule).subscribe((data: any) => {
      if (data.ListResult) {
        this.listModule = data.ListResult;
        this.StartIndex = ((this.modelModule.PageNumber - 1) * this.modelModule.PageSize + 1);
        this.modelModule.totalItems = data.TotalItem;
        this.modelModule.State = data.State;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  addRow() {
    this.listModule.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
        this.modelModule.totalItems--;
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listModule.indexOf(element);
      if (index > -1) {
        this.listModule.splice(index, 1);
      }
    });
    this.searchModule();
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listModule.push(element);
        this.modelModule.totalItems++;
      }
    });
    this.listModule.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
    this.searchModule();
  }

  downloadList() {
    this.componentService.showChooseFolder(0, 0, '').subscribe(result => {
      if (result) {
        this.downloadModel.DownloadPath = result;

        this.downloadModel.ModuleIds = [];
        this.listSelect.forEach(item => {
          this.downloadModel.ModuleIds.push(item.Id);
        });
        this.downloadModel.ProjectId = this.modelModule.ProjectId;
        this.downloadModel.ApiUrl = this.config.ServerApi;
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser && currentUser.access_token) {
          this.downloadModel.Token = currentUser.access_token;
        }

        this.signalRService.invoke('DownloadModuleDesignDocumentShare', this.downloadModel).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }
    }, (reason) => {
      this.messageService.showMessage(reason);
    });

  }


  importMaterials() {
    var path = 'Template/DanhSachVatTu_Download.xlsx';
    let fileTemplateMaterial = this.config.ServerApi + path;
    this.componentService.showChooseFile(fileTemplateMaterial).subscribe(materialPath => {
      if (materialPath) {

        this.componentService.showChooseFolder(0,0,'').subscribe(folderPath => {
          if (folderPath) {
            this.downloadModel.MaterialPath = materialPath;
            this.downloadModel.DownloadPath = folderPath;
            this.downloadModel.ApiUrl = this.config.ServerApi;
            let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
            if (currentUser && currentUser.access_token) {
              this.downloadModel.Token = currentUser.access_token;
            }

            this.signalRService.invoke('DownloadMaterialDesignDocumentShare', this.downloadModel).subscribe(data => {
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

    });
  }

  importMaterialsNiew() {
    var path = 'Template/DanhSachVatTu_Download.xlsx';
    let fileTemplateMaterial = this.config.ServerApi + path;
    this.componentService.showImportExcel(fileTemplateMaterial, true).subscribe(materialPath => {
      if (materialPath) {

        this.componentService.showChooseFolder(0,0,'').subscribe(folderPath => {
          if (folderPath) {
            this.downloadModel.MaterialPath = materialPath;
            this.downloadModel.DownloadPath = folderPath;
            this.downloadModel.ApiUrl = this.config.ServerApi;
            let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
            if (currentUser && currentUser.access_token) {
              this.downloadModel.Token = currentUser.access_token;
            }

            this.signalRService.invoke('DownloadMaterialDesignDocumentShare', this.downloadModel).subscribe(data => {
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

    });
  }

  showImportExcel() {
    this.modelModule.ListIdSelect = [];
    this.listSelect.forEach(element => {
      this.modelModule.ListIdSelect.push(element.Id);
    });
    var path = 'Template/ListModuleInProject.xlsx';
    let fileTemplateMaterial = this.config.ServerApi + path;
    this.componentService.showImportExcel(fileTemplateMaterial, false).subscribe(data => {
      if (data) {
        this.downloadModuleService.importExcelListModel(this.modelModule, data).subscribe(
          data => {
            if (data) {
              this.listModule = data.ListNoSelect;
              this.listSelect = data.ListSelect;
              this.modelModule.totalItems = data.TotalNoSelect;
            }
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }
}