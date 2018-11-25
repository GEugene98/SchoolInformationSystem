import { DataStore } from "../data-store";
import { Injectable } from "@angular/core";
import { User } from "../models/user";

@Injectable()
export class UserState {
  readonly currentUser = new DataStore<User>();
}
