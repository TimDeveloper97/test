import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Constants, Configuration, MessageService, FileProcess, AppSetting, ComboboxService, PermissionService } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { PracticeService } from '../../service/practice.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { PracticeUpdateContentComponent } from '../practice-update-content/practice-update-content.component';


@Component({
  selector: 'app-practice-update',
  templateUrl: './practice-update.component.html',
  styleUrls: ['./practice-update.component.scss']
})
export class PracticeUpdateComponent implements OnInit {

  constructor(
    private router: Router,
    public constant: Constants,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private modalService: NgbModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public appset: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private servicePractice: PracticeService,
    private combobox: ComboboxService,
    private routeA: ActivatedRoute,
    public permissionService: PermissionService,
    private serviceHistory: HistoryVersionService
  ) { }

  Unit: '';
  StartIndex = 1;
  LessonPrice = 0;
  HardwarePrice = 0;
  Quantity = 0;
  isAction: boolean = false;
  Id: string;
  listData: any = [];
  listPracticeGroup = [];
  listPracticeGroupId = [];
  listUnit = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  model: any = {
    Id: '',
    PracticeGroupId: '',
    PracticeGroupName: '',
    Code: '',
    Name: '',
    CurentVersion: '',
    Note: '',
    TrainingTime: '',
    LessonPrice: 0,
    HardwarePrice: 0,
    UnitId: '',
    Quantity: '',
    LeadTime: '',
    TotalPrice: '',
    Content: '',

    MaterialConsumable: '',   // vật tư tiêu hao
    SupMaterial: '',          // thiết bị phụ trợ
    PracticeFile: '',                 // tài liệu
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành, giáo trình";
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCbPracticeGroup();
    this.getCbUnit();
    if (this.model.Id) {
      this.getPracticeInfo();

    }
  }

  getCbPracticeGroup() {
    this.combobox.getCbbPracticeGroup().subscribe((data: any) => {
      if (data) {
        this.listPracticeGroup = data;

      }
    },
      error => {
        this.messageService.showError(error);
      });
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

  getPracticeInfo() {
    this.servicePractice.getPracticeInfo(this.model).subscribe((data: any) => {
      this.model = data;
      this.appSetting.PageTitle = "Chỉnh sửa bài thực hành, giáo trình - " + this.model.Code + " - " + this.model.Name;
      this.LessonPrice = this.model.LessonPrice;
      this.Quantity = this.model.Quantity;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  supUpdate() {
    this.model.LessonPrice = this.LessonPrice;
    this.model.Quantity = this.Quantity;
    this.model.TotalPrice = this.Quantity * this.LessonPrice;
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.servicePractice.updatePractice(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật bài thực hành thành công!');
        this.getPracticeInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updatePractice() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supUpdate();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supUpdate();
        },
        error => {
          
        }
      );
    }
  }

  save() {
    if (this.model.Id) {
      this.updatePractice();
    }
    else {
      this.messageService.showMessage("Có lỗi trong quá trình xử lý");
    }
  }

  closeModal() {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.model.Id;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Practice;
    activeModal.result.then(async (result) => {
      if (result) {
        this.model.CurentVersion = result.Version + 1;
        await this.save();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showUpdateContent() {
    let activeModal = this.modalService.open(PracticeUpdateContentComponent, { container: 'body', windowClass: 'practice-update-conten', backdrop: 'static' });
    activeModal.componentInstance.practiceId = this.model.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.getPracticeInfo();
      }
    }, (reason) => {
    });
  }
}
