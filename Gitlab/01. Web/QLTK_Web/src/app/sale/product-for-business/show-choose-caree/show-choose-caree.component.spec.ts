import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseCareeComponent } from './show-choose-caree.component';

describe('ShowChooseCareeComponent', () => {
  let component: ShowChooseCareeComponent;
  let fixture: ComponentFixture<ShowChooseCareeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChooseCareeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseCareeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
