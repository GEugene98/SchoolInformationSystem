import { Ticket } from "./ticket.model";

export class TicketPack {
  dateToShow: string;
  date: Date;
  timeGroups: TimeGroup[];
}

export class TimeGroup {
  hour: number;
  tickets: Ticket[];
}
