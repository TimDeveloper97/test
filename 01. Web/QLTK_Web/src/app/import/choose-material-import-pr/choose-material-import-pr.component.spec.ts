import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseMaterialImportPrComponent } from './choose-material-import-pr.component';

describe('ChooseMaterialImportPrComponent', () => {
  let component: ChooseMaterialImportPrComponent;
  let fixture: ComponentFixture<ChooseMaterialImportPrComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseMaterialImportPrComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseMaterialImportPrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
