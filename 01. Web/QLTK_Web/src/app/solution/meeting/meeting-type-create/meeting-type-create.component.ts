import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { MeetingTypeService } from '../../service/meeting-type.service';
import { SolutionGroupService } from '../../service/solution-group.service';

@Component({
  selector: 'app-meeting-type-create',
  templateUrl: './meeting-type-create.component.html',
  styleUrls: ['./meeting-type-create.component.scss']
})
export class MeetingTypeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private meetingTypeService: MeetingTypeService,
    private combobox: ComboboxService,
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm giải pháp',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listMeetingType: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã chủng loại' }, { Name: 'Name', Title: 'Tên chủng loại' }];

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    ParentId: null,
  }

  ngOnInit() {
    this.getListMeetingType();
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa chủng loại';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới chủng loại";
    }
  }

  getListMeetingType() {
    this.combobox.getMeetingType().subscribe(data => {
      this.listMeetingType = data;
      this.checkListMeetingType();
    }, error => {
      this.messageService.showError(error);
    });
  }
  checkListMeetingType(){
    for (let i = 0; i < this.listMeetingType.length; i++) {
      if (this.Id == this.listMeetingType[i].Id)
        this.listMeetingType.splice(i, 1);
    }
  }

  getInfo() {
    this.meetingTypeService.getById(this.Id).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  create(isContinue) {
    this.meetingTypeService.createMeeting(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới chủng loại meeting thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới chủng loại meeting thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.meetingTypeService.update(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chủng loại meeting thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
