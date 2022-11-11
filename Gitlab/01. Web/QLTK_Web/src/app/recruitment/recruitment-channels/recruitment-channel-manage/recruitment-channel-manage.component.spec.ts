import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruitmentChannelManageComponent } from './recruitment-channel-manage.component';

describe('RecruitmentChannelManageComponent', () => {
  let component: RecruitmentChannelManageComponent;
  let fixture: ComponentFixture<RecruitmentChannelManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruitmentChannelManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecruitmentChannelManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
