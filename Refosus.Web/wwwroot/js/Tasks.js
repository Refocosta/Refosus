class Tasks
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        //ROUTES//
        this.route = '/tasks';
        this.routeTracings = '/tracings';
        this.routeTypesTasks = '/types-tasks';
        this.routeContacts = '/contacts';
        this.routeResponsables = "/users";
        // INDEX //
        this.taskList = document.getElementById('taskList');
        // STORE //
        this.tasksForm = document.getElementById('tasksForm');
        // SHOW //
        this.detailsId = document.getElementById('detailsId');
        // EDIT //
        this.editId = document.getElementById('editId');
        // UPDATE //
        this.editTask = document.getElementById('editTask');
    }

    index()
    {
        if (this.taskList != null) {
            Fetch(this.route, null, 'GET', document.getElementById('user').value).then(response => {
                if (!response.error) {
                    let status;
                    for (let index = 0; index < response.message.length; index++) {
                        if (response.message[index].Status == 1) {
                            status = '<span class="badge bg-danger">Pendiente</span>'
                        } else {
                            status = '<span class="badge bg-success">Finalizado</span>';
                        }
                        this.taskList.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].Description}</td>
                                <td>${status}</td>
                                <td>${response.message[index].DeadLine.slice(0, 10)}</td>
                                <td>${response.message[index].created_at.replace('T', ' ').slice(0, 19)}</td>
                                <td><a href="Tasks/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                                <td><a href="Tasks/Edit/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Editar</button></a></td>
                            </tr>`;
                    }
                    this.dataTable();
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    store()
    {
        if (this.tasksForm != null) {
            Fetch(this.routeTypesTasks, null, 'GET').then(responseTypesTasks => {
                if (!responseTypesTasks.error) {
                    Fetch(this.routeContacts, null, 'GET', document.getElementById('user').value).then(responseContacts => {
                        Fetch(this.routeResponsables, null, 'GET').then(responseResponsable => {
                            if (!responseResponsable.error) {
                                if (!responseContacts.error) {
                                    let contactsList = document.getElementById('contactsList');
                                    let typesTasksList = document.getElementById('typesTasksList');
                                    let responsableList = document.getElementById('responsableList');
                                    for (let index = 0; index < responseContacts.message.length; index++) { 
                                        contactsList.innerHTML +=
                                            `<option value="${responseContacts.message[index].Id}">
                                                ${responseContacts.message[index].Name}
                                            </option>`;
                                    }
                                    for (let index = 0; index < responseTypesTasks.message.length; index++) {
                                        typesTasksList.innerHTML +=
                                            `<option value="${responseTypesTasks.message[index].Id}">
                                                ${responseTypesTasks.message[index].Name}
                                            </option>`;
                                    }
                                    for (let index = 0; index < responseResponsable.message.length; index++) {
                                        responsableList.innerHTML +=
                                            `<option value="${responseResponsable.message[index].UserName}">
                                                ${responseResponsable.message[index].FirstName + " " + responseResponsable.message[index].LastName}
                                            </option>`;
                                    }
                                    this.save();
                                } else {
                                    toastr.error(responseContacts.message, 'Ups');
                                }
                            } else {
                                toastr.error(responseResponsable.message, 'Ups');
                            }
                        });
                    });
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
                
        }
        return this;
    }

    save()
    {
        if (this.tasksForm != null) {
            this.tasksForm.addEventListener('submit', event => {
                event.preventDefault();
                let description = document.getElementById('description').value;
                let status = 1;
                let contactsId = document.getElementById('contactsList').value;
                let typesTasksId = document.getElementById('typesTasksList').value;
                let deadline = document.getElementById('deadline').value;
                let user = (document.getElementById('responsableList').value.length > 0) 
                                                                            ? document.getElementById('responsableList').value
                                                                            : document.getElementById('user').value;
                const data = {
                    "Description": description,
                    "Status": status,
                    "contactsId": parseInt(contactsId),
                    "TypesTasksId": parseInt(typesTasksId),
                    "DeadLine": deadline,
                    "User"    : user
                };
                Fetch(this.route, data, 'POST').then(response => {
                    if (!response.error) {
                        toastr.success(`Se ha registrado la tarea`, 'OK');
                        setTimeout(() => {
                            location.replace("/Tasks");
                        }, 1000);
                    } else {
                        toastr.error(response.message, 'Ups');
                    }
                });
            })
        }
    }

    show()
    {
        if (this.detailsId != null) {
            const id = this.detailsId.getAttribute('key');
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    document.getElementById('tracingDetails').value = response.message.tracings.Observation;
                    document.getElementById('deadLineDetails').value = response.message.DeadLine.slice(0, 19);
                    document.getElementById('createdDetails').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('updatedDetails').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('typeTaskDetails').value = response.message.types_tasks.Name;
                    document.getElementById('descriptionDetails').value = response.message.Description;
                    document.getElementById('responsableDetails').value = response.message.User;
                    if (response.message.Status == 1) {
                        document.getElementById('statusDetails').innerHTML = '<br /><span class="badge bg-danger"><h6>Pendiente</h6></span>';
                    } else {
                        document.getElementById('statusDetails').innerHTML = '<br /><span class="badge bg-success"><h6>Finalizado</h6></span>';
                    }
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    edit()
    {
        if (this.editId != null) {
            const id = this.editId.getAttribute('key');
            Fetch(this.routeTracings, null, 'GET').then(responseTracings => {
                if (!responseTracings.error) {
                    Fetch(this.routeTypesTasks, null, 'GET').then(responseTypesTasks => {
                        if (!responseTypesTasks.error) {
                            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                                if (!response.error) {
                                    Fetch(this.routeResponsables, null, 'GET').then(responseResponsable => {
                                        if (!responseResponsable.error) {
                                            let tracingsEdit = document.getElementById('tracingsEdit');
                                            let typesTasksEdit = document.getElementById('typesTasksEdit');
                                            let responsableEdit = document.getElementById('responsableEdit');
                                            for (let index = 0; index < responseTracings.message.length; index++) {
                                                if (responseTracings.message[index].Id == response.message.TracingsId) {
                                                    tracingsEdit.innerHTML +=
                                                        `<option value="${responseTracings.message[index].Id}" selected>
                                                            ${responseTracings.message[index].Observation}
                                                        </option>`;
                                                            
                                                } else {
                                                    tracingsEdit.innerHTML +=
                                                        `<option value="${responseTracings.message[index].Id}" >
                                                            ${responseTracings.message[index].Observation}
                                                        </option>`;
                                                }
                                            }
                                            for (let index = 0; index < responseTypesTasks.message.length; index++) {
                                                if (responseTypesTasks.message[index].Id == response.message.TypesTasksId) {
                                                    typesTasksEdit.innerHTML +=
                                                        `<option value="${responseTypesTasks.message[index].Id}" selected>
                                                            ${responseTypesTasks.message[index].Name}
                                                        </option>`;
                                                            
                                                } else {
                                                    typesTasksEdit.innerHTML +=
                                                        `<option value="${responseTypesTasks.message[index].Id}" >
                                                            ${responseTypesTasks.message[index].Name}
                                                        </option>`;
                                                }
                                            }
                                            for (let index = 0; index < responseResponsable.message.length; index++) {
                                                if (response.message.User == responseResponsable.message[index].UserName) {
                                                    responsableEdit.innerHTML +=
                                                        `<option value="${responseResponsable.message[index].UserName}" selected>
                                                            ${responseResponsable.message[index].FirstName + " " + responseResponsable.message[index].LastName}
                                                        </option>`;
                                                } else {
                                                    responsableEdit.innerHTML +=
                                                        `<option value="${responseResponsable.message[index].UserName}" >
                                                            ${responseResponsable.message[index].FirstName + " " + responseResponsable.message[index].LastName}
                                                        </option>`;
                                                }
                                                
                                            }
                                            document.getElementById('deadLineEdit').value = response.message.DeadLine.slice(0, 10);
                                            document.getElementById('createdEdit').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                                            document.getElementById('updatedEdit').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                                            document.getElementById('descriptionEdit').value = response.message.Description;
                                            this.update(id);
                                        } else {
                                            toastr.error(responseResponsable.message, 'Ups');
                                        }
                                    });
                                } else {
                                    toastr.error(response.message, 'Ups');
                                }
                            });
                        } else {
                            toastr.error(response.message, 'Ups');
                        }
                    });
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    update(id)
    {
        if (this.editTask != null) {
            this.editTask.addEventListener('click', () => {
                let description = document.getElementById('descriptionEdit').value;
                let status = (document.getElementById('statusEdit').value.length > 0 )
                                                                            ? document.getElementById('statusEdit').value.length
                                                                            : 1;
                let tracingsId = document.getElementById('tracingsEdit').value;
                let typesTasksId = document.getElementById('typesTasksEdit').value;
                let deadLine = document.getElementById('deadLineEdit').value;
                let user = document.getElementById('responsableEdit').value;
                const data = {
                    'Description': description,
                    'Status': parseInt(status),
                    'TracingsId': parseInt(tracingsId),
                    'TypesTasksId': parseInt(typesTasksId),
                    'DeadLine': deadLine,
                    'User': user
                };
                Fetch(`${this.route}/${id}`, data, 'PUT').then(response => {
                    if (!response.error) {
                        toastr.success(`Registro ${response.message.Description} actualizado`, 'OK');
                        setTimeout(() => {
                            location.reload();
                        }, 1000);
                    } else {
                        toastr.error(response.message, 'Ups');
                    }
                });
            });
        }
        return this;
    }

    dataTable()
    {
        if (document.getElementById('tableTasks') != null) {
            $('#tableTasks').DataTable({
                "language": {
                    "lengthMenu": "Mostrar _MENU_ registros",
                    "zeroRecords": "No hay resultados",
                    "info": "mostrar pagina _PAGE_ de _PAGES_",
                    "infoEmpty": "No records available",
                    "search": "Buscar:",
                    "infoFiltered": "(filtered from _MAX_ total records)",
                    "paginate": {
                        "first": "Primero",
                        "last": "Ultimo",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    },
                }
            });
        }
        return this;
    }
}
(new Tasks()).index().store().show().edit();