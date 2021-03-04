
import {distinctUntilChanged} from 'rxjs/operators';
import { BehaviorSubject ,  Observable } from 'rxjs';


export class DataStore<T extends any> {
  private subject: BehaviorSubject<T>;

  /** Содержимое хранилища */
  public get state(): T { return this.subject.getValue(); }
  public set state(value: T) { this.subject.next(value); }

  constructor(initialValue?: T) {
    this.subject = new BehaviorSubject<T>(initialValue);
  }

  /** Поток хранилища для подписки на него */
  public get state$(): Observable<T> { return this.subject.asObservable().pipe(distinctUntilChanged()); }
}
