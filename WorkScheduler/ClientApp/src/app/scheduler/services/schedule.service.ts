import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { WorkSchedule } from "../../shared/models/work-schedule.model";
import { Day, GeneralSchedule } from "../../shared/models/general-schedule.model";
import { Action } from "../../shared/models/action.model";
import { ActionStatus } from "../../shared/enums/action-status.enum";
import { Ticket } from "../../shared/models/ticket.model";
import { TicketPack } from "../../shared/models/ticket-pack.model";
import { tick } from "@angular/core/testing";
import { Dictionary } from "../../shared/models/dictionary.model";
import { Checklist } from "../../shared/models/checklist.model";

@Injectable()
export class ScheduleService {

  constructor(private http: HttpClient) {

  }

  async getMyWorkSchedules() {
    return await this.http.get<WorkSchedule[]>('api/Schedule/MyWorkSchedules').toPromise();
  }

  async saveReply(ticket: Ticket, transactionId){
    const urlParams = new HttpParams()
      .set('transactionId', transactionId.toString());

    return await this.http.post('api/Ticket/SaveReply', ticket, { params: urlParams }).toPromise();
  }

  async saveChecklistTicketDetails(ticket: Ticket, transactionId){
    const urlParams = new HttpParams()
      .set('transactionId', transactionId.toString())
      .set('ticketId', ticket.id.toString());

    return await this.http.get('api/Ticket/SaveChecklistTicketDetails', { params: urlParams }).toPromise();
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

  async editSchedule(schedule: any) {
    return await this.http.post('api/Schedule/Edit', schedule).toPromise();
  }

  async exportActions(actionIds: number[], targetScheduleId: number, replace: boolean) {
    return await this.http.post('api/Action/Export', {actionIds: actionIds, targetScheduleId: targetScheduleId, replace: replace}).toPromise();
  }

  async getActionsToMake(targetStatus: ActionStatus) {
    const params = new HttpParams()
      .set('targetStatus', targetStatus.toString());
    return await this.http.get<WorkSchedule[]>('api/Action/GetActionsToMake', { params: params }).toPromise();
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

  async addTicket(ticket: Ticket, repeat: boolean = false, dateTo: Date = undefined, days: number[] = undefined) {
    //debugger;
    //const params = new HttpParams()
    //  .set('repeat', repeat.toString())
    //  .set('dateTo', dateTo != null ? dateTo.toString() : undefined)
    //  .set('days', days != null ? days.toString() : undefined);

    ticket.repeat = repeat;
    ticket.dateTo = dateTo;
    ticket.days = days;
    return await this.http.post('api/Ticket/Add', ticket).toPromise();
  }

  async updateTicket(ticket: Ticket) {
    return await this.http.post('api/Ticket/Update', ticket).toPromise();
  }

  async deleteTicket(ticket: Ticket) {
    const params = new HttpParams()
      .set('ticketId', ticket.id.toString());

    var similarTickets = await this.http.get<any>('api/Ticket/SimilarTickets', { params: params }).toPromise();

    if (similarTickets.length == 0) {
      return await this.http.delete('api/Ticket/Delete', { params: params }).toPromise();
    }
    else {
      return similarTickets;
    }
  }

  async deleteSimilarTickets(ticket: Ticket) {
    const params = new HttpParams()
      .set('ticketId', ticket.id.toString())
      .set('deleteAll', 'true');

    return await this.http.delete('api/Ticket/Delete', { params: params }).toPromise();
  }

  async deleteOneTicket(ticket: Ticket) {
    const params = new HttpParams()
      .set('ticketId', ticket.id.toString())
      .set('deleteAll', 'false');

    return await this.http.delete('api/Ticket/Delete', { params: params }).toPromise();
  }

  async myTicketPacks(range: Date[] = null) {
    return await this.http.post<TicketPack[]>('api/Ticket/MyTickets', range).toPromise();
  }

  async assignedTickets() {
    return await this.http.get<Ticket[]>('api/Ticket/AssignedTickets').toPromise();
  }

  async acceptTicket(ticket: Ticket) {
    return await this.http.post('api/Ticket/AcceptTicket', ticket).toPromise();
  }

  async sendTimeline(range: Date[] = null) {
    return await this.http.post('api/Ticket/SendTimeline', range).toPromise();
  }

  async makeDone(ticketId: number, hasChecklist: boolean = false) {
    if (hasChecklist) {
      const params = new HttpParams()
        .set('ticketId', ticketId.toString());
      return await this.http.get<any>('api/Ticket/MakeDoneChecklistTicket', { params: params }).toPromise();
    }
    else {
      const params = new HttpParams()
        .set('ticketId', ticketId.toString());
      return await this.http.get<any>('api/Ticket/MakeDone', { params: params }).toPromise();
    }

  }

  async makeImportant(ticketId: number) {
    const params = new HttpParams()
      .set('ticketId', ticketId.toString());
    return await this.http.get<any>('api/Ticket/MakeImportant', { params: params }).toPromise();
  }

  async deleteSchedule(scheduleId: number) {
    const params = new HttpParams()
      .set('scheduleId', scheduleId.toString());

    return await this.http.delete('api/Schedule/Delete', { params: params }).toPromise();
  }

  async getMyChecklists() {
    return await this.http.get<Checklist[]>('api/Checklist/MyChecklists').toPromise();
  }

  async getChecklist(id: number) {
    const params = new HttpParams()
      .set('id', id.toString());

    return await this.http.get<Checklist>('api/Checklist/GetById', { params: params }).toPromise();
  }

  async addChecklist(checklist: Checklist) {
    return await this.http.post('api/Checklist/Add', checklist).toPromise();
  }

  async editChecklist(checklist: Checklist) {
    return await this.http.post('api/Checklist/Edit', checklist).toPromise();
  }

  async otherChecklists() {
    return await this.http.get<any>('api/Checklist/GetOther').toPromise();
  }

  async deleteChecklist(checklist: Checklist) {
    const params = new HttpParams()
      .set('id', checklist.id.toString());

    return await this.http.delete('api/Checklist/Delete', { params: params }).toPromise();
  }

  async addTicketFromChecklist(ticket: Ticket, transactionId: string = null) {
    let headers = new HttpHeaders()
      .set('transaction-id', transactionId);

    return await this.http.post('api/Ticket/AddFromChecklist', ticket, {headers: headers}).toPromise();
  }

  async editTicketFromChecklist(ticket: Ticket) {
    return await this.http.post('api/Ticket/EditFromChecklist', ticket).toPromise();
  }

  async deleteTicketFromChecklist(ticket: Ticket) {
    const params = new HttpParams()
      .set('id', ticket.id.toString());

    return await this.http.delete('api/Ticket/DeleteFromChecklist', { params: params }).toPromise();
  }

  async declineTicket(ticket: Ticket) {
    const params = new HttpParams()
      .set('id', ticket.id.toString());

    return await this.http.get('api/Ticket/DeclineTicket', { params: params }).toPromise();
  }
}
