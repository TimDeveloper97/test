import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { CustomerService } from '../../service/customer.service';
@Component({
  selector: 'app-customer-meeting',
  templateUrl: './customer-meeting.component.html',
  styleUrls: ['./customer-meeting.component.scss']
})
export class CustomerMeetingComponent implements OnInit {

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
  Meetings: any[] = [];
  startIndex = 0; 
  TotalItems = 0;
  modelInfo: any = {

  }

  ngOnInit(): void {
    this.Id = this.route.snapshot.paramMap.get('Id');
    this.CustomerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
        this.modelInfo = data;
        this.appSetting.PageTitle = "Quản lý khách hàng - " + this.modelInfo.Code + " - " + this.modelInfo.Name;
        this.getListCustomerMeeting()
    });
  }

  getListCustomerMeeting(){
    this.CustomerService.getCustomerMeetings(this.searchModel, this.Id).subscribe(data => {
      this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      // this.searchModel = data.ListResult;
      // this.searchModel.TotalItems = data.TotalItem;
      this.Meetings = data.ListResult;
      this.TotalItems = data.TotalItem;
    });
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-khach-hang']);
  }
}
