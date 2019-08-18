import { Dictionary } from "./dictionary.model";

export class File extends Dictionary<number> {
    extension: string;
    sizeMb: number;
}