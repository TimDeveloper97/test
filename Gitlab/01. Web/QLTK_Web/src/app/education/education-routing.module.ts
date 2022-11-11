import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ClassRoomCreateComponent } from './classroom/class-room-create/class-room-create.component';
import { ClassRoomManageComponent } from './classroom/class-room-manage/class-room-manage.component';
import { EducationProgramManageComponent } from './education-program/education-program-manage/education-program-manage.component';
import { JobCreateComponent } from './job/job-create/job-create.component';
import { JobManageComponent } from './job/job-manage/job-manage.component';
import { JobUpdateComponent } from './job/job-update/job-update.component';
import { JobgroupMangeComponent } from './jobgroup/jobgroup-mange/jobgroup-mange.component';
import { RoomTypeManageComponent } from './roomtype/room-type-manage/room-type-manage.component';
import { SubjectsManageComponent } from './subjects/subjects-manage/subjects-manage.component';

const routes: Routes = [
  // { path: 'quan-ly-loai-phong-hoc', component: RoomTypeManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-mon-hoc', component: SubjectsManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-chuong-trinh-dao-tao', component: EducationProgramManageComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-phong-hoc',
    children: [
      { path: '', component: ClassRoomManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: ClassRoomCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: ClassRoomCreateComponent, canActivate: [AuthGuard] },
    ]
  },
  // { path: 'quan-ly-nhom-nghe', component: JobgroupMangeComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-nghe', 
    children: [
      { path: '', component: JobManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi/:GroupId', component: JobCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: JobUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EducationRoutingModule { }
