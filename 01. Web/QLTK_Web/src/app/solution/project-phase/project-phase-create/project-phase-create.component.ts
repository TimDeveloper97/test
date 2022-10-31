import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectPhaseService } from '../../service/project-phase.service';

@Component({
  selector: 'app-project-phase-create',
  templateUrl: './project-phase-create.component.html',
  styleUrls: ['./project-phase-create.component.scss']
})
export class ProjectPhaseCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private projectPhaseService : ProjectPhaseService,
    private comboboxService: ComboboxService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới giai đoạn dự án',
    SaveText: 'Lưu',
  };

  listSBU: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' },];
  listProjectPhaseType: any[] = [];
  columnNameProjectPhase: any[] = [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }];
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  treeBoxValue: string;
  parentId: string;

  isAction: boolean = false;
  Id: string;
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    SBUId: '',
    ParentId: null,
  }
  ngOnInit() {
    this.GetProjectPhaseType();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa giai đoạn dự án';
      this.ModalInfo.SaveText = 'Lưu';
      this.getProjectPhase();
      this.getListSBU();
    }
    else {
      this.ModalInfo.Title = "Thêm mới giai đoạn dự án";
    }
    this.getListSBU();
  }

  GetProjectPhaseType() {
    this.comboboxService.getProjectPhaseType().subscribe(data => {
      this.listProjectPhaseType = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getName(ProjectPhaseId) {
    for (var item of this.listProjectPhaseType) {
      if (item.Id == ProjectPhaseId) {
        this.model.Code = item.Code;
      }
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.ParentId = e.selectedRowKeys[0];
    this.getName(e.selectedRowKeys[0]);
    this.closeDropDownBox();
    
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }


  getListSBU() {
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getProjectPhase() {
    this.projectPhaseService.getProjectPhase({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createprojectPhase(isContinue) {
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if(validCode){
      this.projectPhaseService.createProjectPhase(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {
              Id: '',
              Name: '',
              Code: '',
              Description: '',
            };
            this.messageService.showSuccess('Thêm mới giai đoạn dự án thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới giai đoạn dự án thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }
    else
    {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.projectPhaseService.createProjectPhase(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {
                  Id: '',
                  Name: '',
                  Code: '',
                  Description: '',
                };
                this.messageService.showSuccess('Thêm mới giai đoạn dự án thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới giai đoạn dự án thành công!');
                this.closeModal(true);
              }
            },
            error => {
              this.messageService.showError(error);
            });
        },
        error => {
          
        }
      );
    }
    
  }

  updateProjectPhase() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.projectPhaseService.updateProjectPhase(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật giai đoạn dự án thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.projectPhaseService.updateProjectPhase(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật giai đoạn dự án thành công!');
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateProjectPhase();
    }
    else {
      this.createprojectPhase(isContinue);
    }
  }

}
