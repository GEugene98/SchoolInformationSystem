import { Subject } from "rxjs";
import { AcademicYear } from "../shared/models/academic-year.model";
import { Dictionary } from "../shared/models/dictionary.model";
import { DictionaryService } from "../shared/services/dictionary.service";
import { AssociationType } from "./models/enums/association-type.enum";
import { Group } from "./models/group.model";
import { PlaningRecord } from "./models/planing-record.model";
import { RegisterRecord } from "./models/register-record.model";
import { RegisterRow } from "./models/register-row.model";
import { GroupService } from "./services/group.service";
import { RegisterPlaningService } from "./services/register-planing.service";
import { RegisterService } from "./services/register.service";
import { Directive } from "@angular/core";

@Directive()
export class RegisterBase {
    allAcademicYears: AcademicYear[];
    selectedAcademicYear: AcademicYear;
    associations: Dictionary<number>[];
    selectedAssociation: Dictionary<number>;
    groups: Dictionary<number>[];
    selectedGroup: Dictionary<number>;
    associationType: AssociationType;
    registerRows: RegisterRow[];
    planingRecords: PlaningRecord[];

    constructor(private dictionary: DictionaryService, associationType: AssociationType, private register: RegisterService, private planingService: RegisterPlaningService) { 
        this.associationType = associationType;
    }

    async ngOnInit() {
        await this.loadData();
    }

    async loadData() { 
        this.allAcademicYears = await this.dictionary.getAcademicYears();
        await this.loadAssociations();
        await this.loadGroups();
    }

    async loadAssociations(academicYearId: number = undefined) {
        if(academicYearId){
            this.selectedAcademicYear = this.allAcademicYears.filter(a => a.id == academicYearId)[0];
        }
        if (this.selectedAcademicYear) {
            this.associations = await this.dictionary.getAssociations(this.associationType, this.selectedAcademicYear.id);
        }
    }

    async loadGroups(associationId: number = undefined) {
        if(associationId){
            this.selectedAssociation = this.associations.filter(a => a.id == associationId)[0];
        }
        if (this.selectedAssociation) {
            this.groups = await this.dictionary.getGroups(this.selectedAcademicYear.id, this.selectedAssociation.id);
        }
    }

    async updateCell(data){
        if(data.cell.content) {
            await this.register.updateCell(data.cell.planingRecordId, data.studentId, data.cell.id, data.cell.content);
            this.registerRows = await this.register.getRecords(this.selectedAcademicYear.id, this.selectedAssociation.id, this.selectedGroup.id);
        }
    }

    async updateSelectedGroup(groupId: number){
        if(groupId){
            this.selectedGroup = this.groups.filter(g => g.id == groupId)[0];
            this.planingRecords = await this.planingService.getRecords(this.selectedAcademicYear.id, this.selectedAssociation.id, groupId);
            this.registerRows = await this.register.getRecords(this.selectedAcademicYear.id, this.selectedAssociation.id, groupId);
        }
    }

    async loadRegister() {
        this.registerRows = await this.register.getRecords(this.selectedAcademicYear.id, this.selectedAssociation.id, this.selectedGroup.id);
    }
}