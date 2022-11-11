import { Component, OnInit, Input } from '@angular/core';
import { PracticeService } from '../../service/practice.service';
import { MessageService, Constants } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';

@Component({
  selector: 'app-practice-content',
  templateUrl: './practice-content.component.html',
  styleUrls: ['./practice-content.component.scss']
})
export class PracticeContentComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private servicePractice: PracticeService,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private modalService: NgbModal,
    private serviceHistory: HistoryVersionService
  ) { }

  model: any = {
    Id: '',
    Content: '',
  }

  ngOnInit() {
    this.model.Id = this.Id;
    this.getPracticeInfo();
  }

  getPracticeInfo() {
    this.servicePractice.getPracticeInfo(this.model).subscribe((data: any) => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  save(){
    this.updatePractice();
  }

  updatePractice() {
    this.servicePractice.updatePractice(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật bài thực hành/công đoạn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Practice;
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
}
