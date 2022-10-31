import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruitmentChannelCreateComponent } from './recruitment-channel-create.component';

describe('RecruitmentChannelCreateComponent', () => {
  let component: RecruitmentChannelCreateComponent;
  let fixture: ComponentFixture<RecruitmentChannelCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruitmentChannelCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecruitmentChannelCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
