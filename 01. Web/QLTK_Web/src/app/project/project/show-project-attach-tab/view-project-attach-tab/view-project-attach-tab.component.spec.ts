import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProjectAttachTabComponent } from './view-project-attach-tab.component';

describe('ViewProjectAttachTabComponent', () => {
  let component: ViewProjectAttachTabComponent;
  let fixture: ComponentFixture<ViewProjectAttachTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewProjectAttachTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewProjectAttachTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
