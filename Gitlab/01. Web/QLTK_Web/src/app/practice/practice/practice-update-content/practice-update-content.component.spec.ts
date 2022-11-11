import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeUpdateContentComponent } from './practice-update-content.component';

describe('PracticeUpdateContentComponent', () => {
  let component: PracticeUpdateContentComponent;
  let fixture: ComponentFixture<PracticeUpdateContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeUpdateContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeUpdateContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
