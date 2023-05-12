import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OrdersComponent } from './orders/orders.component';
import { HttpClientModule } from "@angular/common/http";
import {RouterModule} from "@angular/router";
import { OrderComponent } from './orders/order/order.component';
import { TicketsComponent } from './tickets/tickets.component';
import { TicketComponent } from './tickets/ticket/ticket.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { LoginComponent } from './shared/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { HomeComponent } from './home/home/home.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CreateOrderComponent } from './orders/create-order/create-order.component';
import {DatePipe} from "@angular/common";

@NgModule({
  declarations: [
    AppComponent,
    OrdersComponent,
    OrderComponent,
    TicketsComponent,
    TicketComponent,
    NavbarComponent,
    LoginComponent,
    HomeComponent,
    CreateOrderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot()
  ],
  providers: [
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
