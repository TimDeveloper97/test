import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { PlanService } from '../../service/plan.service';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanCreateComponent } from '../plan-create/plan-create.component';


@Component({
  selector: 'app-view-plan-detail',
  templateUrl: './view-plan-detail.component.html',
  styleUrls: ['./view-plan-detail.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class ViewPlanDetailComponent implements OnInit {

  constructor(
    private router: Router,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private modalService: NgbModal,
    private servicePlan: PlanService,
    public constant: Constants
  ) { }

  isAction: boolean = false;
  listPlanId: any[] = [];
  listData: any[] = [];
  EmployeeCode: string;

  @ViewChild('scrollPracticeMaterial', { static: false }) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader', { static: false }) scrollPracticeMaterialHeader: ElementRef;

  ngOnInit() {
    this.getListPlan();
    
  }

  ngAfterViewInit() {
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  height = 400;

  getListPlan() {
    this.servicePlan.getListPlan(this.listPlanId, this.EmployeeCode).subscribe(data => {
      this.listData = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  viewScheduleProject(Id) {
    this.router.navigate(['du-an/quan-ly-du-an/chinh-sua/', Id]);
    this.closeModal(true);
  }

  showCreateUpdate(Id: string, Types) {
    let activeModal = this.modalService.open(PlanCreateComponent, { container: 'body', windowClass: 'plan-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.Types = Types;
    // activeModal.result.then((result) => {
    //   if (result) {
    //     this.searchPlan();
    //   }
    // }, (reason) => {
    // });
  }
}
