import { Action } from "./action.model";
import { Data } from "@angular/router";

export class GeneralSchedule {
  days: Day[];
  start: Data;
  end: Data;
}

export class Day {
  date: Date;
  shortDayOfWeekName: string;
  isDayOff: boolean;
  actions: Action[]
}
