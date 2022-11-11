import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService } from 'src/app/shared';
import { CustomerRequirementService } from '../service/customer-requirement.service';

@Component({
  selector: 'app-product-need-price-create-modal',
  templateUrl: './product-need-price-create-modal.component.html',
  styleUrls: ['./product-need-price-create-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductNeedPriceCreateModalComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private dateUtils: DateUtils,
    private messageService: MessageService,
    private customerRequirementService: CustomerRequirementService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới thiết bị',
    SaveText: 'Lưu',
  };

  ProductType = [
    { Id: 1, Name: "TM" },
    { Id: 2, Name: "SX" },
    { Id: 3, Name: "Outsource" },
  ];

  Id: any;
  CustomerRequirementId: string;

  model: any = {
    Id: '',
    CustomerRequirementId: '',
    ManufactureName: '',
    Code: '',
    Specifications: '',
    ProductType: '',
    ProductTypeName: '',
    Note: '',
    Unit: '',
    Quantity: '',
    Price: '',
    DeliveryDate: null,
  }
  Index : any;
  Results : any =[];

  ngOnInit(): void {
    if(this.Id)
    {
      this.ModalInfo.Title = 'Chỉnh sửa thiết bị';
      this.getById();
    }else if(this.Id == null && this.Index ==null){

    }else{
      this.ModalInfo.Title = 'Chỉnh sửa thiết bị';
      this.model = this.Results[this.Index];
      this.model.DeliveryDate = this.dateUtils.convertDateToObject(this.model.DeliveryDate);
    }
  }

  getById()
  {
    this.customerRequirementService.getProductNeedPriceById(this.Id).subscribe(
      data => {
        this.model = data;
        if (this.model.DeliveryDate) {
          this.model.DeliveryDate = this.dateUtils.convertDateToObject(this.model.DeliveryDate);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save() {
    if (this.model.ProductType == 1) {
      this.model.ProductTypeName = 'TM';
    }
    else if (this.model.ProductType == 2) {
      this.model.ProductTypeName = 'SX';
    }
    else {
      this.model.ProductTypeName = 'Outsource';
    }
    if (this.model.DeliveryDate) {
      this.model.DeliveryDate = this.dateUtils.convertObjectToDate(this.model.DeliveryDate);
    }
    if(this.Id == null && this.Index ==null){
      this.activeModal.close(this.Results.push(this.model));
    }
    if(this.Id !=null){
      this.Results.forEach(element =>{
        if(element.Id==this.Id){
          this.Results[this.Index] = this.model;
        }
      });
    }
    this.activeModal.close();
  }

  closeModal() {
    this.activeModal.close();
  }
}
