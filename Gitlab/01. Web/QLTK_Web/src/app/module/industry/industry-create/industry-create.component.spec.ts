import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustryCreateComponent } from './industry-create.component';

describe('IndustryCreateComponent', () => {
  let component: IndustryCreateComponent;
  let fixture: ComponentFixture<IndustryCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustryCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustryCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
