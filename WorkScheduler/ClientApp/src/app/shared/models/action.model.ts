import { Dictionary } from "./dictionary.model";
import { User } from "./user";
import { ConfirmationForm } from "./confirmation-form.model";
import { Activity } from "./activity.model";
import { ActionStatus } from "../enums/action-status.enum";
import { WorkSchedule } from "./work-schedule.model";

export class Action extends Dictionary<number> {
  date: Date;
  responsibles: User[];
  confirmationForm: ConfirmationForm;
  confirmationFormId: number;
  status: ActionStatus;
  activity: Activity;
  workSchedule: WorkSchedule;

  scheduleName: string;
  authorName: string;

  endDate: Date;

  isDeleted: boolean;

  //only for frontend
  selected: boolean;
}
