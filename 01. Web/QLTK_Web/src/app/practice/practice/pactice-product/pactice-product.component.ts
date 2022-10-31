import { Component, OnInit, Input } from '@angular/core';
import { ProductGroupService } from 'src/app/device/services/product-group.service';
import { Router } from '@angular/router';
import { Configuration, MessageService, AppSetting, Constants, PermissionService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ProductService } from 'src/app/device/services/product.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { PracticeService } from '../../service/practice.service';

@Component({
  selector: 'app-pactice-product',
  templateUrl: './pactice-product.component.html',
  styleUrls: ['./pactice-product.component.scss']
})
export class PacticeProductComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public appSetting: AppSetting,
    public constants: Constants,
    private productGroup: ProductGroupService,
    private productService: ProductService,
    private service: PracticeService,
    public permissionService: PermissionService

  ) { }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  StartIndex = 0;
  pagination;
  LstpageSize = [5, 10, 15, 20, 25, 30];
  ListProductGroup: any[] = [];
  ListProduct: any[] = [];
  modules: any[] = [];
  TotalItems: 0;
  totalAmount = 0;
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    Quantity: '',
    TotalPrice: '',
    MaterialId: '',
    PracticeId: '',
    listSelect: []
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành/công đoạn";
    this.model.PracticeId = this.Id;
    this.SearchPracticeProduct();

  }

  SearchPracticeProduct() {
    if (!this.permissionService.checkPermission(['F040706'])) {

    this.service.SearchPracticeProduct(this.model).subscribe((data: any) => {
      if (data) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.ListProduct = data.Products;
        this.modules = data.Modules;
        this.totalAmount = this.modules.reduce((a, b) => a + (b.Qty * b.Pricing), 0);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  }
  exportExcel() {
    this.service.exportExcelPracticeProduct(this.model).subscribe(d => {
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
