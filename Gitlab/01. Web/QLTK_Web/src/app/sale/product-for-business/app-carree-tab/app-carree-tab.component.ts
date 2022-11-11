import { ChangeDetectorRef,forwardRef } from '@angular/core';
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';
import { ShowChooseApplicationtComponent } from '../show-choose-applicationt/show-choose-applicationt.component';
import { ShowChooseCareeComponent } from '../show-choose-caree/show-choose-caree.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
@Component({
  selector: 'app-app-carree-tab',
  templateUrl: './app-carree-tab.component.html',
  styleUrls: ['./app-carree-tab.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => AppCarreeTabComponent),
    multi: true
  }
  ],
})
export class AppCarreeTabComponent implements OnInit {
  @Input() Id: string;
  public _listCaree;
  get listCaree(): any {
    return this._listCaree;
  }
  @Input()
  set listCaree(val: any) {
    this._listCaree = val;
  }

  public _product;
  get product(): any {
    return this._product;
  }
  @Input()
  set product(val: any) {
    this._product = val;
  }

  public _listApp;
  get listApp(): any {
    return this._listApp;
  }
  @Input()
  set listApp(val: any) {
    this._listApp = val;
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

  id: string;
  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listCaree = value;
    this.listApp=value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.modelAppCaree = value;
    } else {
      this.modelAppCaree = null;
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
  modelAppCaree={
    listApp:[],
    listCaree:[],
  }
  isAction: boolean = false;
  ngOnInit() {
    this.getAppInfo(this.Id);
    this.getJobInfo(this.Id);
  }
  getAppInfo(id){
    this.saleProductService.getAppByProductId(id).subscribe((data: any) => {
      if (data) {
        this.listApp = data;
        this.modelAppCaree.listApp=data;
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
        this.modelAppCaree.listCaree=data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showSelectApplication() {
    let activeModal = this.modalService.open(ShowChooseApplicationtComponent, { container: 'body', windowClass: 'select-app-model', backdrop: 'static' });
    var listApplicationId = [];
    this.listApp.forEach(element => {
      listApplicationId.push(element.Id);
    });

    activeModal.componentInstance.listApplicationId = listApplicationId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listApp.push(element);
        });
        this.modelAppCaree.listApp=this.listApp;
        this._onChange(this.modelAppCaree);
      }
    }, (reason) => {

    });
  }

  showComfrimDeleteApplication(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá ứng dụng này không?").then(
      data => {
        this.deleteApplication(row);
      },
      error => {
        
      }
    );
  }

  deleteApplication(row: any) {
    var index = this.listApp.indexOf(row);
    if (index > -1) {
      this.listApp.splice(index, 1);
    }
    this.modelAppCaree.listApp=this.listApp;
    this._onChange(this.modelAppCaree);
  }

  showSelectCaree() {
    let activeModal = this.modalService.open(ShowChooseCareeComponent, { container: 'body', windowClass: 'select-caree-model', backdrop: 'static' });
    var listCareeId = [];
    this.listCaree.forEach(element => {
      listCareeId.push(element.Id);
    });

    activeModal.componentInstance.listCareeId = listCareeId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listCaree.push(element);
        });
        this.modelAppCaree.listCaree=this.listCaree;
        this._onChange(this.modelAppCaree);
      }
    }, (reason) => {

    });
  }

  showComfrimDeleteCaree(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá ngành nghề này không?").then(
      data => {
        this.deleteProduct(row);
      },
      error => {
        
      }
    );
  }

  deleteProduct(row: any) {
    var index = this.listCaree.indexOf(row);
    if (index > -1) {
      this.listCaree.splice(index, 1);
    }
    this.modelAppCaree.listCaree=this.listCaree;
    this._onChange(this.modelAppCaree);
  }
}
