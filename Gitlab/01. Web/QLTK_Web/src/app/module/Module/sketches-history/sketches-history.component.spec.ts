import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SketchesHistoryComponent } from './sketches-history.component';

describe('SketchesHistoryComponent', () => {
  let component: SketchesHistoryComponent;
  let fixture: ComponentFixture<SketchesHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SketchesHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SketchesHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
