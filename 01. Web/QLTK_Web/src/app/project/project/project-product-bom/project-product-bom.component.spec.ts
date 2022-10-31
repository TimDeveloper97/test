import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductBomComponent } from './project-product-bom.component';

describe('ProjectProductBomComponent', () => {
  let component: ProjectProductBomComponent;
  let fixture: ComponentFixture<ProjectProductBomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectProductBomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductBomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
