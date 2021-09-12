import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IncomingDocument } from "../models/incoming-document.model";
import { OutgoingDocument } from "../models/outgoing-document.mode";

@Injectable()
export class WorkflowService {
    constructor(private http: HttpClient) {

    }

    async getIncomings(){
        return await this.http.get<IncomingDocument[]>('api/Workflow/GetIncomingDocuments').toPromise();
    }

    async getOutgoings(){
        return await this.http.get<OutgoingDocument[]>('api/Workflow/GetOutgoingDocuments').toPromise();
    }

    async updateIncoming(document: IncomingDocument, transactionId) {
        const params = new HttpParams()
            .set('transactionId', transactionId.toString());
        return await this.http.post('api/Workflow/UpdateIncomingDocument', document, {params: params}).toPromise();
    }

    async updateOutgoing(document: OutgoingDocument, transactionId) {
        const params = new HttpParams()
            .set('transactionId', transactionId.toString());
        return await this.http.post('api/Workflow/UpdateOutgoingDocument', document, {params: params}).toPromise();
    }

    async createIncoming(document: IncomingDocument, transactionId: string) {
        let headers = new HttpHeaders()
            .set('transaction-id', transactionId);

        return await this.http.post('api/Workflow/CreateIncomingDocument', document, {headers: headers}).toPromise();
    }

    async createOutgoing(document: OutgoingDocument, transactionId: string) {
        let headers = new HttpHeaders()
            .set('transaction-id', transactionId);

        return await this.http.post('api/Workflow/CreateOutgoingDocument', document, {headers: headers}).toPromise();
    }
    
    async deleteIncoming(id: number) {
        const params = new HttpParams()
          .set('id', id.toString());
    
        return await this.http.delete('api/Workflow/DeleteIncomingDocument', { params: params }).toPromise();
    }

    async deleteOutgoing(id: number) {
        const params = new HttpParams()
          .set('id', id.toString());
    
        return await this.http.delete('api/Workflow/DeleteOutgoingDocument', { params: params }).toPromise();
    }
}