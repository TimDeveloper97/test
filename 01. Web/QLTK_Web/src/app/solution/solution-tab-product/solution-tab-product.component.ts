import { Component, OnInit, Input } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DegreeService } from 'src/app/practice/degree/service/degree.service';
import { SolutionTabProductService } from '../service/solution-tab-product.service';
import { SolutionProductCreateComponent } from '../solution-product-create/solution-product-create.component';

@Component({
  selector: 'app-solution-tab-product',
  templateUrl: './solution-tab-product.component.html',
  styleUrls: ['./solution-tab-product.component.scss']
})
export class SolutionTabProductComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private solutionProductService: SolutionTabProductService,
    public constants: Constants
  ) { }

  @Input() SolutionId: string;
  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    Code: '',
    SolutionId: '',
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm mã hoặc tên sản phẩm ...',
    Items: [
      {

      },
    ]
  };
  SumTotalPrice = 0;
  ngOnInit() {
    this.model.SolutionId = this.SolutionId;
    //this.appSetting.PageTitle = "Quản lý sản phẩm";
    this.searchSolutionProduct();
  }

  searchSolutionProduct() {
    this.model.SolutionId = this.SolutionId;
    this.solutionProductService.searchSolutionProduct(this.model).subscribe((data: any) => {
      if (data) {
        this.listData = data.searchResult.ListResult;
        this.model.TotalItems = data.searchResult.TotalItem;
        this.SumTotalPrice = data.sumTotalPrice;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      Id: '',
      Name: '',
      Code: '',
      Description: '',
    }
    this.searchSolutionProduct();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SolutionProductCreateComponent, { container: 'body', windowClass: 'solution-product-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.SolutionId = this.SolutionId;
    activeModal.result.then((result) => {
      if (result == true) {
        this.searchSolutionProduct();
      }
      // else if (result == undefined) {

      // } else {
      //   this.listData.push(result);
      // }
    }, (reason) => {
    });
  }

  showConfirmDeleteSolutionProduct(Id: string) {
    if (Id != null) {
      this.messageService.showConfirm("Bạn có chắc muốn xoá sản phẩm này không?").then(
        data => {
          this.deleteSolutionProduct(Id);
          //this.listData.splice(index, 1);
        },
        error => {

        }
      );
    }
    //else if (Id == null) {
    //   this.messageService.showConfirm("Bạn có chắc muốn xoá sản phẩm này không?").then(
    //     data => {
    //       this.listData.splice(index, 1);
    //     }
    //   );
    // }

  }

  deleteSolutionProduct(Id: string) {
    this.solutionProductService.deleteSolutionProduct(Id).subscribe(
      data => {
        this.messageService.showSuccess('Xóa sản phẩm thành công!');
        this.searchSolutionProduct();
      },
      error => {
        this.messageService.showError(error);
      });
  }
  listSolutionProduct: any[] = [];

  modelTemp = {
    SolutionId: '',
    ListSolutionProduct: [],
  }
  save() {
    this.modelTemp.SolutionId = this.SolutionId
    this.modelTemp.ListSolutionProduct = this.listData
    this.solutionProductService.createSolutionProduct(this.modelTemp).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
        this.searchSolutionProduct();
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
