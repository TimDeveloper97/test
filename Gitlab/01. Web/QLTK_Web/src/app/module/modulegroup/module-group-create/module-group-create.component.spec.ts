import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleGroupCreateComponent } from './module-group-create.component';

describe('ModuleGroupCreateComponent', () => {
  let component: ModuleGroupCreateComponent;
  let fixture: ComponentFixture<ModuleGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
