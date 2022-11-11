import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Constants, MessageService } from 'src/app/shared';
import { ProjectProductService } from '../../service/project-product.service';

@Component({
  selector: 'app-incurred-material',
  templateUrl: './incurred-material.component.html',
  styleUrls: ['./incurred-material.component.scss']
})
export class IncurredMaterialComponent implements OnInit {
  @Input() ProjectProductId: string;
  @Input() ModuleId: string;

  constructor(
    public constant: Constants,
    private messageService: MessageService,
    private service: ProjectProductService
  ) { }

  total: number;
  height: number;
  listData: any[] = [];
  model: any = {
    Id: '',
    ModuleId: '',
    Name: '',
    Code: ''
  }

  @ViewChild('scrollHeaderOne') scrollHeaderOne: ElementRef;

  ngOnInit(): void {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.height = window.innerHeight - 330;

    this.model.Id = this.ProjectProductId;
    this.model.ModuleId = this.ModuleId;
    this.getIncurredMaterial();
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
  }

  getIncurredMaterial() {
    this.service.getIncurredMaterial(this.model).subscribe((data: any) => {
      if (data.LitsMaterial) {
        this.total = data.Total;
        this.listData = data.LitsMaterial;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clear() {
    this.model = {
      Id: this.ProjectProductId,
      ModuleId: this.ModuleId,
      Name: '',
      Code: ''
    };

    this.getIncurredMaterial();
  }
}
