import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService } from 'src/app/shared';
import { ModuleServiceService } from '../../services/module-service.service';

@Component({
  selector: 'app-show-document',
  templateUrl: './show-document.component.html',
  styleUrls: ['./show-document.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowDocumentComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private messageService: MessageService,
    private moduleService: ModuleServiceService,
  ) { }

  modelSearch: any = {
    Code: '',
    Name: '',
    GroupCode: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }

  groupCode: string;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  listIdSelect: any = [];
  listIdSelectRequest: any = [];
  IsRequest: boolean;
  index = 0;

  ngOnInit() {
    this.modelSearch.GroupCode = this.groupCode;
    this.listIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchDocument();
  }

  searchDocument() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.moduleService.searchDocument(this.modelSearch).subscribe(data => {
      this.listData = data;
      console.log(data);
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
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
      Code: '',
      Name: '',
      GroupCode: '',
      ListIdSelect: [],
      ListIdChecked: [],
    };

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

    this.modelSearch.GroupCode = this.groupCode;

    this.searchDocument();
  }


  closeModal() {
    this.activeModal.close(false);
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
