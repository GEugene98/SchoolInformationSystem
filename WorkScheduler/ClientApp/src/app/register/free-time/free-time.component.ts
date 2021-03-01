import { Component, OnInit } from '@angular/core';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { AssociationType } from '../models/enums/association-type.enum';
import { RegisterBase } from '../register-base';
import { GroupService } from '../services/group.service';
import { RegisterPlaningService } from '../services/register-planing.service';
import { RegisterService } from '../services/register.service';

@Component({
  selector: 'app-free-time',
  templateUrl: './free-time.component.html',
  styleUrls: ['./free-time.component.css']
})
export class FreeTimeComponent extends RegisterBase implements OnInit {

  constructor(dictionary: DictionaryService, register: RegisterService, planingService: RegisterPlaningService) { 
    super(dictionary, AssociationType.VD, register, planingService);
  }

}
