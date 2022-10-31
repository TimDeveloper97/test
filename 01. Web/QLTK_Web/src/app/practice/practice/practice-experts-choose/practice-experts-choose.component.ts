import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, Constants } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';
import { PracticeExpertService } from '../service/practice-expert.service';

@Component({
  selector: 'app-practice-experts-choose',
  templateUrl: './practice-experts-choose.component.html',
  styleUrls: ['./practice-experts-choose.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PracticeExpertsChooseComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: PracticeExpertService
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  PracticeId: string;
  editField: string;
  isAction: boolean = false;
  listData: any = [];
  modelSearch: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    PracticeId: '',
    listSelect: [],
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }

  listBase: any = [];
  listSelect: any = [];
  IsRequest: boolean;
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  ngOnInit() {
    this.modelSearch.PracticeId = this.PracticeId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchExpert();
  }

  searchExpert() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchExpert(this.modelSearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.modelSearch = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.modelSearch.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.ListIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.ListIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchExpert();
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

  CloseModal() {
    this.activeModal.close(false);
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
}
