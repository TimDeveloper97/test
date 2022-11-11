import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustryManageComponent } from './industry-manage.component';

describe('IndustryManageComponent', () => {
  let component: IndustryManageComponent;
  let fixture: ComponentFixture<IndustryManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustryManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustryManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
