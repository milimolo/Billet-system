import {Component, OnInit} from '@angular/core';
import {Ticket} from "./shared/ticket.model";
import {TicketService} from "./shared/ticket.service";

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.scss']
})
export class TicketsComponent implements OnInit {
  tickets: any = [];
  selectedTicket: Ticket;
  constructor(private ticketService: TicketService) {  }

  ngOnInit(): void {
    this.getTickets();
  }

  onSelect(ticket: Ticket): void {
    this.selectedTicket = ticket;
  }

  getTickets(): void {
    this.ticketService.getTickets()
      .subscribe(tickets => {this.tickets = tickets});
  }
}
