import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { TechnologySolutionService } from '../../service/technology-solution.service';
import { DxTreeListComponent } from 'devextreme-angular';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-technology-solution-create',
  templateUrl: './technology-solution-create.component.html',
  styleUrls: ['./technology-solution-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TechnologySolutionCreateComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  @ViewChild(DxTreeListComponent) treeViewManufacture;

  constructor(
    public constant: Constants,
    public combobox: ComboboxService,
    public techService: TechnologySolutionService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private router: Router,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private modalService: NgbModal,
    private route: ActivatedRoute,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới công nghệ cho giải pháp',
    SaveText: 'Lưu',
  };

  listIndex: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Index: 0,
    IsEnable: true,
    Note: '',
    Description: '',
    CreateBy: '',
    CreateDate: null,
    UpdateBy: '',
    UpdateDate: null,
    ListSupplierGroupId: [],
    ListManufactureGroupId: [],
  }
  Id: string;
  listData: any[] = [];

  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];
  treeBoxValue: string[];
  ListSupplierGroup: any[] = [];
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];

  treeBoxValueManufacture: string[];
  ListManufactureGroup: any[] = [];
  isDropDownBoxOpenedManufacture = false;
  selectedRowKeysManufacture: any[] = [];

  ngOnInit(): void {
    this.getCbbData();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa công nghệ cho giải pháp';
      this.ModalInfo.SaveText = 'Lưu';
      this.gettechInfo();
      this.getIndex();
    }
    else {
      this.getIndex();
      this.ModalInfo.Title = "Thêm mới công nghệ cho giải pháp";

    }
  }

  getCbbData() {
    forkJoin([
      this.combobox.getCbbSupplierGroup(),
      this.combobox.getCbbManufactureGroup(),
    ]).subscribe(results => {
      this.ListSupplierGroup = results[0].ListResult;
      this.ListManufactureGroup = results[1].ListResult;
    });
  }

  getIndex() {
    this.techService.getIndex().subscribe((data: any) => {
      this.listIndex = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listIndex.splice(this.listIndex.length - 1, 1);
      }
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  save() {
    if (this.Id) {
      this.updatetech();
    }
    else {
      this.createtech();
    }
  }


  createtech() {
    this.techService.createTech(this.model).subscribe(result => {
      this.messageService.showSuccess('Thêm mới công nghệ cho giải pháp thành công!');
      this.closeModal();
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updatetech() {
    this.techService.updateTech(this.model).subscribe(result => {
      this.messageService.showSuccess('Cập nhật công nghệ cho giải pháp thành công');
      this.closeModal();
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  gettechInfo() {
    this.techService.getInfoById({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.treeBoxValue = this.model.ListSupplierGroupId;
      this.treeBoxValueManufacture = this.model.ListManufactureGroupId;
      this.selectedRowKeys = this.model.ListSupplierGroupId;
      this.selectedRowKeysManufacture = this.model.ListManufactureGroupId;
      this.getCBBSupplierGroup();
      this.getCBBManufactureGroup();
    });
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
    if (!e.value) {
      this.selectedRowKeys = [];
      this.model.ListSupplierGroupId = [];
      e.previousValue = [];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.model.ListSupplierGroupId = e.selectedRowKeys;
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  getCBBSupplierGroup() {
    this.combobox.getCbbSupplierGroup().subscribe((data: any) => {
      if (data) {
        this.ListSupplierGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  syncTreeViewSelectionManufacture(e) {
    var component = (e && e.component) || (this.treeViewManufacture && this.treeViewManufacture.instance);
    if (!e.value) {
      this.selectedRowKeysManufacture = [];
      this.model.ListManufactureGroupId = [];
      e.previousValue = [];
    }
  }

  treeView_itemSelectionChangedManufacture(e) {
    this.treeBoxValueManufacture = e.selectedRowKeys;
    this.model.ListManufactureGroupId = e.selectedRowKeys;
  }

  onRowDblClickManufacture() {
    this.isDropDownBoxOpenedManufacture = false;
  }

  closeDropDownBoxManufacture() {
    this.isDropDownBoxOpenedManufacture = false;
  }

  getCBBManufactureGroup() {
    this.combobox.getCbbManufactureGroup().subscribe((data: any) => {
      if (data) {
        this.ListManufactureGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

}
