import { AcademicYear } from "../shared/models/academic-year.model";
import { Dictionary } from "../shared/models/dictionary.model";
import { DictionaryService } from "../shared/services/dictionary.service";
import { AssociationType } from "./models/enums/association-type.enum";
import { Group } from "./models/group.model";

export class RegisterBase {
    allAcademicYears: AcademicYear[];
    selectedAcademicYear: AcademicYear;
    associations: Dictionary<number>[];
    academicPeriods: Dictionary<number>[];
    groups: Group[];

    associationType: AssociationType;

    constructor(private dictionary: DictionaryService, associationType: AssociationType) { 
        this.associationType = associationType;
    }

    async ngOnInit() {
        await this.loadData();
    }

    async loadData() {
        this.allAcademicYears = await this.dictionary.getAcademicYears();
        
        if(this.selectedAcademicYear){
            this.associations = await this.dictionary.getAssociations(this.associationType, this.selectedAcademicYear.id);
        }
    }
}