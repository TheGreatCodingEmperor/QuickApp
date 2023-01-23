import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class CRUDService<TEntity> {

    constructor(private http: HttpClient) { }

    getAll():Observable<TEntity[]>{
        throw new Error("not Implemented")   
    }

    create(entity:TEntity):Observable<any>{
        throw new Error("not Implemented")   
    }

    update(id:string|number,entity:TEntity):Observable<any>{
        throw new Error("not Implemented")
    }

    delete(id:string|number):Observable<any>{
        throw new Error("not Implemented")
    }

    deleteRange(ids:string[]|number[]):Observable<any>{
        throw new Error("not Implemented")
    }
}
