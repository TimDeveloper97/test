import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from 'src/app/solution/service/summary-quotes.service';

@Component({
  selector: 'app-show-choose-quotation-step',
  templateUrl: './show-choose-quotation-step.component.html',
  styleUrls: ['./show-choose-quotation-step.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseQuotationStepComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constant: Constants,
    private service: SummaryQuotesService,
    private messageService: MessageService,
  ) { }

  ModalInfo = {
    Title: 'Chọn bước báo giá',
    SaveText: 'Lưu',
  };

  checkedTop = false;
  TotalSuccessRatio: number;
  model : any = {
    
  }
  listQuotes: any[] = [];
  listQuote = [];
  listQuoteSelect = [];
  checkedBot = false;
  selectIndex = -1;
  listByQuotesId: any[] = [];
  listQuotesOutputUpdate: any[]=[];

  ngOnInit(): void {
    this.listQuotes = this. listQuotes;
    if(this.listQuotesOutputUpdate)
    {
      //this.listQuoteSelect = this.listQuotesOutputUpdate;
      this.listQuotes.forEach(element => {
        this.listQuotesOutputUpdate.forEach(x => {
          if(element.QuotesCode == x.QuotesCode)
          {
            element.SuccessRatio = x.SuccessRatio;
            element.Checked = true;
          }
        });
        
      });
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  checkAll(isCheck: any){
    if (isCheck) {
      this.listQuotes.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listQuoteSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  save(){
    this.TotalSuccessRatio = 0
    this.listQuotes.forEach(element => {
      if (element.Checked) {
        this.listQuoteSelect.push(element);
      }
    });
   
    this.listQuoteSelect.forEach(element => {
      var index = this.listQuotes.indexOf(element);
      this.TotalSuccessRatio = this.TotalSuccessRatio + element.SuccessRatio;
    });
    if(this.TotalSuccessRatio > 100)
    {
      this.messageService.showConfirm("Tổng giá trị hoàn thành không vượt quá 100% !!");
      this.listQuoteSelect = []
    }
    if(this.TotalSuccessRatio <= 100)
    {
      this.activeModal.close(this.listQuoteSelect);
      this.listQuoteSelect = []
    }
   
  }
}
