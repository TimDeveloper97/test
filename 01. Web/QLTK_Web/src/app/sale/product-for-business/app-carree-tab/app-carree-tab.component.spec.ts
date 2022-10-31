import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppCarreeTabComponent } from './app-carree-tab.component';

describe('AppCarreeTabComponent', () => {
  let component: AppCarreeTabComponent;
  let fixture: ComponentFixture<AppCarreeTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppCarreeTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppCarreeTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
