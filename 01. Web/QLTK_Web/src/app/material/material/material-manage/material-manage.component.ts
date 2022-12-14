import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { Configuration, MessageService, AppSetting, Constants, ComboboxService, ComponentService } from 'src/app/shared';
import { MaterialService } from '../../services/material-service';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { MaterialBuyHistoryModalComponent } from '../material-buy-history-modal/material-buy-history-modal.component';
import { ImportMaterialBuyHistoryModalComponent } from '../import-material-buy-history-modal/import-material-buy-history-modal.component';
import { ImportMaterialComponent } from '../import-material/import-material.component';
import { MaterialgroupCreateComponent } from '../../materialgroup/materialgroup-create/materialgroup-create.component';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-material-manage',
  templateUrl: './material-manage.component.html',
  styleUrls: ['./material-manage.component.scss']
})
export class MaterialManageComponent implements OnInit, OnDestroy {
  @ViewChild(DxTreeListComponent) treeView;
  @ViewChild('fileInput', { static: false })
  @BlockUI() blockUI: NgBlockUI;
  myInputVariable: ElementRef;

  constructor(
    private router: Router,
    public config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private constant: Constants,
    public appSetting: AppSetting,
    private materialService: MaterialService,
    private materialGroupService: MaterialGroupService,
    private comboboxService: ComboboxService,
    private componentService: ComponentService,
    private signalRService: SignalRService

  ) {
    this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
      { Id: 1, text: 'Th??m', icon: 'fas fa-plus' },
      { Id: 2, text: 'S???a', icon: 'fa fa-edit' },
      { Id: 3, text: 'X??a', icon: 'fas fa-times' }
    ];
  }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  startIndex = 0;
  items: any;
  pagination: any;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  logUserId: string;
  groupId: string;
  listMaterialGroup: any[] = [];
  listMaterialGroupId = [];
  listMaterial: any[] = [];
  listManufacture: any[] = [];
  manufactureIds = [];
  fileToUpload: File = null;

  fileTemplate = this.config.ServerApi + 'Template/L???ch s??? gi?? mua_Template.xlsx';
  fileTemplateSync = this.config.ServerApi + 'Template/Import_SyncSaleMaterial_Template.xls';
  fileTemplateMaterial = '';

  modelMaterialGroup: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    MaterialGroupTPAId: '',
    MaterialGroupTPAName: '',
    MaterialGroupTPACode: '',
    Name: '',
    Code: '',
    ParentId: '',
    Description: '',
  }

  modelConfim: any = {
    UserId: '',
    Overr: '0'
  }

  modelMaterial: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    TotalItemExten: 0,
    TotalNoFile: 0,
    Date: null,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    UnitId: '',
    MaterialGroupId: '',
    ManufactureId: '',
    Code: '',
    Name: '',
    Note: '',
    Pricing: null,
    DeliveryDays: null,
    ImagePath: '',
    ThumbnailPath: '',
    LastBuyDate: '',
    IsUsuallyUse: '',
    MaterialType: '',
    RawMaterialId: '',
    MaterialDesign3DId: '',
    MaterialDataSheetId: '',
    Is3D: '',
    IsDataSheet: '',
    ManufactureName: '',
    UnitName: '',
    RawMaterialName: '',
    MaterialGroupName: '',
    LastDelivery: null,
    MechanicalType: '',
    Status: '',
    Status3D: '',
    StatusDatasheed: '',
    IsAllFile: '',
    IsRedundant: true,
    MaterialPriceType: 1,
    DeliveryDaysType: 1,
    LastDeliveryType: 1,
    IsSendSale: null,
    
  }
  downloadModel: any = {
    DownloadPath: '',
    material: [],
    ApiUrl: '',
    Token: '',
    ApiFileUrl: ''
  }


  modelAll: any = {
    Id: '',
    Name: 'T???t c???',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'T??m ki???m theo m??/t??n v???t t?? ...',
    Items: [
      {
        Name: 'M?? nh??m v???t t??',
        FieldName: 'MaterialGroupName',
        Placeholder: 'Nh???p m?? nh??m',
        Type: 'text'
      },
      {
        Name: 'M?? h??ng s???n xu???t',
        FieldName: 'ManufactureId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'M?? h??ng s???n xu???t' }, { Name: 'Name', Title: 'T??n h??ng s???n xu???t' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Ch???n m?? h??ng s???n xu???t'
      },
      {
        Name: 'Lo???i v???t t??',
        FieldName: 'MaterialType',
        Placeholder: 'Lo???i v???t t??',
        Type: 'select',
        Data: this.constant.MaterialType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'H??nh ???nh',
        FieldName: 'ImagePath',
        Placeholder: 'H??nh ???nh',
        Type: 'select',
        Data: this.constant.MaterialImg,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Gi?? v???t t??',
        FieldName: 'Pricing',
        FieldNameType: 'MaterialPriceType',
        Placeholder: 'Nh???p gi?? v???t t?? ...',
        Type: 'number'
      },
      {
        Name: 'Gi?? l???ch s???',
        FieldName: 'HistoryPrice',
        FieldNameType: 'MaterialHistoryPriceType',
        Placeholder: 'Nh???p gi?? v???t t?? ...',
        Type: 'number'
      },
      {
        Name: 'Th???i gian ?????t h??ng',
        FieldName: 'DeliveryDays',
        FieldNameType: 'DeliveryDaysType',
        Placeholder: 'Nh???p th???i gian ?????t h??ng ...',
        Type: 'number'
      },
      {
        Name: 'Th???i gian mua g???n nh???t',
        FieldName: 'LastDelivery',
        FieldNameType: 'LastDeliveryType',
        Placeholder: 'Nh???p th???i gian mua g???n nh???t ...',
        Type: 'number'
      },
      {
        Name: 'T??nh tr???ng v???t t??',
        FieldName: 'Status',
        Placeholder: 'T??nh tr???ng v???t t??',
        Type: 'select',
        Data: this.constant.MaterialStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'V???t t?? th???a',
        FieldName: 'RedundantStatus',
        Placeholder: 'V???t t?? th???a',
        Type: 'select',
        Data: this.constant.MaterialRedundantStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'V???t li???u',
        FieldName: 'RawMaterialName',
        Placeholder: 'Nh???p v???t li???u',
        Type: 'text'
      },
      {
        Name: 'Lo???i v???t t?? c?? kh??',
        FieldName: 'MechanicalType',
        Placeholder: 'Nh???p lo???i v???t t?? c?? kh??',
        Type: 'text'
      },
      {
        Name: 'T??nh tr???ng t??i li???u 3d',
        FieldName: 'Status3D',
        Placeholder: 'T??nh tr???ng t??i li???u 3d',
        Type: 'select',
        Data: this.constant.MaterialFile3D,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'T??nh tr???ng t??i li???u Datasheet',
        FieldName: 'StatusDatasheed',
        Placeholder: 'T??nh tr???ng t??i li???u Datasheet',
        Type: 'select',
        Data: this.constant.MaterialFileDatasheet,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tr???ng th??i t??i li???u',
        FieldName: 'IsAllFile',
        Placeholder: 'Tr???ng th??i t??i li???u',
        Type: 'select',
        Data: this.constant.MaterialSatusIsAllFile,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Chuy???n th?? vi???n',
        FieldName: 'IsSendSale',
        Placeholder: 'Ch???n t??nh tr???ng chuy???n th?? vi???n',
        Type: 'select',
        Data: this.constant.SendSale,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  importModel: any = {
    Path: ''
  }

  materialGroupId: '';
  selectedMaterialGroupId = '';
  height = 0;
  private onDownloadMaterial = new BroadcastEventListener<any>('downloadFileMaterialDocument3Ds');
  private downloadSub: Subscription;
  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "V???t t??";
    this.searchMaterialGroup();
    this.searchMaterial(this.materialGroupId);
    this.getCbbManufacture();
    this.selectedMaterialGroupId = localStorage.getItem("selectedMaterialGroupId");
    localStorage.removeItem("selectedMaterialGroupId");
    this.signalRService.listen(this.onDownloadMaterial, true);
    this.downloadSub = this.onDownloadMaterial.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 2) {
          this.messageService.showMessage(data.Message);
        }
        else {
              var link = document.createElement('a');
              link.setAttribute("type", "hidden");
              link.href = this.config.ServerApi + data.Data;
              link.download = 'Download.docx';
              document.body.appendChild(link);
              // link.focus();
              link.click();
              document.body.removeChild(link);
          this.messageService.showMessage('Download th??nh c??ng!');
        }
      }
      // this.blockUI.stop();
    });
  }

  itemClick(e) {
    if (this.materialGroupId == '' || this.materialGroupId == null) {
      this.messageService.showMessage("????y kh??ng ph???i nh??m v???t t??!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdate(this.materialGroupId, false)
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdate(this.materialGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteMaterialGroup(this.materialGroupId);
      }
    }
  }

  selectIndex = -1;
  loadValue(param, index) {
    this.selectIndex = index;
  }

  clear() {
    this.modelMaterial = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      Date: null,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      UnitId: '',
      MaterialGroupId: '',
      ManufactureId: '',
      Code: '',
      Name: '',
      Note: '',
      Pricing: null,
      DeliveryDays: null,
      ImagePath: '',
      ThumbnailPath: '',
      LastDeliveryDate: '',
      IsUsuallyUse: '',
      IsAllFile: '',
      MaterialType: '',
      RawMaterialId: '',
      MaterialDesign3DId: '',
      MaterialDataSheetId: '',
      Is3D: '',
      IsDataSheet: '',
      ManufactureName: '',
      UnitName: '',
      RawMaterialName: '',
      MaterialGroupName: '',
      LastDelivery: null,
      MechanicalType: '',
      Status: '',
      IsDocument3D: '',
      IsDocumentDataSheet: '',
      MaterialPriceType: 1,
      IsSendSale: null,
    }
    this.manufactureIds = [];
    this.selectedMaterialGroupId = '';
    //this.selectedMaterialGroupId = localStorage.getItem("selectedMaterialGroupId");
    this.searchMaterialGroup();
    this.searchMaterial(this.selectedMaterialGroupId);
  }

  searchMaterialGroup() {
    this.materialGroupService.searchMaterialGroup(this.modelMaterialGroup).subscribe((data: any) => {
      if (data.ListResult) {
        this.listMaterialGroup = data.ListResult;
        this.listMaterialGroup.unshift(this.modelAll);
        this.modelMaterialGroup.totalItems = data.TotalItem;
        if (this.selectedMaterialGroupId == null) {
          this.selectedMaterialGroupId = this.listMaterialGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedMaterialGroupId];
        for (var item of this.listMaterialGroup) {
          this.listMaterialGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchMaterial(materialGroupId: string) {
    this.modelMaterial.MaterialGroupId = materialGroupId;
    this.materialService.searchMaterial(this.modelMaterial).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.modelMaterial.PageNumber - 1) * this.modelMaterial.PageSize + 1);
        this.listMaterial = data.ListResult;
        if (this.checkeds) {
          this.listMaterial.forEach(element => {
            element.Checked = true;
          });
        }
        this.modelMaterial.totalItems = data.TotalItem;
        this.modelMaterial.TotalItemExten = data.TotalItemExten;
        this.modelMaterial.TotalNoFile = data.TotalNoFile;
        this.modelMaterial.Date = data.Date;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCbbManufacture() {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      this.listManufacture = data;
    });
  }

  showCreate() {
    localStorage.setItem("selectedMaterialGroupId", this.selectedMaterialGroupId);
    this.groupId = this.selectedMaterialGroupId;
    this.router.navigate(['vat-tu/quan-ly-vat-tu/them-moi']);
  }

  showUpdate(Id: string) {
    localStorage.setItem("selectedMaterialGroupId", this.selectedMaterialGroupId);
    this.router.navigate(['vat-tu/quan-ly-vat-tu/chinh-sua/', Id]);

  }

  showConfirmDelete(Id: string) {
    localStorage.setItem("selectedMaterialGroupId", this.selectedMaterialGroupId);
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? v???t t?? n??y kh??ng?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.materialService.deleteMaterial({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.selectedMaterialGroupId = localStorage.getItem("selectedMaterialGroupId");
        this.searchMaterial(this.selectedMaterialGroupId);
        this.messageService.showSuccess('X??a v???t t?? th??nh c??ng!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showImportPopup() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.materialService.importFile(data).subscribe(
          data => {
            this.searchMaterial(this.materialGroupId);
            this.messageService.showSuccess('Import l???ch s??? gi?? v???t t?? th??nh c??ng!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  showBuyHistory(Id) {
    let activeModal = this.modalService.open(MaterialBuyHistoryModalComponent, { container: 'body', windowClass: 'buy-history-modal', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedMaterialGroupId) {
      this.selectedMaterialGroupId = e.selectedRowKeys[0];
      this.searchMaterial(e.selectedRowKeys[0]);
      this.materialGroupId = e.selectedRowKeys[0];
    }

  }

  downloadTemplate() {
    this.materialService.getGroupInTemplate(this.importModel).subscribe(d => {
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

  showImportMaterialPopup() {
    this.materialService.getGroupInTemplate(this.importModel).subscribe(d => {
      this.fileTemplateMaterial = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplateMaterial, false).subscribe(data => {
        if (data) {
          this.materialService.importFileMaterial(data).subscribe(
            data => {
              this.searchMaterial(this.materialGroupId);
              this.messageService.showSuccess('Import v???t t?? th??nh c??ng!');
            },
            error => {
              this.messageService.showError(error);
            });
        }
      });
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportExcel() {
    this.materialService.exportExcel(this.modelMaterial).subscribe(d => {
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

  // popup th??m m???i v?? ch???nh s???a
  showCreateUpdate(Id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(MaterialgroupCreateComponent, { container: 'body', windowClass: 'materialgroup-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.Id = Id;
    } else {
      if (Id) {
        activeModal.componentInstance.parentId = Id;
      } else {
        activeModal.componentInstance.parentId = this.materialGroupId;
      }

    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchMaterialGroup();
      }
    }, (reason) => {
    });
  }

  deleteMaterialGroup(Id: string) {
    this.materialGroupService.deleteMaterialGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        //this.check=true;
        this.modelMaterialGroup.Id = '';
        this.searchMaterialGroup();
        this.messageService.showSuccess('X??a nh??m nh??m v???t t?? th??nh c??ng!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  //xo?? nh??m v???t t??
  listDAById: any[] = [];
  searchMaterialGroupById(Id: string) {
    this.materialGroupService.searchMaterialGroup(this.modelMaterialGroup).subscribe((data: any) => {
      if (data) {
        this.listDAById = data;
        this.modelMaterialGroup.TotalItems = data.TotalItem;
        if (this.listDAById.length == 1) {
          this.deleteMaterialGroup(Id);
        } else {
          this.messageService.showConfirm("X??a nh??m v???t t?? n??y s??? x??a h???t c??? c??c nh??m v???t t?? con thu???c nh??m, B???n c?? ch???c ch???n mu???n x??a kh??ng?").then(
            data => {
              this.deleteMaterialGroup(Id);
            },
            error => {

            }
          );
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteMaterialGroup(Id: string) {
    this.messageService.showConfirm("B???n c?? ch???c ch???n mu???n x??a nh??m v???t t?? n??y kh??ng?").then(
      data => {
        this.modelMaterialGroup.Id = Id;
        this.searchMaterialGroupById(Id);
      }
    );
  }

  isDropDownBoxOpeneds = false;
  closeDropDownBoxManufacture() {
    this.isDropDownBoxOpeneds = false;
  }

  change() {
    if (this.manufactureIds != null) {
      this.modelMaterial.ManufactureId = this.manufactureIds[0];
    } else {
      this.modelMaterial.ManufactureId = "";
    }
    this.closeDropDownBoxManufacture();
    this.searchMaterial(this.materialGroupId);
  }

  fileImport: any;
  showImportSyncSaleMaterial() {
    this.componentService.showImportExcel(this.fileTemplateSync, false).subscribe(data => {
      if (data) {
        this.fileImport = data;
        this.materialService.importSyncSaleMaterial(data, this.isConfirm).subscribe(
          data => {
            if (data) {
              this.importConfirm(data);
            } else {
              this.searchMaterial(this.materialGroupId);
              this.messageService.showSuccess('?????ng b??? s???n ph???m kinh doanh th??nh c??ng!');
            }
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  importConfirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.materialService.importSyncSaleMaterial(this.fileImport, true,).subscribe(
          data => {
            this.searchMaterial(this.materialGroupId);
            this.messageService.showSuccess('?????ng b??? s???n ph???m kinh doanh th??nh c??ng!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    );
  }

  checkeds: boolean = false;
  listCheck: any[] = [];
  selectAllFunction() {
    this.listMaterial.forEach(element => {
      element.Checked = this.checkeds;
    });
  }

  pushChecker(row: any) {
    if (row.Checked) {
      this.listCheck.push(row);
    } else {
      this.checkeds = false;
      this.listCheck.splice(this.listCheck.indexOf(row), 1);
    }
  }

  isConfirm: boolean = false;
  syncSaleMaterial(isConfirm: boolean) {
    let list = [];
    this.listCheck.forEach(element => {
      list.push(element.Id);
    });
    this.materialService.syncSaleMaterial(this.checkeds, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.searchMaterial(this.materialGroupId);
          this.messageService.showSuccess('?????ng b??? s???n ph???m kinh doanh th??nh c??ng!');
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  confirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.syncSaleMaterial(true);
      }
    );
  }

  searchGoogle(row) {   
    let content = row.Code + '+' + row.ManufactureCode;
    window.open("http://google.com/search?q=" + content + '&tbm=isch', "_blank");
  }

  showImportExcel(){
    // this.materialService.getGroupMaterialCodeInTemplate().subscribe(d => {
      this.fileTemplateMaterial = this.config.ServerApi + "Template/MaVatTu_Template.xlsx";;
      this.componentService.showImportExcel(this.fileTemplateMaterial, false).subscribe(data => {
        if (data) {
          this.materialService.importFileMaterialCode(data).subscribe(
            data => {
              this.downloadDocument3D(data);
            },
            error => {
              this.messageService.showError(error);
            });
        }
      });
    // }, e => {
    //   this.messageService.showError(e);
    // });
  }
  downloadDocument3D(data) {
        this.downloadModel.DownloadPath = "D:\/";
        this.downloadModel.material = data;
        this.downloadModel.ApiUrl = this.config.ServerApi;
        this.downloadModel.ApiFileUrl = this.config.ServerFileApi;
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser && currentUser.access_token) {
          this.downloadModel.Token = currentUser.access_token;
        }
        this.signalRService.invoke('DownloadMaterialDocument3Ds', this.downloadModel).subscribe(data => {
          if (data) {
            // this.blockUI.start('');
          }
          else {
            this.messageService.showMessage("Kh??ng k???t n???i ???????c service");
          }
        });
  }
  ngOnDestroy() {
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownloadMaterial);
  }
}
