import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { MaterialService } from 'src/app/material/services/material-service';
import { ClassRoomService } from 'src/app/education/service/class-room.service';

@Component({
  selector: 'app-select-material',
  templateUrl: './select-material.component.html',
  styleUrls: ['./select-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SelectMaterialComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private materialService: MaterialService,
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
    MaterialGroupId: '',
    MaterialGroupName: '',
    Note: '',
    listData: [],
    ListIdSelect: [],
    ListIdChecked: [],
    Quantity:'1'
  }

  ngOnInit() {
    this.modelSearch.ClassRoomId = this.ClassRoomId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchMaterial();
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên vật tư',
        FieldName: 'Name',
        Placeholder: 'Nhập tên vật tư ...',
        Type: 'text'
      },
      {
        Name: 'Nhóm vật tư',
        FieldName: 'MaterialGroupId',
        Placeholder: 'Nhóm vật tư',
        Type: 'select',
        DataType: this.constants.SearchDataType.MaterialGroup,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  searchMaterial() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    //this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
    this.classRoomService.searchMaterial(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }



  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        element.Type = 1;
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
      MaterialGroupId: '',
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
    this.searchMaterial();
  }

  choose() {
    var a = 0;
    for (var item of this.listSelect) {
      if (item.Quantity <= 0) {
        a++;
      }
      item.Type=1;
    }
    if (a > 0) {
      this.messageService.showMessage("Bạn không được để trống số lượng thiết bị")
    }
    else {
      this.activeModal.close(this.listSelect);
    }
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
