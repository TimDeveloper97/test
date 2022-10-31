import { Component, OnInit } from '@angular/core';
import { SolutionGroupService } from '../service/solution-group.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-solution-group-create',
  templateUrl: './solution-group-create.component.html',
  styleUrls: ['./solution-group-create.component.scss']
})
export class SolutionGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: SolutionGroupService,
    private combobox: ComboboxService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm giải pháp',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listSolotionGroup: any[] = [];
  listProductStandardGroup: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    ParentId: null,
  }



  ngOnInit() {
    this.getListSolutionGroup();
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm giải pháp';
      this.modalInfo.SaveText = 'Lưu';
      this.getSolutionGroupInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm giải pháp";
    }
  }

  getListSolutionGroup() {
    this.combobox.getListSolutionGroup().subscribe(data => {
      this.listSolotionGroup = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getSolutionGroupInfo() {
    this.service.getSolutionGroupInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createSolutionGroup(isContinue) {
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

  updateSolutionGroup() {
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
      this.updateSolutionGroup();
    }
    else {
      this.createSolutionGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.service.createSolutionGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm giải pháp thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm giải pháp thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.service.updateSolutionGroup(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm giải pháp thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
