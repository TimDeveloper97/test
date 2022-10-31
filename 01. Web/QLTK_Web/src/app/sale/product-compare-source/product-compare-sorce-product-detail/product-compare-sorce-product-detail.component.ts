import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService } from 'src/app/shared';
import { ProductCompareSourceService } from '../../service/product-compare-source.service';

@Component({
  selector: 'app-product-compare-sorce-product-detail',
  templateUrl: './product-compare-sorce-product-detail.component.html',
  styleUrls: ['./product-compare-sorce-product-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductCompareSorceProductDetailComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private productCompareService : ProductCompareSourceService,
    public constants: Constants
  ) { }

  isAction: boolean = false;

  modelTitle={
    title:'Chi tiết sản phẩm sai khác'
  }
  id: string;
  modelType = 0;

  modelSaleProduct: any = {

  }

  modelSource: any = {

  }

  ngOnInit() {
    this.getProductCompareSource();
  }

  getProductCompareSource() {
    this.productCompareService.getProductCompareSourceById(this.id).subscribe(data => {
      this.modelSaleProduct = data.SaleProductInfo;
      this.modelSource = data.ProductCompare;
      this.modelType = data.SaleProductInfo.Type;
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
    this.productCompareService.UpdateSaleProduct(this.id).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật sản phẩm sai khác thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
