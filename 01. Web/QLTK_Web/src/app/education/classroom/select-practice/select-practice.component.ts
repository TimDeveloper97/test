import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { ClassRoomService } from '../../service/class-room.service';

@Component({
  selector: 'app-select-practice',
  templateUrl: './select-practice.component.html',
  styleUrls: ['./select-practice.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SelectPracticeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private classRoomService: ClassRoomService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  ClassRoomId: string;
  isAction: boolean = false;
  listSelect: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  listData: any = [];
  IsRequest: boolean;

  modelSearch: any = {
    TotalItem: 0,

    Id: '',
    Code: '',
    Name: '',
    PracticeGroupId: '',
    listData: [],
    ListIdSelect: [],
    ListIdChecked: [],
  }

  ngOnInit() {
    this.modelSearch.ClassRoomId = this.ClassRoomId;
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
        Name: 'Tên bài thực hành',
        FieldName: 'Name',
        Placeholder: 'Nhập tên bài thực hành ...',
        Type: 'text'
      },
      {
        Name: 'Nhóm bài thực hành',
        FieldName: 'PracticeGroupId',
        Placeholder: 'Nhóm bài thực hành',
        Type: 'select',
        DataType: this.constants.SearchDataType.PracaticeGroup,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  searchPractice() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    //this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
    this.classRoomService.searchPractice(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
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

  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
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
      TotalItem: 0,
      Id: '',
      Code: '',
      Name: '',
      PracticeGroupId: '',
      listData: [],
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

  choose() {
    var a = 0;
    for (var item of this.listSelect) {
      if (item.Quantity <= 0) {
        a++;
      }
    }
    if (a > 0) {
      this.messageService.showMessage("Bạn không được để trống số lượng thiết bị")
    }
    else {
      this.classRoomService.GetPricePractice(this.listSelect).subscribe(data => {
        if (data) {
          this.listSelect = data;
          this.activeModal.close(this.listSelect);
        }
      }, error => {
        this.messageService.showError(error);
      });
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
