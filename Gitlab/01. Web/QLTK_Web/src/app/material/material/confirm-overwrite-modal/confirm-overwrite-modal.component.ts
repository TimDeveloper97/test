import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MaterialService } from '../../services/material-service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-confirm-overwrite-modal',
  templateUrl: './confirm-overwrite-modal.component.html',
  styleUrls: ['./confirm-overwrite-modal.component.scss']
})
export class ConfirmOverwriteModalComponent implements OnInit {
  data = [];
  model = {
    ListExist: [],
  };
  message:'';

  constructor(
    private activeModal: NgbActiveModal,
    private materialService: MaterialService,
    private messageService: MessageService,
  ) { }

  ngOnInit() {
  }

  overwrite() {
    this.model.ListExist = this.data;
    this.materialService.overwriteBuyHistory(this.model).subscribe((event: any) => {
      this.messageService.showSuccess("Import danh sách vật tư thành công!");
      this.closeModal();
    }, error => {
      this.messageService.showError(error);
    });
  }

  creatnew() {
    this.model.ListExist = this.data;
    this.materialService.creatNewBuyHistory(this.model).subscribe((event: any) => {
      this.messageService.showSuccess("Import danh sách vật tư thành công!");
      this.closeModal();
    }, error => {
      this.messageService.showMessage(error);
    });
  }

  closeModal() {
    this.activeModal.close(false);
  }
}
