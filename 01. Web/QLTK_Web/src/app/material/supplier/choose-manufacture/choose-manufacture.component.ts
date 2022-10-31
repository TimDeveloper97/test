import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { SupplierService } from '../../services/supplier-service';

@Component({
  selector: 'app-choose-manufacture',
  templateUrl: './choose-manufacture.component.html',
  styleUrls: ['./choose-manufacture.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseManufactureComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private service: SupplierService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  IsRequest: boolean;

  model: any = {
    Id: '',
    Code: '',
    Name: '',
    ListIdSelect: [],
    ListIdChecked: []
  }

  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.model.ListIdSelect.push(element);
    });
    this.searchManufacturer();
  }

  searchManufacturer() {
    this.listSelect.forEach(element => {
      this.model.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.model.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchManufacture(this.model).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.model.totalItems = data.TotalItems;
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
    this.model = {
      Id: '',
      Code: '',
      Name: '',
      ListIdSelect: [],
      ListIdChecked: []
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
    this.searchManufacturer();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck) {
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
