export interface Ticket {
  id: number;
  name: string;
  location: string;
  eventDate: Date;
  price: number;
  ticketsRemaining: number;
  ticketsReserved: number;
}
