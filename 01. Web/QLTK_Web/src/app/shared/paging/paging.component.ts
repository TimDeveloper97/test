import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';

@Component({
  selector: 'app-paging',
  templateUrl: './paging.component.html',
  styleUrls: ['./paging.component.css']
})
export class PagingComponent implements OnChanges, OnInit {

  constructor() { }

  @Input() PageNumber: number;
  @Input() TotalItems: number;
  @Input() PageSize: number;

  curentPage = 0;
  totalItems = 0;
  pageSize = 0;

  startRecord = 0;
  endRecord = 0;

  isNext = false;
  isPrevious = false;

  ngOnChanges(changes: SimpleChanges) {
    this.curentPage = changes.PageNumber ? changes.PageNumber.currentValue : this.PageNumber;
    this.totalItems = changes.TotalItems ? changes.TotalItems.currentValue : this.TotalItems;
    this.pageSize = changes.PageSize ? changes.PageSize.currentValue : this.PageSize;
    this.calculatorPage();
  }

  @Output()
  change: EventEmitter<number> = new EventEmitter<number>();

  ngOnInit() {
    this.calculatorPage();
  }

  previousPage() {
    this.curentPage--;
    this.change.emit(this.curentPage);
  }

  nextPage() {
    this.curentPage++;
    this.change.emit(this.curentPage);
  }

  calculatorPage() {
    this.startRecord = (this.curentPage - 1) * this.pageSize + 1;
    if (this.startRecord > 1) {
      this.isPrevious = true;
    } else {
      this.isPrevious = false;
    }

    this.startRecord = this.startRecord == 0 ? 1 : this.startRecord;

    this.endRecord = this.curentPage * this.pageSize;

    this.endRecord = this.endRecord == 0 ? 1 : this.endRecord;
    if (this.endRecord > this.totalItems) {
      this.endRecord = this.totalItems;
      this.isNext = false;
    } else {
      this.isNext = true;
    }

  }

}
