import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService,Constants } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';

@Component({
  selector: 'app-show-choose-caree',
  templateUrl: './show-choose-caree.component.html',
  styleUrls: ['./show-choose-caree.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseCareeComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public constant: Constants,
    public saleProductService: SaleProductService,
  ) { }

  listCaree:any[]=[];
  listCareeSelect = [];
  listCareeId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;

  modelSearch: any = {
    Name: '',
    Code: '',
    Description: '',
    DegreeId: '',
    JobGroupId: '',
    SubjectId: '',
    SubjectName: '',
    ListIdSelect:[]

  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nghề',
    Items: [
      {
        Name: 'Tên nghề',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nghề',
        Type: 'text'
      },
      {
        Name: 'Trình độ',
        FieldName: 'DegreeId',
        Placeholder: 'Trình độ',
        Type: 'select',
        DataType: this.constant.SearchDataType.Degree,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Môn học',
        FieldName: 'SubjectName',
        Placeholder: 'Nhập môn học',
        Type: 'text'
      },
    ]
  };

  ngOnInit() {
    this.modelSearch.ListIdSelect = this.listCareeId;
    this.searchCaree();
  }

  searchCaree(){
    this.saleProductService.SearchJob(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listCaree = data.ListResult;
        this.listCaree.forEach((element, index) => {
          element.Index = index + 1;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  checkAll(isCheck: any){
    if (isCheck) {
      this.listCaree.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listCareeSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow(){
    this.listCaree.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listCareeSelect.push(element);
      }
    });
    this.listCareeSelect.forEach(element => {
      var index = this.listCaree.indexOf(element);
      if (index > -1) {
        this.listCaree.splice(index, 1);
      }
    });
  }

  removeRow(){
    this.listCareeSelect.forEach(element => {
      if (element.Checked) {
        this.listCaree.push(element);
      }
    });
    this.listCaree.forEach(element => {
      var index = this.listCareeSelect.indexOf(element);
      if (index > -1) {
        this.listCareeSelect.splice(index, 1);
      }
    });
  }

  choose(){
    this.activeModal.close(this.listCareeSelect);
  }

  clear(){
    this.modelSearch = {
      Name:'',
      Code:'',
      Email:'',
      ListIdSelect:[],
    }
    this.modelSearch.ListIdSelect = this.listCareeId;
    this.searchCaree();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
