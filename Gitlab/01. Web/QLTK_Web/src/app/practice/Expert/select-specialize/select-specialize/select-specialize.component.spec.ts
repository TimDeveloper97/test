import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectSpecializeComponent } from './select-specialize.component';

describe('SelectSpecializeComponent', () => {
  let component: SelectSpecializeComponent;
  let fixture: ComponentFixture<SelectSpecializeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectSpecializeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectSpecializeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
