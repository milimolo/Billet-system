import {Component, OnInit} from '@angular/core';
import {Order} from "../shared/order.model";
import {OrderLine} from "../shared/orderLine.model";
import {OrderService} from "../shared/order.service";
import {BsModalRef} from "ngx-bootstrap/modal";
import {Location} from "@angular/common";

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss']
})
export class CreateOrderComponent implements OnInit{

  constructor(private orderService: OrderService,
              private modalRef: BsModalRef,
              private location: Location) {
  }

  order: Order;
  orderlines: OrderLine[] = [];
  totalPrice: number = 0;
  cart = JSON.parse(<string>localStorage.getItem('currentCart'));
  user = JSON.parse(<string>localStorage.getItem('currentUser'));

  orderConfirmed = false;

  ngOnInit(): void {
    // @ts-ignore
    // let cart = JSON.parse(localStorage.getItem('currentCart'));
    if(this.cart){
      for (const cartElement of this.cart) {
        let tempOL: OrderLine = {
          productId: cartElement.id,
          noOfItems: cartElement.quantity,
          price: cartElement.price
        };
        this.totalPrice += tempOL.price;
        this.orderlines.push(tempOL);
      }

      this.order = {
        id: 0,
        customerId: this.user.id,
        orderLines: this.orderlines,
        totalPrice: this.totalPrice,
        orderStatus: 0,
        date: new Date()
      };
      console.log(this.order);
    }
  }

  placeOrder(order: Order) {
    if(this.cart.length !== 0){
      this.orderService.addOrder(order)
        .subscribe(response =>{
          if(response){
            console.log(response);
            this.orderConfirmed = true;
            let cart = this.cart;
            cart = [];
            localStorage.setItem('currentCart', JSON.stringify(cart));
          }
        });
    }
  }
}
