import { Subject } from "rxjs";
import { AcademicYear } from "../shared/models/academic-year.model";
import { Dictionary } from "../shared/models/dictionary.model";
import { DictionaryService } from "../shared/services/dictionary.service";
import { AssociationType } from "./models/enums/association-type.enum";
import { Group } from "./models/group.model";
import { RegisterRow } from "./models/register-row.model";
import { GroupService } from "./services/group.service";
import { RegisterService } from "./services/register.service";

export class RegisterBase {
    allAcademicYears: AcademicYear[];
    selectedAcademicYear: AcademicYear;
    associations: Dictionary<number>[];
    selectedAssociation: Dictionary<number>;
    groups: Dictionary<number>[];
    selectedGroup: Dictionary<number>;
    associationType: AssociationType;
    refreshPlaning: Subject<boolean> = new Subject();
    registerRows: RegisterRow[] = [];

    constructor(private dictionary: DictionaryService, associationType: AssociationType, private register: RegisterService) { 
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

    async updateSelectedGroup(groupId: number){
        if(groupId){
            this.selectedGroup = this.groups.filter(g => g.id == groupId)[0];
            this.refreshPlaning.next();
            this.registerRows = await this.register.getRecords(this.selectedAcademicYear.id, this.selectedAssociation.id, groupId);
        }
    }
}