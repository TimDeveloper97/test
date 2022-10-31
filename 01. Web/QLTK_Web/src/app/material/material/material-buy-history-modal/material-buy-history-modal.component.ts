import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MaterialService } from '../../services/material-service';
import { MessageService, Constants } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-material-buy-history-modal',
  templateUrl: './material-buy-history-modal.component.html',
  styleUrls: ['./material-buy-history-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MaterialBuyHistoryModalComponent implements OnInit {
  Id:any;
  StartIndex = 0;
  LstpageSize = [5, 10, 15, 20, 25, 30];
  ModalInfo = {
    Title: 'Xem lịch sử giá vật tư'
  }

  model:any = {
    Id:'',
    Filter:'',
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
  }
  ListData = [];

  constructor(
    private materialService: MaterialService,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants
  ) { }

  ngOnInit() {
    this.getHistoryByMaterialId();
  }

  getHistoryByMaterialId() {
    this.model.Id = this.Id;
    this.materialService.getHistoryByMaterialId(this.model).subscribe((data: any) => {
      if (data.ListMaterialBuyHistory) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.ListData = data.ListMaterialBuyHistory;
        this.model.TotalItem = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  closeModal() {
    this.activeModal.close(false);
  }

  loadPage($event) {
    this.model.PageNumber = $event;
    this.getHistoryByMaterialId();
  }
}
