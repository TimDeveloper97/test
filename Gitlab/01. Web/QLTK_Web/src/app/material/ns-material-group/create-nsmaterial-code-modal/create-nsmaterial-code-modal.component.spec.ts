import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateNsmaterialCodeModalComponent } from './create-nsmaterial-code-modal.component';

describe('CreateNsmaterialCodeModalComponent', () => {
  let component: CreateNsmaterialCodeModalComponent;
  let fixture: ComponentFixture<CreateNsmaterialCodeModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateNsmaterialCodeModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateNsmaterialCodeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
