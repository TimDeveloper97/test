import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectTransferAttachComponent } from './show-project-transfer-attach.component';

describe('ShowProjectTransferAttachComponent', () => {
  let component: ShowProjectTransferAttachComponent;
  let fixture: ComponentFixture<ShowProjectTransferAttachComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectTransferAttachComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectTransferAttachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
