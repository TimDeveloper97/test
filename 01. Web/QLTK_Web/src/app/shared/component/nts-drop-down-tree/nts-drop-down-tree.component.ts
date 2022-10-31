import { Component, ViewEncapsulation, ViewChild, Input, Output, EventEmitter, forwardRef, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import {
  DxTreeListComponent
} from 'devextreme-angular';

@Component({
  selector: 'nts-drop-down-tree',
  templateUrl: './nts-drop-down-tree.component.html',
  styleUrls: ['./nts-drop-down-tree.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NTSDropDownTreeComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class NTSDropDownTreeComponent implements ControlValueAccessor {

  @Input() ntsLabel: string;
  @Input() ntsValue: string;
  @Input() ntsParentId: string;
  @Input() autoExpandAll: boolean;
  @Input() placeholder: string;
  @Input() ntsMode: string;
  @Input() ntsRecursiveSelection: boolean;
  @Input() ntsAutoClose: boolean;
  @Input() ntsDisabled: boolean;
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

  constructor(
    private _cd: ChangeDetectorRef, ) { }

  selectedItems: any[] = [];
  selectedTreeItems: any[] = [];
  @ViewChild(DxTreeListComponent) treeListComponent;
  public isShowDropDown = false;
  public _items = [];
  public _columns: any[] = [];

  private _onChange = (_: any) => { };
  private _onTouched = () => { };
  private mode = 'single';

  writeValue(value: any | any[]): void {
    if (value != null) {
      if (this.ntsMode == 'multiple') {
        this.selectedItems = value;
        this.selectedTreeItems = value;
      }
      else {
        this.selectedItems = [];
        this.selectedTreeItems = [];
        this.selectedItems.push(value);
        this.selectedTreeItems.push(value);
      }
    } else {
      this.selectedItems = [];
      this.selectedTreeItems = [];
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
    if (e.event.key == "Enter") {
      var rowCount = this._items.length;
      for (let i = 0; i < rowCount; i++) {
        if (this.treeListComponent.focusedRowIndex) {
          this.selectedItems = [this._items[this.treeListComponent.focusedRowIndex].Id];
          this.isShowDropDown = false;
          this._onChange(this.selectedItems.length > 0 ? this.selectedItems[0] : null);
          this.changeEvent.emit(this.selectedItems[0]);
          break;
        }
      }
    }
  }

  onSelectionChanged() {
    if (!this.isShowDropDown) {
      return;
    }

    this.selectedItems = this.treeListComponent.instance.getSelectedRowKeys("all");

    // if (this.ntsMode == 'multiple') {
    //   this._onChange(this.selectedItems ? this.selectedItems : null);
    //   this.changeEvent.emit(this.selectedItems ? this.selectedItems : null);
    // }
    // else {
    //   this._onChange(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
    //   this.changeEvent.emit(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
    // }

    if (this.ntsAutoClose) {
      this.isShowDropDown = false;
    }
  }

  onValueChanged() {
    if (!this.selectedItems || this.selectedItems.length == 0) {
      this.selectedTreeItems = null;
    }
    setTimeout(() => {
      // this.selectedItems = this.treeListComponent.instance.getSelectedRowKeys("all");

      if (this.ntsMode == 'multiple') {
        this._onChange(this.selectedItems ? this.selectedItems : null);
        this.changeEvent.emit(this.selectedItems ? this.selectedItems : null);
      }
      else {
        this._onChange(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
        this.changeEvent.emit(this.selectedItems && this.selectedItems.length > 0 ? this.selectedItems[0] : null);
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
