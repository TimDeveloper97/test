import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanCreateComponent } from '../../plan/plan-create/plan-create.component';
import { DashbroadProjectService } from '../../service/dashbroad-project.service';
import { PlanService } from '../../service/plan.service';

@Component({
  selector: 'app-view-detail',
  templateUrl: './view-detail.component.html',
  styleUrls: ['./view-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ViewDetailComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private dashbroadProjectService: DashbroadProjectService,
    private planService: PlanService

  ) { }

  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;

  StartIndex = 1;
  height = 400;
  listData: any[] = [];
  list_Design: any[] = [];
  planType = 1;
  projectId = '';
  valueType = '';

  ModalInfo = {
    Title: 'Danh sách công việc',
  };

  ngOnInit() {
    this.ModalInfo.Title;
    this.listData;

    this.searchPlan();

    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

  }
  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  ngAfterViewInit() {    

    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }
  isAction: boolean = false;
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(PlanCreateComponent, { container: 'body', windowClass: 'plan-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchPlan();
      }
    }, (reason) => {
    });
  }

  searchPlan() {
    if (this.planType == 1) {
      this.dashbroadProjectService.viewDetailDesign(this.projectId, this.valueType).subscribe((data: any) => {
        this.listData = data;
      }, error => {
        this.messageService.showError(error);
      });
    } else if (this.planType == 2) {
      this.dashbroadProjectService.viewDetailDocument(this.projectId, this.valueType).subscribe((data: any) => {
        this.listData = data;
      }, error => {
        this.messageService.showError(error);
      });
    } else if (this.planType == 3) {
      this.dashbroadProjectService.viewDetailTransfer(this.projectId, this.valueType).subscribe((data: any) => {
        this.listData = data;
      }, error => {
        this.messageService.showError(error);
      });
    }
  }

  // showConfirmDeletePlan(Id: string) {
  //   this.messageService.showConfirm("Bạn có chắc muốn xoá kế hoạch này không?").then(
  //     data => {
  //       this.deletePlan(Id);
  //     },
  //     error => {
        
  //     }
  //   );
  // }

  // deletePlan(Id: string) {
  //   this.planService.deletePlan({ Id: Id }).subscribe(
  //     data => {
  //       this.searchPlan();
  //       this.messageService.showSuccess('Xóa kế hoạch thành công!');
  //     },
  //     error => {
  //       this.messageService.showError(error);
  //     });
  // }
}
