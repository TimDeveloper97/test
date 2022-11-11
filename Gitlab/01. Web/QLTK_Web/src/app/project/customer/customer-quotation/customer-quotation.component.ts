import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from 'src/app/solution/service/summary-quotes.service';
import { CustomerService } from '../../service/customer.service';

@Component({
  selector: 'app-customer-quotation',
  templateUrl: './customer-quotation.component.html',
  styleUrls: ['./customer-quotation.component.scss']
})
export class CustomerQuotationComponent implements OnInit {

  constructor(
    public constant: Constants,
    private route: ActivatedRoute,
    private CustomerService: CustomerService,
    public appSetting: AppSetting,
    private router: Router,
    private messageService: MessageService,
    private service: SummaryQuotesService,
  ) { }

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: true,
  }
  Id: string;
  
  startIndexPlan = 1;
  selectIndex = -1;
  listData: any[] = [];
  listShowPlan: any[] = [];
  startIndex = 0;
  total = 0;
  listDataPlan: any[] = [];
  DepartmentName: string;
  ModalInfo: any = {
    Title: '',
  };

  ngOnInit(): void {
    this.Id = this.route.snapshot.paramMap.get('Id');
    this.CustomerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
      this.ModalInfo = data;
      this.appSetting.PageTitle = "Quản lý khách hàng - " + this.ModalInfo.Code + " - " + this.ModalInfo.Name;
      });
    this.searchQuotation(this.Id);
  }

  searchQuotation(CustomerId) {
    this.service.getQuotationByCustomerId(this.model, CustomerId).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
        this.listData.forEach(element => {
          this.total = this.total + element.QuotationPrice;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  closeModal() {
    this.router.navigate(['du-an/quan-ly-khach-hang']);
  }

  clear() {
    this.model = {
        PageSize: 10,
        totalItems: 0,
        PageNumber: 1,
        OrderBy: 'CreateDate',
        OrderType: true,
    }
    this.searchQuotation(this.Id);
  }

  selectQuotes(QuotationId, index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.service.getQuotationById(QuotationId).subscribe(data => {
        this.ModalInfo.Title = ' - ' + data.data.Code;
        this.DepartmentName = data.departmentName.Name;
      });
      this.getPlan(QuotationId);
    }
    else {
      this.selectIndex = -1;
      this.listDataPlan = [];
      this.ModalInfo.Title = ''
    }
  }

  getPlan(QuotationId) {
    this.service.getQuotationPlan(QuotationId).subscribe(data => {
      if (data) {
        this.listDataPlan = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

}
