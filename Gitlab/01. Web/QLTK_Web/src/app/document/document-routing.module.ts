import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { DocumentCreateComponent } from './document-create/document-create.component';
import { DocumentManageComponent } from './document-manage/document-manage.component';
import { DocumentSearchManageComponent } from './document-search-manage/document-search-manage.component';
import { DocumentTypeManageComponent } from './document-type/document-type-manage/document-type-manage.component';

const routes: Routes = [
  {
    path: 'quan-ly-tai-lieu',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: DocumentManageComponent },
      { path: 'them-moi', component: DocumentCreateComponent },
      { path: 'chinh-sua/:Id', component: DocumentCreateComponent },
    ]
  },
  { path: 'tra-cuu-tai-lieu', component: DocumentSearchManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-loai-tai-lieu', component: DocumentTypeManageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentRoutingModule { }
