import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { ChooseMaterialService } from '../services/choose-material.service';

@Component({
  selector: 'app-choose-material-for-template',
  templateUrl: './choose-material-for-template.component.html',
  styleUrls: ['./choose-material-for-template.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseMaterialForTemplateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private materialService: ChooseMaterialService,
  ) { }

  isAction: boolean = false;
  listSelect: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  listData: any = [];
  IsRequest: boolean;
  ProductCode: '';
  modelSearch: any = {
    MaterialCode: '',
    MaterialName: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }

  ngOnInit() {
    this.modelSearch.ProductCode = this.ProductCode;
    this.searchMaterial();
  }

  searchOptions: any = {
    FieldContentName: 'MaterialCode',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên vật tư',
        FieldName: 'MaterialName',
        Placeholder: 'Nhập tên vật tư ...',
        Type: 'text'
      },
    ]
  };

  searchMaterial() {
    // this.listSelect.forEach(element => {
    //   this.modelSearch.ListIdSelect.push(element.Id);
    // });
    // this.listData.forEach(element => {
    //   if (element.Checked) {
    //     this.modelSearch.ListIdChecked.push(element.Id);
    //   }
    // });
    this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
      this.listData = data;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
      this.activeModal.close();
    })
  }


  addRow() {
    let index = 1;
    this.listData.forEach(element => {
      if (element.Checked) {
        //this.listSelect.push(Object.assign({}, element));
        this.listSelect.push(element);
        index++;
      }
    });

    this.listSelect.forEach(element => {
      var index = this.listData.indexOf(element);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
    });
    for (var item of this.listData) {
      item.Index = index;
      index++;
    }
  }

  removeRow() {
    let index = 1;
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

    for (var item of this.listData) {
      item.Index = index;
      index++;
    }
  }

  // removeRow() {
  //   this.listSelect.forEach(element => {
  //     if (element.Checked) {
  //       this.listData.push(element);
  //     }
  //   });
  //   this.listData.forEach(element => {
  //     var index = this.listSelect.indexOf(element);
  //     if (index > -1) {
  //       this.listSelect.splice(index, 1);
  //     }
  //   });
  // }

  clear() {
    this.modelSearch = {
    }
    this.listSelect = [];
    this.modelSearch.ProductCode = this.ProductCode;
    this.searchMaterial();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
