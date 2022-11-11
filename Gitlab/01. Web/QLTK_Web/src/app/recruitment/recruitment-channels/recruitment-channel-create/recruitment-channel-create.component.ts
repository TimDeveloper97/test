import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { clear } from 'console';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { RecruitmentChannelService } from '../../services/recruitment-channel.service';

@Component({
  selector: 'app-recruitment-channel-create',
  templateUrl: './recruitment-channel-create.component.html',
  styleUrls: ['./recruitment-channel-create.component.scss']
})
export class RecruitmentChannelCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private recruitmentChannelService: RecruitmentChannelService,
    private combobox: ComboboxService,
    public constant: Constants
  ) { }

  modalInfo = {
    Title: 'Thêm mới kênh tuyển dụng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  channelModel: any = {
    Id:null,
    Name: null,
    Note: null,
    Status: true
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa Kênh tuyển dụng';
      this.modalInfo.SaveText = 'Lưu';
      this.getChannelById();
    }
    else {
      this.modalInfo.Title = 'Thêm mới Kênh tuyển dụng';
    }
  }

  getChannelById() {
    this.recruitmentChannelService.getById({ Id: this.id }).subscribe(
      result => {
        this.channelModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.recruitmentChannelService.create(this.channelModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới Kênh tuyển dụng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới Kênh tuyển dụng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.recruitmentChannelService.update(this.channelModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật Kênh tuyển dụng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.channelModel = {
      Id: '',
      Name: '',
      Note: '',
      Status: true
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
