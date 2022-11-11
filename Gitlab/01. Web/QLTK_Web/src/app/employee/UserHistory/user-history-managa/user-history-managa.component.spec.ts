import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserHistoryManagaComponent } from './user-history-managa.component';

describe('UserHistoryManagaComponent', () => {
  let component: UserHistoryManagaComponent;
  let fixture: ComponentFixture<UserHistoryManagaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserHistoryManagaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserHistoryManagaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
