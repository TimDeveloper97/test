import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessoryViewDeatilComponent } from './accessory-view-deatil.component';

describe('AccessoryViewDeatilComponent', () => {
  let component: AccessoryViewDeatilComponent;
  let fixture: ComponentFixture<AccessoryViewDeatilComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccessoryViewDeatilComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessoryViewDeatilComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
