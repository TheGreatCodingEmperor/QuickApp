// import { Component, OnInit } from '@angular/core';
// import { MessageService } from 'primeng/api';
// import { Role } from 'src/app/models/role.model';
// import { AccountEndpoint } from 'src/app/services/account-endpoint.service';
// import { AccountService } from 'src/app/services/account.service';

// @Component({
//   selector: 'app-role-management',
//   templateUrl: './role-management.component.html',
//   styleUrls: ['./role-management.component.scss'],
//   providers:[MessageService]
// })
// export class RoleManagementComponent<Role> implements OnInit {

//   entityDialog: boolean = false;

//   deleteEntityDialog: boolean = false;

//   deleteEntitiesDialog: boolean = false;

//   entities: Role[] = [];

//   editEntity: Role = null;

//   selectedEntities: Role[] = [];

//   submitted: boolean = false;

//   cols: any[] = [];

//   statuses: any[] = [];

//   rowsPerPageOptions = [5, 10, 20];

//   constructor(
//     private acccount: AccountService, 
//     private messageService: MessageService,
//     ) { }

//   ngOnInit() {
//       this.acccount.getRoles().subscribe(data => this.entities = data);

//       this.cols = [
//           { field: 'product', header: 'Product' },
//           { field: 'price', header: 'Price' },
//           { field: 'category', header: 'Category' },
//           { field: 'rating', header: 'Reviews' },
//           { field: 'inventoryStatus', header: 'Status' }
//       ];

//       this.statuses = [
//           { label: 'INSTOCK', value: 'instock' },
//           { label: 'LOWSTOCK', value: 'lowstock' },
//           { label: 'OUTOFSTOCK', value: 'outofstock' }
//       ];
//   }

//   openNew() {
//       this.editEntity = {};
//       this.submitted = false;
//       this.entityDialog = true;
//   }

//   deleteSelectedProducts() {
//       this.deleteEntitiesDialog = true;
//   }

//   editProduct(product: Product) {
//       this.editEntity = { ...product };
//       this.entityDialog = true;
//   }

//   deleteProduct(product: Product) {
//       this.deleteEntityDialog = true;
//       this.editEntity = { ...product };
//   }

//   confirmDeleteSelected() {
//       this.deleteEntitiesDialog = false;
//       this.products = this.products.filter(val => !this.selectedEntities.includes(val));
//       this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Products Deleted', life: 3000 });
//       this.selectedEntities = [];
//   }

//   confirmDelete() {
//       this.deleteEntityDialog = false;
//       this.products = this.products.filter(val => val.id !== this.editEntity.id);
//       this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Product Deleted', life: 3000 });
//       this.editEntity = {};
//   }

//   hideDialog() {
//       this.entityDialog = false;
//       this.submitted = false;
//   }

//   saveProduct() {
//       this.submitted = true;

//       if (this.editEntity.name?.trim()) {
//           if (this.editEntity.id) {
//               // @ts-ignore
//               this.editEntity.inventoryStatus = this.editEntity.inventoryStatus.value ? this.editEntity.inventoryStatus.value : this.editEntity.inventoryStatus;
//               this.products[this.findIndexById(this.editEntity.id)] = this.editEntity;
//               this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Product Updated', life: 3000 });
//           } else {
//               this.editEntity.id = this.createId();
//               this.editEntity.code = this.createId();
//               this.editEntity.image = 'product-placeholder.svg';
//               // @ts-ignore
//               this.editEntity.inventoryStatus = this.editEntity.inventoryStatus ? this.editEntity.inventoryStatus.value : 'INSTOCK';
//               this.products.push(this.editEntity);
//               this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Product Created', life: 3000 });
//           }

//           this.products = [...this.products];
//           this.entityDialog = false;
//           this.editEntity = {};
//       }
//   }

//   findIndexById(id: string): number {
//       let index = -1;
//       for (let i = 0; i < this.products.length; i++) {
//           if (this.products[i].id === id) {
//               index = i;
//               break;
//           }
//       }

//       return index;
//   }

//   createId(): string {
//       let id = '';
//       const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
//       for (let i = 0; i < 5; i++) {
//           id += chars.charAt(Math.floor(Math.random() * chars.length));
//       }
//       return id;
//   }

//   onGlobalFilter(table: Table, event: Event) {
//       table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
//   }
// }