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

  //front only
  dateToGroup: any;
  dateToShow: string;

  repeat: boolean;
  dateTo: Date;
  days: number[];
}
