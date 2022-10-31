import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectProductService } from '../../service/project-product.service';

@Component({
  selector: 'app-project-product-create',
  templateUrl: './project-product-create.component.html',
  styleUrls: ['./project-product-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectProductCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private serviceProjectProduct: ProjectProductService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
  ) { }

  modalInfo = {
    Title: 'Thêm mới sản phẩm',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  model: any = {
    Id: '',
    ProjectId: '',
    ParentId: null,
    ModuleId: '',
    ProductId: '',
    ContractCode: '',
    ContractName: '',
    Specifications: '',
    DataType: '',
    ModuleStatus: '',
    DesignStatus: '',
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
    DesignWorkStatus: false,
    DesignCloseDate: '',
    GeneralDesignLastDate: ''
  }
  idUpdate: string;
  projectId: string;
  projectProductPrentId: string;
  designFinishDate: any;
  makeFinishDate: any;
  deliveryDate: any;
  transferDate: any;
  expectedDesignFinishDate: any;
  expectedMakeFinishDate: any;
  expectedTransferDate: any;
  listProjectProduct: any[] = [];
  listProjectProductId: any[] = [];
  listModule: any[] = [];
  listModuleId: any[] = [];
  listProduct: any[] = [];
  listProductId: any[] = [];
  listData: any = [];
  columnProjectProduct: any[] = [{ Name: 'Code', Title: 'Mã sản phẩm' }, { Name: 'Name', Title: 'Tên sản phẩm' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã theo thiết kế' }, { Name: 'Name', Title: 'Tên theo thiết kế' }];
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
  ]

  designWorkStatus: boolean = false;
  designCloseDate: any = null;
  generalDesignLastDate: any = null;

  ngOnInit() {
    this.model.Id = this.idUpdate;
    this.model.ProjectId = this.projectId;
    if (this.projectProductPrentId) {
      this.model.ParentId = this.projectProductPrentId;
    }
    this.searchProjectProduct();
    this.getListModule();
    this.getListProduct();
    if (this.idUpdate) {
      this.modalInfo.Title = 'Chỉnh sửa sản phẩm';
      this.modalInfo.SaveText = 'Lưu';
    }
    else {
      this.modalInfo.Title = "Thêm mới sản phẩm";
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {

    if (this.designCloseDate) {
      this.model.DesignCloseDate = this.dateUtils.convertObjectToDate(this.designCloseDate);
    }

    if (this.generalDesignLastDate) {
      this.model.GeneralDesignLastDate = this.dateUtils.convertObjectToDate(this.generalDesignLastDate);
    }

    if (this.idUpdate) {
      this.updateProjectProduct();
    }
    else {
      this.createProjectProduct(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    if (this.designFinishDate) {
      this.model.DesignFinishDate = this.dateUtils.convertObjectToDate(this.designFinishDate);
    }
    if (this.makeFinishDate) {
      this.model.MakeFinishDate = this.dateUtils.convertObjectToDate(this.makeFinishDate);
    }
    if (this.deliveryDate) {
      this.model.DeliveryDate = this.dateUtils.convertObjectToDate(this.deliveryDate);
    }
    if (this.transferDate) {
      this.model.TransferDate = this.dateUtils.convertObjectToDate(this.transferDate);
    }
    if (this.expectedDesignFinishDate) {
      this.model.ExpectedDesignFinishDate = this.dateUtils.convertObjectToDate(this.expectedDesignFinishDate);
    }
    if (this.expectedMakeFinishDate) {
      this.model.ExpectedMakeFinishDate = this.dateUtils.convertObjectToDate(this.expectedMakeFinishDate);
    }
    if (this.expectedTransferDate) {
      this.model.ExpectedTransferDate = this.dateUtils.convertObjectToDate(this.expectedTransferDate);
    }
    this.serviceProjectProduct.createProjectProduct(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          var parentId = this.model.ParentId;
          this.model = {
            Id: '',
            ProjectId: '',
            ParentId: null,
            ModuleId: '',
            ProductId: '',
            ContractCode: '',
            ContractName: '',
            Specifications: '',
            DataType: '',
            ModuleStatus: '',
            DesignStatus: '',
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
            DesignWorkStatus: false,
            DesignCloseDate: '',
            GeneralDesignLastDate: ''
          };
          this.model.ParentId = parentId;
          this.model.ProjectId = this.projectId;
          this.designFinishDate = null;
          this.makeFinishDate = null;
          this.deliveryDate = null;
          this.transferDate = null;
          this.expectedDesignFinishDate = null;
          this.expectedMakeFinishDate = null;
          this.expectedTransferDate = null;
          this.searchProjectProduct();
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //Thêm mới
  createProjectProduct(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.ContractCode);
    if (validCode) {
      this.supCreate(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supCreate(isContinue);
        },
        error => {

        }
      );
    }
  }

  supUpdate() {
    if (this.model.DesignFinishDate != null && this.model.DesignFinishDate != '') {
      this.model.DesignFinishDate = this.dateUtils.convertObjectToDate(this.model.DesignFinishDate);
    }
    if (this.model.MakeFinishDate != null && this.model.MakeFinishDate != '') {
      this.model.MakeFinishDate = this.dateUtils.convertObjectToDate(this.model.MakeFinishDate);
    }
    if (this.model.DeliveryDate != null && this.model.DeliveryDate != '') {
      this.model.DeliveryDate = this.dateUtils.convertObjectToDate(this.model.DeliveryDate);
    }
    if (this.model.TransferDate != null && this.model.TransferDate != '') {
      this.model.TransferDate = this.dateUtils.convertObjectToDate(this.model.TransferDate);
    }
    this.serviceProjectProduct.updateProjectProduct(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật sản phẩm thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  searchProjectProduct() {
    this.combobox.getListProjectProduct(this.projectId).subscribe((data: any) => {
      if (data) {
        this.listProjectProduct = data;
        // lấy list id expandedRowKeys 
        for (var item of this.listProjectProduct) {
          this.listProjectProductId.push(item.Id);
        }
      }
    });
  }

}
