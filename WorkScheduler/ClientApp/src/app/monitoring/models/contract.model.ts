import { User } from "../../shared/models/user";
import { ContractStatus } from "./contractstatus.model";
import { Organization } from "./organization.model";

export class Contract {

  constructor() {

  }
  id: number;
  organization: Organization;
  number: string;
  signingDate: Date;
  subject: string;
  signedBy: User;
  sum: number;
  status: ContractStatus;
  controlDate: Date;
  comment: string;
}