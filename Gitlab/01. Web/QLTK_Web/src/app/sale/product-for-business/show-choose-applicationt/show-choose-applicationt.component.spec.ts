import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseApplicationtComponent } from './show-choose-applicationt.component';

describe('ShowChooseApplicationtComponent', () => {
  let component: ShowChooseApplicationtComponent;
  let fixture: ComponentFixture<ShowChooseApplicationtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChooseApplicationtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseApplicationtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
