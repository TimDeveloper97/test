import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowQcCheckListComponent } from './show-qc-check-list.component';

describe('ShowQcCheckListComponent', () => {
  let component: ShowQcCheckListComponent;
  let fixture: ComponentFixture<ShowQcCheckListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowQcCheckListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowQcCheckListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
