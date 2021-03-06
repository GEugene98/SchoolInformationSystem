import { Component, OnInit, Input, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router, Data } from '@angular/router';
import { WorkSchedule } from '../../shared/models/work-schedule.model';
import { Action } from '../../shared/models/action.model';
import { User, isUserInRole } from '../../shared/models/user';
import { ConfirmationForm } from '../../shared/models/confirmation-form.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { UserState } from '../../shared/states/user.state';
import { MessageService } from 'primeng/api';
import { ScheduleService } from '../services/schedule.service';
   
import { Title } from '@angular/platform-browser';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { Activity } from '../../shared/models/activity.model';

@Component({
  selector: 'app-schedule-details',
  templateUrl: './schedule-details.component.html',
  styleUrls: ['./schedule-details.component.css']
})
export class ScheduleDetailsComponent implements OnInit {

  modalRef: BsModalRef;
  @ViewChild("selectDate", { static: true }) selectDateModal;
  selectedAll: boolean;
  bsConfig: any;
  scheduleId: number;
  currentSchedule: any;
  editedSchedule: any;
  selectedDate: Date;
  selectedEndDate: Date;
  actions: Action[];
  allResponsibles;
  selectedResponsibles: User[];
  allConfForms;
  selectedConfFormId: number;
  selectedName: string;
  editedAction: Action;
  confirmDate: Date;
  acceptDate: Date;
  allAcademicYears: AcademicYear[];
  allActivities: Activity[];
  mySchedules: WorkSchedule[];
  targetScheduleId: number;
  replace: boolean = false;
  actionNames: string[];

  showEditMessage: boolean = false;

  constructor(private activateRoute: ActivatedRoute,
    private modalService: BsModalService,
    private schedule: ScheduleService,
    private router: Router,
    private dictionary: DictionaryService,
    private messageService: MessageService,
     
    private titleService: Title,
    private userState: UserState) {
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    this.selectedResponsibles = new Array<User>();
  }

  async ngOnInit() {
     
    this.scheduleId = this.activateRoute.snapshot.params['id'];
    await this.loadData();
    this.titleService.setTitle(this.currentSchedule.name);
    this.selectedResponsibles.push(this.userState.currentUser.state);
      
  }

  async loadData() {
    this.currentSchedule = await this.schedule.getSchedule(this.scheduleId);
    this.allActivities = await this.dictionary.getActivities();
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.actionNames = await this.dictionary.getActionNames();
    try {
      this.actions = await this.schedule.getActions(this.scheduleId);
    } catch (e) {
      location.href = '/scheduler/my-schedule';
    }
    this.allResponsibles = this.dictionary.getResponsibles();
    this.allConfForms = this.dictionary.getConfForms();
    this.mySchedules = await this.schedule.getMyWorkSchedules();
    this.mySchedules.forEach(s => s.academicYearName = s.academicYear.name);
  }

  async export(){
    if(!this.targetScheduleId){
      this.messageService.add({ severity: 'info', summary: 'Предупреждение', detail: 'Необходимо выбрать план, в который будет выполнен экспорт', life: 5000 });
      return;
    }
    try {
       
      await this.schedule.exportActions(this.actions.filter(a => a.selected).map(a => a.id), this.targetScheduleId, this.replace);
      await this.loadData();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Мероприятия экспортированы", life: 5000 });
      this.closeModal();
      window.scrollTo(0, 0);
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally{
        
    }
  }

  selectActionName(name: string) {
    this.selectedName = name;
  }

  openExportModal(modal){
    this.clearFields();
    if(this.actions.filter(a => a.selected).length == 0){
      this.messageService.add({ severity: 'info', summary: 'Предупреждение', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

    this.openModal(modal);
  }

  openModal(modal) {
    this.showEditMessage = false;

    this.editedSchedule = Object.assign({}, this.currentSchedule);
    if (this.isActionFreezed(this.editedAction)) {
      this.showEditMessage = true;
    }
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
    this.clearFields();
  }

  clearFields() {
    this.selectedDate = undefined;
    this.selectedEndDate = undefined;
    this.selectedName = undefined;
    this.selectedConfFormId = undefined;
    this.selectedResponsibles = new Array<User>();
    this.selectedResponsibles.push(this.userState.currentUser.state);
    this.editedAction = undefined;
    this.confirmDate = null;
    this.acceptDate = null;
    this.targetScheduleId = 0;
    this.replace = false;
  }

  selection() {
    this.selectedAll = !this.selectedAll;

    if (this.selectedAll) {
      this.actions.forEach(a => {
   
          a.selected = true;
        
      });
    }
    else {
      this.actions.forEach(a => a.selected = false);
    }
  }

  select() {
    for (var i = 0; i < this.actions.length; i++) {

      if (this.actions[i].selected) {
        this.selectedAll = true;
      }
      else {
        this.selectedAll = false;
        break;
      }
    }
  }

  isActionFreezed(action: Action): boolean {
    if (!action) {
      return false;
    }

    if (isUserInRole(this.userState.currentUser.state, "Директор")) {
      return false;
    }
    else if (isUserInRole(this.userState.currentUser.state, "Администратор") && action.status == 2) {
      return false;
    }
    else {
      return (action.status == 2 || action.status == 3);
    }
  }

  copy(action: Action) {
    action.responsibles.forEach(r => r.fullName = `${r.lastName} ${r.firstName[0]}. ${r.surName[0]}.`);
    this.editedAction = Object.assign({}, action);
    this.editedAction.date = new Date(this.editedAction.date.toString()); //Костыль для ngx-datepicker'а
  }

  async createAction() {
    var action = new Action();
    action.date = this.selectedDate;
    action.endDate = this.selectedEndDate;
    action.name = this.selectedName;
    action.responsibles = this.selectedResponsibles;
    action.confirmationFormId = this.selectedConfFormId;
    try {
      await this.schedule.addAction(action, this.scheduleId);
      await this.loadData();
      this.clearFields();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async sendSchedule() {
    this.messageService.add({ severity: 'success', detail: "Идет отправка плана...", life: 5000 });
    try {
      await this.schedule.sendSchedule(this.scheduleId);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "План успешно отправлен на вашу почту", life: 5000 });
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async applyChanges() {
    try {
      await this.schedule.editAction(this.editedAction);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Изменения сохранены", life: 5000 });
      this.loadData();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async delete(action: Action) {
    if (this.isActionFreezed(action)) {
      return;
    }
    try {
      await this.schedule.deleteAction(action);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Мероприятие удалено", life: 5000 });
      await this.loadData();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async editSchedule(){
    try {
      await this.schedule.editSchedule(this.editedSchedule);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "План изменен", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
    } catch(e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async confirm() {

    let actionIdsToConfirm = this.actions.filter(a => a.selected).map(a => a.id);
    if (actionIdsToConfirm.length == 0) {
      this.messageService.add({ severity: 'info', summary: 'Предупреждение', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

     

    try {
      await this.schedule.allowConfirm(actionIdsToConfirm);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия отправлены на согласование", life: 5000 });
      await this.loadData();
      this.selectedAll = false;
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
        
    }
  }

  async accept() {

    let actionIdsToAccept = this.actions.filter(a => a.selected).map(a => a.id);
    if (actionIdsToAccept.length == 0) {
      this.messageService.add({ severity: 'info', summary: 'Предупреждение', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

     

    try {
      await this.schedule.confirm(actionIdsToAccept);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия отправлены на утверждение", life: 5000 });
      await this.loadData();
      this.selectedAll = false;
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
        
    }
  }

  checkRole(role: string, user: User = null) {
    if (user == null) {
      user = this.userState.currentUser.state;
    }

    return isUserInRole(user, role);
  }

  async deleteSchedule() {
    try {
      await this.schedule.deleteSchedule(this.scheduleId);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "План удалён", life: 5000 });
      this.router.navigate(['/scheduler/my-schedule']);
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
      this.closeModal();
    }
  }

  next() {
    this.closeModal();
    this.modalRef = this.modalService.show(this.selectDateModal);
  }

  getDocument() {
    window.open(`/api/Report/ForSchedule?`
      + `scheduleId=${this.scheduleId}`
      + `&confDay=${this.confirmDate.getDate()}&confMonth=${this.confirmDate.getMonth() + 1}&confYear=${this.confirmDate.getFullYear()}`
      + `&acpDay=${this.acceptDate.getDate()}&acpMonth=${this.acceptDate.getMonth() + 1}&acpYear=${this.acceptDate.getFullYear()}`);
    this.closeModal();
  }

  actionIdToOpenProtocol: number;

  async checkProtocol(actionId: number, modal) {
    let exists = await this.schedule.protocolExists(actionId);
    if (exists) {
      this.router.navigate(['//scheduler/protocol-details/' + actionId]);
    }
    else {
      this.openModal(modal);
      this.actionIdToOpenProtocol = actionId;
    }
  }

  createProtocol() {
    this.router.navigate(['//scheduler/protocol-details/' + this.actionIdToOpenProtocol]);
    this.closeModal();
  }
}
