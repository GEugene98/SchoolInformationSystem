import { Dictionary } from "../../shared/models/dictionary.model";
import { Ticket } from "../../shared/models/ticket.model";
import { User } from "../../shared/models/user";

export class OutgoingDocument {
  id: number;
  num: number;
  name: string;
  organization: Dictionary<number>;
  type: Dictionary<number>;
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
  typeId?: number;
  createTicket: boolean;
  userIdsToCheck: string[];
}