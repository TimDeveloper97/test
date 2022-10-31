import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportMaterialBuyHistoryModalComponent } from './import-material-buy-history-modal.component';

describe('ImportMaterialBuyHistoryModalComponent', () => {
  let component: ImportMaterialBuyHistoryModalComponent;
  let fixture: ComponentFixture<ImportMaterialBuyHistoryModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportMaterialBuyHistoryModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportMaterialBuyHistoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
