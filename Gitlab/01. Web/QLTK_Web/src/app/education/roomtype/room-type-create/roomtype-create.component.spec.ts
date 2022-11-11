import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomtypeCreateComponent } from './roomtype-create.component';

describe('RoomtypeCreateComponent', () => {
  let component: RoomtypeCreateComponent;
  let fixture: ComponentFixture<RoomtypeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoomtypeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomtypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
