import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Association } from "../models/association.model";
import { AssociationType } from "../models/enums/association-type.enum";

@Injectable()
export class AssociationService {
    constructor(private http: HttpClient) { }

    async getAssotiations(type: AssociationType, academicYearId: number) {
        const params = new HttpParams()
            .set('type', type.toString())
            .set('academicYearId', academicYearId.toString());
  
        return await this.http.get<Association[]>('api/Association/GetAssociations', { params: params } ).toPromise();
    }

    async createAssotiation(association: Association, academicYearId: number) {
        const params = new HttpParams()
            .set('academicYearId', academicYearId.toString());
  
        return await this.http.post('api/Association/CreateAssociation', association, { params: params }).toPromise();
    }

    async editAssotiation(association: Association) {
        return await this.http.post('api/Association/EditAssociation', association).toPromise();
    }

    async deleteAssotiation(associationId: number){
        const params = new HttpParams()
            .set('id', associationId.toString());

        return await this.http.delete('api/Association/DeleteAssociation', { params: params }).toPromise();
    }

}