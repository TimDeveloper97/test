import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, Constants, FileProcess, MessageService , ComboboxService} from 'src/app/shared';
import { ModuleServiceService } from '../../services/module-service.service';

@Component({
  selector: 'app-module-choose-designer-tab',
  templateUrl: './module-choose-designer-tab.component.html',
  styleUrls: ['./module-choose-designer-tab.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleChooseDesignerTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private combobox: ComboboxService,
    private service: ModuleServiceService,
  ) { }

  ModuleId: string;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  ListModuleId: any = [];
  listSBU: any[] = [];
  listDepartment: [] = [];
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
    ModuleId: '',
    EmployeeId: '',
    SBUId: '',
    DepartmentId: '',
    listData: [],
    ListEmployee: [],
    ListIdSelect: [],
    ListIdChecked: [],
  }

  ngOnInit() {
    this.modelSearch.ModuleId = this.ModuleId;
    this.getCbbSBU();
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchEmployee();
  }

  searchEmployee() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchEmployee(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  getCbbSBU() {
    this.combobox.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


  GetCbbDepartment() {
    this.combobox.getCbbDepartmentBySBU(this.modelSearch.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  choose() {
    this.modelSearch.ListEmployee = this.listSelect;
    this.service.AddDesigner(this.modelSearch).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật nhà thiết kế thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.activeModal.close(true);
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
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Code: '',
      Name: '',
      SBUId: '',
      DepartmentId: '',
      JobPositionId: '',
      Status: '',
      ListIdSelect: [],
      ListIdChecked: [],
      ListEmployee: []
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
    this.searchEmployee();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
