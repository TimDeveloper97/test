import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Constants, MessageService, FileProcess, AppSetting, ComboboxService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { PracticeService } from '../../service/practice.service';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-practice-create',
  templateUrl: './practice-create.component.html',
  styleUrls: ['./practice-create.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class PracticeCreateComponent implements OnInit {

  constructor(
    private router: Router,
    public appSetting: AppSetting,
    public constant: Constants,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public appset: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private serPractice: PracticeService,
    private combobox: ComboboxService,
    public permissionService: PermissionService
  ) { }

  Unit: '';
  Total: number;
  StartIndex = 1;
  LessonPrice = 10000;
  HardwarePrice = 0;
  Quantity = 1;
  Amount2 = 0;
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
    LessonPrice: 100000,
    HardwarePrice: '',
    UnitId: '1e8dde73-44bb-44c6-9fdd-bb42d3b2bec9',
    Quantity: 0,
    LeadTime: 0,
    TotalPrice: 0,
    Content: '',
    codePracticeb: '',
    // check tồn tại
    MaterialConsumable: true, // vật tư tiêu hao
    SupMaterial: true,  // thiết bị phụ trợ
    PracticeFile: true, // tài liệu
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Thêm mới bài thực hành/công đoạn";
    this.model.PracticeGroupId = localStorage.getItem("selectedPracticeGroupId");
    this.getCbPracticeGroup();
    this.getCbUnit();
    forkJoin([
      this.combobox.getCbbPracticeGroup(),
    ]
    ).subscribe(([res1]) => {
      this.listPracticeGroup = res1;
      this.getCodePracticeGroup(this.model.PracticeGroupId);
    });
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
  get(){

  }
  getCodePracticeGroup(PracticeGroupId) {
    for(var item of this.listPracticeGroup)
      if(item.Id == PracticeGroupId){
        this.model.Code = item.Code;
        // this.model.Code = this.codePracticea + this.model.codePracticeb
      }
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
          if(!this.permissionService.checkPermission(['F040702'])){
            this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh/chinh-sua-bai-thuc-hanh/', data]);
          }
          else{
            this.closeModal();
          }
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

  closeModal() {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }
}
