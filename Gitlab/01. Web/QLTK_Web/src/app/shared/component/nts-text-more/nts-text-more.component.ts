import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nts-text-more',
  templateUrl: './nts-text-more.component.html',
  styleUrls: ['./nts-text-more.component.scss']
})
export class NtsTextMoreComponent implements OnInit {

  @Input()
  set ntsText(value: string) {
    this._ntsText = value;
    this.viewText();
  }
  @Input() ntsLimit: number;
  _ntsText: string;

  constructor() { }

  firstText: string;
  moreText: string;
  isMoreShow: boolean = false;
  isMore: boolean = false;
  ngOnInit(): void {

  }

  viewText() {
    this.isMoreShow = false;
    this.isMore = false;
    if (this._ntsText) {
      if (this._ntsText.length > this.ntsLimit + 12) {
        this.firstText = this._ntsText.substring(0, this.ntsLimit);
        this.moreText = this._ntsText.substring(this.ntsLimit, this._ntsText.length);
        this.isMore = true;
      }
      else {
        this.firstText = this._ntsText;
        this.isMore = false
      }
    }
  }

  showMore(){
    this.isMoreShow = !this.isMoreShow;
  }
}
