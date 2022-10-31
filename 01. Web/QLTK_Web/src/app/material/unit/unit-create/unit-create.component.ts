import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { UnitService } from '../../services/unit-service';

@Component({
  selector: 'app-unit-create',
  templateUrl: './unit-create.component.html',
  styleUrls: ['./unit-create.component.scss']
})
export class UnitCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private unitService: UnitService
  ) { }

  modalInfo = {
    Title: 'Thêm mới đơn vị tính',
    SaveText: 'Lưu',

  };

  isAction: boolean = false;
  Id: string;
  listUnit: any[] = []
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Index: 0,
    Description: '',
  }


  ngOnInit() {
    this.getCbbUnit();
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa đơn vị tính';
      this.modalInfo.SaveText = 'Lưu';
      this.getUnitInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới đơn vị tính";
    }
  }

  getUnitInfo() {
    this.unitService.getUnitInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createUnit(isContinue) {
    this.unitService.createUnit(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới đơn vị tính thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới đơn vị tính thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateUnit() {
    this.unitService.updateUnit(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật đơn vị tính thành công');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbUnit() {
    this.unitService.getCbbUnit().subscribe((data: any) => {
      this.listUnit = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listUnit.splice(this.listUnit.length - 1, 1);
      }
    });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateUnit();
    }
    else {
      this.createUnit(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
