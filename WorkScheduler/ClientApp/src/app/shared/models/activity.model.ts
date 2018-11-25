import { Dictionary } from "./dictionary.model";

export class Activity extends Dictionary<number> {
  color: Color;
}

export enum Color {
  white,
  red,
  blue,
  green,
  purple,
}
