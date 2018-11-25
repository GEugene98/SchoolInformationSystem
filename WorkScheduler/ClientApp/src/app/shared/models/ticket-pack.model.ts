import { Ticket } from "./ticket.model";

export class TicketPack {
  date: Date;
  timeGroups: TimeGroup[];
}

export class TimeGroup {
  hour: number;
  tickets: Ticket[];
}
