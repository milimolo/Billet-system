import {OrderLine} from "./orderLine.model";

export interface Order {
  id?: number;
  customerId: number;
  date: Date;
  orderLines?: OrderLine[];
  orderStatus?: OrderStatus;
  totalPrice: number;
}

export enum OrderStatus {
  Tentative,
  Completed,
  Cancelled
}
