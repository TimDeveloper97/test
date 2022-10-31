import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyCreateComponent } from './apply-create.component';

describe('ApplyCreateComponent', () => {
  let component: ApplyCreateComponent;
  let fixture: ComponentFixture<ApplyCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplyCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplyCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
