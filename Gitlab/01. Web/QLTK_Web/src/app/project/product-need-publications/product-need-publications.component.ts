import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AppSetting, Configuration, MessageService, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { ProjectServiceService } from '../service/project-service.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowProjectComponent } from '../project/show-project/show-project.component'

@Component({
  selector: 'app-product-need-publications',
  templateUrl: './product-need-publications.component.html',
  styleUrls: ['./product-need-publications.component.scss']
})
export class ProductNeedPublicationsComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    private combobox: ComboboxService,
    private messageService: MessageService,
    private projectService: ProjectServiceService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private route: ActivatedRoute,
    private modalService: NgbModal,
  ) { }

  startIndex = 0;
  StartIndex = 0;
  pagination;
  selectIndex = -1;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  listData: any[] = [];
  SbuId: string;
  status1: number;
  status2: number;
  status3: number;
  sbuid = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;

  listPublications: any[] = [
    { Id: 1, Name: "Ảnh", Checked: false },
    { Id: 2, Name: "Video", Checked: false },
    { Id: 3, Name: "Catalog", Checked: false },
    { Id: 4, Name: "Web", Checked: false },
    { Id: 5, Name: "Kênh online khác", Checked: false },
  ]


  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    SBUId: '',
    DepartmentId: '',
    DateFrom: '',
    DateTo: '',
    Status: '',
    Note: '',
    search: '',
    CustomerTypeId: '',
    DateToV: '',
    DateFromV: '',
    CreateDate: '',
    Type: '',
    ProductId:'',
    ProjectErrorStatus: null
  }

  modelProduct: any = {
    PageSize: 10,
    TotalItems: 0,
    Status1: 0,
    Status2: 0,
    Date: null,
    TotalItemExten: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    ProductGroupId: '',
    Code: '',
    Name: '',
    CurentVersion: '',
    ProcedureTime: '',
    Status: null,
    IsManualExist: '',
    IsQuoteExist: '',
    IsPracticeExist: '',
    IsLayoutExist: '',
    IsMaterialExist: '',
    Pricing: '',
    IsEnought: '',
    SBUId: '',
    DepartmentId: '',
    IsSendSale: null,
    TypeCatalogs: '',
    TypeGuidePractice: '',
    TypeDMBTH: '',
    TypeGuideMaintenance: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên Thiết bị',
    Items: [

      {
        Name: 'Tình trạng tài liệu thiết bị',
        FieldName: 'Status',
        Placeholder: 'Tình trạng tài liệu thiết bị',
        Type: 'select',
        Data: this.constant.ProductStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Catalogs',
        FieldName: 'TypeCatalogs',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Hướng dẫn thực hành',
        FieldName: 'TypeGuidePractice',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Hướng dẫn sử dụng',
        FieldName: 'TypeDMBTH',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Hướng dẫn bảo trì',
        FieldName: 'TypeGuideMaintenance',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      
    ]
  };

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      console.log(params);
      if (params.errorStatus) {
        this.model.ErrorStatus = parseInt(params.errorStatus);
        this.model.DateFromV = null;
        this.model.DateToV = null;
      }

      this.model.DepartmentId = params.departmentId;
      this.model.SBUId = params.sbuId;
      if (!this.model.SBUId) {
        this.model.SBUId = this.sbuid
      }
    });
    this.searchProductNeedPublications();

    this.appSetting.PageTitle = "Thiết bị cần ấn phẩm";
  }

  searchProductNeedPublications() {
    this.projectService.searchProductNeedPublications(this.modelProduct).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.modelProduct.PageNumber - 1) * this.modelProduct.PageSize + 1);
        this.listData = data.ListResult;
        this.modelProduct.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchProjectNeedPublications(id: any) {
    this.projectService.searchProjectNeedPublications(id).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.modelProduct.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      PageNumber: 1,
      OrderBy: 'CreateDate',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      DateFrom: '',
      DateTo: '',
      Status: '',
      Note: '',
      search: '',
      CustomerTypeId: '',
      DateToV: '',
      DateFromV: '',
      CreateDate: '',
      Type: null
    }
  }

  select(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.model = [];
      this.searchProjectNeedPublications(this.listData[index].Id);
    }
    else {
      this.selectIndex = -1;
      this.model = [];
      this.listDA = [];
    }
  }

 
}
