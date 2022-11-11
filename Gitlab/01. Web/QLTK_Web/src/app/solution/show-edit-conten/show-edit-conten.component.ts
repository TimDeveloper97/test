import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-show-edit-conten',
  templateUrl: './show-edit-conten.component.html',
  styleUrls: ['./show-edit-conten.component.scss']
})
export class ShowEditContenComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
  ) { }

  curentVersion: number;
  model: any = {
    CurentVersion: 0,
    EditContent: ''
  }

  ngOnInit() {
    this.model.CurentVersion = this.curentVersion + 1;
  }

  save() {
    this.activeModal.close(this.model);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }
}
