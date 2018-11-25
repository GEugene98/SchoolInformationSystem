import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { WorkSchedule } from "../../shared/models/work-schedule.model";
import { Day, GeneralSchedule } from "../../shared/models/general-schedule.model";
import { Action } from "../../shared/models/action.model";
import { ActionStatus } from "../../shared/enums/action-status.enum";
import { Ticket } from "../../shared/models/ticket.model";
import { TicketPack } from "../../shared/models/ticket-pack.model";

@Injectable()
export class ScheduleService {

  constructor(private http: HttpClient) {

  }

  async getMyWorkSchedules() {
    return await this.http.get<WorkSchedule[]>('api/Schedule/MyWorkSchedules').toPromise();
  }

  async deleteAction(action: Action) {
    const params = new HttpParams()
      .set('actionId', action.id.toString());

    return await this.http.delete('api/Action/Delete', { params: params }).toPromise();
  }

  async addWorkSchedule(academicYearId: number, activityId: number, name: string) {
    const params = new HttpParams()
      .set('academicYearId', academicYearId.toString())
      .set('name', name)
      .set('activityId', activityId.toString());

    return await this.http.get('api/Schedule/AddWorkSchedule', { params: params }).toPromise();
  }

  async getActions(workScheduleId: number) {
    const params = new HttpParams()
      .set('workScheduleId', workScheduleId.toString());
    return await this.http.get<Action[]>('api/Schedule/Actions', { params: params }).toPromise();
  }

  async addAction(action: Action, workScheduleId: number) {
    const urlParams = new HttpParams()
      .set('workScheduleId', workScheduleId.toString());

    return await this.http.post('api/Action/Add', action, { params: urlParams }).toPromise();
  }

  async editAction(action: Action) {
    return await this.http.post('api/Action/Edit', action).toPromise();
  }

  async getScheduleForDay(date: Date, showMine = false) {
    const params = new HttpParams()
      .set('showMine', showMine.toString());
    return await this.http.post<Day>('api/Schedule/ForDay', date, { params: params }).toPromise();
  }

  async getScheduleForPeriod(start: Date, end: Date, showMine = false) {
    const params = new HttpParams()
      .set('showMine', showMine.toString());

    let model = new GeneralSchedule();
    model.start = start;
    model.end = end;

    return await this.http.post<GeneralSchedule>('api/Schedule/ForPeriod', model, { params: params }).toPromise();
  }

  async sendSchedule(scheduleId: number) {
    const params = new HttpParams()
      .set('scheduleId', scheduleId.toString());
    return await this.http.get('api/Schedule/SendSchedule', { params: params }).toPromise();
  }

  async getSchedule(scheduleId: number) {
    const params = new HttpParams()
      .set('scheduleId', scheduleId.toString());
    return await this.http.get<any>('api/Schedule/GetWorkSchedule', { params: params }).toPromise();
  }

  async getActionsToMake(targetStatus: ActionStatus) {
    const params = new HttpParams()
      .set('targetStatus', targetStatus.toString());
    return await this.http.get<Action[]>('api/Action/GetActionsToMake', { params: params }).toPromise();
  }

  async allowConfirm(ids: number[]) {
    return await this.http.post('api/Schedule/AllowConfirm', ids).toPromise();
  }

  async cancelAcepting(ids: number[]) {
    return await this.http.post('api/Schedule/CancelAccepting', ids).toPromise();
  }

  async cancelConfirming(ids: number[]) {
    return await this.http.post('api/Schedule/CancelConfirming', ids).toPromise();
  }

  async confirm(ids: number[]) {
    return await this.http.post('api/Schedule/Confirm', ids).toPromise();
  }

  async accept(ids: number[]) {
    return await this.http.post('api/Schedule/Accept', ids).toPromise();
  }

  async addTicket(ticket: Ticket) {
    return await this.http.post('api/Ticket/Add', ticket).toPromise();
  }

  async updateTicket(ticket: Ticket) {
    return await this.http.post('api/Ticket/Update', ticket).toPromise();
  }

  async deleteTicket(ticket: Ticket) {
    const params = new HttpParams()
      .set('ticketId', ticket.id.toString());

    return await this.http.delete('api/Ticket/Delete', { params: params }).toPromise();
  }

  async myTicketPacks(dateTo: Date = null) {
    return await this.http.post<TicketPack[]>('api/Ticket/MyTickets', dateTo).toPromise();
  }

  async deleteSchedule(scheduleId: number) {
    const params = new HttpParams()
      .set('scheduleId', scheduleId.toString());

    return await this.http.delete('api/Schedule/Delete', { params: params }).toPromise();
  }
}
