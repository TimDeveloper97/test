import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import { SubjectsService } from 'src/app/education/service/subjects.service';

@Component({
  selector: 'app-subject-select-skill',
  templateUrl: './subject-select-skill.component.html',
  styleUrls: ['./subject-select-skill.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SubjectSelectSkillComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private subjectsService: SubjectsService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  SubjectsId: string;
  isAction: boolean = false;
  listSelect: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  listBase: any = [];
  IsRequest: boolean;

  modelSearch: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    Name: '',
    RoomTypeName: '',
    Description: '',
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã kỹ năng',
    Items: [
      {
        Name: 'Tên kỹ năng',
        FieldName: 'Name',
        Placeholder: 'Nhập tên kỹ năng',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.modelSearch.SubjectsId = this.SubjectsId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchSkill();
  }

  searchSkill() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.subjectsService.searchSkill(this.modelSearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
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

  clear() {
    this.modelSearch = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Code: '',
      Name: '',
      RoomTypeName: '',
      RoomTypeId: '',
      Description: '',
      listBase: [],
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
    this.searchSkill();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
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
