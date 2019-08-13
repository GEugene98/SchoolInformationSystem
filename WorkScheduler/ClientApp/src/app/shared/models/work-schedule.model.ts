import { Activity } from "./activity.model";
import { AcademicYear } from "./academic-year.model";
import { Action } from "./action.model";
import { User } from "./user";

export class WorkSchedule {
  id: number;
  name: string;
  activity: Activity;
  academicYear: AcademicYear;
  academicYearName: string;
  actions: Action[];
  user: User;
}
