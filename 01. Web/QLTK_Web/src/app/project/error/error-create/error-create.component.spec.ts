import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorCreateComponent } from './error-create.component';

describe('ErrorCreateComponent', () => {
  let component: ErrorCreateComponent;
  let fixture: ComponentFixture<ErrorCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
