import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Contract } from "../models/contract.model";
import { async } from "@angular/core/testing";


@Injectable()
export class ContractService {
     
    public contracts: Contract[];

    constructor (private http: HttpClient) {

    }

    async getContracts() {
        return await this.http.get<Contract[]>('api/Contract/GetContracts').toPromise();
    }

    async loadContracts(){
        this.contracts = await this.http.get<Contract[]>('api/Contract/GetContracts').toPromise();
    }

    async deleteContract(contractId: number) {
        const params = new HttpParams().set('id', contractId.toString());

        return await this.http.delete('api/Contract/DeleteContract', {params: params}).toPromise();
    }

    async editContarct(contract: Contract){
        return await this.http.post('api/Contract/UpdateContract', contract).toPromise();
    }

    async createContract(contract: Contract){
        return await this.http.post('api/Contract/CreateContract', contract).toPromise();
    }
}