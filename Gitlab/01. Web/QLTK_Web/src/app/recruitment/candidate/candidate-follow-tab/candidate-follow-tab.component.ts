import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Constants, MessageService, DateUtils } from 'src/app/shared';
import { CandidateService } from '../../services/candidate.service';

@Component({
  selector: 'app-candidate-follow-tab',
  templateUrl: './candidate-follow-tab.component.html',
  styleUrls: ['./candidate-follow-tab.component.scss']
})
export class CandidateFollowTabComponent implements OnInit {
  @Input() candidateId: string;
  constructor(public constants: Constants,
    private router: Router,
    private messageService: MessageService,
    private candidateService: CandidateService,
    private dateUtils: DateUtils) { }

  follows: any[] = [];

  followModel: any = {
    FollowDate: '',
    Content: ''
  };

  startIndex: number = 1;

  ngOnInit(): void {
    this.getFollows();
  }

  getFollows() {
    this.candidateService.getFollow(this.candidateId).subscribe(
      data => {
        this.follows = data;
        this.follows.forEach(element => {
          if (element.FollowDate) {
            element.FollowDateV = this.dateUtils.convertDateToObject(element.FollowDate);
          }
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Liên hệ này không?").then(
      data => {
        this.deleteFollow(index);
      },
      error => {

      }
    );
  }

  deleteFollow(index: any) {
    if (this.follows.length > 0) {
      this.follows.splice(index, 1);
    }
  }

  addFollow() {
    if (this.followModel.FollowDate && this.followModel.Content) {
      this.messageService.showMessage('Bạn chưa nhập đủ thông tin');
      return;
    }

    let newModel = Object.assign({}, this.followModel);
    this.follows.push(newModel);
    this.followModel = {
      FollowDate: '',
      Content: ''
    }
  }

  save() {

    this.follows.forEach(element => {
      if (element.FollowDateV) {
        element.FollowDate = this.dateUtils.convertObjectToDate(element.FollowDateV);
      }
    });

    this.candidateService.updateFollow(this.candidateId, this.follows).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật thông tin liên hệ thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['tuyen-dung/ung-tuyen']);
  }

}
