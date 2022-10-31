import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuoteStepCreateComponent } from './quote-step-create.component';

describe('QuoteStepCreateComponent', () => {
  let component: QuoteStepCreateComponent;
  let fixture: ComponentFixture<QuoteStepCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuoteStepCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteStepCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
