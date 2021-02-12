import { User } from "../../shared/models/user";
import { Organization } from "./organization.model";

export class Contract {

  constructor() {

  }
  id: number;
  organization: Organization;
  number: string;
  signingData: Date;
  subject: string;
  signedBy: User;
  sum: number;
  status: ContractStatus;
  controlDate: Date;
  comment: string;
}