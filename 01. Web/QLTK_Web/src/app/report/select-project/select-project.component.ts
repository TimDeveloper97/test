import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { FuturePersonnelForecastService } from '../service/future-personnel-forecast.service';

@Component({
  selector: 'app-select-project',
  templateUrl: './select-project.component.html',
  styleUrls: ['./select-project.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SelectProjectComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    public forecastService: FuturePersonnelForecastService,
  ) { }
  listBase: any = [];
  listSelect: any = [];
  isAction: boolean = false;
  IsRequest: boolean;
  ListIdSelectRequest: any = [];
  ListIdSelect: any = [];
  CourseId: string;
  model:any={
    TotalItem:0,

    Id: '',
    Name: '',
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }
  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.model.ListIdSelect.push(element);
    });
    this.searchSelectProject();
  }

  searchSelectProject(){
    this.listSelect.forEach(element => {
      this.model.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.model.ListIdChecked.push(element.Id);
      }
    });
    this.forecastService.SearchSelectProject(this.model).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.model.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear(){
    this.model = {
      TotalItem:0,

      Id: '',
      Name: '',
      listBase: [],
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.model.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.ListIdSelectRequest.forEach(element => {
        this.model.ListIdSelect.push(element);
      });
    } else {
      this.ListIdSelect.forEach(element => {
        this.model.ListIdSelect.push(element);
      });
    }
    this.searchSelectProject();
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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
