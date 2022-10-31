import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { AppSetting, Constants } from 'src/app/shared';
import { PracticeService } from '../../service/practice.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-practice-version-tab',
  templateUrl: './practice-version-tab.component.html',
  styleUrls: ['./practice-version-tab.component.scss']
})
export class PracticeVersionTabComponent implements OnInit {

  constructor(
    private router: Router,
    private activeModal: NgbActiveModal,
    private titleservice: Title,
    public appSetting: AppSetting,
    private praticeService: PracticeService,
    public constants: Constants
  ) { }
  
  @Input() Id: string;
  ngOnInit() {
    
    this.modelModule.Id = this.Id;
    this.getHistoryInfo();
  }

  isAction: boolean = false;
  ListHistory: any[] = [];
  StartIndex = 1;
  modelModule: any = {
    OrderBy: 'Version',
    ListHistory: []
  }

  getHistoryInfo() {
    this.praticeService.getPracticeInfo(this.modelModule).subscribe(data => {
      this.ListHistory = data.ListHistory;
    });
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }

}
