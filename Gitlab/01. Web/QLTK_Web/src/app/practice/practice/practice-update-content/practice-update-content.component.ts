import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { PracticeService } from '../../service/practice.service';

@Component({
  selector: 'app-practice-update-content',
  templateUrl: './practice-update-content.component.html',
  styleUrls: ['./practice-update-content.component.scss']
})
export class PracticeUpdateContentComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: PracticeService,
  ) { }
  practiceId: string;

  model: any = {
    PracticeId: '',
    Content: '',
  }

  ngOnInit() {
    this.model.PracticeId = this.practiceId;
    if (this.practiceId) {
      this.getContentPractice();
    }
  }

  getContentPractice() {
    this.service.getContentPractice(this.practiceId).subscribe(data => {
      this.model.Content = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  save() {
    if (this.practiceId) {
      this.updateContent();
    }
  }

  updateContent() {
    this.service.updateContent(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nội dung thay đổi thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close(true);
  }

}
