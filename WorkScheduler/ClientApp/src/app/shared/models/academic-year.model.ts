import { Dictionary } from "./dictionary.model";
import { WorkSchedule } from "./work-schedule.model";

export class AcademicYear extends Dictionary<number>{
  workSchedules?: WorkSchedule[];
}
