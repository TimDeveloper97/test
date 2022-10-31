import { Component, OnInit } from '@angular/core';
import { ModuleServiceService } from '../../services/module-service.service';
import { MessageService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-module-update-content',
  templateUrl: './module-update-content.component.html',
  styleUrls: ['./module-update-content.component.scss']
})
export class ModuleUpdateContentComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ModuleServiceService,
  ) { }
  moduleId: string;

  model: any = {
    ModuleId: '',
    Content: '',
  }

  ngOnInit() {
    this.model.ModuleId = this.moduleId;
    if (this.moduleId) {
      this.getContentModule();
    }
  }

  getContentModule() {
    this.service.getContentModule(this.moduleId).subscribe(data => {
      this.model.Content = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  save() {
    if (this.moduleId) {
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
