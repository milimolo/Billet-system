import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Ticket} from "./ticket.model";
import {environment} from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  constructor(private http: HttpClient) { }

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(environment.ticketApiUrl + '/Tickets');
  }

  addTicket(ticket: Ticket): Observable<Ticket> {
    return this.http.post<Ticket>(environment.ticketApiUrl + '/Tickets', ticket);
  }

  getTicket(id: number): Observable<Ticket>{
    return this.http.get<Ticket>(environment.ticketApiUrl + '/Tickets/' + id);
  }
}
