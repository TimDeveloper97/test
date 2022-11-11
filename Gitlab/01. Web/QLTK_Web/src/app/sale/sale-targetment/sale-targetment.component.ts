import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, DateUtils, MessageService } from 'src/app/shared';
import { CreateUpdateSaleTartgetmentComponent } from '../create-update-sale-tartgetment/create-update-sale-tartgetment.component';
import { SaleTartgetmentService } from '../service/sale-tartgetment.service';

@Component({
  selector: 'app-sale-targetment',
  templateUrl: './sale-targetment.component.html',
  styleUrls: ['./sale-targetment.component.scss']
})
export class SaleTargetmentComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private saleTartgetmentService : SaleTartgetmentService,
    public dateUtils: DateUtils,

  ) { }
  StartIndex = 0;
  listData: any[] = [];
  TotalTartgetment =0;
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Index',
    OrderType: 'true',
    Name: '',
    DateFrom :null,
    DateTo: null

  }

  searchOptions: any = {
    FieldContentName: 'CustomerName',
    Placeholder: 'Tìm kiếm theo tên KH',
    Items: [
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Thời gian đăng ký dự kiến',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      }
    ]
  };
  ngOnInit(): void {
    this.appSetting.PageTitle = "Đăng ký doanh số";
    this.searchSaleTartgetment();

  }

  searchSaleTartgetment() {
    this.TotalTartgetment=0;
    if(this.model.DateFromV != null &&this.model.DateFromV != '' ){
      this.model.DateFrom =this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if(this.model.DateToV != null &&this.model.DateToV != '' ){
      this.model.DateTo =this.dateUtils.convertObjectToDate(this.model.DateToV);
    }

    this.saleTartgetmentService.searchSaleTartgetment(this.model).subscribe(
      data => {
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        this.listData.forEach(element =>{
          this.TotalTartgetment = this.TotalTartgetment +element.SaleTarget;
        })
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
      },
      error => {
        this.messageService.showError(error);
      });
  }
  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Index',
      OrderType: 'true',
      Name: '',
      DateFrom: null,
      DateTo: null

    }
    this.searchSaleTartgetment();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(CreateUpdateSaleTartgetmentComponent, { container: 'body', windowClass: 'create-update-sale-tartgetment', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    this.listData.forEach(element =>{
      if(element.Id == Id){
        activeModal.componentInstance.model = element;
      }
    });
    activeModal.result.then((result) => {
      if (result) {
        this.searchSaleTartgetment();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá đăng ký này không?").then(
      data => {
        this.deleteSaleTartgetment(Id);
      },
      error => {
      }
    );
  }
  deleteSaleTartgetment(Id: string) {
    var model=
    {
      Id : Id,
    }
    this.saleTartgetmentService.deleteSaleTartgetment(model).subscribe(
      data => {
        this.searchSaleTartgetment();
      },
      error => {
        this.messageService.showError(error);
      });
  }


}
