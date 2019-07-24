import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForwardTicketComponent } from './forward-ticket.component';

describe('ForwardTicketComponent', () => {
  let component: ForwardTicketComponent;
  let fixture: ComponentFixture<ForwardTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForwardTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForwardTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
