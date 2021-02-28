import { Component, OnInit } from '@angular/core';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { AssociationType } from '../models/enums/association-type.enum';
import { RegisterBase } from '../register-base';
import { GroupService } from '../services/group.service';
import { RegisterService } from '../services/register.service';

@Component({
  selector: 'app-register-gpd',
  templateUrl: './register-gpd.component.html',
  styleUrls: ['./register-gpd.component.css']
})
export class RegisterGpdComponent extends RegisterBase implements OnInit {

  constructor(dictionary: DictionaryService, register: RegisterService) { 
    super(dictionary, AssociationType.GPD, register);
  }


}
