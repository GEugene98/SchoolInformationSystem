import { Component, OnInit } from '@angular/core';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { AssociationType } from '../models/enums/association-type.enum';
import { RegisterBase } from '../register-base';

@Component({
  selector: 'app-register-gpd',
  templateUrl: './register-gpd.component.html',
  styleUrls: ['./register-gpd.component.css']
})
export class RegisterGpdComponent extends RegisterBase implements OnInit {

  constructor(dictionary: DictionaryService) { 
    super(dictionary, AssociationType.GPD);
  }


}
