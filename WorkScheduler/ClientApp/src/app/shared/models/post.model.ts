import { User } from "./user";

export class Post {
  id: number;
  text: string;
  author: User;
  createdAt: Date;
  color: string;
  notifyAfterPosting: boolean;
}