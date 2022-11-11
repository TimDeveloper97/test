import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserCreateComponent } from './group-user-create.component';

describe('GroupUserCreateComponent', () => {
  let component: GroupUserCreateComponent;
  let fixture: ComponentFixture<GroupUserCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupUserCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
