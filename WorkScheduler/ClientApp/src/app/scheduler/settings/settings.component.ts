import { Component, OnInit } from '@angular/core';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { User } from '../../shared/models/user';
import { Dictionary } from '../../shared/models/dictionary.model';
import { AccountService } from '../../shared/services/account.service';
import { Register } from '../../shared/models/register.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  users: User[];

  modalRef: BsModalRef;

  allRoles: Dictionary<string>[];

  selectedRoles: Dictionary<string>[];
  firstName: string;
  lastName: string;
  surName: string;

  constructor(private dictionary: DictionaryService,
    private account: AccountService,
    private modalService: BsModalService,
    private titleService: Title) {
    this.titleService.setTitle('Настройки');
  }

  async ngOnInit() {
    await this.loadData();
    this.selectedRoles = new Array<Dictionary<string>>();
  }

  async loadData() {
    this.users = await this.dictionary.getUsers();
    this.allRoles = await this.dictionary.getRoles();
  }

  async create() {
    let newUser = new Register();
    newUser.firstName = this.firstName;
    newUser.lastName = this.lastName;
    newUser.surName = this.surName;
    newUser.roles = this.selectedRoles.map(r => r.name);

    try {
      var result = await this.account.register(newUser);
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
    this.selectedRoles = new Array<Dictionary<string>>();
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  async delete(user: User) {
    await this.account.block(user.id);
    await this.loadData();
  }
}
