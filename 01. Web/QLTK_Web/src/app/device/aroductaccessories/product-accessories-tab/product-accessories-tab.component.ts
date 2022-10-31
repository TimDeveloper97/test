import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Constants, AppSetting, MessageService, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ProductAccessoriesService } from '../../services/product-accessories.service';
import { ProductaccessorieschooseComponent } from '../../productaccessorieschoose/productaccessorieschoose.component';

@Component({
  selector: 'app-product-accessories-tab',
  templateUrl: './product-accessories-tab.component.html',
  styleUrls: ['./product-accessories-tab.component.scss']
})
export class ProductAccessoriesTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private service: ProductAccessoriesService,
    public constants: Constants
  ) { }

  listData: any = [];
  modelProductAccessories: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    ProductId: '',
    MaterialId: '',
    ListMaterial: []
  }

  totalAmount = 0;
  @ViewChild('scrollAccessories',{static:false}) scrollAccessories: ElementRef;
  @ViewChild('scrollAccessoriesHeader',{static:false}) scrollAccessoriesHeader: ElementRef;
  height = 500;
  ngOnInit() {
    this.modelProductAccessories.ProductId = this.Id;
    //this.getProductaccessoriesInfo();
    this.scrollAccessories.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollAccessoriesHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }
  ngOnDestroy() {
    this.scrollAccessories.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  

}
