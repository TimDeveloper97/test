import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { GuidelineUpdateComponent } from '../guideline-update/guideline-update.component';
import { GuidelineService } from '../service/guideline.service';

@Component({
  selector: 'app-guide-line',
  templateUrl: './guide-line.component.html',
  styleUrls: ['./guide-line.component.scss']
})
export class GuideLineComponent implements OnInit {

  constructor(
    private guidelineService: GuidelineService,
    private messageService: MessageService,
    private modalService: NgbModal,
  ) { }

  model: any = {
    Id: '',
    Content: '',
  }
  ngOnInit(): void {
    this.getGuidelineInfo();
  }

  showConfirmUpdate(){
    let activeModal = this.modalService.open(GuidelineUpdateComponent, { container: 'body', windowClass: 'update-guideline', backdrop: 'static' });
    activeModal.componentInstance.id = this.model.Id;
    activeModal.componentInstance.content = this.model.Content;
    activeModal.result.then(async (result) => {
      if (result) {
      }
      this.getGuidelineInfo();
    }, (reason) => {
    });
  };

  getGuidelineInfo(){
    this.guidelineService.getGuidelineInfo(this.model).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    })
  }
}
