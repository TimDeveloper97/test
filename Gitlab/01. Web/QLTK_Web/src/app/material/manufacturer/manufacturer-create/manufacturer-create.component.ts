import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';

import { MessageService, ComboboxService, Constants } from 'src/app/shared';
import { ManufactureService } from '../../services/manufacture-service';
import { MaterialGroupService } from '../../services/materialgroup-service';


@Component({
  selector: 'app-manufacturer-create',
  templateUrl: './manufacturer-create.component.html',
  styleUrls: ['./manufacturer-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ManufacturerCreateComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private materialService: ManufactureService,
    private titleservice: Title,
    private combobox: ComboboxService,
    private materialGroupService: MaterialGroupService,
    public constant: Constants,
  ) {
    titleservice.setTitle("Hãng sản xuất");
  }


  ModalInfo = {
    Title: 'Thêm mới hãng sản xuất',
    SaveText: 'Lưu',
  };
  isAction: boolean = false;
  listSupplierManufacture: any[] = [];
  listManufactureGroup: any[] = [];
  listParentId: any[] = [];
  listParentName: any[] = [];
  treeBoxValue: string[];
  leadtime: any;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  materialGroupName = '';
  manufactureGroupId: '';

  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Email: '',
    Address: '',
    Phone: '',
    Origination: '',
    Description: '',
    MaterialType: '1',
    Status: '0',
    Website: '',
    ListManufactureGroupId: [],
  }

  supplierModel: any = {
    Id: '',
    ManufactureId: '',
    Code: '',
    Name: ''
  }

  Id: string;

  ngOnInit() {
    this.model.MaterialType = '1';
    this.searchSupplierManufacture();
    this.getListManufactureGroup();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa hãng sản xuất';
      this.ModalInfo.SaveText = 'Lưu';
      this.getManufactureInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm hãng sản xuất";

    }
  }

  // getListManufactureGroup() {
  //   this.combobox.getListManufactureGroup().subscribe((data: any) => {
  //     if (data) {
  //       this.listManufactureGroup = data;
  //     }
  //   },
  //     error => {
  //       this.messageService.showError(error);
  //     });
  // }

  getListManufactureGroup() {
    this.materialGroupService.getMaterialGroups().subscribe((data: any) => {
      if (data) {
        this.listManufactureGroup = data;
        if (this.manufactureGroupId != null) {
          this.model.ListManufactureGroupId.push(this.manufactureGroupId);
          this.treeBoxValue = this.model.ListManufactureGroupId;
          this.selectedRowKeys = this.treeBoxValue;

        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  createManufacture(isContinue) {
    this.model.ListManufactureGroupId = this.selectedRowKeys;
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.materialService.createManufacture(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            page: 1,
            PageSize: 10,
            totalItems: 0,
            PageNumber: 1,
            OrderBy: 'Code',
            OrderType: true,

            Id: '',
            Name: '',
            Code: '',
            Email: '',
            Address: '',
            Phone: '',
            Leadtime: '',
            Origination: '',
            Description: '',
            MaterialType: '1',
            Status: '0',
            Website: '',
            ListManufactureGroupId: []
          };
          this.materialGroupName = '';
          this.selectedRowKeys = [];
          this.messageService.showSuccess('Thêm mới hãng sản xuất thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới hãng sản xuất thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }

  updateManufacture() {
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.materialService.updateManufacture(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật hãng sản xuất thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }

  searchSupplierManufacture() {
    this.supplierModel.ManufactureId = this.Id;
    this.materialService.searchSupplierManufacture(this.supplierModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSupplierManufacture = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clear() {
    this.supplierModel = {
      Id: '',
      ManufactureId: '',
      Code: '',
      Name: ''
    }
    this.model.ManufactureId = this.Id;
    this.searchSupplierManufacture();
  }

  getManufactureInfo() {
    this.materialService.getManufactureInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.ModalInfo.Title = "Chỉnh sửa hãng sản xuất - " + this.model.Code + " - " + this.model.Name;
      //this.treeBoxValue = data.manu;
      this.treeBoxValue = this.model.ListManufactureGroupId;
      this.selectedRowKeys = this.model.ListManufactureGroupId;

    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
    if (this.materialGroupName) {
      if (this.Id) {
        this.updateManufacture();
      }
      else {
        this.createManufacture(isContinue);
      }
    }
    else {
      this.messageService.showMessage('Nhóm vật tư không được để trống!');
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }
  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  treeView_itemSelectionChanged(e) {
    this.materialGroupName = '';
    // var listId = []
    // e.selectedRowsData.forEach(item => {
    //   listId.push(item.Id);
    //   if (item.ListChildId.length > 0) {
    //     item.ListChildId.forEach(ite => {
    //       listId.push(ite);
    //       var materialGroup = this.listManufactureGroup.filter(function (data) {
    //         return data.Id == ite;
    //       });
    //       if (materialGroup[0].ListChildId.length > 0) {
    //         this.addChild(materialGroup[0], listId);
    //       }
    //     });
    //   }
    //   if (item.ParentId != null && item.ParentId.length > 0) {
    //     var parent = this.listManufactureGroup.filter(function (data) {
    //       return data.Id == item.ParentId;
    //     });
    //     if (parent.length > 0) {
    //       var count = 0;
    //       parent[0].ListChildId.forEach(ite => {
    //         var check = e.selectedRowKeys.filter(function (data) {
    //           return data == ite;
    //         });
    //         if (check.length > 0) {
    //           count++;
    //         }
    //       });
    //       if (parent[0].ListChildId.length == count) {
    //         var exist = listId.filter(function (data) {
    //           return data == parent[0].Id;
    //         });
    //         if (exist.length == 0) {
    //           listId.push(parent[0].Id);
    //         }
    //       }
    //     }
    //   }
    // });
    // this.treeBoxValue = listId;
    // e.selectedRowKeys = listId;
    // this.model.ListManufactureGroupId = e.selectedRowKeys;
    // this.listManufactureGroup.forEach(item => {
    //   listId.forEach(ite => {
    //     if (item.Id == ite) {
    //       this.materialGroupName += item.Name + ", ";
    //     }
    //   });
    // });
    e.selectedRowsData.forEach(item => {
      this.materialGroupName += item.Name + ", ";
    });
    if (this.materialGroupName.length > 0) {
      this.materialGroupName = this.materialGroupName.substring(0, this.materialGroupName.length - 2);
    }
  }

  addChild(item, listId) {
    item.ListChildId.forEach(ite => {
      listId.push(ite)
      var materialGroup = this.listManufactureGroup.filter(function (data) {
        return data.Id == ite;
      });
      if (materialGroup[0].ListChildId.length > 0) {
        this.addChild(materialGroup[0], listId);
      }
    });
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
