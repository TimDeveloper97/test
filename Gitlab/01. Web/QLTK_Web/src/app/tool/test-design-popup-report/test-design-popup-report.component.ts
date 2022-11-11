import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-test-design-popup-report',
  templateUrl: './test-design-popup-report.component.html',
  styleUrls: ['./test-design-popup-report.component.scss']
})
export class TestDesignPopupReportComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,) { }

  htmlText: string;
  isAction: boolean = false;
  ngOnInit() {
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
