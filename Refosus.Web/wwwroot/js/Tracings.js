class Tracings
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        // ROUTES //
        this.route = 'https://nativacrm.api.local/api/v1/tracings';
        this.routeTypesObservations = 'https://nativacrm.api.local/api/v1/types-observations';
        this.routeContacts = 'https://nativacrm.api.local/api/v1/contacts';
        this.routeTypesChannels = 'https://nativacrm.api.local/api/v1/types-channels';
        this.routeTypesTasks = 'https://nativacrm.api.local/api/v1/types-tasks';
        // INDEX //
        this.tracings = document.getElementById('tracings');
        // STORE //
        this.tracingsForm = document.getElementById('tracingsForm');
        // SHOW //
        this.detailsId = document.getElementById('detailsId');

        // ATTRIBUTES //
        this.tasks = [];
    }

    index()
    {
        if (this.tracings != null) {
            Fetch(this.route, null, 'GET').then(response => {
                if (!response.error) {
                    for (let index = 0; index < response.message.length; index++) {
                        this.tracings.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].contacts.Name}</td>
                                <td>${response.message[index].types_observations.Name}</td>
                                <td>${response.message[index].Observation.slice(0, 90)}...</td>
                                <td>${response.message[index].created_at.replace('T', ' ').slice(0, 19)}</td>
                                <td><a href="Tracings/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                            </tr>`;
                    }
                    this.dataTable();
                } else {
                    toastr.error(responseContacts.message, 'Ups');
                }
            });
        }
        return this;
    }

    store()
    {
        if (this.tracingsForm != null) {
            Fetch(this.routeContacts, null, 'GET').then(responseContacts => {
                if (!responseContacts.error) {
                    Fetch(this.routeTypesObservations, null, 'GET').then(responseTypesObservations => {
                        if (!responseTypesObservations.error) {
                            Fetch(this.routeTypesChannels, null, 'GET').then(responseChannels => {
                                if (!responseChannels.error) {
                                    let listContacts = document.getElementById('listContacts');
                                    let listTypesObservations = document.getElementById('listTypesObservations');
                                    let listChannels = document.getElementById('listChannels');
                                    for (let index = 0; index < responseContacts.message.length; index++) {
                                        listContacts.innerHTML +=
                                                        `<option value="${responseContacts.message[index].Id}" >
                                                            ${responseContacts.message[index].Name}
                                                        </option>`;
                                    }
                                    for (let index = 0; index < responseTypesObservations.message.length; index++) {
                                        listTypesObservations.innerHTML +=
                                                                `<option value="${responseTypesObservations.message[index].Id}" >
                                                                    ${responseTypesObservations.message[index].Name}
                                                                </option>`;
                                    }
                                    for (let index = 0; index < responseChannels.message.length; index++) {
                                        listChannels.innerHTML +=
                                                        `<option value="${responseChannels.message[index].Id}" >
                                                            ${responseChannels.message[index].Name}
                                                        </option>`;
                                    }
                                    this.storeTasks();
                                    this.save();
                                } else {
                                    toastr.error(response.message, 'Ups');
                                }
                            });
                        } else {
                            toastr.error(responseTypesObservations.message, 'Ups');
                        }
                    });
                } else {
                    toastr.error(responseContacts.message, 'Ups');
                }
            })
            
        }
        return this;
    }

    save()
    {
        if (this.tracingsForm != null) {
            this.tracingsForm.addEventListener('submit', event => {
                event.preventDefault();
                let contactId = document.getElementById('listContacts').value;
                let typeObservationId = document.getElementById('listTypesObservations').value;
                let channelId = document.getElementById('listChannels').value;
                let observation = document.getElementById('observation').value;
                const data = {
                    "TypesObservationsId": parseInt(typeObservationId),
                    "ContactsId": parseInt(contactId),
                    "TypesChannelsId": parseInt(channelId),
                    "UsersId": parseInt(1),
                    "Observation": observation,
                    "tasks": this.tasks
                };
                Fetch(this.route, data, 'POST').then(response => {
                    if (!response.error) {
                        toastr.success(response.message, 'OK');
                        setTimeout(() => {
                            location.replace("/Tracings");
                        }, 1000);
                    } else {
                        toastr.error(response.message, 'Ups');
                    }
                });
            });
        }
        return this;
    }

    show()
    {
        if (this.detailsId != null) {
            const id = this.detailsId.getAttribute('key');
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    document.getElementById('contactDetails').value = response.message.contacts.Name;
                    document.getElementById('typeObservationDetails').value = response.message.types_observations.Name;
                    document.getElementById('typeChannelDetails').value = response.message.types_channels.Name;
                    document.getElementById('createdDetails').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('updatedDetails').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('observationDetails').value = response.message.Observation;
                    let tasks = document.getElementById('tasks');
                    let list = response.message.tasks.reverse();
                    if (list.length > 0) {
                        for (let index = 0; index < list.length; index++) {
                            tasks.innerHTML += TasksOfTracings([
                                list[index].Description,
                                (list[index].Status == 1) ? 
                                            '<br /><span class="badge bg-danger"><h6>Pendiente</h6></span>' : 
                                            '<br /><span class="badge bg-success"><h6>Finalizado</h6></span>',
                                list[index].DeadLine,
                                list[index].created_at.replace('T', ' ').slice(0, 19),
                                list[index].types_tasks.Name
                            ]);
                        }
                    } else {
                        tasks.innerHTML = `<div class="alert alert-info" role="alert">
                                            Este seguimiento no tiene tareas asignadas
                                        </div>`;
                    }
                    
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    // SECTION TASK //

    storeTasks()
    {
        let moreTask = document.getElementById('moreTask');
        let i = 0;
        let status = 1;
        if (moreTask != null ) {
            moreTask.addEventListener('click', () => {
                Fetch(this.routeTypesTasks, null, 'GET').then(responseTypesTasks => {
                    if (!responseTypesTasks.error) {
                        i = i + 1;
                        document.getElementById('listTask').innerHTML += MoreTask(i);
                        TypesTasksInTracings(responseTypesTasks.message, i);
                        let moreTask = document.getElementsByClassName('moreTask');
                        for (let index = 0; index < moreTask.length; index++) {
                            moreTask[index].addEventListener('submit', event => {
                                event.preventDefault();
                                this.tasks[index] = {
                                    "Description": document.getElementsByClassName('description')[index].value,
                                    "Status": status,
                                    "DeadLine": document.getElementsByClassName('deadline')[index].value,
                                    "TypesTasksId": parseInt(document.getElementsByClassName('typesTasksList')[index].value)
                                };
                                document.getElementsByClassName('addTasks')[index].disabled = true;
                                toastr.success('Tarea añadida', 'OK');
                            });
                        }
                        this.removeTask();
                    } else {
                        toastr.error(responseTypesTasks.message, 'Ups');
                    }
                });
            });
        }
        return this;
    }

    removeTask()
    {
        let removeTasks = document.getElementsByClassName('removeTasks');
        for (let index = 0; index < removeTasks.length; index++) {
            removeTasks[index].addEventListener('click', () => {
                const key = removeTasks[index].getAttribute('key');
                const child = document.getElementById('cardTask_' + key);
                child.remove();
                this.tasks.splice(index, 1);
                toastr.error('Tarea removida', 'Ups');
            });
        }
    }

    dataTable()
    {
        if (document.getElementById('tableTracings') != null) {
            $('#tableTracings').DataTable({
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
(new Tracings()).index().store().show();