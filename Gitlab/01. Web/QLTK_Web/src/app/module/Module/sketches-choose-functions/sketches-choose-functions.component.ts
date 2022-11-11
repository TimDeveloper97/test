import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService } from 'src/app/shared';
import { ModuleSketchesService } from '../../services/module-sketches-service';

@Component({
  selector: 'app-sketches-choose-functions',
  templateUrl: './sketches-choose-functions.component.html',
  styleUrls: ['./sketches-choose-functions.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SketchesChooseFunctionsComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private moduleSketchesService: ModuleSketchesService,
    private messageService: MessageService,
  ) { }


  modelsearch: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Code: '',
    Name: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }

  listBase: any = [];
  listSelect: any = [];
  listIdSelect: any = [];
  ngOnInit() {
    this.listIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });
    //this.searchFunction();
  }

  searchFunction() {
    this.listSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelsearch.ListIdChecked.push(element.Id);
      }
    });
    this.moduleSketchesService.searchFunction(this.modelsearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelsearch.TotalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.modelsearch = {
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Code: '',
      Name: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }

    this.listIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });

    this.searchFunction();
  }

  addRow() {
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
  }

  removeRow() {
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
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal() {
    this.activeModal.close(false);
  }
}
