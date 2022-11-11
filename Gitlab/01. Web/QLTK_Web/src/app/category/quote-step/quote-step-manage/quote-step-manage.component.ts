import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { QuoteStepService } from '../../service/quote-step.service';
import { QuoteStepCreateComponent } from '../quote-step-create/quote-step-create.component';

@Component({
  selector: 'app-quote-step-manage',
  templateUrl: './quote-step-manage.component.html',
  styleUrls: ['./quote-step-manage.component.scss']
})
export class QuoteStepManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private modalService: NgbModal,
    public constant: Constants,
    public quoteService: QuoteStepService,
    private messageService: MessageService,
  ) { }

  logUserId: string;
  listData: any[] = [];
  listDataQuote: any[] = [];

  model: any = {
    OrderBy: 'Index',
    OrderType: true,

    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Code: '',
    Id:'',
    Name: '',
    IsEnable: '',
    SuccessRadio: '',
    SBUCode: '',
    SBUName: '',
    Description: ''
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên/ mã... ',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Id: 'Tên SBU' }]
      }
          
    ]
  };

  totalRole: 0;
  startIndex: number = 0;
  SBUId = '';

  ngOnInit(): void {
    this.appSetting.PageTitle = "Các bước báo giá";
    let userStore = localStorage.getItem('qltkcurrentUser');
    if (userStore) {
      this.SBUId = JSON.parse(userStore).SBUId;
    }
    this.searchQuote();
  }

  searchQuote(){
    this.quoteService.searchQuoteStep(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      OrderBy: 'Index',
      OrderType: true,
  
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Id:'',
      Name: '',
      IsEnable: '',
      SuccessRadio: '',
      SBUCode: '',
      SBUName: '',
      Description: ''
    }
    this.searchQuote();
  }

  showCreateUpdate(Id){
    let activeModal = this.modalService.open(QuoteStepCreateComponent, { container: 'body', windowClass: 'quote-step-create-update-modal', backdrop: 'static' });
    activeModal.componentInstance.Id = Id;
    activeModal.result.then(result => {
      if (result) {
      }
      this.searchQuote();
    })
  }

  showConfirmDelete(Id: string, IsEnable: boolean, Index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bước báo giá này không?").then(
      data => {
        this.delete(Id, IsEnable, Index);
        
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  delete(Id, IsEnable, Index: number) {
    this.quoteService.deleteQuote({Id: Id, IsEnable: IsEnable, Index: Index, LogUserId: this.logUserId} ).subscribe(result => {
      this.searchQuote();
      this.messageService.showSuccess("Xóa bước báo giá thành công!")
    },
    error => {
      this.messageService.showError(error);
    });
    
  }
  modelQuoteModule: any = {
    Id: '',
    ListQuote: [],
  }

  onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.listData, event.previousIndex, event.currentIndex);
   
    this.modelQuoteModule.ListQuote = [];
    this.save();
  }

  
  save(){
    this.listData.forEach(element => {
      if (element) {
        this.modelQuoteModule.ListQuote.push(element);
      }
    });
    
    this.quoteService.createIndex(this.modelQuoteModule).subscribe(
      data => {
        this.searchQuote();
        this.messageService.showSuccess('Chỉnh sửa vị trí bước báo giá thành công!');
          
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }
  

}
