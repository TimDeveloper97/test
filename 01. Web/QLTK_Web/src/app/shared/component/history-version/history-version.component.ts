import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from '../..';
import { HistoryVersionService } from '../../services/history-version.service';

@Component({
  selector: 'app-history-version',
  templateUrl: './history-version.component.html',
  styleUrls: ['./history-version.component.scss']
})
export class HistoryVersionComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: HistoryVersionService
  ) { }

  id: string;
  type: number;
  model: any = {
    Id: '',
    Type: 0,
    Version: 0,
    Content: '',
    EditContent: '',
    UpdateDate: null
  }

  ngOnInit() {
    this.model.Id = this.id;
    this.model.Type = this.type;
    this.getDataHistoryVersion();
  }

  getDataHistoryVersion() {
    this.service.getDataHistoryVersion(this.model).subscribe(
      data => {
        this.model = data;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  save() {
    this.closeModal(this.model);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }
}
