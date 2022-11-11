import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeVersionTabComponent } from './practice-version-tab.component';

describe('PracticeVersionTabComponent', () => {
  let component: PracticeVersionTabComponent;
  let fixture: ComponentFixture<PracticeVersionTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeVersionTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeVersionTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
