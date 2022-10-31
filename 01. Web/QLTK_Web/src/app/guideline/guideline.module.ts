import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GuidelineRoutingModule } from './guideline-routing.module';
import { GuideLineComponent } from './guide-line/guide-line.component';
import { EditorModule } from '@tinymce/tinymce-angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { GuidelineUpdateComponent } from './guideline-update/guideline-update.component';


@NgModule({
  declarations: [
    GuideLineComponent,
    GuidelineUpdateComponent
  ],
  imports: [
    CommonModule,
    GuidelineRoutingModule,
    EditorModule,
    NgbModule,
    FormsModule,
  ]
})
export class GuidelineModule { }
