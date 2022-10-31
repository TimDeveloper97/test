import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-messageconfirm',
  templateUrl: './messageconfirm.component.html',
  styleUrls: ['./messageconfirm.component.css']
})
export class MessageconfirmComponent implements OnInit {

  message: string;

  constructor(private activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  ok() {
    this.activeModal.close(true);
  }

  closeModal() {
    this.activeModal.close(false);
  }

}
