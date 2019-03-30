import { User } from "./user";
import { Ticket } from "./ticket.model";

export class Checklist {
  user: User;
  createdOn: Date;
  deadline: Date;
  comment: string;
  tickets: Ticket[];
}
