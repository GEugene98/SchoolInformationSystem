import { Action } from "./action.model";

export class Protocol {
  id: number;
  name: string;
  action: Action;
  createdAt: Date;
  number: string;
  header: string;
  chairman: string;
  secretary: string;
  attended: string;
  protocolContentJSON: string;
}