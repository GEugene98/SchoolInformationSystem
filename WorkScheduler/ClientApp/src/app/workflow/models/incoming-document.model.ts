import { Dictionary } from "../../shared/models/dictionary.model";
import { Ticket } from "../../shared/models/ticket.model";
import { User } from "../../shared/models/user";

export class IncomingDocument {
  id: number;
  num: number;
  name: string;
  organization: Dictionary<number>;
  type: string;
  taken: Date;
  deadline: Date;
  done: boolean;
  description: string;
  userId: string;
  user: User;
  files: File[];
  tickets: Ticket[];

  //For creating
  organizationId?: number;
  createTicket: boolean;
  typeId?: number;
  userIdsToCheck: string[];
}