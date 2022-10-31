import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Constants } from 'src/app/shared';

@Component({
  selector: 'app-follow-tab',
  templateUrl: './follow-tab.component.html',
  styleUrls: ['./follow-tab.component.scss']
})
export class FollowTabComponent implements OnInit {

  constructor(public constants: Constants,
    private router: Router,) { }

  follows: any[] = [];

  followModel: any = {
    FollowDate: '',
    Content: ''
  };

  startIndex: number = 1;

  ngOnInit(): void {
  }

  showCreateUpdate(id: any) {

  }

  deleteFollow(index: any) {
    if (this.follows.length > 0) {
      this.follows.splice(index, 1);
    }
  }

  validateFollow() {
    let newModel = Object.assign({}, this.followModel);
    this.follows.push(newModel);
    this.followModel = {
      FollowDate: '',
      Content: ''
    }
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['nhan-vien/ho-so-ung-vien']);
  }

}
