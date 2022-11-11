import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ShowProjectComponent } from '../../project/show-project/show-project.component';
import { CustomerService } from '../../service/customer.service';

@Component({
  selector: 'app-customer-project',
  templateUrl: './customer-project.component.html',
  styleUrls: ['./customer-project.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerProjectComponent implements OnInit  {
  constructor(
    public constant: Constants,
    private router: Router,
    private messageService: MessageService,
    private customerservice: CustomerService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private CustomerService: CustomerService,
    public appSetting: AppSetting,
  ) { }
  listDA: any[] = [];
  startIndex = 0;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: true,
  }
  Id: string;
  customerTypeId: string;

  modelInfo: any = {

  }
  
  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;


  ngOnInit(): void {
    this.Id= this.route.snapshot.paramMap.get('Id');
    this.CustomerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
      this.modelInfo = data;
        this.appSetting.PageTitle = "Quản lý khách hàng - " + this.modelInfo.Code + " - " + this.modelInfo.Name;
        
        this.searchCustomerProject();
    });
  } 

  closeModal() {
    this.router.navigate(['du-an/quan-ly-khach-hang']);
  }

  searchCustomerProject()
  {
    this.customerservice.searchCustomerProject(this.model, this.Id).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showProject(Id: string) {
    let activeModal = this.modalService.open(ShowProjectComponent, { container: 'body', windowClass: 'show-project-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchCustomerProject();
      }
    }, (reason) => {
    });
  }
}
