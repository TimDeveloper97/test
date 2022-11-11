import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfig } from 'ngx-perfect-scrollbar';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { ProductCompareSourceService } from '../../service/product-compare-source.service';
import { ProductCompareSorceProductDetailComponent } from '../product-compare-sorce-product-detail/product-compare-sorce-product-detail.component';

@Component({
  selector: 'app-product-compare-source-manage',
  templateUrl: './product-compare-source-manage.component.html',
  styleUrls: ['./product-compare-source-manage.component.scss']
})
export class ProductCompareSourceManageComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public constant: Constants,
    public config: Configuration,
    private modalService: NgbModal,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
    private productCompareService : ProductCompareSourceService

  ) { }

  @ViewChild('scrollProducStandard',{static:false}) scrollProducStandard: ElementRef;
  @ViewChild('scrollProducStandardHeader',{static:false}) scrollProducStandardHeader: ElementRef;

  minWidth = 530;
  height = 0;
  listData = []
  selectIndex = -1;
  isAction: boolean = false;
  checkedTop = false;

  model: any = {
    TotalItems: 0,

    Id: '',
    CodeName: '',
    Source: '',
  }

  modelUpdate: any = {
    ListIdSaleProduct: [],
  }

  searchOptions: any = {
    FieldContentName: 'CodeName',
    Placeholder: 'Tìm kiếm theo mã / tên sản phẩm',
    Items: [
      {
        Name: 'Nguồn sản phẩm',
        FieldName: 'Source',
        Placeholder: 'Nguồn sản phẩm',
        Type: 'select',
        Data: this.constant.Source,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý sai khác so với nguồn";
    this.height = window.innerHeight - 330;
    this.minWidth = 530;
    // this.scrollProducStandard.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
    //   this.scrollProducStandardHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    // }, true);
    this.searchProductCompareSource();

  }

  ngAfterViewInit() {
    this.scrollProducStandard.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollProducStandard.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollProducStandard.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  clear(){
    this.model = {
      TotalItems: 0,

      Id: '',
      CodeName: '',
      Source: '',
    }

    this.searchProductCompareSource();
  }

  listSelect = [];
  checkAll(){
      this.listData.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
          this.listSelect.push(element);
        } else {
          element.Checked = false;
          this.listSelect = [];
        }
      });
  }

  checkItem(row) {
    if (row.Checked == true) {
      this.listSelect.push(row);
    }
    else {
      var index = this.listSelect.indexOf(row);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    }
    if (this.listSelect.length == this.listData.length) {
      this.checkedTop = true;
    }
    else {
      this.checkedTop = false;

    }
  }

  selectProduct(index) {
    this.selectIndex == index;
  }

  searchProductCompareSource() {
    this.productCompareService.searchProductCompareSource(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showComfirmUpdate(){
    this.messageService.showConfirm("Bạn có chắc cập nhật sản phẩm so với nguồn này không?").then(
      data => {
        this.update();
      },
      error => {
        
      }
    );
  }

  update(){
    this.listSelect.forEach(item => {
      this.modelUpdate.ListIdSaleProduct.push(item.IdSaleProduct)
    })
    this.productCompareService.UpdateListSaleProduct(this.modelUpdate).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật danh sách sản phẩm sai khác thành công!');
        this.searchProductCompareSource();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showProductCompareDetail(id){
    let activeModal = this.modalService.open(ProductCompareSorceProductDetailComponent, { container: 'body', windowClass: 'product-compare-sourc-detail', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductCompareSource();
      }
    }, (reason) => {
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
