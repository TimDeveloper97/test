import { Component, OnInit } from '@angular/core';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { DateUtils } from 'src/app/shared';

@Component({
  selector: 'app-test-search-new',
  templateUrl: './test-search-new.component.html',
  styleUrls: ['./test-search-new.component.scss']
})
export class TestSearchNewComponent implements OnInit {

  constructor(
    private materialGroupService: MaterialGroupService,
    public dateUtils: DateUtils,
  ) { }

  searchModel: any = {
    MaterialName: '',
    MaterialStatus: '',
    MaterialDate: null,
    SearchContent: '',
    MaterialBuyDateToV: null,
    MaterialBuyDateFromV: null,
    MaterialPrice: '',
    MaterialPriceType: 1
  };

  searchOptions: any = {
    FieldContentName: 'SearchContent',
    Placeholder: 'Tìm kiếm theo tên hoặc mã ...',
    Items: [
      {
        Name: 'Tên vật tư',
        FieldName: 'MaterialName',
        Placeholder: 'Nhập tên vật tư ...',
        Type: 'text'
      },
      {
        Name: 'Giá vật tư',
        FieldName: 'MaterialPrice',
        FieldNameType: 'MaterialPriceType',
        Placeholder: 'Nhập giá vật tư ...',
        Type: 'number'
      },
      {
        Name: 'Tình trạng vật tư',
        FieldName: 'MaterialStatus',
        Placeholder: 'Nhập tên vật tư ...',
        Type: 'select',
        Data: [{ Id: 0, Name: 'Còn sử dụng' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Ngày mua vật tư',
        FieldNameTo: 'MaterialBuyDateToV',
        FieldNameFrom: 'MaterialBuyDateFromV',
        Type: 'date'
      },
      {
        Name: 'Ngày mua vật tư',
        FieldName: 'MaterialDate',
        Type: 'dropdown',
        Data: [{ Id: 0, Name: 'Còn sử dụng' }],
        Columns: [{ Name: 'Id', Title: 'Id' }, { Name: 'Name', Title: 'Tình trạng' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn đi'
      },
    ]
  };

  ngOnInit() {

  }

  search() {
    if (this.searchModel.MaterialBuyDateToV) {
      this.searchModel.MaterialBuyDateTo = this.dateUtils.convertObjectToDate(this.searchModel.MaterialBuyDateToV);
    }

    console.log(this.searchModel);
  }

  clear() {
    this.searchModel = {
      MaterialName: '',
      MaterialStatus: '',
      MaterialDate: null,
      SearchContent: ''
    };
  }
}
