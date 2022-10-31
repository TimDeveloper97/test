import { ChangeDetectorRef,forwardRef } from '@angular/core';
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { SaleProductService } from '../../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
@Component({
  selector: 'app-app-caree-detail',
  templateUrl: './app-caree-detail.component.html',
  styleUrls: ['./app-caree-detail.component.scss']
})
export class AppCareeDetailComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constant: Constants,
    private modalService: NgbModal,
    public config: Configuration,
    private _cd: ChangeDetectorRef,
    public saleProductService: SaleProductService,
  ) { }

  ngOnInit(): void {
    this.getAppInfo(this.Id);
    this.getJobInfo(this.Id);
  }
  listApp:any[]=[];
  listCaree:any[]=[];
  getAppInfo(id){
    this.saleProductService.getAppByProductId(id).subscribe((data: any) => {
      if (data) {
        this.listApp = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  getJobInfo(id){
    this.saleProductService.getJobByProductId(id).subscribe((data: any) => {
      if (data) {
        this.listCaree = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
}
