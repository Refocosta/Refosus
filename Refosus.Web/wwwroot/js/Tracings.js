class Tracings
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };
        // ROUTES //
        this.route = '/tracings';

        this.routeTypesObservations = '/types-observations';
        this.routeContacts = '/contacts';
        this.routeTypesChannels = '/types-channels';
        this.routeTypesTasks = '/types-tasks';
        this.routeResponsables = '/users';
        // INDEX //
        this.tracings = document.getElementById('tracings');
        // STORE //
        this.tracingsForm = document.getElementById('tracingsForm');
        // SHOW //
        this.detailsId = document.getElementById('detailsId');
        // EDIT //
        this.editId = document.getElementById('editId');
        // UPDATE //
        this.updateTracing = document.getElementById('updateTracing');
        // ATTRIBUTES //
        this.tasks = [];
    }

    index()
    {
        if (this.tracings != null) {
            Fetch(this.route, null, 'GET').then(response => {
                if (!response.error) {
                    for (let index = 0; index < response.message.length; index++) {
                        let button;
                        if (response.message[index].Auto != 1) {
                            button = `<td><a href="Tracings/Edit/${response.message[index].Id}"><button class="btn btn-outline-primary">Editar</button></a></td>`;
                        } else {
                            button = `<td><a><button class="btn btn-outline-primary" disabled>Editar</button></a></td></tr>`;
                        }
                        this.tracings.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].contacts.Name}</td>
                                <td>${response.message[index].types_observations.Name}</td>
                                <td>${response.message[index].Observation.slice(0, 90)}...</td>
                                <td>${response.message[index].created_at.replace('T', ' ').slice(0, 19)}</td>
                                <td><a href="Tracings/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                                ${button}`
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
        if (this.tracingsForm != null) {
            Fetch(this.routeContacts, null, 'GET', document.getElementById('user').value).then(responseContacts => {
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
                let channelId = 1;
                let observation = document.getElementById('observation').value;
                let quotation = document.getElementById('quotation').value;
                let price = document.getElementById('price').value;
                const data = {
                    "TypesObservationsId": parseInt(typeObservationId),
                    "ContactsId": parseInt(contactId),
                    "TypesChannelsId": parseInt(channelId),
                    "UsersId": parseInt(1),
                    "Observation": observation,
                    "tasks": this.tasks,
                    "Quotation": quotation,
                    "Price": price
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
                    document.getElementById('quotationDetails').value = Intl.NumberFormat().format(response.message.Price);
                    document.getElementById('quotationDateDetails').value = (response.message.QuotationDate != null) ? response.message.QuotationDate.replace('T', ' ').slice(0, 19) : "";
                    document.getElementById('valueDetails').value = Intl.NumberFormat().format(response.message.Value);
                    document.getElementById('valueDateDetails').value = (response.message.SaleDate != null) ? response.message.SaleDate.replace('T', ' ').slice(0, 19) : "";
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

    edit()
    {
        if (this.editId != null) {
            const id = this.editId.getAttribute('key');
            Fetch(this.routeContacts, null, 'GET', document.getElementById('user').value).then(responseContacts => {
                if (!responseContacts.error) {
                    Fetch(this.routeTypesObservations, null, 'GET').then(responseTypesObservations => {
                        if (!responseTypesObservations.error) {
                            Fetch(this.routeTypesChannels, null, 'GET').then(responseTypesChannels => {
                                if (!responseTypesChannels.error) {
                                    Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                                        if (!response.error) {
                                            let contactEdit = document.getElementById('contactEdit');
                                            let typeObservationEdit = document.getElementById('typeObservationEdit');
                                            let typeChannelEdit = document.getElementById('typeChannelEdit');
                                            for (let index = 0; index < responseContacts.message.length; index++) {
                                                if (responseContacts.message[index].Id == response.message.ContactsId) {
                                                    contactEdit.innerHTML +=
                                                        `<option value="${responseContacts.message[index].Id}" selected>
                                                            ${responseContacts.message[index].Name}
                                                        </option>`;
                                                } else {
                                                    contactEdit.innerHTML +=
                                                        `<option value="${responseContacts.message[index].Id}" >
                                                            ${responseContacts.message[index].Name}
                                                        </option>`;
                                                }   
                                            }
                                            for (let index = 0; index < responseTypesObservations.message.length; index++) {
                                                if (responseTypesObservations.message[index].Id == response.message.TypesObservationsId) {
                                                    typeObservationEdit.innerHTML += 
                                                        `<option value="${responseTypesObservations.message[index].Id}" selected>
                                                            ${responseTypesObservations.message[index].Name}
                                                        </option>`;
                                                } else {
                                                    typeObservationEdit.innerHTML += 
                                                        `<option value="${responseTypesObservations.message[index].Id}">
                                                            ${responseTypesObservations.message[index].Name}
                                                        </option>`;
                                                }
                                            }
                                            for (let index = 0; index < responseTypesChannels.message.length; index++) {
                                                if (responseTypesChannels.message[index].Id == response.message.TypesChannelsId) {
                                                    typeChannelEdit.innerHTML +=
                                                        `<option value="${responseTypesChannels.message[index].Id}" selected>
                                                            ${responseTypesChannels.message[index].Name}
                                                        </option>`;
                                                } else {
                                                    typeChannelEdit.innerHTML +=
                                                        `<option value="${responseTypesChannels.message[index].Id}">
                                                            ${responseTypesChannels.message[index].Name}
                                                        </option>`;
                                                }
                                            }
                                            if (response.message.Quotation == 1) {
                                                document.getElementById('quotationEdit').setAttribute('checked', true);
                                                document.getElementById('quotationEdit').setAttribute('disabled', true);
                                                document.getElementById('priceEdit').setAttribute('readonly', true);
                                                document.getElementById('quotationEdit').value = 1;
                                            } else {
                                                document.getElementById('quotationEdit').addEventListener('click', () => {
                                                    if (document.getElementById('quotationEdit').checked == true) {
                                                        document.getElementById('priceEdit').removeAttribute('readonly');
                                                        document.getElementById('quotationEdit').value = 1;
                                                    } else {
                                                        document.getElementById('priceEdit').setAttribute('readonly', true);
                                                        document.getElementById('quotationEdit').value = 0;
                                                        document.getElementById('priceEdit').value = 0;
                                                    }
                                                });
                                            }
                                            if (response.message.Sale == 1) {
                                                document.getElementById('saleEdit').setAttribute('checked', true);
                                                document.getElementById('saleEdit').setAttribute('disabled', true);
                                                document.getElementById('valueEdit').setAttribute('readonly', true);
                                                document.getElementById('saleEdit').value = 1;
                                            } else {
                                                document.getElementById('saleEdit').addEventListener('click', () => {
                                                    if (document.getElementById('saleEdit').checked == true) {
                                                        if (document.getElementById('quotationEdit').checked == 1) {
                                                            document.getElementById('valueEdit').removeAttribute('readonly');
                                                            document.getElementById('saleEdit').value = 1;
                                                        } else {
                                                            toastr.error('Debes seleccionar primero una cotizacion', 'UPS')
                                                        }
                                                    } else {
                                                        document.getElementById('valueEdit').setAttribute('readonly', true);
                                                        document.getElementById('saleEdit').value = 0;
                                                        document.getElementById('valueEdit').value = 0;
                                                    }
                                                });
                                            }
                                            document.getElementById('priceEdit').value = Intl.NumberFormat().format(response.message.Price);
                                            document.getElementById('valueEdit').value = Intl.NumberFormat().format(response.message.Value);
                                            document.getElementById('createdEdit').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                                            document.getElementById('updatedEdit').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                                            document.getElementById('observationEdit').value = response.message.Observation;

                                            this.update(id);
                                        } else {
                                            toastr.error(response.message, 'Ups');
                                        }
                                    });
                                } else {
                                    toastr.error(responseTypesChannels.message, 'Ups');
                                }
                            });
                        } else {
                            toastr.error(responseTypesObservations.message, 'Ups');
                        }
                    });
                } else {
                    toastr.error(responseContacts.message, 'Ups');
                }
            });
        }
        return this;
    }

    update(id)
    {
        if (this.updateTracing != null) {
            this.updateTracing.addEventListener('click', () => {
                let contactEdit = document.getElementById('contactEdit').value;
                let typeObservationEdit = document.getElementById('typeObservationEdit').value;
                let typeChannelEdit = document.getElementById('typeChannelEdit').value;
                let observationEdit = document.getElementById('observationEdit').value;
                let quotation = document.getElementById('quotationEdit').value;
                let price = document.getElementById('priceEdit').value;
                let sale = document.getElementById('saleEdit').value;
                let value = document.getElementById('valueEdit').value;
                price = price.replace(/\./g, "");
                value = value.replace(/\./g, "");
                const data = {
                    "ContactsId": parseInt(contactEdit),
                    "TypesObservationsId": parseInt(typeObservationEdit),
                    "TypesChannelsId": parseInt(typeChannelEdit),
                    "Observation": observationEdit,
                    "Quotation": quotation,
                    "Price": price,
                    "Sale": sale,
                    "Value": value
                };
                Fetch(`${this.route}/${id}`, data, 'PUT').then(response => {
                    if (!response.error) {
                        toastr.success(`Registro ${response.message.id} actualizado`, 'OK');
                        setTimeout(() => {
                            location.reload();
                        }, 1000);
                    } else {
                        toastr.error(response.message, 'Ups');
                    }
                });
            })
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
                        Fetch(this.routeResponsables, null, 'GET').then(responseResponsable => {
                            if (!responseResponsable.error) {
                                i = i + 1;
                                document.getElementById('listTask').innerHTML += MoreTask(i);
                                TypesTasksInTracings(responseTypesTasks.message, i);
                                responsableInTracings(responseResponsable.message, i);
                                let moreTask = document.getElementsByClassName('moreTask');
                                for (let index = 0; index < moreTask.length; index++) {
                                    moreTask[index].addEventListener('submit', event => {
                                        event.preventDefault();
                                        let user = (document.getElementsByClassName('responsablesList')[index].value.length > 0) 
                                                                            ? document.getElementsByClassName('responsablesList')[index].value
                                                                            : document.getElementById('user').value;
                                        this.tasks[index] = {
                                            "Description": document.getElementsByClassName('description')[index].value,
                                            "Status": status,
                                            "DeadLine": document.getElementsByClassName('deadline')[index].value,
                                            "TypesTasksId": parseInt(document.getElementsByClassName('typesTasksList')[index].value),
                                            "User": user
                                        };
                                        document.getElementsByClassName('addTasks')[index].disabled = true;
                                        toastr.success('Tarea añadida', 'OK');
                                    });
                                }
                                this.removeTask();
                            }
                        });
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

    quotationContent()
    {
        let quotation = document.getElementById('quotation');
        if (quotation != null) {
            quotation.addEventListener('click', () => {
                if (quotation.checked) {
                    document.getElementById('quotationContent').removeAttribute('hidden');
                    quotation.value = 1;
                    document.getElementById('price').setAttribute('required', true);
                } else {
                    document.getElementById('quotationContent').setAttribute('hidden', true);
                    quotation.value = 0;
                    document.getElementById('price').removeAttribute('required');
                }
            });
        }
        return this;
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
(new Tracings()).index().store().show().edit().quotationContent();