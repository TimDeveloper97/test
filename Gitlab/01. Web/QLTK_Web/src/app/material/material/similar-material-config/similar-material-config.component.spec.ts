import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimilarMaterialConfigComponent } from './similar-material-config.component';

describe('SimilarMaterialConfigComponent', () => {
  let component: SimilarMaterialConfigComponent;
  let fixture: ComponentFixture<SimilarMaterialConfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimilarMaterialConfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimilarMaterialConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
