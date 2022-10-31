import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { FuturePersonnelForecastService } from '../service/future-personnel-forecast.service';
import { SelectProjectComponent } from '../select-project/select-project.component';

@Component({
  selector: 'app-future-personnel-forecast',
  templateUrl: './future-personnel-forecast.component.html',
  styleUrls: ['./future-personnel-forecast.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FuturePersonnelForecastComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    public forecastService: FuturePersonnelForecastService,
    public comboboxService: ComboboxService,
    public dateUnity: DateUtils
  ) { }
  listData = [];
  listProject = [];
  startIndex = 1;
  selectIndex = -1;
  listProjectProduct: any = [];
  listExplanedId: any = [];
  dateOfBirth: number;
  minDateNotificationV: NgbDateStruct;
  isSearchConditon: boolean;
  sbuId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;

  ngOnInit() {
    this.appSetting.PageTitle = "Dự báo nhân sự tương lai";
    this.model.SBUId = this.sbuId;
    this.getListProject();
    //this.search();
  }

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    ModuleGroupId: '',
    DateStart: '',
    DateEnd: '',
    SBUId: '',
    DepartmentId: '',
    ProductId: '',
    DateStartV: '',
    DateEndV: '',
    listBase: []
  }

  reportProjectModel: any = {
    Id: '',
    Index: 0
  }

  direction: string = 'horizontal'
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhân viên',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 0
      },
    ]
  };

  tableHeight = 400;

  search() {
    
    this.tableHeight = window.innerHeight - 180;

    this.forecastService.SearchPlan(this.model).subscribe((data: any) => {
      if (data) {
        this.listProjectProduct = data.listResult;
        this.listProjectProduct.forEach(element => {
          this.listExplanedId.push(element.Id);
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });

  }
  getListProject() {
    this.comboboxService.getListProject().subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  idSelected: string;
  onSelectionChanged(e) {
    this.selectIndex = e.selectedRowKeys[0];
    this.idSelected = e.selectedRowKeys[0];
  }


  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      ModuleGroupId: '',
      DateStart: '',
      DateEnd: '',
      SBUId: '',
      DepartmentId: '',
      listBase: []
    }
    this.search();
  }
  listBase = []
  showSelectproject() {
    let activeModal = this.modalService.open(SelectProjectComponent, { container: 'body', windowClass: 'select-Project-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listBase.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      this.listBase = [];
      if (result && result.length > 0) {
        result.forEach(element => {
          var value = Object.assign({}, this.reportProjectModel);
          value.Id = element.Id;
          value.Index = element.Index;
          this.listBase.push(value);
        });

        this.model.listBase = this.listBase;
        this.search();
      }
    }, (reason) => {

    });
  }
  sizes = {
    percent: {
      area1: 40,
      area2: 60,
    },
    pixel: {
      area1: 120,
      area2: '*',
      area3: 160,
    },
  }
  dragEnd(unit, { sizes }) {
    if (unit === 'percent') {
      this.sizes.percent.area1 = sizes[0];
      this.sizes.percent.area2 = sizes[1];
    }
    else if (unit === 'pixel') {
      this.sizes.pixel.area1 = sizes[0];
      this.sizes.pixel.area2 = sizes[1];
      this.sizes.pixel.area3 = sizes[2];
    }
  }
}
