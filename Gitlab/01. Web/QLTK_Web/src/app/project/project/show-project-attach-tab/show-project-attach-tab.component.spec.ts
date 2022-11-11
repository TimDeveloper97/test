import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectAttachTabComponent } from './show-project-attach-tab.component';

describe('ShowProjectAttachTabComponent', () => {
  let component: ShowProjectAttachTabComponent;
  let fixture: ComponentFixture<ShowProjectAttachTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectAttachTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectAttachTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
