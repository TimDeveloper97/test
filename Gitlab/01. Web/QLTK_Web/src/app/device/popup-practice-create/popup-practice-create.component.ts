import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { Constants, Configuration, MessageService, FileProcess, AppSetting, ComboboxService } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { PracticeService } from 'src/app/practice/service/practice.service';

@Component({
  selector: 'app-popup-practice-create',
  templateUrl: './popup-practice-create.component.html',
  styleUrls: ['./popup-practice-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PopupPracticeCreateComponent implements OnInit {

  constructor(
    private router: Router,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private modalService: NgbModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public appset: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private serPractice: PracticeService,
    private combobox: ComboboxService
  ) { }

  ModalInfo = {
    Title: 'Thêm mới bài thực hành',
    SaveText: 'Lưu',
  };

  Unit: '';
  Total: number;
  StartIndex = 1;
  LessonPrice = 10000;
  HardwarePrice = 0;
  Quantity = 1;
  isAction: boolean = false;
  Id: string;
  listData: any = [];
  listPracticeGroup = [];
  listUnit = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  model: any = {
    Id: '',
    PracticeGroupId: '',
    Code: '',
    Name: '',
    CurentVersion: 0,
    Note: '',
    TrainingTime: 1,
    LessonPrice: '',
    HardwarePrice: '',
    UnitId: '1e8dde73-44bb-44c6-9fdd-bb42d3b2bec9',
    Quantity: 0,
    LeadTime: 0,
    TotalPrice: 0,
    Content: '',
    MaterialConsumable: true, // vật tư tiêu hao
    SupMaterial: true,  // thiết bị phụ trợ
    PracticeFile: true, // tài liệu
  }
  Amount2: string;
  Date2: string;
  Unit2: string;
  ngOnInit() {
    this.getCbPracticeGroup();
    this.getCbUnit();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa bài thực hành/công đoạn';
      this.ModalInfo.SaveText = 'Lưu';
    }
    else {
      this.ModalInfo.Title = "Thêm mới bài thực hành/công đoạn";
    }
  }

  getCbPracticeGroup() {
    this.combobox.getCbbPracticeGroup().subscribe(
      data => {
        this.listPracticeGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbUnit() {
    this.combobox.getCbbUnit().subscribe(
      data => {
        this.listUnit = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createPractice(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    this.model.Quantity = this.Quantity;
    this.model.LessonPrice = this.LessonPrice;
    this.model.HardwarePrice = this.HardwarePrice;
    this.model.TotalPrice = this.Quantity * this.LessonPrice;
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supCreate(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supCreate(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.serPractice.createPractice(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới bài thực hành/công đoạn thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới bài thực hành/công đoạn thành công!');
          // this.closeModal(true);
          this.activeModal.close(data);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


  save(isContinue: boolean) {
    if (this.Id) {
      this.messageService.showMessage("Có lỗi trong quá trình xử lý");
    }
    else {
      this.createPractice(isContinue);
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  changePracticeCode() {
    var isNext = true;
    this.listPracticeGroup.forEach(group => {
      if (isNext) {
        if (this.model.PracticeGroupId == group.Id) {
          this.model.Code = group.Code;
          isNext = false;
        }
      }
    });
  }

}
