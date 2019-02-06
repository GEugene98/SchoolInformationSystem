import { Dictionary } from "./dictionary.model";
import { User } from "./user";
import { Action } from "./action.model";
import { Time } from "./time.model";

export class Ticket extends Dictionary<number> {
  action: Action;
  comment: string;
  done: boolean;
  important: boolean;
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
