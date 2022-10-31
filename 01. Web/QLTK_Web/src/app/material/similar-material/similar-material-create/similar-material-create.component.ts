import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { SimilarMaterialConfigService } from '../../services/similar-material-config.service';

@Component({
  selector: 'app-similar-material-create',
  templateUrl: './similar-material-create.component.html',
  styleUrls: ['./similar-material-create.component.scss']
})
export class SimilarMaterialCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private similarmaterialservice: SimilarMaterialConfigService,
  ) { }

  infoModal = {
    Title: 'Thêm mới nhóm vật tư tương tự',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    Name: '',
  }

  ngOnInit() {
    if (this.Id) {
      this.infoModal.Title = 'Chỉnh sửa nhóm vật tư tương tự';
      this.infoModal.SaveText = 'Lưu';
      this.getSimilarMaterialInfo();
    }
    else {
      this.infoModal.Title = "Thêm mới nhóm vật tư tương tự";
    }
  }

  getSimilarMaterialInfo() {
    this.similarmaterialservice.getSimilarMaterialInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createSimilarMaterial(isContinue) {
    this.similarmaterialservice.createSimilarMaterial(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm vật tư tương tự thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm vật tư tương tự thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSimilarMaterial() {
    this.similarmaterialservice.updateSimilarMaterial(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm vật tư tương tự thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSimilarMaterial();
    }
    else {
      this.createSimilarMaterial(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
