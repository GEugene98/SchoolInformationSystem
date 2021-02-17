import { Component, OnInit } from '@angular/core';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { Dictionary } from '../../shared/models/dictionary.model';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { AssociationType } from '../models/enums/association-type.enum';
import { Group } from '../models/group.model';
import { RegisterBase } from '../register-base';

@Component({
  selector: 'app-additional',
  templateUrl: './additional.component.html',
  styleUrls: ['./additional.component.css']
})
export class AdditionalComponent extends RegisterBase implements OnInit {

  constructor(dictionary: DictionaryService) { 
    super(dictionary, AssociationType.DO);
  }

}
