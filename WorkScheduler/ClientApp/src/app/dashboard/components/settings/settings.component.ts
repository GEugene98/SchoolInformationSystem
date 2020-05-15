import { Component, OnInit } from '@angular/core';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { User } from '../../../shared/models/user';
import { Dictionary } from '../../../shared/models/dictionary.model';
import { AccountService } from '../../../shared/services/account.service';
import { Register } from '../../../shared/models/register.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  users: User[] = [];

  selectedUser: User;

  nameToAdd: string;
  actionNames: string[] = [];

  allActivity: string[] = [];
  range: Date[];

  modalRef: BsModalRef;

  allRoles: Dictionary<string>[] = [];

  selectedRole: Dictionary<string>;
  firstName: string;
  lastName: string;
  surName: string;
  email: string;

  constructor(private dictionary: DictionaryService,
    private account: AccountService,
    private modalService: BsModalService,
    private titleService: Title) {
    this.titleService.setTitle('Настройки');
    this.range = new Array<Date>();
  }

  async ngOnInit() {
    await this.loadData();
    this.selectedRole = new Dictionary<string>();
  }

  async loadData() {
    this.users = await this.dictionary.getUsers();
    this.allRoles = await this.dictionary.getRoles();
    let actionNamesResponse = await this.dictionary.getActionNames();
    if (actionNamesResponse) {
      this.actionNames = actionNamesResponse;
    }
    else {
      this.actionNames = new Array<string>();
    }
  }

  async create() {
    let registerModel = new Register();
    registerModel.firstName = this.firstName;
    registerModel.lastName = this.lastName;
    registerModel.surName = this.surName;
    registerModel.role = this.selectedRole.name;
    registerModel.email = this.email;

    try {
      var result = await this.account.register(registerModel);
      alert(`${result.fullName} Логин: ${result.username} Пароль: ${result.password}`);
      this.clear();
      await this.loadData();
    } catch (e) {
      alert(`Ошибка ${e.error}`);
    }

  }

  clear() {
    this.firstName = undefined;
    this.lastName = undefined;
    this.surName = undefined;
    this.selectedRole = new Dictionary<string>();
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  async delete(user: User) {
    var confirmation = confirm("Заблокировать пользователя " + user.fullName + " ?");

    if (confirmation) {
      await this.account.block(user.id);
      await this.loadData();
    }
  }

  copy(user: User) {
    this.selectedUser = Object.assign({}, user);
  }

  async loadActivity(user: User) {
    this.selectedUser.activity = await this.dictionary.getUserActivity(user.id);
  }

  async loadAllActivity() {
    this.allActivity = await this.dictionary.getAllActivity(this.range);
  }

  async generateNewPassword(){
    alert('Новый пароль: ' + await this.account.getNewPassword(this.selectedUser.id));
  }

  async saveChanges(){
    await this.dictionary.saveUser(this.selectedUser);
    await this.loadData();
  }

  async createActionName() {
    this.actionNames.push(this.nameToAdd);
    this.nameToAdd = undefined;

    await this.dictionary.updateActionNames(JSON.stringify(this.actionNames));
  }

  async deleteActionName(name: string) {
    var index = this.actionNames.indexOf(name);
    if (index > -1) {
      this.actionNames.splice(index, 1);
    }

    await this.dictionary.updateActionNames(JSON.stringify(this.actionNames));
  }
}
