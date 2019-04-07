import { User } from "./user";
import { Ticket } from "./ticket.model";

export class Checklist {
  user: User;
  id: number;
  name: string;
  createdOn: Date;
  deadline: Date;
  comment: string;
  tickets: Ticket[];

  assignedCount: number; 
  acceptedCount: number;  
  doneCount: number; 
  totalCount: number; 

  //frontend only
  chartData: any;
}
