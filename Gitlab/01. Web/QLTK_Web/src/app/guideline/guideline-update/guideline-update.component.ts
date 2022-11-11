import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { GuidelineService } from '../service/guideline.service';

@Component({
  selector: 'app-guideline-update',
  templateUrl: './guideline-update.component.html',
  styleUrls: ['./guideline-update.component.scss']
})
export class GuidelineUpdateComponent implements OnInit {

  constructor(
    private guidelineService: GuidelineService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,

  ) { }
  id :string;
  content : string;
  model: any = {
    Id: '',
    Content: '',
  }
  ngOnInit(): void {
    this.model.Id = this.id;
    this.model.Content = this.content;
  }
  save() {
    this.UpdateGuidelineInfo();
    this.closeModal(this.model);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }
  UpdateGuidelineInfo(){
    this.guidelineService.UpdateGuidelineInfo(this.model).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    })
  }
}
