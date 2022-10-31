import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductCreateComponent } from './project-product-create.component';

describe('ProjectProductCreateComponent', () => {
  let component: ProjectProductCreateComponent;
  let fixture: ComponentFixture<ProjectProductCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectProductCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
