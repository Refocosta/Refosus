class Tasks
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        //ROUTES//
        this.route = "https://nativacrm.api.local/api/v1/tasks";
        this.routeTracings = "https://nativacrm.api.local/api/v1/tracings";
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
            Fetch(this.route, null, 'GET').then(response => {
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
                                <td>${response.message[index].created_at}</td>
                                <td><a href="Tasks/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                                <td><a href="Tasks/Edit/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Editar</button></a></td>
                            </tr>`;
                    }
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
            Fetch(this.routeTracings, null, 'GET').then(responseTracings => {
                if (!responseTracings.error) {
                    let tracingsList = document.getElementById('tracingsList');
                    for (let index = 0; index < responseTracings.message.length; index++) {
                        if (responseTracings.message[index].TypesObservationsId != 1) {
                            tracingsList.innerHTML +=
                                `<option value="${responseTracings.message[index].Id}">
                                ${responseTracings.message[index].Observation}
                            </option>`;
                        }
                    }
                    this.save();
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
                let tracingsId = document.getElementById('tracingsList').value;
                const data = {
                    "Description": description,
                    "Status": status,
                    "TracingsId": parseInt(tracingsId)
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
                    document.getElementById('createdDetails').value = response.message.created_at;
                    document.getElementById('updatedDetails').value = response.message.updated_at;
                    document.getElementById('descriptionDetails').value = response.message.Description;
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
                    Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                        if (!response.error) {
                            let tracingsEdit = document.getElementById('tracingsEdit');
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
                            document.getElementById('createdEdit').value = response.message.created_at;
                            document.getElementById('updatedEdit').value = response.message.updated_at;
                            document.getElementById('descriptionEdit').value = response.message.Description;
                            this.update(id);
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
                let status = document.getElementById('statusEdit').value;
                let tracingsId = document.getElementById('tracingsEdit').value;
                const data = {
                    'Description': description,
                    'Status': parseInt(status),
                    'TracingsId': parseInt(tracingsId)
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
}
(new Tasks()).index().store().show().edit();