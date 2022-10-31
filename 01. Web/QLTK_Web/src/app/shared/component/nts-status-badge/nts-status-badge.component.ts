import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nts-status-badge',
  templateUrl: './nts-status-badge.component.html',
  styleUrls: ['./nts-status-badge.component.scss']
})
export class NtsStatusBadgeComponent implements OnInit {

  constructor() { }

  badgeClass = '';
  statusName = '';
  private _items: any[] = [];
  private _value: any[] = [];

  @Input()
  get items() { return this._items };
  set items(item: any[]) {
    this._items = item;
    this.viewBadge();
  };

  @Input()
  get value() { return this._value };
  set value(value: any) {
    this._value = value;
    this.viewBadge();
  };

  ngOnInit(): void {
    //this.viewBadge();
  }

  viewBadge() {
    this._items.forEach(item => {
      if (item.Id == this._value) {
        this.statusName = item.Name;
        this.badgeClass = item.BadgeClass;
      }
    });
  }

}
