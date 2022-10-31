import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecializeCreateComponent } from './specialize-create.component';

describe('SpecializeCreateComponent', () => {
  let component: SpecializeCreateComponent;
  let fixture: ComponentFixture<SpecializeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpecializeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecializeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
