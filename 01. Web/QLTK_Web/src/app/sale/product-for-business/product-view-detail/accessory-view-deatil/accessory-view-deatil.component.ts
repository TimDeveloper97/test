import { ChangeDetectorRef, Component, Input, OnInit, forwardRef } from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { DxTreeListComponent } from 'devextreme-angular';
import { Configuration, MessageService, AppSetting, Constants } from 'src/app/shared';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SaleProductService } from '../../sale-product.service';

@Component({
  selector: 'app-accessory-view-deatil',
  templateUrl: './accessory-view-deatil.component.html',
  styleUrls: ['./accessory-view-deatil.component.scss']
})
export class AccessoryViewDeatilComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public appSetting: AppSetting,
    public constant: Constants,
    private _cd: ChangeDetectorRef,
    public saleProductService: SaleProductService,
  ) { }
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  ngOnInit() {
    this.getAccessoryInfo(this.Id)
  }
  listAccessory:any[]=[];
  getAccessoryInfo(id){
    this.saleProductService.getAccessoryByProductId(id).subscribe((data: any) => {
      if (data) {
        this.listAccessory = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

}
