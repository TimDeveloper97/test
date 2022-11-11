import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowTabComponent } from './follow-tab.component';

describe('FollowTabComponent', () => {
  let component: FollowTabComponent;
  let fixture: ComponentFixture<FollowTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FollowTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FollowTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
