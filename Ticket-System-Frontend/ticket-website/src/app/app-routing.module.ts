import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {OrdersComponent} from "./orders/orders.component";
import {OrderComponent} from "./orders/order/order.component";
import {CommonModule} from "@angular/common";
import {TicketsComponent} from "./tickets/tickets.component";
import {TicketComponent} from "./tickets/ticket/ticket.component";

const routes: Routes = [
  {path: 'Orders', component: OrdersComponent},
  {path: 'Orders/:id', component: OrderComponent},
  {path: 'Tickets', component: TicketsComponent},
  {path: 'Tickets/:id', component: TicketComponent}
];

@NgModule({
  imports: [CommonModule,
    RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
