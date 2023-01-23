import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/demo/api/product';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { CRUDService } from './crud.service';
import { FormGroup } from '@angular/forms';

@Component({
    templateUrl: './crud.component.html',
    providers: [MessageService]
})
export class CrudComponent<TEntity> implements OnInit {
    EntityName = "Product"
    entityDialog: boolean = false;

    deleteEntityDialog: boolean = false;

    deleteEntitiesDialog: boolean = false;

    entities: TEntity[] = [];

    entityForm = new FormGroup({});
    entity: TEntity = {} as TEntity;

    selectedEntities: TEntity[] = [];

    submitted: boolean = false;

    cols: any[] = [];

    statuses: any[] = [];

    rowsPerPageOptions = [5, 10, 20];

    pkCol = 'id'

    constructor(
        private service: CRUDService<TEntity>,
        private messageService: MessageService
    ) { }

    ngOnInit() {
        this.search();

        this.cols = [
            { field: 'product', header: 'Product' },
            { field: 'price', header: 'Price' },
            { field: 'category', header: 'Category' },
            { field: 'rating', header: 'Reviews' },
            { field: 'inventoryStatus', header: 'Status' }
        ];

        this.statuses = [
            { label: 'INSTOCK', value: 'instock' },
            { label: 'LOWSTOCK', value: 'lowstock' },
            { label: 'OUTOFSTOCK', value: 'outofstock' }
        ];
    }

    search(){
        this.service.getAll().subscribe(data => this.entities = data);
    }

    openNew() {
        this.entityForm.reset();
        this.submitted = false;
        this.entityDialog = true;
    }

    deleteSelectedEntities() {
        this.deleteEntitiesDialog = true;
    }

    editEntity(entity: TEntity) {
        this.entityForm.patchValue(entity);
        this.entityDialog = true;
    }

    deleteEntity(entity: TEntity) {
        this.deleteEntityDialog = true;
        this.entityForm.patchValue(entity);
    }

    confirmDeleteSelected() {
        this.deleteEntitiesDialog = false;
        this.service.deleteRange(this.selectedEntities.map(x => x[this.pkCol])).subscribe(res => {
            this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${this.EntityName} Deleted`, life: 3000 });
            this.selectedEntities = [];
            this.search();
        });
    }

    confirmDelete() {
        this.deleteEntityDialog = false;
        this.service.deleteRange(this.selectedEntities.map(x => x[this.pkCol])).subscribe(res => {
            this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${this.EntityName} Deleted`, life: 3000 });
            this.selectedEntities = [];
            this.search();
        });
    }

    hideDialog() {
        this.entityDialog = false;
        this.submitted = false;
    }

    saveEntity() {
        this.submitted = true;

        if (this.entityForm.get(this.pkCol).value) {
            // @ts-ignore
            this.service.create(this.entityForm.value).subscribe(res => {
                this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${this.EntityName} Updated`, life: 3000 });
                this.search();
                this.entityForm.reset();
            })
        } else {
            this.service.update(this.entityForm.get(this.pkCol).value,this.entityForm.value as TEntity).subscribe(res => {
                this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${this.EntityName} Created`, life: 3000 });
                this.search();
                this.entityForm.reset();
            })
        }

        this.entityDialog = false;
        // this.entity = {} as TEntity;

    }

    createId(): string {
        let id = '';
        const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        for (let i = 0; i < 5; i++) {
            id += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        return id;
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }
}
