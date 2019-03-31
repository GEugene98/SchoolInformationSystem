import { User } from "./user";
import { Ticket } from "./ticket.model";

export class Checklist {
  user: User;
  name: string;
  createdOn: Date;
  deadline: Date;
  comment: string;
  tickets: Ticket[];
}
