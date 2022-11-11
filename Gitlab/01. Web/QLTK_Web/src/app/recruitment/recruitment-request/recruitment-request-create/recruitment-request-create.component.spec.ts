import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruitmentRequestCreateComponent } from './recruitment-request-create.component';

describe('RecruitmentRequestCreateComponent', () => {
  let component: RecruitmentRequestCreateComponent;
  let fixture: ComponentFixture<RecruitmentRequestCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruitmentRequestCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecruitmentRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
