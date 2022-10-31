import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { clear } from 'console';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';

@Component({
  selector: 'app-apply-check-candidate',
  templateUrl: './apply-check-candidate.component.html',
  styleUrls: ['./apply-check-candidate.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ApplyCheckCandidateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private applyService: ApplyService,
    private combobox: ComboboxService,
    public constant: Constants,
    private router: Router,
  ) { }

  isAction: boolean = false;
  id: string;

  candidateModel: any = {
    Name: null,
    PhoneNumber: null,
    Email: null
  };

  candidates: any[] = [];

  ngOnInit(): void {

  }

  checkCandidate() {
    this.applyService.checkCandidate(this.candidateModel).subscribe(
      data => {

        if (data && data.length > 0) {
          this.candidates = data;
          this.messageService.showWarning('Ứng viên có thể đã được lập hồ sơ!');
        } else {
          //const navigationExtras: NavigationExtras = {queryParams: {name:  this.candidateModel.Name, phone: this.candidateModel.PhoneNumber, email: this.candidateModel.Email}};
          this.router.navigate(['tuyen-dung/ung-tuyen/them-moi',  this.candidateModel.Name ,  this.candidateModel.PhoneNumber, this.candidateModel.Email] );


          this.activeModal.close();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  apply(id) {
    this.router.navigate(['tuyen-dung/ung-tuyen/them-moi-yeu-cau'], { queryParams: { candidateId: id } });
    this.activeModal.close();
  }

  closeModal() {
    this.activeModal.close();
  }
}
