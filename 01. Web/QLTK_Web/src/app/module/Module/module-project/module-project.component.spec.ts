import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleProjectComponent } from './module-project.component';

describe('ModuleProjectComponent', () => {
  let component: ModuleProjectComponent;
  let fixture: ComponentFixture<ModuleProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
