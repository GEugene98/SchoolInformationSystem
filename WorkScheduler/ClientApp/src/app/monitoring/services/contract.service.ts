import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Contract } from "../models/contract.model";
import { async } from "@angular/core/testing";


@Injectable()
export class ContractService {
     
    constructor (private http: HttpClient) {

    }

    async getContracts() {
        return await this.http.get<Contract[]>('api/Contract/GetContracts').toPromise();
    }

    async deleteContract(contractId: number) {
        const params = new HttpParams()
        .set('id', contractId.toString());

        return await this.http.delete('api/Contract/DeleteContract', {params: params}).toPromise();
    }
}