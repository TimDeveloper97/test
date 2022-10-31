import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-choose-question',
  templateUrl: './choose-question.component.html',
  styleUrls: ['./choose-question.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseQuestionComponent implements OnInit {

  listIdSelect: any[] = [];
  listBase: any = [];
  listSelect: any = [];

  isAction: boolean = false;
  checkedTop: boolean = false;
  checkedBot: boolean = false;

  constructor(private activeModal: NgbActiveModal,) { }

  modelsearch: any = {
    Name: '',
    ListIdSelect: [],
    ListIdChecked: [],

  }
  

  ngOnInit(): void {
  }

  searchQuestion() {

  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listBase.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow() {
    var index = 1;
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listBase.indexOf(element);
      if (index > -1) {
        this.listBase.splice(index, 1);
      }
    });
    for (var item of this.listBase) {
      item.Index = index;
      index++;
    }
  }

  removeRow() {
    var index = 1;
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listBase.push(element);
      }
    });

    this.listBase.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
    for (var item of this.listBase) {
      item.Index = index;
      index++;
    }
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  clear() {

  }

  closeModal() {
    this.activeModal.close(false);
  }

}
