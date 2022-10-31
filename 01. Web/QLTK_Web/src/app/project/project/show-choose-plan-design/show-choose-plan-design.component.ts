import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, OnDestroy, AfterViewInit } from '@angular/core';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';

@Component({
  selector: 'app-show-choose-plan-design',
  templateUrl: './show-choose-plan-design.component.html',
  styleUrls: ['./show-choose-plan-design.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChoosePlanDesignComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: ProjectGeneralDesignService,
    public appSetting: AppSetting,
  ) { }
  @ViewChild('scrollPlanHeader',{static:false}) scrollPlanHeader: ElementRef;
  @ViewChild('scrollPlan',{static:false}) scrollPlan: ElementRef;
  id: string;
  projectProductId: string;
  approveStatus: number;
  listModule: any[];
  checked: boolean = false;
  listData: any[] = [];

  modalInfo = {
    Title: 'Danh sách công việc chưa hoàn thành',
  };

  model: any = {
    Id: '',
    ApproveStatus: 0,
    ListPlan: []
  }

  ngOnDestroy() {
    this.scrollPlan.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  ngOnInit() {
    this.model.Id = this.id;
    this.model.ApproveStatus = this.approveStatus;
    this.getListPlanByProjectProduct();
  }

  getListPlanByProjectProduct() {
    var modelData = {
      ListModule: this.listModule
    }
    this.service.getListPlanByProjectProduct(modelData).subscribe(data => {
      if (data) {
        this.listData = data;
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  save() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.model.ListPlan.push(element);
      }
    });
    this.model.ApproveStatus = 1;
    this.service.updateApproveStatus(this.model).subscribe(data => {
      this.messageService.showSuccess('Phê duyệt tổng hợp thành công!');
      this.closeModal();
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.activeModal.close(this.model.ApproveStatus);
  }

  checkAllPlan(isCheck: any) {
    this.listData.forEach(element => {
      element.Checked = isCheck;
    });
  }

  ngAfterViewInit(){
    this.scrollPlan.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPlanHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }
}
