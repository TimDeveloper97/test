import { ChangeDetectorRef,forwardRef } from '@angular/core';
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ChooseGroupProductSaleComponent } from '../choose-group-product-sale/choose-group-product-sale.component';
@Component({
  selector: 'app-group-sale-product',
  templateUrl: './group-sale-product.component.html',
  styleUrls: ['./group-sale-product.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => GroupSaleProductComponent),
    multi: true
  }
  ],
})
export class GroupSaleProductComponent implements OnInit {
  @Input() Id: string;
  public _listGroup;
  get listGroup(): any {
    return this._listGroup;
  }
  @Input()
  set listGroup(val: any) {
    this._listGroup = val;
  }
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constant: Constants,
    private modalService: NgbModal,
    public config: Configuration,
    private _cd: ChangeDetectorRef,
    public saleProductService: SaleProductService,
  ) { }
  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listGroup=value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.listGroup = value;
    } else {
      this.listGroup = null;
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
  listEmployee:any[]=[];
  isAction: boolean = false;
  ngOnInit() {
    this.getGroupProductInfo(this.Id);
  }
  getGroupProductInfo(id){
    this.saleProductService.getGroupByProductId(id).subscribe((data: any) => {
      if (data) {
        this.listGroup = data;
        this._onChange(this.listGroup);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showComfrimDeleteGroup(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá ứng dụng này không?").then(
      data => {
        this.deleteGroup(row);
      }
    );
  }

  deleteGroup(row: any) {
    var index = this.listGroup.indexOf(row);
    if (index > -1) {
      this.listGroup.splice(index, 1);
    }
    this._onChange(this.listGroup);
  }

  showSelectGroup() {
    let activeModal = this.modalService.open(ChooseGroupProductSaleComponent, { container: 'body', windowClass: 'select-group-product-model', backdrop: 'static' });
    var listGroupId = [];
    this.listGroup.forEach(element => {
      listGroupId.push(element.Id);
    });

    activeModal.componentInstance.listGroupProductId = listGroupId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listGroup.push(element);
        });
        this._onChange(this.listGroup);
      }
    }, (reason) => {

    });
  }
  ClickedRow(id){
    this.saleProductService.getEmployeeByGroupId(id).subscribe((data: any) => {
      if (data) {
        this.listEmployee = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
}
