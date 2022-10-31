import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralMaterialSapComponent } from './general-material-sap.component';

describe('GeneralMaterialSapComponent', () => {
  let component: GeneralMaterialSapComponent;
  let fixture: ComponentFixture<GeneralMaterialSapComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralMaterialSapComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralMaterialSapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
