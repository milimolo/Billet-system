import {Component, Input, OnInit, TemplateRef} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {Ticket} from "../shared/ticket.model";
import {TicketService} from "../shared/ticket.service";
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {DatePipe, Location} from "@angular/common";

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})
export class TicketComponent implements OnInit{

  constructor(private route: ActivatedRoute,
              private location: Location,
              private ticketService: TicketService,
              private modalService: BsModalService,
              private datePipe: DatePipe) { }
  modalRef: BsModalRef;
  message: string;
  formattedDate: string;
  amountLeft: number;

  @Input() ticket: Ticket;
  @Input() noOfTickets: number;

  ngOnInit(): void {
    const id = this.route.snapshot.params['id'];
    this.ticketService.getTicket(id)
      .subscribe(ticket => {
        this.ticket = ticket;
        this.formattedDate = <string>this.datePipe.transform(this.ticket.eventDate, 'dd-MM-yyyy HH:mm');
        this.amountLeft = ticket.ticketsRemaining - ticket.ticketsReserved;
      });
  }

  putInCart(ticketId: number, ticketAmount: number, template: TemplateRef<any>): void {
    if(ticketAmount > 0 && ticketAmount < 50 && this.noOfTickets < this.amountLeft){
      const cartItem = {
        id: this.ticket.id,
        quantity: this.noOfTickets,
        price: this.ticket.price * this.noOfTickets,
        name: this.ticket.name,
        date: this.formattedDate
      };

      // @ts-ignore
      let cart = JSON.parse(localStorage.getItem('currentCart'));
      let isInCart = false;
      if(cart){
        // @ts-ignore
        isInCart = cart.some(item => item.id === cartItem.id);
      } else {
        cart = [];
      }

      if(isInCart) {
        // @ts-ignore type of any
        cart.map(item => {
          if (item.id === cartItem.id) {
            item.quantity += cartItem.quantity
            item.price += cartItem.quantity * this.ticket.price
          }
          return item;
        });
      } else {
        cart.push(cartItem);
      }

      localStorage.setItem('currentCart', JSON.stringify(cart));
      this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
    }
  }

  confirm(): void {
    this.message = 'Confirmed!';
    this.modalRef.hide();
    this.location.back();
  }
}
