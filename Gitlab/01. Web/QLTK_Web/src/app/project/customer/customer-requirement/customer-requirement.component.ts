import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { CustomerService } from '../../service/customer.service';

@Component({
  selector: 'app-customer-requirement',
  templateUrl: './customer-requirement.component.html',
  styleUrls: ['./customer-requirement.component.scss']
})
export class CustomerRequirementComponent implements OnInit {

  constructor(
    private CustomerService: CustomerService,
    public constant: Constants,
    private route: ActivatedRoute,
    private router: Router,
    public appSetting: AppSetting,
    private messageService: MessageService,
  ) { }
  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
  };
  
  Id: string;
  listRequest: any[] = [];
  startIndex = 0;
  modelInfo: any = {

  }

  ngOnInit(): void {
    this.Id= this.route.snapshot.paramMap.get('Id');
    this.CustomerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
        this.modelInfo = data;
        this.appSetting.PageTitle = "Quản lý khách hàng - " + this.modelInfo.Code + " - " + this.modelInfo.Name;
        this.getListCustomerRequiment()
    });
  }
  getListCustomerRequiment() {
    this.CustomerService.getListCustomerRequiment(this.searchModel, this.Id).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.listRequest = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-khach-hang']);
  }
}
