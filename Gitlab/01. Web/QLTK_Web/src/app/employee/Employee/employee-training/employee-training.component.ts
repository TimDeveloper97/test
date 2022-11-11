import { Component, OnInit, Input } from '@angular/core';
import { Constants, Configuration, MessageService } from 'src/app/shared';
import { EmployeeUpdateService } from '../../service/employee-update.service';

@Component({
  selector: 'app-employee-training',
  templateUrl: './employee-training.component.html',
  styleUrls: ['./employee-training.component.scss']
})
export class EmployeeTrainingComponent implements OnInit {

  constructor(
    public constants: Constants,
    private config: Configuration,
    private service: EmployeeUpdateService,
    private messageService: MessageService,
  ) { }

  @Input() Id: string;
  @Input() EmployeeName: string;
  @Input() EmployeeCode: string;
  
  listData: any[] = [];
  model: any = {
    Id: '',
  }
  StartIndex = 1;
  ngOnInit() {
    this.model.Id = this.Id;
    this.getListEmployeeTraining();
  }
  getListEmployeeTraining() {
    this.service.getListEmployeeTraining(this.model).subscribe(
      data => {
        this.listData = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }



}
