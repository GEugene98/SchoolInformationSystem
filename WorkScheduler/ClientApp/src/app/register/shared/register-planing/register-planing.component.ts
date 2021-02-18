import { Component, OnInit } from '@angular/core';
import { RegisterPlaningService } from '../../services/register-planing.service';

@Component({
  selector: 'app-register-planing',
  templateUrl: './register-planing.component.html',
  styleUrls: ['./register-planing.component.css']
})
export class RegisterPlaningComponent implements OnInit {

  constructor(private planingService: RegisterPlaningService) { }

  ngOnInit() {
  }

  async uploadExcel(uploadForm){
    await this.planingService.uploadPlaningExcel(new FormData(uploadForm));
  }

}
