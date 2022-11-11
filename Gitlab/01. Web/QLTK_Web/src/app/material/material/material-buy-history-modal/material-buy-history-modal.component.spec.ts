import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialBuyHistoryModalComponent } from './material-buy-history-modal.component';

describe('MaterialBuyHistoryModalComponent', () => {
  let component: MaterialBuyHistoryModalComponent;
  let fixture: ComponentFixture<MaterialBuyHistoryModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialBuyHistoryModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialBuyHistoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
