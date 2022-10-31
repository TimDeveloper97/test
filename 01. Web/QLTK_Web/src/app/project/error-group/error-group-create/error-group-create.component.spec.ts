import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorGroupCreateComponent } from './error-group-create.component';

describe('ErrorGroupCreateComponent', () => {
  let component: ErrorGroupCreateComponent;
  let fixture: ComponentFixture<ErrorGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
