import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuoteStepManageComponent } from './quote-step-manage.component';

describe('QuoteStepManageComponent', () => {
  let component: QuoteStepManageComponent;
  let fixture: ComponentFixture<QuoteStepManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuoteStepManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteStepManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
