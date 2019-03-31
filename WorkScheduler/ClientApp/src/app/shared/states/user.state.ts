import { DataStore } from "../data-store";
import { Injectable } from "@angular/core";
import { User } from "../models/user";
import { Ticket } from "../models/ticket.model";

@Injectable()
export class UserState {
  readonly currentUser = new DataStore<User>();
  readonly assignedTickets = new DataStore<Ticket[]>();
  readonly assignedTicketCount = new DataStore<number>();
}
