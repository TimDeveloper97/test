import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';

@Component({
  selector: 'app-show-choose-applicationt',
  templateUrl: './show-choose-applicationt.component.html',
  styleUrls: ['./show-choose-applicationt.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseApplicationtComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public saleProductService: SaleProductService,
  ) { }

  listApplication: any[]=[];

  listApplicationSelect = [];
  listApplicationId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;

  modelSearch: any = {
    Name: '',
    ListIdSelect:[],
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
      
      },
    ]
  };

  ngOnInit() {
    this.modelSearch.ListIdSelect = this.listApplicationId;
    this.searchApplication();
  }

  imagePath: string;
  searchApplication() {
    this.saleProductService.SearchApp(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listApplication = data.ListResult;
        this.listApplication.forEach((element, index) => {
          element.Index = index + 1;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.listApplication.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listApplicationSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow() {
    this.listApplication.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listApplicationSelect.push(element);
      }
    });
    this.listApplicationSelect.forEach(element => {
      var index = this.listApplication.indexOf(element);
      if (index > -1) {
        this.listApplication.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listApplicationSelect.forEach(element => {
      if (element.Checked) {
        this.listApplication.push(element);
      }
    });
    this.listApplication.forEach(element => {
      var index = this.listApplicationSelect.indexOf(element);
      if (index > -1) {
        this.listApplicationSelect.splice(index, 1);
      }
    });
  }

  choose() {
    this.activeModal.close(this.listApplicationSelect);
  }

  clear() {
    this.modelSearch = {
      Name: '',
      ListIdSelectL:[],
    }
    this.modelSearch.ListIdSelect = this.listApplicationId;
    this.searchApplication();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
