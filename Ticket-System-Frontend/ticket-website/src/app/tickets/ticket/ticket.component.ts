import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {Ticket} from "../shared/ticket.model";
import {TicketService} from "../shared/ticket.service";

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})
export class TicketComponent implements OnInit{

  constructor(private route: ActivatedRoute,
              private ticketService: TicketService) { }
  @Input() ticket: Ticket;
  ngOnInit(): void {
    const id = +!this.route.snapshot.queryParamMap.get('id');
    this.ticketService.getTicket(id)
      .subscribe(ticket => {
        this.ticket = ticket;
      });
  }

}
