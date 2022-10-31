import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimilarMaterialConfigManageComponent } from './similar-material-config-manage.component';

describe('SimilarMaterialConfigManageComponent', () => {
  let component: SimilarMaterialConfigManageComponent;
  let fixture: ComponentFixture<SimilarMaterialConfigManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimilarMaterialConfigManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimilarMaterialConfigManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
