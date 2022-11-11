import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Constants, DateUtils, MessageService } from 'src/app/shared';
import { SaleTartgetmentService } from '../service/sale-tartgetment.service';

@Component({
  selector: 'app-create-update-sale-tartgetment',
  templateUrl: './create-update-sale-tartgetment.component.html',
  styleUrls: ['./create-update-sale-tartgetment.component.scss']
})
export class CreateUpdateSaleTartgetmentComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private combobox: ComboboxService,
    private saleTartgetmentService : SaleTartgetmentService,
    public dateUtils: DateUtils,
    ) { }

    ModalInfo = {
      Title: 'Thêm mới doanh số',
      SaveText: 'Lưu',
    };
    isAction: boolean = false;
  Id: string;
  CustomerType :1;
  listApplication: any[] = []
  listCustomer: any[] = []
  Customers = [{ Name: 'Code', Title: 'Mã KH' }, { Name: 'Name', Title: 'Tên KH' }];
  listJob: any[] = []
  Jobs = [{ Name: 'Code', Title: 'Mã lĩnh vực' }, { Name: 'Name', Title: 'Tên lĩnh vực' }];
  Applications = [{ Name: 'Code', Title: 'Mã ứng dụng' }, { Name: 'Name', Title: 'Tên ứng dụng' }];
  listIndustry: any[] = []
  Industries = [{ Name: 'Code', Title: 'Mã ngành hàng' }, { Name: 'Name', Title: 'Tên ngành hàng' }];



  model: any = {
    Id: '',
    PlanContractDate :'',
    SaleTarget : 0,
    CustomerName:'',
    CustomerCode :'',
    DomainId: '',
    ApplicationId :'',
    IndustryId:'',
    CustomerId :'',
    CustomerType :1,
  }
  ngOnInit(): void {
    this.Id = this.model.Id;

    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa doanh số';
      this.ModalInfo.SaveText = 'Lưu';
      // this.getSaleTartgetmentInfo();
      if(this.model.CustomerId !='' && this.model.CustomerId != null){
        this.model.CustomerType = 2;
      }else{
        this.model.CustomerType = 1;
      }
      this.model.PlanContractDate = this.dateUtils.convertDateToObject(this.model.PlanContractDate);
    }
    else {
      this.ModalInfo.Title = "Thêm mới doanh số";
    }
    this.GetAllCustomer();
    this.GetAllDomain();
    this.GetAllApplication();
    this.GetAllIndustry();
  }

  getSaleTartgetmentInfo() {
    
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


  addSaleTartgetment() {
    this.model.PlanContractDate =this.dateUtils.convertObjectToDate(this.model.PlanContractDate);
    this.saleTartgetmentService.addSaleTartgetment(this.model).subscribe(
      data => {
          this.messageService.showSuccess('Thêm mới doanh số thành công!');
          this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save() {
    if (this.Id) {
      this.updateSaleTartgetment();
    }
    else {
      this.addSaleTartgetment();
    }
  }

  updateSaleTartgetment() {
    this.model.PlanContractDate =this.dateUtils.convertObjectToDate(this.model.PlanContractDate);
    this.saleTartgetmentService.updateSaleTartgetment(this.model).subscribe(
      data => {
          this.messageService.showSuccess('Cập nhật doanh số thành công!');
          this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      });
  }
  GetAllCustomer(){
    this.combobox.getListCustomer().subscribe(
      data => {
          this.listCustomer =data;
      },
      error => {
        this.messageService.showError(error);
      });
  }
  GetAllDomain(){
    this.combobox.getListJob().subscribe(
      data => {
          this.listJob =data;
      },
      error => {
        this.messageService.showError(error);
      });
  }
  GetAllApplication(){
    this.combobox.getApplication().subscribe(
      data => {
          this.listApplication =data;
      },
      error => {
        this.messageService.showError(error);
      });
  }
  GetAllIndustry(){
    this.combobox.getCbbIndustry().subscribe(
      data => {
          this.listIndustry =data;
      },
      error => {
        this.messageService.showError(error);
      });
  }
  getCustomerInfo(){
    this.listCustomer.forEach(element => {
      if(element.Id == this.model.CustomerId){
        this.model.CustomerName =element.Name;
        this.model.CustomerCode =element.Code;
      }
    });
  }
  
  clearKH(){
    this.model.CustomerCode ='';
    this.model.CustomerName ='';
    this.model.CustomerId ='';
  }
}
