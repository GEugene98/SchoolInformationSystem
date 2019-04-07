import { Dictionary } from "./dictionary.model";
import { User } from "./user";
import { Action } from "./action.model";
import { Time } from "./time.model";
import { Checklist } from "./checklist.model";

export class Ticket extends Dictionary<number> {
  action: Action;
  comment: string;
  done: boolean;
  important: boolean;
  checklist: Checklist;
  date: Date;
  hours: number;
  minutes: number;
  start: Time;
  end: Time;
  hasChecklist: boolean;
  checklistId: number;
  userId: string;
  user: User;

  //front only
  dateToGroup: any;
  dateToShow: string;
  blockedData: boolean;
  blockedTime: boolean;

  repeat: boolean;
  dateTo: Date;
  days: number[];
}
