import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nts-status',
  templateUrl: './nts-status.component.html',
  styleUrls: ['./nts-status.component.scss']
})
export class NtsStatusComponent implements OnInit {

  constructor() { }

  badgeClass = '';
  statusName = '';
  private _items: any[] = [];
  private _value: any[] = [];

  @Input()
  get items() { return this._items };
  set items(item: any[]) {
    this._items = item;
  };
  
  @Input()
  get value() { return this._value };
  set value(value: any) {
    this._value = value;
  };

  ngOnInit(): void {
    this._items.forEach(item => {
      if (item.Id == this._value) {
        this.statusName = item.Name;
      }
    });
  }

}
