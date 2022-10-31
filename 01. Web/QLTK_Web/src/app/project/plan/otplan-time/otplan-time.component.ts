import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-otplan-time',
  templateUrl: './otplan-time.component.html',
  styleUrls: ['./otplan-time.component.scss']
})
export class OTPlanTimeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
  ) { }
  ot: number = 0;

  ngOnInit() {

  }

  save() {
    this.activeModal.close(this.ot);
  }

  closeModal() {
    this.activeModal.close();
  }

}
