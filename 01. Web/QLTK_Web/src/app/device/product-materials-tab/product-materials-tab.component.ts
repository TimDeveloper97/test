import { Component, OnInit, Input } from '@angular/core';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductMaterialsChooseTabComponent } from '../product-materials-choose-tab/product-materials-choose-tab.component';
import { ProductMaterialsService } from '../services/product-materials.service';

@Component({
  selector: 'app-product-materials-tab',
  templateUrl: './product-materials-tab.component.html',
  styleUrls: ['./product-materials-tab.component.scss']
})
export class ProductMaterialsTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private service: ProductMaterialsService,
    public constants: Constants
  ) { }

  listData: any = [];
  ListMaterial: any = [];
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'MaterialCode',
    OrderType: true,

    Id: '',
    MaterialName: '',
    MaterialCode: '',
    ProductId: '',
    MaterialId: '',
    ListMaterial: [],
  }
  ngOnInit() {
    this.model.ProductId = this.Id;
    this.searchProductMaterialsTab();
  }

  showClick() {
    let activeModal = this.modalService.open(ProductMaterialsChooseTabComponent, { container: 'body', windowClass: 'ProductMaterialsChoose-model', backdrop: 'static' });
    activeModal.componentInstance.ProductId = this.model.ProductId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.MaterialId);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      this.searchProductMaterialsTab();
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {
    });
  }

  searchProductMaterialsTab() {
    this.service.searchProductMaterials(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa phụ kiện này không?").then(
      data => {
        this.deleteProductMaterialsTab(Id);
      },
      error => {
        
      }
    );
  }

  deleteProductMaterialsTab(Id) {
    this.service.deleteProductMaterials({ Id: Id }).subscribe(
      data => {
        this.searchProductMaterialsTab();
        this.messageService.showSuccess('Xóa phụ kiện thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ExportExcel() {
    this.service.exportExcel(this.model).subscribe(d => {
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

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'MaterialCode',
      OrderType: true,

      Id: '',
      MaterialName: '',
      MaterialCode: '',
      ProductId: this.Id,
      MaterialId: '',
      ListMaterial: []
    }
    this.searchProductMaterialsTab();
  }

}
