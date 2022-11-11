import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleGroupChooseProductStandardComponent } from './module-group-choose-product-standard.component';

describe('ModuleGroupChooseProductStandardComponent', () => {
  let component: ModuleGroupChooseProductStandardComponent;
  let fixture: ComponentFixture<ModuleGroupChooseProductStandardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleGroupChooseProductStandardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleGroupChooseProductStandardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
