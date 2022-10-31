import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SkillService } from '../service/skill.service';

@Component({
  selector: 'app-choose-pracite',
  templateUrl: './choose-pracite.component.html',
  styleUrls: ['./choose-pracite.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChoosePraciteComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    public constant: Constants,
    private service: SkillService
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  editField: string;
  isAction: boolean = false;
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
    PracticeGroupName: '',
    CurentVersion: '',
    TrainingTime: '',
    Note: '',
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

  ListPracaticeGroup: any = [];

  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchPractice();
  }



  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Nhóm bài thực hành',
        FieldName: 'PracaticeGroupId',
        Placeholder: 'Nhóm bài thực hành',
        Type: 'select',
        DataType: this.constant.SearchDataType.PracaticeGroup,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };
  searchPractice() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchPractice(this.modelSearch).subscribe(data => {
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
      TestCriteriaGroupId: '',
      TechnicalRequirements: '',
      Note: '',
      ListTestCriteriaModule: [],

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
    this.searchPractice();
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

  save() {
    this.modelSearch.listSelect = this.listSelect;
    this.service.addPracticeSkill(this.modelSearch).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới bài thực hành thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
