import { Component, OnInit, ViewEncapsulation, ViewChild, Input, Output, EventEmitter, forwardRef, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import {
  DxDataGridComponent, DxDropDownBoxComponent
} from 'devextreme-angular';

@Component({
  selector: 'nts-drop-down',
  templateUrl: './nts-drop-down.component.html',
  styleUrls: ['./nts-drop-down.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NTSDropDownComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class NTSDropDownComponent implements ControlValueAccessor {

  @Input() ntsDisabled:boolean;
  @Input() ntsLabel: string;
  @Input() ntsValue: string;
  @Input() placeholder: string;
  @Input() ntsMode: string;
  @Input() ntsAutoClose: boolean;
  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this._items = value;
  };

  @Input()
  get columns() { return this._items };
  set columns(value: any[]) {
    this._columns = value;
  };

  @Output('change') changeEvent = new EventEmitter();
  @Output('changeText') changeTextEvent = new EventEmitter();

  constructor(
    private _cd: ChangeDetectorRef, ) { }

  selectedItems: any[] = [];
  _selectedItems: any[] = [];
  @ViewChild(DxDataGridComponent) dataGridComponent;
  @ViewChild(DxDropDownBoxComponent) dropDownBoxComponent;
  public isShowDropDown = false;
  public _items = [];
  public _columns: any[] = [];
  private _isWriteValue:boolean = false;

  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    this._isWriteValue = true;
    if (value != null) {
      if (this.ntsMode == 'multiple') {
        this.selectedItems = value;
        this._selectedItems = Object.assign({}, value);
      }
      else {
        this.selectedItems = [];
        this.selectedItems.push(value);
        this._selectedItems = [];
        this._selectedItems.push(value);
      }
    } else {
      this.selectedItems = [];
      this._selectedItems = [];
    }
    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.ntsDisabled = isDisabled;
    this._cd.markForCheck();
  }

  // Sự kiến ấn phím trên table
  onKeyDown(e) {
    // if (e.event.key == "Enter") {
    //   var rowCount = this._items.length;
    //   for (let i = 0; i < rowCount; i++) {
    //     if (this.dataGridComponent.focusedRowIndex) {
    //       this.selectedItems = [this._items[this.dataGridComponent.focusedRowIndex].Id];
    //       this.isShowDropDown = false;
    //       this._onChange(this.selectedItems.length > 0 ? this.selectedItems[0] : null);
    //       this.changeEvent.emit(this.selectedItems[0]);
    //       break;
    //     }
    //   }
    // }
  }

  onSelectionChanged() {
    if (!this.isShowDropDown) {
      return;
    }

    // if (this.ntsMode == 'multiple') {
    //   this._onChange(this.selectedItems ? this.selectedItems : null);
    //   this.changeEvent.emit(this.selectedItems ? this.selectedItems : null);
    // }
    // else {
    //   this._onChange(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
    //   this.changeEvent.emit(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
    // }

    // this.changeTextEvent.emit( this.dropDownBoxComponent.instance.text);

    if (this.ntsAutoClose) {
      this.isShowDropDown = false;
    }
  }

  onValueChanged() {
    setTimeout(() => {

      let isChange = false;
      if (this.ntsMode == 'multiple') {
        this.selectedItems.forEach(item => {
          this._selectedItems.forEach(_item => {
            if (item != _item) {
              isChange = true;
            }
          });
        });

        if (isChange || this.selectedItems.length != this._selectedItems.length) {
          this.selectedItems = []
          this._selectedItems.forEach(_item => {
            this.selectedItems.push(_item);
          });
          this._onChange(this.selectedItems ? this.selectedItems : null);
          this.changeEvent.emit(this.selectedItems ? this.selectedItems : null);
        }
      }
      else {
        if ((this.selectedItems && !this._selectedItems) || (!this.selectedItems && this._selectedItems)) {
          isChange = true;
        }
        else if (this.selectedItems.length != this._selectedItems.length) {
          isChange = true;
        }
        else if (this.selectedItems.length > 0 && this._selectedItems.length > 0 && this.selectedItems[0] != this._selectedItems[0]) {
          isChange = true;
        }

        if (isChange) {
          this.selectedItems = [];
          if (this._selectedItems && this._selectedItems.length > 0) {
            this.selectedItems.push(this._selectedItems[0]);
          }
          this._onChange(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
          this.changeEvent.emit(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
        }
      }
    }, 0);
  }

  // Sự kiện mở drop dơn
  onOpened() {
    // if (this.gridBoxValue && this.gridBoxValue.length > 0) {
    //   let selectIndex = this.dataGridComponent.instance.getRowIndexByKey(this.gridBoxValue[0]);
    //   this.dataGridComponent.instance.focus(this.dataGridComponent.instance.getCellElement(selectIndex, 0));
    // }
    // else {
    //   this.dataGridComponent.instance.focus(this.dataGridComponent.instance.getCellElement(0, 0));
    // }
  }

  close() {
    this.isShowDropDown = false;
  }
}
