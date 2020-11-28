import { User } from "../../shared/models/user";

export class Contract {

  constructor() {
      this.organization = 'fefwef';
      this.contractNumber = "wefwefwewef";
      this.subject = "wfwefwefwe";
  }
  organization: string;
  contractNumber: string;
  dateSigning: Date;
  signedBy: User;
  subject: string;
  contractSum: number;
  status;
  controlDate: Date;
  comment: string;
}