import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowListMoneyCollectionComponent } from './show-list-money-collection.component';

describe('ShowListMoneyCollectionComponent', () => {
  let component: ShowListMoneyCollectionComponent;
  let fixture: ComponentFixture<ShowListMoneyCollectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowListMoneyCollectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowListMoneyCollectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
