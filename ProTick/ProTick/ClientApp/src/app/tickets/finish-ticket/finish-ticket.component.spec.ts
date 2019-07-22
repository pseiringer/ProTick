import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishTicketComponent } from './finish-ticket.component';

describe('FinishTicketComponent', () => {
  let component: FinishTicketComponent;
  let fixture: ComponentFixture<FinishTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FinishTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FinishTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
