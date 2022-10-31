import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruitmentRequestManageComponent } from './recruitment-request-manage.component';

describe('RecruitmentRequestManageComponent', () => {
  let component: RecruitmentRequestManageComponent;
  let fixture: ComponentFixture<RecruitmentRequestManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruitmentRequestManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecruitmentRequestManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
