import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductBomCreateComponent } from './project-product-bom-create.component';

describe('ProjectProductBomCreateComponent', () => {
  let component: ProjectProductBomCreateComponent;
  let fixture: ComponentFixture<ProjectProductBomCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectProductBomCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductBomCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
