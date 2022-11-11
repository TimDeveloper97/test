import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuidelineUpdateComponent } from './guideline-update.component';

describe('GuidelineUpdateComponent', () => {
  let component: GuidelineUpdateComponent;
  let fixture: ComponentFixture<GuidelineUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GuidelineUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidelineUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
