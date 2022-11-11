import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { CustomerTypeService } from '../../service/customer-type.service';
import { PlanService } from '../../service/plan.service';
import { DateUtils } from 'src/app/shared';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-problem-exist-create-change-plan',
  templateUrl: './problem-exist-create-change-plan.component.html',
  styleUrls: ['./problem-exist-create-change-plan.component.scss']
})
export class ProblemExistCreateChangePlanComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public dateUtils: DateUtils,) { }

  ngOnInit(): void {
  }

  isAction: boolean = false;
  Id: string;
  parentId: string;
  isChanged = true;
  listCreate: any[]=[];
  Reason: string;
  
  closeModal(isOK: boolean) {
    this.activeModal.close({isOK: false, reason : this.Reason});
  }

  save() {
      this.activeModal.close({ isOK: true, reason: this.Reason });
  }
  

}


