import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { SolutionAnalysisSupplierService } from '../service/solution-analysis-supplier.service';

@Component({
  selector: 'app-solution-analysis-supplier-create',
  templateUrl: './solution-analysis-supplier-create.component.html',
  styleUrls: ['./solution-analysis-supplier-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionAnalysisSupplierCreateComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private solutionAnalysisSupplierService: SolutionAnalysisSupplierService,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public constant: Constants

  ) { }

  listSupplier = [];

  listSupplierSelect = [];
  listSupplierId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;
  

  modelSearch: any ={
    Name:'',
    Code:'',
    Email:'',
    listSupplierId:[],
  }

  searchOptions ={
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhà cung cấp/ mã nhà cung cấp ...',
    Items: [
      {
        Name: 'Email nhà cung cấp',
        FieldName: 'Email',
        Placeholder: 'Nhập email nhà cung cấp ...',
        Type: 'text'
      },
    ]
  }

  ngOnInit() {
    this.modelSearch.listSupplierId = this.listSupplierId;
    this.searchSupplier();
  }

  searchSupplier(){
    this.listSupplierSelect.forEach(item => {
      this.modelSearch.listSupplierId.push(item.Id);
    })
    this.solutionAnalysisSupplierService.getlistSupplier(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listSupplier = data;
        this.listSupplier.forEach((element, index) => {
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
      this.listSupplier.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listSupplierSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow(){
    this.listSupplier.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listSupplierSelect.push(element);
      }
    });
    this.listSupplierSelect.forEach(element => {
      var index = this.listSupplier.indexOf(element);
      if (index > -1) {
        this.listSupplier.splice(index, 1);
      }
    });
  }

  removeRow(){
    this.listSupplierSelect.forEach(element => {
      if (element.Checked) {
        this.listSupplier.push(element);
      }
    });
    this.listSupplier.forEach(element => {
      var index = this.listSupplierSelect.indexOf(element);
      if (index > -1) {
        this.listSupplierSelect.splice(index, 1);
      }
    });
  }

  choose(){
    this.activeModal.close(this.listSupplierSelect);
  }

  clear(){
    this.modelSearch = {
      Name:'',
      Code:'',
      Email:'',
      listSupplierId:[],
    }
    this.searchSupplier();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
