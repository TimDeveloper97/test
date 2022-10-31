import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NsMaterialGroupService } from '../../services/ns-material-group.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-ns-material-group-manage',
  templateUrl: './ns-material-group-manage.component.html',
  styleUrls: ['./ns-material-group-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NsMaterialGroupManageComponent implements OnInit {
  model: any = {
    Id: '',
    Code: '',
    Name: '',
    ManufactureId: '',
    PageSize: 10,
    PageNumber: 1,
    TotalItem: 0,
  };

  deleteModel = {
    Id: ''
  }
  index = 1;
  listData = [];
  listManufacture = [];
  listNSMaterialType = [];
  selectIndex = -1;

  constructor(
    public appset: AppSetting,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private nsMaterialGroupService: NsMaterialGroupService,
    private router: Router,
    public constant: Constants
  ) { }


  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nhóm vật tư',
    Items: [
      {
        Name: 'Tên nhóm vật tư',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nhóm vật tư',
        Type: 'text'
      },
      {
        Name: 'Hãng',
        FieldName: 'ManufactureId',
        Placeholder: 'Hãng sản xuất',
        Type: 'select',
        DataType: this.constant.SearchDataType.Manuafacture,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  ngOnInit() {
    this.appset.PageTitle = "Nhóm vật tư phi tiêu chuẩn";
    this.getCbbManufacture();
    this.getCbbNSMaterialType();
    this.searchNSMaterialGroup();
  }

  getCbbManufacture() {
    this.comboboxService.getCbbManufacture().subscribe(
      data => {
        this.listManufacture = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbNSMaterialType() {
    this.comboboxService.getCbbNSMaterialType().subscribe(
      data => {
        this.listNSMaterialType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  startIndex = 0;
  searchNSMaterialGroup() {
    this.nsMaterialGroupService.searchNSMaterialGroup(this.model).subscribe(
      data => {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListNSMaterialGroup;
        this.model.TotalItem = data.TotalItem;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  loadParam(index) {
    this.selectIndex = index;
  }

  create() {
    this.router.navigate(['vat-tu/nhom-vat-tu-phi-tieu-chuan/them-moi']);
  }

  update(id) {
    this.router.navigate(['vat-tu/nhom-vat-tu-phi-tieu-chuan/chinh-sua', id]);
  }

  delete(id) {
    this.deleteModel.Id = id;
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa vật tư phi tiêu chuẩn này không?").then(
      data => {
        if (data) {
          this.nsMaterialGroupService.deleteNSMaterialGroup(this.deleteModel).subscribe(
            data => {
              this.messageService.showSuccess("Xóa vật tư phi tiêu chuẩn thành công!");
              this.searchNSMaterialGroup();
            },
            error => {
              this.messageService.showError(error);
            }
          );
        }

      },
      error => {
        
      }
    );
  }

  clear() {
    this.model = {
      Id: '',
      Code: '',
      Name: '',
      ManufactureId: '',
      NSMaterialTypeId: '',
      PageSize: 10,
      PageNumber: 1,
      Page: 1,
      TotalItem: 0,
    };
    this.searchNSMaterialGroup();
  }
}
