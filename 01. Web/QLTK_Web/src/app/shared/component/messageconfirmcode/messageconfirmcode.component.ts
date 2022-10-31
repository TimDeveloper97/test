import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-messageconfirmcode',
  templateUrl: './messageconfirmcode.component.html',
  styleUrls: ['./messageconfirmcode.component.scss']
})
export class MessageconfirmcodeComponent implements OnInit {
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
