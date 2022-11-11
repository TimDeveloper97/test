import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { QuoteStepService } from '../../service/quote-step.service';

@Component({
  selector: 'app-quote-step-create',
  templateUrl: './quote-step-create.component.html',
  styleUrls: ['./quote-step-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class QuoteStepCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    public combobox: ComboboxService,
    public quoteService: QuoteStepService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới bước báo giá',
    SaveText: 'Lưu',
  };

  listIndex: any[] = []

  model: any ={
    Id:'',
    Name:'',
    Index: 0,
    IsEnable:true,
    SuccessRadio: 0,
    SBUCode: '',
    SBUName: '',
    SBUId: '',
    Description: '',
    CreateBy:'',
    CreateDate:null,
    UpdateBy:'',
    UpdateDate:null,
  }
  Id: string;
  SBUId = null;
  listData: any[] = [];

  ngOnInit(): void {
    
    let userStore = localStorage.getItem('qltkcurrentUser');
    if (userStore) {
      this.SBUId = JSON.parse(userStore).SBUId;
    }
    this.getListSBU();
   
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa báo giá';
      this.ModalInfo.SaveText = 'Lưu';
      this.getQuoteInfo();
      this.getIndex();
    }
    else {
      this.model.SBUId = this.SBUId;
      this.getIndex();
      this.ModalInfo.Title = "Thêm mới bước báo giá";

    }
  }

  getIndex() {
    this.quoteService.getIndex().subscribe((data: any) => {
      this.listIndex = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listIndex.splice(this.listIndex.length - 1, 1);
      }
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  save(){
    if(this.Id){
      this.updateQuote();
    }
    else{
      this.createQuote();
    }
  }

  
  createQuote(){
    this.quoteService.createQuote(this.model).subscribe(result=>{
      this.messageService.showSuccess('Thêm mới bước báo giá thành công!');
      this.closeModal();
    },
    error => {
      this.messageService.showError(error);
    }
  );
  }

  updateQuote(){
    this.quoteService.updateQuote(this.model).subscribe(result=>{
      this.messageService.showSuccess('Cập nhật bước báo giá thành công');
      this.closeModal();
    },
    error => {
      this.messageService.showError(error);
    }
  );
  }

  getListSBU(){
    this.combobox.getCbbSBU().subscribe(data => {
      this.listData = data;
    });
  }

  getQuoteInfo(){
    this.quoteService.getInfoById({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }
}
