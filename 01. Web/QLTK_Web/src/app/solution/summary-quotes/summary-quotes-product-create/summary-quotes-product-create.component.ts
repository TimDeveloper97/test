import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from '../../service/summary-quotes.service';
import { forkJoin } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-summary-quotes-product-create',
  templateUrl: './summary-quotes-product-create.component.html',
  styleUrls: ['./summary-quotes-product-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SummaryQuotesProductCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constant: Constants,
    private service: SummaryQuotesService,
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private routeA: ActivatedRoute,
  ) { }

  modalInfo = {
    Title: 'Thêm mới thiết bị',
    SaveText: 'Lưu',
  };

  Id: string;
  QuotationProductId: string;
  isAction: boolean = false;
  listModule: any[] = []; //1
  listProduct: any[] = []; //2
  listSaleProduct: any[] = []; //3
  listMaterial: any[] = []; //4
  listIndustry: any[] = [];
  listUnit: any[] = [];
  listManufacture: any[] = [];

  model: any = {
    QuotationId: '',
    Name: '',
    Code: '',
    ObjectId: '',
    ObjectType: -1,
    IndustryId: '',
    IndustryName: '',
    UnitId: '',
    ManufactureId: '',
    Price: '',
    Quantity: '',
    Description: ''
  }
  QuotationId: string;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];

  ngOnInit(): void {
    this.model.QuotationId = this.QuotationId;

    if (this.QuotationProductId) {
      this.modalInfo.Title = 'Chỉnh sửa thiết bị';
      this.modalInfo.SaveText = 'Lưu';
      this.getQuotationProductInfor(this.QuotationProductId);
    }
    else {
      this.modalInfo.Title = "Thêm mới thiết bị";
    }
  }

  changObjectType(){
    this.model.ObjectId = '';
    this.model.Code = '';
    this.model.Name = '';
  }

  getQuotationProductInfor(Id: string){
    this.service.getQuotationProductInfor(Id).subscribe(
      data => {
        this.model.Name = data.data.Name;
        this.model.Code = data.data.Code;
        this.model.QuotationId = data.data.QuotationId;
        this.model.ObjectId = data.data.ObjectId;
        this.model.ObjectType = data.data.ObjectType;
        this.model.IndustryId = data.data.IndustryId;
        this.model.UnitId = data.data.UnitId;
        this.model.ManufactureId = data.data.ManufactureId;
        this.model.Price = data.data.Price;
        this.model.Quantity = data.data.Quantity;
        this.model.Description = data.data.Description;
        this.service.changeIndustry(this.model.IndustryId).subscribe(
          data => {
            this.model.IndustryName = data.IndustryName;
          },
          error => {
            this.messageService.showError(error);
          }
        );
        this.modalInfo.Title = 'Chỉnh sửa thiết bị' +' - '+ this.model.Code +' - '+ this.model.Name;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  
 
  changeIndustry() {
    this.model.IndustryName = '';
    this.service.changeIndustry(this.model.IndustryId).subscribe(
      data => {
        this.model.IndustryName = data.IndustryName;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(){
    this.service.AddProduct(this.model).subscribe(
      data => {
        this.model = {
          Id: '',
          QuotationId: '',
          Name: '',
          Code: '',
          ObjectId: '',
          ObjectType: -1,
          IndustryId: '',
          IndustryName: '',
          UnitId: '',
          ManufactureId: '',
          Price: '',
          Quantity: '',
          Description: ''
        }
        this.messageService.showSuccess('Thêm mới danh mục thiết bị thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update(){
    this.model.Id = this.QuotationProductId;
    this.service.UpdateProduct(this.model).subscribe(
      data => {
        this.model = {
          Id: '',
          QuotationId: '',
          Name: '',
          Code: '',
          ObjectId: '',
          ObjectType: -1,
          IndustryId: '',
          IndustryName: '',
          UnitId: '',
          ManufactureId: '',
          Price: '',
          Quantity: '',
          Description: ''
        }
        this.messageService.showSuccess('Chỉnh sửa danh mục thiết bị thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeModule(ObjectId){
    this.service.changeModule(ObjectId).subscribe(
      data => {
        if(data)
        {
        this.model.Code = data.Code;
        this.model.Name = data.Name;
        }
        if(!data && ObjectId != ''){
          this.messageService.showConfirm("Bạn đã chọn NHÓM module. Vui lòng chọn đúng module!").then(
            data =>
            {
              this.model.Code = "";
              this.model.Name = "";
            }
          )
      }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeProduct(ObjectId){
      this.service.changeProduct(ObjectId).subscribe(
        data => {
            if(data)
            {
              this.model.Code = data.Code;
              this.model.Name = data.Name;
            }
            if(!data && ObjectId != ''){
                this.messageService.showConfirm("Bạn đã chọn NHÓM thiết bị. Vui lòng chọn đúng thiết bị!").then(
                  data =>
                  {
                    this.model.Code = "";
                    this.model.Name = "";
                  }
                )
            }
        },
        error => {
          this.messageService.showError(error);
        }
      );
  }

  changeSaleProduct(ObjectId){
    this.service.changeSaleProduct(ObjectId).subscribe(
      data => {
        if(data)
            {
              this.model.Code = data.Code;
              this.model.Name = data.Name;
            }
            if(!data && ObjectId != ''){
                this.messageService.showConfirm("Bạn đã chọn NHÓM thư viện sản phẩm kinh doanh. Vui lòng chọn đúng thư viện sản phẩm kinh doanh!").then(
                  data =>
                  {
                    this.model.Code = "";
                    this.model.Name = "";
                  }
                )
            }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeMaterial(ObjectId){
    this.service.changeMaterial(ObjectId).subscribe(
      data => {
        if(data)
        {
          this.model.Code = data.Code;
          this.model.Name = data.Name;
        }
        if(!data && ObjectId != ''){
            this.messageService.showConfirm("Bạn đã chọn NHÓM vật tư. Vui lòng chọn đúng vật tư!").then(
              data =>
              {
                this.model.Code = "";
                this.model.Name = "";
              }
            )
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }
}
