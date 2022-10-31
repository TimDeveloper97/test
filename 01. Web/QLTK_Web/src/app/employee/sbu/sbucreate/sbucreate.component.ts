import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { SbuService } from '../../service/sbu.service';
import { DepartmentService } from '../../service/department.service';
import { DepartmentCreateComponent } from '../../department/department-create/department-create.component';

@Component({
  selector: 'app-sbucreate',
  templateUrl: './sbucreate.component.html',
  styleUrls: ['./sbucreate.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SBUCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private serviceSBU: SbuService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public constant: Constants,
    private departmentservice: DepartmentService,
    private modalService: NgbModal,
  ) { }

  isAction: boolean = false;
  Id: string;
  listSBU: any[] = []
  startIndex = 0;
  listData: any[] = [];
  listManager: any[] = [];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Status: '0',
    Location: '',
  }

  departmentModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    SBUId: '',
    ManagerId: '',
    ManagerName: '',
  }

  modalInfo = {
    Title: 'Thêm mới SBU',
    SaveText: 'Lưu',
  };

  ngOnInit() {
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa SBU';
      this.modalInfo.SaveText = 'Lưu';
      this.getSBUInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới SBU";
    }
  }

  getSBUInfo() {
    this.serviceSBU.getSBUInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  supCreate(isContinue) {
    this.serviceSBU.createSBU(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Note: '',
            Status: '0',
            Location: '',
          };
          this.messageService.showSuccess('Thêm mới SBU thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới SBU thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createSBU(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
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

  supUpdate() {
    this.serviceSBU.updateSBU(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật SBU thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSBU() {
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

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSBU();
    }
    else {
      this.createSBU(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  searchDepartment() {
    this.departmentModel.SBUId = this.Id;
    this.serviceSBU.searchDepartment(this.departmentModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.departmentModel.PageNumber - 1) * this.departmentModel.PageSize + 1);
        this.listData = data.ListResult;
        this.departmentModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(DepartmentCreateComponent, { container: 'body', windowClass: 'department-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.SBUId = this.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchDepartment();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteDepartment(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng ban này không?").then(
      data => {
        this.deleteDepartment(Id);
      },
      error => {
        
      }
    );
  }

  deleteDepartment(Id: string) {
    this.departmentservice.deleteDepartment({ Id: Id }).subscribe(
      data => {
        this.searchDepartment();
        this.messageService.showSuccess('Xóa phòng ban thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
