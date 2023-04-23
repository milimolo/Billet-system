import {OrderLine} from "./orderLine.model";

export interface Order {
  id: number;
  date: Date;
  customerId: number;
  orderLines: OrderLine[];
  totalPrice: number;
}
