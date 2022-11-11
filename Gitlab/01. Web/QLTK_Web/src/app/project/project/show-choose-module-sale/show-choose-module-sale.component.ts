import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from 'src/app/practice/service/practice.service';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';

@Component({
  selector: 'app-show-choose-module-sale',
  templateUrl: './show-choose-module-sale.component.html',
  styleUrls: ['./show-choose-module-sale.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseModuleSaleComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private practiceService: PracticeService,
    private service: ProjectGeneralDesignService,
    public appSetting: AppSetting,
  ) { }

  projectId: string;
  projectProductId: string;
  editField: string;
  isAction: boolean = false;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  listData: any = [];
  modelSearch: any = {
    TotalItem: 0,

    Id: '',
    Name: '',
    ProjectId: null,
    ProjectProductId: null,
    listSelect: [],
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }

  listBase: any[] = [];
  listSelect: any[] = [];
  IsRequest: boolean;
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm tên/mã module ...',
    Items: []
  };

  ngOnInit() {
    this.modelSearch.ProjectId = this.projectId;
    this.modelSearch.ProjectProductId = this.projectProductId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchModule();
  }

  searchModule() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchModuleSale(this.modelSearch).subscribe(data => {
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
      TotalItem: 0,

      Id: '',
      Name: '',
      ProjectId: this.projectId,
      ProjectProductId: this.projectProductId,
      listSelect: [],
      listBase: [],
      ListIdSelect: [],
      ListIdChecked: [],
      IsRequest: false
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
    this.searchModule();
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
    for (var item of this.listSelect) {
      item.Type = 3;
    }
    this.practiceService.getPriceModule(this.listSelect).subscribe(data => {
      if (data) {
        this.listSelect = data;
        this.activeModal.close(this.listSelect);
      }
    }, error => {
      this.messageService.showError(error);
    });
    // this.activeModal.close(this.listSelect);
  }

  closeModal() {
    this.activeModal.close(false);
  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listBase.forEach(element => {
        element.Checked = this.checkedTop;
      });
    } else {
      this.listSelect.forEach(element => {
        element.Checked = this.checkedBot;
      });
    }
  }

}
