import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPracticeMaterialComsumableComponent } from './select-practice-material-comsumable.component';

describe('SelectPracticeMaterialComsumableComponent', () => {
  let component: SelectPracticeMaterialComsumableComponent;
  let fixture: ComponentFixture<SelectPracticeMaterialComsumableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectPracticeMaterialComsumableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectPracticeMaterialComsumableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
