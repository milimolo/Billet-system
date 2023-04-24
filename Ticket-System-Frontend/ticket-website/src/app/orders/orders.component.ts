import {Component, OnInit} from '@angular/core';
import {OrderService} from "./shared/order.service";
import {Order} from "./shared/order.model";

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit{
  orders: any = [];
  selectedOrder: any = [];

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.getOrders();
  }

  onSelect(order: Order): void {
    this.selectedOrder = order;
  }

  getOrders(): void {
    this.orderService.getOrders()
      .subscribe(orders => {this.orders = orders});
  }
}
