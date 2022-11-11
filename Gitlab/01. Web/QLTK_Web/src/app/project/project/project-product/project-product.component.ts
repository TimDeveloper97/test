import { Component, OnInit, Input, ViewChild, ViewEncapsulation } from '@angular/core';

import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MessageService, DateUtils, ComboboxService, Configuration, Constants, ComponentService, PermissionService, FileProcess } from 'src/app/shared';
import { ProjectProductService } from '../../service/project-product.service';
import { ProjectProductCreateComponent } from '../project-product-create/project-product-create.component';
import { DxTreeListComponent } from 'devextreme-angular';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectGeneralDesignCreateComponent } from '../project-general-design-create/project-general-design-create.component';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { ProjectProductBomService } from '../../service/project-product-bom.service';
import { ProjectProductBomCreateComponent } from '../project-product-bom-create/project-product-bom-create.component';
import { DownloadService } from 'src/app/shared/services/download.service';
import { Router } from '@angular/router';
import { QcCheckListCreateComponent } from '../qc-check-list-create/qc-check-list-create.component';
import { CopyQcCheckListComponent } from '../copy-qc-check-list/copy-qc-check-list.component';
import { ShowQcCheckListComponent } from '../show-qc-check-list/show-qc-check-list.component';
@Component({
  selector: 'app-project-product',
  templateUrl: './project-product.component.html',
  styleUrls: ['./project-product.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectProductComponent implements OnInit {
  private onDownloadProductProject = new BroadcastEventListener<any>('downloadProductDocuments');
  private downloadSub: Subscription;

  @Input() Id: string;
  @Input() PriceFCM: number;
  @ViewChild(DxTreeListComponent) treeView;
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private messageService: MessageService,
    private service: ProjectProductService,
    private serviceProjectGaneralDesign: ProjectGeneralDesignService,
    private combobox: ComboboxService,
    private config: Configuration,
    public dateUtils: DateUtils,
    public constants: Constants,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private componentService: ComponentService,
    public permissionService: PermissionService,
    private signalRService: SignalRService,
    private serviceBom: ProjectProductBomService,
    private dowloadservice: DownloadService,
    private router: Router,
    public fileProcess: FileProcess,
  ) {
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times text-danger' },
      { Id: 3, text: 'Lập tổng hợp thiết kế', icon: 'fas fa-file-excel text-success' },
    ];
  }

  totalParent: 0;
  totalAmount: number = 0;
  totalAmountTHTK: number = 0;
  totalAmountIncurred: number = 0;
  colorTHTK: boolean = false;
  isNeedQC: boolean = false;
  isDelete: boolean = false;
  items: any = [];
  startIndex = 0;
  listDA: any[] = [];
  projectProductId: '';
  selectedProjectProductId = '';
  designFinishDate: any;
  makeFinishDate: any;
  deliveryDate: any;
  transferDate: any;
  expectedDesignFinishDate: any;
  expectedMakeFinishDate: any;
  expectedTransferDate: any;
  listProductGroupId: any[] = [];
  showIframeHider = false;
  listProjectProduct: any[] = [];
  listProjectProductId: any[] = [];
  listModule: any[] = [];
  listModuleId: any[] = [];
  listProduct: any[] = [];
  listProductId: any[] = [];
  listDAById: any[] = [];
  listCheck: any[] = [];
  columnProjectProduct: any[] = [{ Name: 'Code', Title: 'Mã sản phẩm' }, { Name: 'Name', Title: 'Tên sản phẩm' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã theo thiết kế' }, { Name: 'Name', Title: 'Tên theo thiết kế' }];
  fileTemplate = this.config.ServerApi + 'Template/ImportSanPham_Template.xls';
  listDataType: any[] = [
    { Id: 1, Name: 'Bài thực hành/công đoạn' },
    { Id: 2, Name: 'Sản phẩm/Lai sản xuất' },
    { Id: 3, Name: 'Mô hình/máy' },
    { Id: 4, Name: 'Module' },
  ];
  listModuleStatus: any[] = [
    { Id: 1, Name: 'Dự án' },
    { Id: 2, Name: 'Bổ sung' },
  ];
  listDesignStatus: any[] = [
    { Id: 1, Name: 'Thiết kế mới' },
    { Id: 2, Name: 'Sửa thiết kế cũ' },
    { Id: 3, Name: 'Tận dụng' },
    { Id: 4, Name: 'Hàng bán thẳng' }
  ];
  listPublications: any[] = [
    { Id: 1, Name: "Ảnh", Checked: false },
    { Id: 2, Name: "Video", Checked: false },
    { Id: 3, Name: "Catalog", Checked: false },
    { Id: 4, Name: "Web", Checked: false },
    { Id: 5, Name: "Kênh online khác", Checked: false },
  ]

  listProductSelectable=[];

  model: any = {
    Id: '',
    ProjectId: '',
    ContractCode: '',
    ContractName: '',
    Code: '',
    Name: '',
    DataType: 0,
    ModuleStatus: 0,
    DesignStatus: 0,
    CostType: null,
    MaterialName:null
  }

  checkList: any[]=[];


  sizes = {
    percent: {
      area1: 30,
      area2: 70,
    },
    pixel: {
      area1: 120,
      area2: '*',
      area3: 160,
    },
  }

  downloadModel: any = {
    DownloadPath: '',
    ProjectId: '',
    ApiUrl: '',
    Token: '',
    ApiFileUrl: ''
  }

  searchOptions: any = {
    FieldContentName: 'ContractCode',
    Placeholder: 'Nhập mã theo hợp đồng',
    Items: [
      {
        Name: 'Tên theo hợp đồng',
        FieldName: 'ContractName',
        Placeholder: 'Nhập tên nguồn',
        Type: 'text'
      },
      {
        Name: 'Mã theo thiết kế',
        FieldName: 'Code',
        Placeholder: 'Nhập mã theo thiết kế',
        Type: 'text'
      },
      {
        Name: 'Tên theo thiết kế',
        FieldName: 'Name',
        Placeholder: 'Nhập tên theo thiết kế',
        Type: 'text'
      },
      {
        Name: 'Mã/tên vật tư',
        FieldName: 'MaterialName',
        Placeholder: 'Nhập tên/mã vật tư',
        Type: 'text'
      },
      {
        Name: 'Kiểu sản phẩm',
        FieldName: 'DataType',
        Placeholder: 'Kiểu sản phẩm',
        Type: 'select',
        Data: this.constants.ListDataType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng module',
        FieldName: 'ModuleStatus',
        Placeholder: 'Tình trạng module',
        Type: 'select',
        Data: this.constants.ListModuleStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng thiết kế',
        FieldName: 'DesignStatus',
        Placeholder: 'Tình trạng thiết kế',
        Type: 'select',
        Data: this.constants.ListDesignStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng module',
        FieldName: 'ModuleStatus',
        Placeholder: 'Tình trạng module',
        Type: 'select',
        Data: this.constants.ListModuleStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  projectProductModel: any = {
    Id: '',
    ProjectId: '',
    ParentId: null,
    ModuleId: '',
    ProductId: '',
    ContractCode: '',
    ContractName: '',
    Specifications: '',
    DataType: 0,
    ModuleStatus: '',
    DesignStatus: 0,
    DesignFinishDate: null,
    MakeFinishDate: null,
    DeliveryDate: null,
    TransferDate: null,
    ExpectedDesignFinishDate: '',
    ExpectedMakeFinishDate: '',
    ExpectedTransferDate: '',
    Note: '',
    Quantity: 1,
    Price: 0,
    ContractIndex: '',
    ProductName: '',
    IsGeneralDesign: false,
    IsCantalogs: false,
    IsMarketingDevice: false,
    IsMarketingMaintenance: false,
    IsMarketingPractice: false,
    DesignWorkStatus: false,
    DesignCloseDate: '',
    GeneralDesignLastDate: '',
    ListPublications: [],
    QCQuantity: 0,
  }
  active = 'app-product-tab-document-attach';

  checkModel: any = {
    MarketingCatalogs: false,
    MarketingPractice: false,
    MarketingDevice: false,
    MarketingMaintenance: false,
  }

  activeTabId = 1;

  designCloseDate: any = null;
  generalDesignLastDate: any = null;

  listData: any[] = [];

  modelData: any = {
    Id: '',
    ModuleId: '',
    ProjectProductId: '',
    Version: 1,
  }

  modelDowload: any = {
    Name: '',
    ListDatashet: []
  }
  projectId : string;

  ngOnInit() {
    this.model.ProjectId = this.Id;
    this.projectProductModel.ProjectId = this.Id;
    this.searchProjectProduct();
    this.getListProjectProduct();
    this.getListModule();
    this.getListProduct();
    this.selectedProjectProductId = localStorage.getItem("selectedProjectProductId");
    localStorage.removeItem("selectedProjectProductId");

    this.signalRService.listen(this.onDownloadProductProject, true);
    this.downloadSub = this.onDownloadProductProject.subscribe((data: any) => {
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

  searchProjectProduct() {
    this.service.searchProjectProduct(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        //this.startIndex = ((this.projectProductModel.PageNumber - 1) * this.projectProductModel.PageSize + 1);
        this.listDA = data.ListResult;
        this.totalParent = data.Total;
        this.totalAmount = data.ToTalAmount;
        this.totalAmountTHTK = data.ToTalAmountTHTK;
        this.totalAmountIncurred = data.TotalAmountIncurred;
        this.colorTHTK = data.ColorTHTK;
        this.projectProductModel.totalItems = data.TotalItem;
        if (!this.selectedProjectProductId && this.listDA.length > 0) {
          this.selectedProjectProductId = this.listDA[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedProjectProductId];
        for (var item of this.listDA) {
          this.listProductGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.showCreateUpdate('', this.projectProductId);
    } else if (e.itemData.Id == 2) {
      this.showConfirmDeleteProjectProduct(this.selectedProjectProductId);
    } else if (e.itemData.Id == 3) {
      if (this.projectProductModel.DataType == 2 || this.projectProductModel.DataType == 3 || this.projectProductModel.DataType == 4) {
        this.showProjectGeneralDesign(this.selectedProjectProductId);
      } else {
        this.messageService.showMessage("Bạn chỉ được chọn kiểu dữ liệu là sản phẩm, module, mô hình!");
      }
    }
  }

  onSelectionChanged(e) {
    this.activeTabId = 1;
    this.checkModel = {
      MarketingCatalogs: false,
      MarketingPractice: false,
      MarketingDevice: false,
      MarketingMaintenance: false,
    }
    this.selectedProjectProductId = e.selectedRowKeys[0];
    //this.searchProjectProduct();
    this.projectProductId = e.selectedRowKeys[0];
    this.clearDate()
    this.getParentProduct();
    if (this.selectedProjectProductId) {
      this.getProjectProductInfo();
      this.modelData.ProjectProductId = this.projectProductId;
      this.projectId = this.Id;
      this.searchBOMDesignTwo();
      this.searchCheckList();
      // this.getProjectProductInfoBom();
    }
  }

  clearDate() {
    this.designFinishDate = null;
    this.makeFinishDate = null;
    this.deliveryDate = null;
    this.transferDate = null;
    this.expectedDesignFinishDate = null;
    this.expectedMakeFinishDate = null;
    this.expectedTransferDate = null;
    this.designCloseDate = null;
    this.generalDesignLastDate = null;
  }

  clear() {
    this.model = {
      Id: '',
      ProjectId: '',
      ContractCode: '',
      ContractName: '',
      Code: '',
      Name: '',
      DataType: 0,
      ModuleStatus: 0,
      DesignStatus: 0,
      CostType: null
    };
    // this.projectProductId = '';
    this.selectedProjectProductId = '';
    this.model.ProjectId = this.Id;
    this.projectProductModel.ProjectId = this.Id;
    this.searchProjectProduct();
  }

  searchProjectProductById(Id: string) {
    this.service.searchProjectProductById({ Id: Id }).subscribe((data: any) => {
      if (data) {
        this.listDAById = data;
        if (this.listDAById.length == 1) {
          this.deleteProjectProduct(Id);
        } else {
          this.messageService.showConfirm("Xóa sản phẩm này sẽ xóa hết cả các sản phẩm con thuộc nhóm, Bạn có chắc chắn muốn xóa không?").then(
            data => {
              this.deleteProjectProduct(Id);
            },
            error => {

            }
          );
        }
      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  showConfirmDeleteProjectProduct(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá sản phẩm này không?").then(
      data => {
        this.searchProjectProductById(Id);
      },
      error => {

      }
    );
  }

  deleteProjectProduct(Id: string) {
    this.service.deleteProjectProduct({ Id: Id }).subscribe(
      data => {
        this.searchProjectProduct();
        this.messageService.showSuccess('Xóa sản phẩm thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  moduleId: string;
  parentModuleId: string;
  getProjectProductInfo() {
    this.service.getProjectProductInfo({ Id: this.selectedProjectProductId }).subscribe(data => {
      this.projectProductModel = data;
      this.isNeedQC = data.IsNeedQC;
      if (data.DataType == 3 || data.DataType == 4) {
        this.moduleId = data.ModuleId;
        this.parentModuleId = null;
      } else {
        if (data.ParentId != null && data.ParentId != "" && (data.ParentDataType == 3 || data.ParentDataType == 4)) {
          this.parentModuleId = data.ParentModuleId;
          this.moduleId = null;
        }
      }

      if(data.MarketingCatalogs){
        this.checkModel.MarketingCatalogs = true;
      }
      if(data.MarketingPractice){
        this.checkModel.MarketingPractice = true;
      }
      if(data.MarketingDevice){
        this.checkModel.MarketingDevice = true;
      }
      if(data.MarketingMaintenance){
        this.checkModel.MarketingMaintenance = true;
      }


      if (data.DesignFinishDate != null) {
        let dateArray1 = data.DesignFinishDate.split('T')[0];
        let dateValue1 = dateArray1.split('-');
        let tempDateFromV1 = {
          'day': parseInt(dateValue1[2]),
          'month': parseInt(dateValue1[1]),
          'year': parseInt(dateValue1[0])
        };
        this.designFinishDate = tempDateFromV1;
      }
      if (data.MakeFinishDate != null) {
        let dateArray2 = data.MakeFinishDate.split('T')[0];
        let dateValue2 = dateArray2.split('-');
        let tempDateFromV2 = {
          'day': parseInt(dateValue2[2]),
          'month': parseInt(dateValue2[1]),
          'year': parseInt(dateValue2[0])
        };
        this.makeFinishDate = tempDateFromV2;
      }
      if (data.DeliveryDate != null) {
        let dateArray3 = data.DeliveryDate.split('T')[0];
        let dateValue3 = dateArray3.split('-');
        let tempDateFromV3 = {
          'day': parseInt(dateValue3[2]),
          'month': parseInt(dateValue3[1]),
          'year': parseInt(dateValue3[0])
        };
        this.deliveryDate = tempDateFromV3;
      }
      if (data.TransferDate != null) {
        let dateArray4 = data.TransferDate.split('T')[0];
        let dateValue4 = dateArray4.split('-');
        let tempDateFromV4 = {
          'day': parseInt(dateValue4[2]),
          'month': parseInt(dateValue4[1]),
          'year': parseInt(dateValue4[0])
        };
        this.transferDate = tempDateFromV4;
      }
      if (data.ExpectedDesignFinishDate != null) {
        let dateArray5 = data.ExpectedDesignFinishDate.split('T')[0];
        let dateValue5 = dateArray5.split('-');
        let tempDateFromV5 = {
          'day': parseInt(dateValue5[2]),
          'month': parseInt(dateValue5[1]),
          'year': parseInt(dateValue5[0])
        };
        this.expectedDesignFinishDate = tempDateFromV5;
      }
      if (data.ExpectedMakeFinishDate != null) {
        let dateArray6 = data.ExpectedMakeFinishDate.split('T')[0];
        let dateValue6 = dateArray6.split('-');
        let tempDateFromV6 = {
          'day': parseInt(dateValue6[2]),
          'month': parseInt(dateValue6[1]),
          'year': parseInt(dateValue6[0])
        };
        this.expectedMakeFinishDate = tempDateFromV6;
      }
      if (data.ExpectedTransferDate != null) {
        let dateArray7 = data.ExpectedTransferDate.split('T')[0];
        let dateValue7 = dateArray7.split('-');
        let tempDateFromV7 = {
          'day': parseInt(dateValue7[2]),
          'month': parseInt(dateValue7[1]),
          'year': parseInt(dateValue7[0])
        };
        this.expectedTransferDate = tempDateFromV7;
      }

      if (data.DesignCloseDate) {
        this.designCloseDate = this.dateUtils.convertDateToObject(data.DesignCloseDate);
      }

      if (data.GeneralDesignLastDate) {
        this.generalDesignLastDate = this.dateUtils.convertDateToObject(data.GeneralDesignLastDate);
      }

    }, error => {
      this.messageService.showError(error);
    });
  }

  getListModule() {
    this.combobox.getListModule().subscribe((data: any) => {
      this.listModule = data;
      for (var item of this.listModule) {
        this.listModuleId.push(item.Id);
      }
    });
  }

  getListProduct() {
    this.combobox.getListProduct().subscribe((data: any) => {
      this.listProduct = data;
      for (var item of this.listProduct) {
        this.listProductId.push(item.Id);
      }
    });
  }

  getListProjectProduct() {
    this.combobox.getListProjectProduct(this.Id).subscribe((data: any) => {
      if (data) {
        this.listProjectProduct = data;
        // lấy list id expandedRowKeys 
        for (var item of this.listProjectProduct) {
          this.listProjectProductId.push(item.Id);
        }
        this.getParentProduct();
      }
    });
  }
  getParentProduct(){
    this.listProductSelectable=[];
    for (var item of  this.listProjectProduct) {
      if((this.selectedProjectProductId != null ||this.selectedProjectProductId != "" )&& item.Id != this.selectedProjectProductId){
        this.listProductSelectable.push(item);
      }
    }
    
  }

  supUpdate() {
    if (this.designCloseDate) {
      this.projectProductModel.DesignCloseDate = this.dateUtils.convertObjectToDate(this.designCloseDate);
    }

    if (this.generalDesignLastDate) {
      this.projectProductModel.GeneralDesignLastDate = this.dateUtils.convertObjectToDate(this.generalDesignLastDate);
    }

    if (this.designFinishDate) {
      this.projectProductModel.DesignFinishDate = this.dateUtils.convertObjectToDate(this.designFinishDate);
    } else { this.projectProductModel.DesignFinishDate = null; }
    if (this.makeFinishDate) {
      this.projectProductModel.MakeFinishDate = this.dateUtils.convertObjectToDate(this.makeFinishDate);
    } else { this.projectProductModel.MakeFinishDate = null; }
    if (this.deliveryDate) {
      this.projectProductModel.DeliveryDate = this.dateUtils.convertObjectToDate(this.deliveryDate);
    } else { this.projectProductModel.DeliveryDate = null; }
    if (this.transferDate) {
      this.projectProductModel.TransferDate = this.dateUtils.convertObjectToDate(this.transferDate);
    } else { this.projectProductModel.TransferDate = null; }
    if (this.expectedDesignFinishDate) {
      this.projectProductModel.ExpectedDesignFinishDate = this.dateUtils.convertObjectToDate(this.expectedDesignFinishDate);
    } else { this.projectProductModel.ExpectedDesignFinishDate = null; }
    if (this.expectedMakeFinishDate) {
      this.projectProductModel.ExpectedMakeFinishDate = this.dateUtils.convertObjectToDate(this.expectedMakeFinishDate);
    } else { this.projectProductModel.ExpectedMakeFinishDate = null; }
    if (this.expectedTransferDate) {
      this.projectProductModel.ExpectedTransferDate = this.dateUtils.convertObjectToDate(this.expectedTransferDate);
    } else { this.projectProductModel.ExpectedTransferDate = null; }
    this.projectProductModel.ListPublications = this.checkModel;
    this.service.updateProjectProduct(this.projectProductModel).subscribe(
      data => {
        this.searchProjectProduct();
        this.messageService.showSuccess('Cập nhật sản phẩm thành công!');
        this.getListProjectProduct();
        this.isDelete = data;
        if(this.isDelete){
          this.messageService.showConfirm("Số lượng thực nhỏ hơn số lượng đã QC xác nhận xóa toàn bộ Qc đã thực hiện?").then(
            data => {
              this.deleteAllHasQc();
            },
            error => {
    
            }
          );

        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  deleteAllHasQc(){
    this.service.deleteAllHasQc(this.projectProductModel.Id).subscribe(
      data => {
        this.messageService.showSuccess('Tạo mới sản phẩm Qc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  //cập nhật nhóm module
  updateProjectProduct() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.ContractCode);
    if (validCode) {
      this.supUpdate();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supUpdate();
        },
        error => {

        }
      );
    }
  }

  save(isContinue: boolean) {
    if (this.projectProductModel.Id) {
     
      this.updateProjectProduct();
      
    }
  }

  exportExcel() {
    this.service.compareContract(this.model).subscribe(d => {
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

  showProjectGeneralDesign(Id: string) {
    if (Id) {
      this.serviceProjectGaneralDesign.checkApproveStatus(Id).subscribe(
        data => {
          if (data) {
            this.show(Id);
          } else {
            this.messageService.showMessage("Bạn chưa phê duyệt lân tổng hợp gần nhất")
          }
        },
        error => {
          this.messageService.showError(error);
        });
    } else {
      this.show(Id);
    }
  }

  show(Id: string) {
    let activeModal = this.modalService.open(ProjectGeneralDesignCreateComponent, { container: 'body', windowClass: 'project-ganeral-design-create', backdrop: 'static' })
    activeModal.componentInstance.Id = '';
    activeModal.componentInstance.projectProductId = Id;
    activeModal.componentInstance.projectId = this.model.ProjectId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectProduct();
      }
    }, (reason) => {
    });
  }

  showCreateUpdate(Id: string, projectProductPrentId: string) {
    let activeModal = this.modalService.open(ProjectProductCreateComponent, { container: 'body', windowClass: 'project-product-create', backdrop: 'static' })
    activeModal.componentInstance.idUpdate = Id;
    activeModal.componentInstance.projectId = this.Id;
    activeModal.componentInstance.projectProductPrentId = projectProductPrentId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectProduct();
      }
    }, (reason) => {
    });
  }

  Link: string;

  importProduct() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.service.importProduct(data, this.Id).subscribe(
          data => {
            this.searchProjectProduct();
            this.messageService.showSuccess('Import danh sách sản phẩm thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  downloadDocument() {
    this.componentService.showChooseFolder(0, 0, '').subscribe(result => {
      if (result) {
        this.downloadModel.DownloadPath = result;
        this.downloadModel.ProjectId = this.model.ProjectId;
        this.downloadModel.ApiUrl = this.config.ServerApi;
        this.downloadModel.ApiFileUrl = this.config.ServerFileApi;
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser && currentUser.access_token) {
          this.downloadModel.Token = currentUser.access_token;
        }

        this.signalRService.invoke('DownloadProductDocuments', this.downloadModel).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }
    }, (reason) => {

    });
  }

  onRowUpdated(e) {
    console.log(e);

    this.service.updateIsGeneralDesign(e.data).subscribe(
      data => {
        this.messageService.showSuccess('Thay đổi tình trạng THTK thành công!');

        if (this.selectedProjectProductId == e.data.Id) {
          this.getProjectProductInfo();
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }


  //add new tab
  searchBOMDesignTwo() {
    this.serviceBom.searchBOMDesignTwo(this.modelData).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá BOM này không?").then(
      data => {
        this.deleteBOMDesignTwo(Id);
      },
      error => {
        
      }
    );
  }

  deleteBOMDesignTwo(Id: string) {
    this.serviceBom.deleteBOMDesignTwo({ Id: Id ,ModuleId :this.projectProductModel.ModuleId}).subscribe(
      data => {
        this.searchBOMDesignTwo();
        this.messageService.showSuccess('Xóa BOM thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcelBOM(Id: string) {
    this.modelData.Id = Id;
    this.serviceBom.exportExcel(this.modelData).subscribe(d => {
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

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({
        Path: element.Path,
        FileName: element.FileName
      });
    });
    this.modelDowload.Name = "BOMTK2";
    this.modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(this.modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showCreate() {
    let activeModal = this.modalService.open(ProjectProductBomCreateComponent, { container: 'body', windowClass: 'project-product-bom-create', backdrop: 'static' })
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.componentInstance.moduleId = this.projectProductModel.ModuleId;
    activeModal.componentInstance.listDA = this.listDA;
    activeModal.componentInstance.projectId = this.projectId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchBOMDesignTwo();
        this.searchProjectProduct();
      }
    }, (reason) => {
    });
  }

  getProjectProductInfoBom() {
    var projectProduct ={
      Id : this.projectProductId
    }
    this.service.getProjectProductInfo(projectProduct).subscribe(data => {
      this.modelData.ModuleId = data.ModuleId;
    }, error => {
      this.messageService.showError(error);
    });
  }

  searchCheckList(){
    this.service.searchCheckList(this.modelData.ProjectProductId).subscribe((data: any) => {
      if (data.ListResult) {
        this.checkList = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateQCChecklist(){
    let activeModal = this.modalService.open(QcCheckListCreateComponent, { container: 'body', windowClass: 'qc-check-list-create', backdrop: 'static' })
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.componentInstance.projectId = this.projectId;
    activeModal.componentInstance.isNeedQC = this.projectProductModel.IsNeedQC;
    activeModal.result.then((result) => {
      if (result) {
        this.searchBOMDesignTwo();
        this.searchProjectProduct();
        this.searchCheckList();
      }
    }, (reason) => {
    });
  }

  showDelete(id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tiêu chí này không?").then(
      data => {
        this.delete(id);
      },
      error => {

      }
    );
  }

  delete(id) {
    this.service.deleteQCChecklist(id).subscribe(
      data => {
        this.searchCheckList();
        this.messageService.showSuccess('Xóa Ứng tuyển thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  CopyCreateQCChecklist(){
    let activeModal = this.modalService.open(CopyQcCheckListComponent, { container: 'body', windowClass: 'copy-qc-check-list', backdrop: 'static' })
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.componentInstance.projectId = this.projectId;
    activeModal.componentInstance.check = this.listCheck;
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
    });
  }

  SelectStandard(){

    this.model.ProjectId = this.projectId;
    this.model.ProjectProductId = this.projectProductId;
    this.service.SelectStandard(this.model).subscribe(
      data => {
        this.searchCheckList();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowQCStandards(Id: string) {
    let activeModal = this.modalService.open(ShowQcCheckListComponent, { container: 'body', windowClass: 'show-qc-check-list', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        
      }
    }, (reason) => {
    });
  }

  getQCQuantity(){
    if(this.projectProductModel.IsNeedQC){
      this.projectProductModel.QCQuantity = this.projectProductModel.RealQuantity;
    }
  }

  exportExcelQC() {
    this.service.ExportExcelQC(this.model).subscribe(d => {
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
  checkeds: boolean = false;

  selectAllFunction() {
    this.checkList.forEach(element => {
      element.Checked = this.checkeds;
      if (this.checkeds) {
        this.listCheck.push(element)
      }
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

}
