import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseWorktypeComponent } from './choose-worktype.component';

describe('ChooseWorktypeComponent', () => {
  let component: ChooseWorktypeComponent;
  let fixture: ComponentFixture<ChooseWorktypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseWorktypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseWorktypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
