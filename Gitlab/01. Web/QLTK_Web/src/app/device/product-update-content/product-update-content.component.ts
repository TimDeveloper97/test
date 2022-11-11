import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-product-update-content',
  templateUrl: './product-update-content.component.html',
  styleUrls: ['./product-update-content.component.scss']
})
export class ProductUpdateContentComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ProductService,
  ) { }
  productId: string;

  model: any = {
    ProductId: '',
    Content: '',
  }

  ngOnInit() {
    this.model.ProductId = this.productId;
    if (this.productId) {
      this.getContentProduct();
    }
  }

  getContentProduct() {
    this.service.getContentProduct(this.productId).subscribe(data => {
      this.model.Content = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  save() {
    if (this.productId) {
      this.updateContent();
    }
  }

  updateContent() {
    this.service.updateContent(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nội dung thay đổi thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close(true);
  }
}
