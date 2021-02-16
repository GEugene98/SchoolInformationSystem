import { Man, User } from "./user";

// Повестка дня
export class Agenda {
  content: string;
  author: Man;
  listen: InnerContent[] = [new InnerContent()];
  speaked: InnerContent[] = [new InnerContent()];
  decided: string;
}

//Слушали, Выступили
export class InnerContent {
  content: string;
  user: Man;
}