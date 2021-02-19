import { Dictionary } from "../../shared/models/dictionary.model";

export class PlaningRecord extends Dictionary<number> {
    date: Date;
    hours: number;
    comment: number;
}