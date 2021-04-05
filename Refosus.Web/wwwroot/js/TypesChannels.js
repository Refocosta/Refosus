class TypesChannels
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        //ROUTES//
        const server = 'https://nativacrm.api.local/api/v1';
        this.route = server + '/types-channels';
        this.routeChannels = server + '/channels';
        //INDEX//
        this.typesChannels = document.getElementById('typesChannels');
        //STORE//
        this.typesChannelsform = document.getElementById('typesChannelsform');
        //SHOW//
        this.detailsId = document.getElementById('detailsId');
        //EDIT//
        this.editId = document.getElementById('editId');
        //UPDATE//
        this.editTypesChannels = document.getElementById('editTypesChannels');
        //DISABLE//
        this.deleteTypesChannels = document.getElementsByClassName('deleteTypesChannels');
    }

    index()
    {
        if (this.typesChannels != null) {
            Fetch(this.route, null, 'GET').then(response => {
                if (!response.error) {
                    for (let index in response.message) {
                        this.typesChannels.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].Name}</td>
                                <td><span class="badge bg-success">Activo</span></td>
                                <td>${response.message[index].created_at.replace('T', ' ').slice(0, 19)}</td>
                                <td><a href="TypesChannels/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                                <td><a href="TypesChannels/Edit/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Editar</button></a></td>
                                <td><button  key="${response.message[index].Id}" class="btn btn-outline-primary deleteTypesChannels" >Eliminar</button></td>
                            </tr>`;
                    }
                    this.dataTable();
                    this.disable();
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    store()
    {
        if (this.typesChannelsform != null) {
            Fetch(this.routeChannels, null, 'GET').then(responseChannels => {
                if (!responseChannels.error) {
                    let channelsSelect = document.getElementById('channels');
                    for (let index in responseChannels.message) {
                        channelsSelect.innerHTML +=
                            `<option value="${responseChannels.message[index].Id}" >
                                ${responseChannels.message[index].Name}
                            </option>`;
                    }
                    document.getElementById('typesChannelsform').addEventListener('submit', event => {
                        event.preventDefault();
                        let name = document.getElementById('name').value;
                        let channelId = channelsSelect.value;
                        let status = 1;
                        const data = {
                            "Name": name,
                            "ChannelsId": parseInt(channelId),
                            "Status": status
                        }
                        Fetch(this.route, data, 'POST').then(response => {
                            if (!response.error) {
                                toastr.success(`Se ha registrado el contacto ${response.message.Name}`, 'OK');
                                setTimeout(() => {
                                    location.replace("/TypesChannels");
                                }, 1000);
                            } else {
                                toastr.error(response.message, 'Ups');
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

    show() {
        if (this.detailsId != null) {
            const id = this.detailsId.getAttribute('key');
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    document.getElementById('nameDetails').value = response.message.Name;
                    document.getElementById('createdDetails').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('updatedDetails').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('channelsList').innerHTML =
                        `<label>Canal general</label>
                        <ul class="list-group" >
                            <li class="list-group-item" >${response.message.channels.Name}</li>
                        </ul>`;
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
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    Fetch(this.routeChannels, null, 'GET').then(responseChannels => {
                        if (responseChannels) {
                            document.getElementById('nameEdit').value = response.message.Name;
                            document.getElementById('createdEdit').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                            document.getElementById('updatedEdit').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                            let channelsList = document.getElementById('channels');
                            for (let index in responseChannels.message) {
                                if (responseChannels.message[index].Id == response.message.channels.Id) {
                                    channelsList.innerHTML +=
                                        `<option value="${responseChannels.message[index].Id}" selected >
                                            ${responseChannels.message[index].Name}
                                        </option>`;
                                }
                                if (responseChannels.message[index].Id != response.message.channels.Id) {
                                    channelsList.innerHTML +=
                                        `<option value="${responseChannels.message[index].Id}" >
                                            ${responseChannels.message[index].Name}
                                        </option>`;
                                }
                            }
                            this.update();
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

    update()
    {
        if (this.editTypesChannels != null) {
            this.editTypesChannels.addEventListener('click', () => {
                const id = this.editId.getAttribute('key');
                let name = document.getElementById('nameEdit').value;
                let channelId = document.getElementById('channels').value;
                let status = 1;
                const data = {
                    "Name": name,
                    "ChannelsId": parseInt(channelId),
                    "Status": status
                };
                Fetch(`${this.route}/${id}`, data, 'PUT').then(response => {
                    if (!response.error) {
                        toastr.success(`Registro ${response.message.name} actualizado`, 'OK');
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

    disable()
    {
        if (this.deleteTypesChannels != null) {
            for (let index = 0; index < this.deleteTypesChannels.length; index++) {
                this.deleteTypesChannels[index].addEventListener('click', () => {
                    let id = this.deleteTypesChannels[index].getAttribute('key');
                    Fetch(`${this.route}/${id}`, null, 'PATCH').then(response => {
                        if (!response.error) {
                            toastr.success(`Actualizacion exitosa`, response.message);
                            setTimeout(() => {
                                location.reload();
                            }, 1000)
                        } else {
                            toastr.error(response.message, 'Ups');
                        }
                    });
                });
            }
        }
        return this;
    }

    dataTable()
    {
        if (document.getElementById('tableTpesChannels') != null) {
            $('#tableTpesChannels').DataTable({
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
(new TypesChannels()).index().store().show().edit();
