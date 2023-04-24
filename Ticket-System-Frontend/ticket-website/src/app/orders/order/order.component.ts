import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {OrderService} from "../shared/order.service";
import {Order} from "../shared/order.model";

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit{

  constructor(private route: ActivatedRoute,
              private orderService: OrderService) { }
  @Input() order: Order;
  ngOnInit(): void {
    const id = +!this.route.snapshot.queryParamMap.get('id');
    this.orderService.getOrder(id)
      .subscribe(order => {
        this.order = order;
      });
  }

}
