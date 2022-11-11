import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimilarMaterialCreateComponent } from './similar-material-create.component';

describe('SimilarMaterialCreateComponent', () => {
  let component: SimilarMaterialCreateComponent;
  let fixture: ComponentFixture<SimilarMaterialCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimilarMaterialCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimilarMaterialCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
