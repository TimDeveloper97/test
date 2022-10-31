import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import { SolutionService } from '../service/solution.service';

@Component({
  selector: 'app-choose-project-solution',
  templateUrl: './choose-project-solution.component.html',
  styleUrls: ['./choose-project-solution.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseProjectSolutionComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private service: SolutionService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  listIdSelect: any = [];
  listIdSelectRequest: any = [];
  IsRequest: boolean;
  index = 0;
  modelSearch: any = {
    Id: '',
    Code: '',
    Name: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }
  ngOnInit() {
    this.listIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchSolution();
  }

  searchSolution() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchProjectSolution(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listData.indexOf(element);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listData.push(element);
      }
    });
    this.listData.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  clear() {
    this.modelSearch = {
      Id: '',
      Code: '',
      Name: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.modelSearch.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.listIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.listIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchSolution();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.listData.forEach(element => {
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
