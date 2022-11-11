import { ChangeDetectorRef, Component, Input, OnInit, forwardRef } from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { DxTreeListComponent } from 'devextreme-angular';
import { Configuration, MessageService, AppSetting, Constants } from 'src/app/shared';
import { ChooseAccessoryComponent } from '../choose-accessory/choose-accessory.component';
import { SaleProductService } from '../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
@Component({
  selector: 'app-accessory-manage',
  templateUrl: './accessory-manage.component.html',
  styleUrls: ['./accessory-manage.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => AccessoryManageComponent),
    multi: true
  }
  ],
})
export class AccessoryManageComponent implements OnInit {
  @Input() Id: string;
  public _listAccessory;
  get listAccessory(): any {
    return this._listAccessory;
  }
  @Input()
  set listAccessory(val: any) {
    this._listAccessory = val;
  }
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
  
  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listAccessory = value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.listAccessory = value;
    } else {
      this.listAccessory = [];
    }

    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this._cd.markForCheck();
  }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  ngOnInit() {
    this.getAccessoryInfo(this.Id)
  }

  getAccessoryInfo(id){
    this.saleProductService.getAccessoryByProductId(id).subscribe((data: any) => {
      if (data) {
        this.listAccessory = data;
        this._onChange(this.listAccessory);   
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  showChooseProduct()
  {
      let activeModal = this.modalService.open(ChooseAccessoryComponent, { container: 'body', windowClass: 'choose-accessory-model', backdrop: 'static' })
      var listAccessoryId = [];
    this.listAccessory.forEach(element => {
      listAccessoryId.push(element.Id);
    });

    activeModal.componentInstance.listAccessoryId = listAccessoryId;
      activeModal.result.then((result) => {
        if (result && result.length > 0) {
          result.forEach(element => {
            this.listAccessory.push(element);
          });
          this._onChange(this.listAccessory);
        }
      }, (reason) => {
      });
    }
    showComfrimDelete(row:any){
      this.messageService.showConfirm("Bạn có chắc muốn xoá phụ kiện này không?").then(
        data => {
          this.deleteProduct(row);
        },
        error => {
          
        }
      );
    }
    deleteProduct(row: any) {
      var index = this.listAccessory.indexOf(row);
      if (index > -1) {
        this.listAccessory.splice(index, 1);
      }
      this._onChange(this.listAccessory);
    }
}
