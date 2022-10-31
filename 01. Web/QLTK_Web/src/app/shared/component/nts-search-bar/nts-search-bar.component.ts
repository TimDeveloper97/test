import { Component, ViewEncapsulation, OnInit, Input, Output, EventEmitter, forwardRef, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { ComboboxService } from '../../services/combobox.service';
import { Constants } from '../../common/Constants';
import { DateUtils } from '../../common/date-utils';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'nts-search-bar',
  templateUrl: './nts-search-bar.component.html',
  styleUrls: ['./nts-search-bar.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NTSSearchBarComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class NTSSearchBarComponent implements ControlValueAccessor {

  constructor(private _cd: ChangeDetectorRef,
    private comboboxService: ComboboxService,
    public constants: Constants,
    private dateUtils: DateUtils,
    private routeA: ActivatedRoute,
    ) { }

  count = 0;
  @Input()
  get options() { return this._options };
  set options(value: any) {
    this._options = value;
    this.initOptions();
  };

  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  public _options: any = {};
  public _searchModel: any = {};
  public _searchModelView: any = {};
  private _searchItemId = 0;
  public _searchItems: any[] = [];
  public _searchValues: any[] = [];

  @Output('change') changeEvent = new EventEmitter();
  disabled = false;

  ngOnInit() {

  }

  initOptions() {
    this._searchItems = [];
    for (let i = 0; i < this._options.Items.length; i++) {
      this._options.Items[i].Index = i;
      this._searchItems.push({
        Id: i,
        Name: this._options.Items[i].Name,
        Value: null,
        FieldName: this._options.Items[i].FieldName,
        FieldNameTo: this._options.Items[i].FieldNameTo,
        FieldNameFrom: this._options.Items[i].FieldNameFrom,
        FieldNameType: this._options.Items[i].FieldNameType,
        Type: this._options.Items[i].Type,
        ChangePlan: this._options.Items[i].ChangePlan,
        Permission: this._options.Items[i].Permission,
        Checked: false,
        ObjectId: this._options.Items[i].ObjectId,
        IsRelation: this._options.Items[i].IsRelation,
        RelationIndexTo: this._options.Items[i].RelationIndexTo
      });

      if (this._options.Items[i].DataType) {
        //this.getDataByDataType(this._options.Items[i], true);
      }
    }
  }

  writeValue(value: any | any[]): void {

    if (value) {
      this._searchModel = value;
      this._searchModelView = Object.assign({}, value);
      this._searchValues = [];

      for (let i = 0; i < this._searchItems.length; i++) {
        this._searchItems[i].Checked = false;
        this._searchItems[i].Value = null;
        if (!this._options.Items[i].DataType) {
          if (this._searchItems[i].Type != 'date') {
            if ((this._searchModel[this._searchItems[i].FieldName] != null
              && this._searchModel[this._searchItems[i].FieldName] != ''
              && this._searchModel[this._searchItems[i].FieldName] != undefined) || this._searchModel[this._searchItems[i].FieldName] === false) {

              if (this._options.Items[i].Type == 'select') {
                this.setValueDefaut(this._options.Items[i]);
              }else {
                this._searchItems[i].Checked = true;
                this._searchItems[i].Value = this._searchModel[this._searchItems[i].FieldName];
              }
            }
          }
          else {
            if (this._searchModel[this._searchItems[i].FieldNameFrom] != null
              && this._searchModel[this._searchItems[i].FieldNameFrom] != ''
              && this._searchModel[this._searchItems[i].FieldNameFrom] != undefined) {
              this._searchItems[i].Checked = true;
              this._searchItems[i].Value = (this._searchModel[this._searchItems[i].FieldNameFrom].day < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameFrom].day : this._searchModel[this._searchItems[i].FieldNameFrom].day) + '/' + (this._searchModel[this._searchItems[i].FieldNameFrom].month < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameFrom].month : this._searchModel[this._searchItems[i].FieldNameFrom].month) + '/' + this._searchModel[this._searchItems[i].FieldNameFrom].year;
            }

            if (this._searchModel[this._searchItems[i].FieldNameTo] != null
              && this._searchModel[this._searchItems[i].FieldNameTo] != ''
              && this._searchModel[this._searchItems[i].FieldNameTo] != undefined) {
              this._searchItems[i].Checked = true;
              this._searchItems[i].Value += ' - ' + (this._searchModel[this._searchItems[i].FieldNameTo].day < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameTo].day : this._searchModel[this._searchItems[i].FieldNameTo].day) + '/' + (this._searchModel[this._searchItems[i].FieldNameTo].month < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameTo].month : this._searchModel[this._searchItems[i].FieldNameTo].month) + '/' + this._searchModel[this._searchItems[i].FieldNameTo].year;
            }
          }

          if (
            this._searchItems[i].Checked && this._options.Items[i].Type != 'select') {
            this._searchValues.push({
              Name: this._searchItems[i].Name,
              Value: this._searchItems[i].Value,
              Index: i
            });
          }
        }

      }

      this.getData();
    }
    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    this._cd.markForCheck();
  }

  searchItemChange(event) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value >= 0) {
      this._searchItems[value].Checked = true;
      this.count++;
    }
  }

  removeItem(index) {
    this._searchItems[index].Checked = false;
    this.count--;
  }

  removeSearch(index, itemIndex) {
    this._searchItems[itemIndex].Checked = false;
    this._searchItems[itemIndex].Value = null;
    if (this._searchItems[itemIndex].Type == 'date') {
      this._searchModelView[this._searchItems[itemIndex].FieldNameFrom] = null;
      this._searchModelView[this._searchItems[itemIndex].FieldNameTo] = null;
    } else if (this._searchItems[itemIndex].Type == 'number') {
      this._searchModelView[this._searchItems[itemIndex].FieldName] = null;
      this._searchModelView[this._searchItems[itemIndex].FieldNameType] = null;
    }else if (this._searchItems[itemIndex].Type == 'StageStatus') {
      this._searchModelView[this._searchItems[itemIndex].FieldName] = null;
      this._searchModelView[this._searchItems[itemIndex].FieldNameType] = null;
    }
    else {
      this._searchModelView[this._searchItems[itemIndex].FieldName] = null;
    }

    this._searchValues.splice(index, 1);
    this.count--;

    this._searchModel = Object.assign({}, this._searchModelView);

    this._onChange(this._searchModel);
    this.changeEvent.emit(this._searchModel);

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(this._options.Items[this._options.Items[index].RelationIndexTo], false);
    }

  }

  search() {
    this._options;
    this._searchModelView;
    this._searchModel;
    this._searchValues = [];
    this.count = 0;
    for (let i = 0; i < this._searchItems.length; i++) {
      if (this._searchItems[i].Checked && this._searchItems[i].Value) {
        if(this._searchItems[i].Type !='StageStatus'){
          this._searchValues.push({
            Name: this._searchItems[i].Name,
            Value: this._searchItems[i].Value,
            Index: i
          });
          this.count++;
        }else{
          var item  = this._options.Items[i];
          this.setValueDefautStage(item);
        }
      }
      else {
        this._searchItems[i].Checked = false;
        if (this._searchItems[i].Type == 'date') {
          this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
          this._searchModelView[this._searchItems[i].FieldNameTo] = null;
        }else if (this._searchItems[i].Type == 'number') {
          this._searchModelView[this._searchItems[i].FieldName] = null;
          this._searchModelView[this._searchItems[i].FieldNameType] = null;
        }else if (this._searchItems[i].Type =='StageStatus') {
          this._searchModelView[this._searchItems[i].FieldName] = null;
          this._searchModelView[this._searchItems[i].FieldNameType] = null;
        }
        else {
          this._searchModelView[this._searchItems[i].FieldName] = null;
        }
      }
    }

    this._searchModel = Object.assign({}, this._searchModelView);

    this._onChange(this._searchModel);
    this.changeEvent.emit(this._searchModel);
  }

  selectChange(event, index, name) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value) {
      this._searchItems[index].Value = event.currentTarget.selectedOptions[0].text;
    }
    else {
      this._searchItems[index].Value = ''
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(this._options.Items[this._options.Items[index].RelationIndexTo], false);
    }
  }

  selectChangeExpressionType(event, index, name) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value && this._searchModelView[name] != null && this._searchModelView[name] != '') {
      this._searchItems[index].Value = event.currentTarget.selectedOptions[0].text + ' ' + this._searchModelView[name];
    }
    else {
      this._searchItems[index].Value = ''
    }
  }

  searchContentChange(contentName) {
    this._searchModel[contentName] = this._searchModelView[contentName];
    this._onChange(this._searchModel);
  }

  textChange(index, name) {
    this._searchItems[index].Value = this._searchModelView[name];
  }

  numberChange(index, name, nameType) {
    if (this._searchModelView[nameType] > 0) {
      this._searchItems[index].Value = this.constants.SearchExpressionTypes[this._searchModelView[nameType] - 1].Name + ' ' + this._searchModelView[name];
    }
    else {
      this._searchItems[index].Value = '';
    }
  }

  numberChangeYear(index, name) {
    this._searchItems[index].Value = this._searchModelView[name];
  }

  showPopover(popover) {
    if (popover.isOpen()) {
      popover.close();
    } else {
      this.count = 0;

      this._searchModelView = Object.assign({}, this._searchModel);

      for (let i = 0; i < this._searchItems.length; i++) {
        this._searchItems[i].Checked = false;
        this._searchItems[i].Value = null;

        for (let j = 0; j < this._searchValues.length; j++) {
          if (this._searchValues[j].Index == i) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = this._searchValues[j].Value;
          }
        }

        if (!this._searchItems[i].Checked) {
          if (this._searchItems[i].Type == 'date') {
            this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
            this._searchModelView[this._searchItems[i].FieldNameTo] = null;
          } else if (this._searchItems[i].Type == 'number') {
            this._searchModelView[this._searchItems[i].FieldName] = null;
            this._searchModelView[this._searchItems[i].FieldNameType] = null;
          }else if (this._searchItems[i].Type =='StageStatus') {
            this._searchModelView[this._searchItems[i].FieldName] = null;
            this._searchModelView[this._searchItems[i].FieldNameType] = null;
          }
          else {
            this._searchModelView[this._searchItems[i].FieldName] = null;
          }
        }
      }

      popover.open();
    }
  }

  dateChange(index, nameFrom, nameTo) {
    this._searchItems[index].Value = '';
    if (this._searchModelView[nameFrom]) {
      this._searchItems[index].Value = (this._searchModelView[nameFrom].day < 10 ? '0' + this._searchModelView[nameFrom].day : this._searchModelView[nameFrom].day) + '/' + (this._searchModelView[nameFrom].month < 10 ? '0' + this._searchModelView[nameFrom].month : this._searchModelView[nameFrom].month) + '/' + this._searchModelView[nameFrom].year;
    }

    if (this._searchModelView[nameTo]) {
      this._searchItems[index].Value += ' - ' + (this._searchModelView[nameTo].day < 10 ? '0' + this._searchModelView[nameTo].day : this._searchModelView[nameTo].day) + '/' + (this._searchModelView[nameTo].month < 10 ? '0' + this._searchModelView[nameTo].month : this._searchModelView[nameTo].month) + '/' + this._searchModelView[nameTo].year;
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(this._options.Items[this._options.Items[index].RelationIndexTo], false);
    }
  }

  dropdownChange(index) {
    let displayName = this._options.Items[index].DisplayName;
    let valueName = this._options.Items[index].ValueName;
    let valueSelect = this._searchModelView[this._options.Items[index].FieldName];
    if(this._options.Items[index].SelectMode !='multiple'){
      let selected = this._options.Items[index].Data.filter(function (data) {
        if (data[valueName] == valueSelect) {
          return data;
        }
      });
  
      if (selected && selected.length > 0) {
        this._searchItems[index].Value = selected[0][displayName];
      }
      else {
        this._searchItems[index].Value = null;
      }
    }else{
      var result =[];
      let selected = this._options.Items[index].Data.filter(function (data) {
        valueSelect.forEach( value =>{
          if (data[valueName] == value) {
            result.push(data);
          }
        });
        return result;
      });
  
      if (result && result.length > 0) {
        this._searchItems[index].Value =null;
        result.forEach(r =>{
          this._searchItems[index].Value = this._searchItems[index].Value == null? r[displayName] : this._searchItems[index].Value + ", "+ r[displayName];
        })
      }
      else {
        this._searchItems[index].Value = null;
      }
    }
      
    if (this._options.Items[index].IsRelation) {
      this._options.Items[this._options.Items[index].RelationIndexTo].ObjectId = valueSelect;
      this.getDataByDataType(this._options.Items[this._options.Items[index].RelationIndexTo], false);
    }
  }

  getData() {
    for (let i = 0; i < this._options.Items.length; i++) {
      if (this._options.Items[i].DataType && (!this._options.Items[i].RelationIndexFrom || this._options.Items[i].RelationIndexFrom == 0)) {
        this.getDataByDataType(this._options.Items[i], true);
      }
    }
  }

  setValueDefaut(item) {
    var isExist = false;
    for (let i = 0; i < this._searchItems.length; i++) {
      if (item.Index == this._searchItems[i].Id) {
        if ((this._searchModel[this._searchItems[i].FieldName] != null
          && this._searchModel[this._searchItems[i].FieldName] != ''
          && this._searchModel[this._searchItems[i].FieldName] != undefined) || this._searchModel[this._searchItems[i].FieldName] === false) {

          let valueName = item.ValueName;
          let valueSelect = this._searchModelView[item.FieldName];
          let selected = item.Data.filter(function (data) {
            if (data[valueName] == valueSelect) {
              return data;
            }
          });

          if (selected && selected.length > 0) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = selected[0][item.DisplayName];

            isExist = false;
            for (let index = 0; index < this._searchValues.length; index++) {
              if (this._searchValues[index].Index == i) {
                isExist = true;
                this._searchValues[index].Name = this._searchItems[i].Name;
                this._searchValues[index].Value = this._searchItems[i].Value;
              }
            }

            if (!isExist) {
              this._searchValues.push({
                Name: this._searchItems[i].Name,
                Value: this._searchItems[i].Value,
                Index: i
              });
            }
          }
        }
      }
    }
  }
  setValueDefautStage(item) {
    var isExist = false;
    for (let i = 0; i < this._searchItems.length; i++) {
      if (item.Index == this._searchItems[i].Id) {
          let valueName = item.ValueName;
          let valueSelect = this._searchModelView[item.FieldName];
          let valueSelectStatus = this._searchModelView[item.FieldNameType];

          let selected = item.Data.filter(function (data) {
            if (data[valueName] == valueSelect) {
              return data;
            }
          });
          let selectedStageStatus = this.constants.StageStatus.filter(function (data) {
            if (data[valueName] == valueSelectStatus) {
              return data;
            }
          });

          if (selected && selected.length > 0) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = selected[0][item.DisplayName] +" "+ selectedStageStatus[0].Name;

            isExist = false;
            for (let index = 0; index < this._searchValues.length; index++) {
              if (this._searchValues[index].Index == i) {
                isExist = true;
                this._searchValues[index].Name = this._searchItems[i].Name;
                this._searchValues[index].Value = this._searchItems[i].Value;
              }
            }

            if (!isExist) {
              this._searchValues.push({
                Name: this._searchItems[i].Name,
                Value: this._searchItems[i].Value,
                Index: i
              });
            }
          }
      }
    }
  }

  getDataByDataType(item, isLoad: boolean) {
    switch (item.DataType) {
      // danh sách hãng
      case this.constants.SearchDataType.Manuafacture:
        this.getCbbManufacture(item, isLoad);
        break;

      // nhóm tpa
      case this.constants.SearchDataType.GroupTPA:
        this.getCbbTPA(item, isLoad);
        break;

      // danh sách sbu
      case this.constants.SearchDataType.SBU:
        this.getCbbSBU(item, isLoad);
        break;

      // danh sách phòng ban
      case this.constants.SearchDataType.Department:
        this.GetCbbDepartment(item, isLoad);
        break;

      // danh sách trưởng phòng
      case this.constants.SearchDataType.Manager:
        this.GetCbbManager(item, isLoad);
        break;

      // danh sách chuyên môn
      case this.constants.SearchDataType.Specialize:
        this.GetCbbSpecialize(item, isLoad);
        break;

      // danh sách đơn vị công tác
      case this.constants.SearchDataType.WorkPlace:
        this.GetCbbWorkPlace(item, isLoad);
        break;

      // danh sách bằng cấp
      case this.constants.SearchDataType.Degree:
        this.GetCbbDegree(item, isLoad);
        break;

      // danh sách phòng học
      case this.constants.SearchDataType.ClassRoom:
        this.GetCbbClassRoom(item, isLoad);
        break;

      // danh sách loại khách hàng
      case this.constants.SearchDataType.CustomerType:
        this.GetCbbCustomerType(item, isLoad);
        break;

      case this.constants.SearchDataType.Customer:
        this.GetCbbCustomer(item, isLoad);
        break;

      // danh sách nhân viên
      case this.constants.SearchDataType.Employee:
        this.GetCbbEmployee(item, isLoad);
        break;
      // danh sách công đoạn
      case this.constants.SearchDataType.Stage:
        this.GetCbbStage(item, isLoad);
        break;

      case this.constants.SearchDataType.Project:
        this.GetCbbProject(item, isLoad);
        break;

      case this.constants.SearchDataType.Job:
        this.GetCbbJob(item, isLoad);
        break;

      case this.constants.SearchDataType.PracaticeGroup:
        this.getCbbPracticeGroup(item, isLoad);
        break;

      case this.constants.SearchDataType.ModuleGroup:
        this.getCbbModuleGroup(item, isLoad);
        break;

      case this.constants.SearchDataType.SkillGroup:
        this.getCbbSkillGroup(item, isLoad);
        break;

      case this.constants.SearchDataType.MaterialGroup:
        this.getCbbMaterialGroup(item, isLoad);
        break;

      case this.constants.SearchDataType.Task:
        this.getListTask(item, isLoad);
        break;


      case this.constants.SearchDataType.ResponsiblePersion:
        this.getListEmployeesStatus(item, isLoad);
        break;

      case this.constants.SearchDataType.ModuleGroupParentChild:
        this.getListModuleGroupParentChild(item, isLoad);
        break;

      case this.constants.SearchDataType.Product:
        this.getListProduct(item, isLoad);
        break;

      case this.constants.SearchDataType.JobPosition:
        this.getCbbJobPositions(item, isLoad);
        break;

      case this.constants.SearchDataType.ProjectByUser:
        this.GetCbbProjectByUser(item, isLoad);
        break;
      //danh sách module
      case this.constants.SearchDataType.Module:
        this.getListModule(item, isLoad);
        break;

      case this.constants.SearchDataType.Skill:
        this.getListSkill(item, isLoad);
        break;
      case this.constants.SearchDataType.ProjectByUserDate:
        this.GetCbbProjectByUserAndDate(item, isLoad);
        break;
      case this.constants.SearchDataType.ProjectByUserSBU:
        this.GetListProjectDownloadDocumentDesign(item, isLoad);
        break;
      case this.constants.SearchDataType.Employees:
        this.getCbbEmployeeList(item, isLoad);
        break;
      case this.constants.SearchDataType.SolutionGroup:
        this.getListSolutionGroup(item, isLoad);
        break;

      case this.constants.SearchDataType.ProductGroup:
        this.getListproductGroup(item, isLoad);
        break;
      case this.constants.SearchDataType.ProductStandardGroup:
        this.getListproductStandardGroup(item, isLoad);
        break;
      case this.constants.SearchDataType.TestCriteriaGroup:
        this.getListTestCriteriaGroup(item, isLoad);
        break;
      case this.constants.SearchDataType.WorkType:
        this.getWorkType(item, isLoad);
        break;
      case this.constants.SearchDataType.Country:
        this.GetCbbCountry(item, isLoad);
        break;
      case this.constants.SearchDataType.ProductStandTPAType:
        this.getProductStandTPAType(item, isLoad);
        break;
      case this.constants.SearchDataType.Supplier:
        this.getSupplier(item, isLoad);
        break;
      case this.constants.SearchDataType.Application:
        this.getApplication(item, isLoad);
        break;
      case this.constants.SearchDataType.SaleProductType:
        this.getSaleProductType(item, isLoad);
        break;
      case this.constants.SearchDataType.DocumentGroup:
        this.getCbbDocumentGroup(item, isLoad);
        break;
      case this.constants.SearchDataType.FlowStage:
        this.getCbbFlowStage(item, isLoad);
        break;
      case this.constants.SearchDataType.ErrorAffect:
        this.getCbbErrorAffect(item, isLoad);
        break;
      case this.constants.SearchDataType.QuestionGroup:
        this.getCbbQuestionGroup(item, isLoad);
        break;
      case this.constants.SearchDataType.DocumentType:
        this.getDocumentType(item, isLoad);
        break;
      case this.constants.SearchDataType.RecruitmentRequest:
        this.getAllCbbRecruitmentRequest(item, isLoad);
        break;
      case this.constants.SearchDataType.MeetingType:
        this.getMeetingType(item, isLoad);
        break;
      case this.constants.SearchDataType.Role:
          this.getRoleType(item, isLoad);
          break;
      case this.constants.SearchDataType.SupplierInProject:
        this.getSupplierbyProject(item, isLoad);
          break;
      default:
        break;
    }
  }

  getCbbQuestionGroup(item, isLoad: boolean) {
    this.comboboxService.getQuestionGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getSaleProductType(item, isLoad: boolean) {
    this.comboboxService.getCBBSaleProductType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getProductStandTPAType(item, isLoad: boolean) {
    this.comboboxService.getCBBProductStandardTPAType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getSupplier(item, isLoad: boolean) {
    this.comboboxService.getCBBSupplier().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getApplication(item, isLoad: boolean) {
    this.comboboxService.getApplication().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListSolutionGroup(item, isLoad: boolean) {
    this.comboboxService.getListSolutionGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListproductGroup(item, isLoad: boolean) {
    this.comboboxService.getListProductGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListproductStandardGroup(item, isLoad: boolean) {
    this.comboboxService.getCbbProductStandardGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListTestCriteriaGroup(item, isLoad: boolean) {
    this.comboboxService.getCbbCriter().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getWorkType(item, isLoad: boolean) {
    this.comboboxService.getListWorkType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
      }
    });
  }

  getCbbEmployeeList(item, isLoad: boolean) {
    this.comboboxService.getCbbEmployee().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbManufacture(item, isLoad: boolean) {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbTPA(item, isLoad: boolean) {
    this.comboboxService.getCbbMaterialGroupTPA().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbSBU(item, isLoad: boolean) {
    this.comboboxService.getCbbSBU().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);

        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbDepartment(item, isLoad: boolean) {
    var sbuId = '';

    if (item.RelationIndexFrom >= 0 && this._searchModelView) {
      sbuId = this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldName];
    }

    this.comboboxService.getCbbDepartmentBySBU(sbuId).subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbManager(item, isLoad: boolean) {
    this.comboboxService.getCbbManager().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbSpecialize(item, isLoad: boolean) {
    this.comboboxService.getListSpecialize().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbWorkPlace(item, isLoad: boolean) {
    this.comboboxService.getListWorkPlace().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbDegree(item, isLoad: boolean) {
    this.comboboxService.getCbbDegree().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbClassRoom(item, isLoad: boolean) {
    this.comboboxService.getListClassRoom().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbCustomerType(item, isLoad: boolean) {
    this.comboboxService.getListCustomerType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbCustomer(item, isLoad: boolean) {
    this.comboboxService.getListCustomer().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbEmployee(item, isLoad: boolean) {
    var departmentId = '';

    if (item.RelationIndexFrom >= 0 && this._searchModelView) {
      departmentId = this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldName];
    }

    this.comboboxService.GetCbbEmployeeByDepartmentId(departmentId).subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbStage(item, isLoad: boolean) {
    this.comboboxService.getCbbStage().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbProject(item, isLoad: boolean) {
    this.comboboxService.getListProject().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetListProjectDownloadDocumentDesign(item, isLoad: boolean) {
    this.comboboxService.getListProjectDownloadDocumentDesign().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbJob(item, isLoad: boolean) {
    this.comboboxService.getListJob().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbPracticeGroup(item, isLoad: boolean) {
    this.comboboxService.getCbbPracticeGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbModuleGroup(item, isLoad: boolean) {
    this.comboboxService.getListModuleGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbSkillGroup(item, isLoad: boolean) {
    this.comboboxService.getCbbSkillGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbMaterialGroup(item, isLoad: boolean) {
    this.comboboxService.getCbbMaterialGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbDocumentGroup(item, isLoad: boolean) {
    this.comboboxService.getCbbDocumentGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbFlowStage(item, isLoad: boolean) {
    this.comboboxService.getCbbFlowStage().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbErrorAffect(item, isLoad: boolean) {
    this.comboboxService.getCbbErrorAffect().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListTask(item, isLoad: boolean) {
    this.comboboxService.getListTask().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListEmployeesStatus(item, isLoad: boolean) {
    this.comboboxService.getListEmployeesStatus().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListModuleGroupParentChild(item, isLoad: boolean) {
    this.comboboxService.getListModuleGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListProduct(item, isLoad: boolean) {
    this.comboboxService.getListProductGroup().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getCbbJobPositions(item, isLoad: boolean) {
    this.comboboxService.getListJobPosition().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbProjectByUser(item, isLoad: boolean) {
    this.comboboxService.getListProjectByUser().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbCountry(item, isLoad: boolean) {
    this.comboboxService.getCbbCountry().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  GetCbbProjectByUserAndDate(item, isLoad: boolean) {
    let dateFrom = null, dateTo = null;

    if (item.RelationIndexFrom >= 0 && this._searchModelView) {
      if (this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldNameFrom]) {
        dateFrom = this.dateUtils.convertObjectToDate(this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldNameFrom]);
      }

      if (this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldNameTo]) {
        dateTo = this.dateUtils.convertObjectToDate(this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldNameTo]);
      }
    }

    this.comboboxService.getListProjectByUserAndDate({ DateFrom: dateFrom, DateTo: dateTo }).subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getListModule(item, isLoad: boolean) {
    if (!item.ObjectId) {
      this.comboboxService.getListModule().subscribe((data: any) => {
        item.Data = data;
        if (isLoad) {
          this.setValueDefaut(item);
          if (item.IsRelation) {
            this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
          }
        }
      });
    }
    else {
      this.comboboxService.GetListModuleByProjectId(item.ObjectId).subscribe((data: any) => {
        item.Data = data;
        if (isLoad) {
          this.setValueDefaut(item);
          if (item.IsRelation) {
            this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
          }
        }
      });
    }

  }

  getListSkill(item, isLoad: boolean) {
    this.comboboxService.getListSkill().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getDocumentType(item, isLoad: boolean) {
    this.comboboxService.getDocumentType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getAllCbbRecruitmentRequest(item, isLoad: boolean) {
    this.comboboxService.getAllCbbRecruitmentRequest().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getMeetingType(item, isLoad: boolean) {
    this.comboboxService.getMeetingType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getRoleType(item, isLoad: boolean) {
    this.comboboxService.getRoleType().subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }

  getSupplierbyProject(item, isLoad: boolean) {
    var Id = this.routeA.snapshot.paramMap.get('Id');

    this.comboboxService.getSupplierbyProject(Id).subscribe((data: any) => {
      item.Data = data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
        }
      }
    });
  }
}