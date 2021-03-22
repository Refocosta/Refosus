class Contacts
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        //ROUTES//
        this.route = "https://nativacrm.api.local/api/v1/contacts";
        this.routeChannels = "https://nativacrm.api.local/api/v1/channels";
        //INDEX//
        this.crmContacts = document.getElementById('crmContacts');
        //STORE//
        this.contactsform = document.getElementById('contactsform');
        //SHOW//
        this.detailsId = document.getElementById('detailsId');
        //EDIT//
        this.editId = document.getElementById('editId');
        //UPDATE//
        this.editContact = document.getElementById('editContact');
        //DISABLE//
        this.deleteContacts = document.getElementsByClassName('deleteContacts');
    }

    index()
    {
        if (this.crmContacts != null) {
            Fetch(this.route, null, 'GET').then(response => {
                if (!response.error) {
                    for (let index = 0; index < response.message.length; index++) {
                        this.crmContacts.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].Name}</td>
                                <td>${response.message[index].Cellphone}</td>
                                <td>${response.message[index].Email}</td>
                                <td><span class="badge bg-success">Activo</span></td>
                                <td>${response.message[index].created_at}</td>
                                <td><a href="Contacts/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                                <td><a href="Contacts/Edit/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Editar</button></a></td>
                                <td><button key="${response.message[index].Id}" class="btn btn-outline-primary deleteContacts" >Eliminar</button></td>
                            </tr>`;
                    }
                    this.disable();
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
        if (this.contactsform != null) {
            this.contactsform.addEventListener('submit', event => {
                event.preventDefault();
                let arrayChannels = [];
                let arrayTypesChannels = [];
                const channels = document.getElementsByClassName('channels');
                const typeschannels = document.getElementsByClassName('typeschannels');
                for (let index = 0; index < channels.length; index++) {
                    if (channels[index].checked == true) {
                        arrayChannels.push(channels[index].value);
                    }
                }
                for (let index = 0; index < typeschannels.length; index++) {
                    if (typeschannels[index].checked == true) {
                        arrayTypesChannels.push(typeschannels[index].value);
                    }
                }
                let name = document.getElementById('name').value;
                let cellPhone = document.getElementById('cellPhone').value;
                let email = document.getElementById('email').value;
                let petition = document.getElementById('petition').value;
                let status = 1;
                const data = {
                    "Name": name,
                    "Cellphone": cellPhone,
                    "Email": email,
                    "Petition": petition,
                    "Status": status,
                    "ChannelId": arrayChannels,
                    "TypeChannelId": arrayTypesChannels
                };
                Fetch(this.route, data, 'POST').then(response => {
                    if (!response.error) {
                        toastr.success(`Se ha registrado el contacto ${response.message.Name}`, 'OK');
                        setTimeout(() => {
                            location.replace("/Contacts");
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
                    document.getElementById('nameDetails').value = response.message.Name;
                    document.getElementById('cellPhoneDetails').value = response.message.Cellphone;
                    document.getElementById('emailDetails').value = response.message.Email;
                    document.getElementById('createdDetails').value = response.message.created_at;
                    document.getElementById('updatedDetails').value = response.message.updated_at;
                    document.getElementById('petitionDetails').value = response.message.Petition;
                    response.message.channels.sort(((a, b) => a.Id - b.Id));
                    if (response.message.channels.length > 0) {
                        for (let index = 0; index < response.message.channels.length; index++) {
                            document.getElementById('listChannels').innerHTML +=
                                `<ul class="list-group" >
                                    <li class="list-group-item" >${response.message.channels[index].Name}</li>
                                </ul>`;
                        }
                    } else {
                        document.getElementById('listChannels').innerHTML = '<alert class="alert alert-info" >No tiene canales de comunicación asignados</alert>';
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
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    Fetch(`${this.routeChannels}`, null, 'GET').then(responseChannels => {
                        if (!responseChannels.error) {
                            document.getElementById('nameEdit').value = response.message.Name;
                            document.getElementById('cellPhoneEdit').value = response.message.Cellphone;
                            document.getElementById('emailEdit').value = response.message.Email;
                            document.getElementById('createdEdit').value = response.message.created_at;
                            document.getElementById('updatedEdit').value = response.message.updated_at;
                            document.getElementById('petitionEdit').value = response.message.Petition;
                            let channelsList = document.getElementById('listChannelsEdit');
                            channelsList.innerHTML = `<label>Editar Canales asignados</label><ul class="list-group" >`;
                            response.message.channels.sort(((a, b) => a.Id - b.Id));
                            for (let index in responseChannels.message) {
                                for (let i in response.message.channels) {
                                    if (responseChannels.message[index].Id == response.message.channels[i].Id) {
                                        channelsList.innerHTML +=
                                            `<li class="list-group-item" >
                                                <input  class="channels" type="checkbox" value="${responseChannels.message[index].Id}" checked /><span class="ml-2" >${responseChannels.message[index].Name}<span>
                                            </li>`;
                                        responseChannels.message.splice(index, 1);
                                    }
                                }
                                if (responseChannels.message[index] != undefined) {
                                    channelsList.innerHTML +=
                                        `<li class="list-group-item" >
                                        <input class="channels" type="checkbox" value="${responseChannels.message[index].Id}" /><span class="ml-2" >${responseChannels.message[index].Name}</span>
                                    </li>`;
                                }

                            }
                            channelsList.innerHTML += `</ul>`;
                            this.update();
                        } else {
                            toastr.error(responseChannels.message, 'Ups');
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
        if (this.editContact != null) {
            let arrayChannels = [];
            let arrayChannelsDel = [];
            const channels = document.getElementsByClassName('channels');
            for (let index = 0; index < channels.length; index++) {
                channels[index].addEventListener('click', () => {
                    if (channels[index].checked == true) {
                        arrayChannels.push(channels[index].value);
                        let i = arrayChannelsDel.indexOf(channels[index].value);
                        arrayChannelsDel.splice(i);
                    } else {
                        arrayChannelsDel.push(channels[index].value);
                        let i = arrayChannels.indexOf(channels[index].value);
                        arrayChannels.splice(i);
                    }
                });
            }
            this.editContact.addEventListener('click', () => {
                const idTmp = document.getElementById('editId');
                let id = idTmp.getAttribute('key');
                let name = document.getElementById('nameEdit').value;
                let cellPhone = document.getElementById('cellPhoneEdit').value;
                let email = document.getElementById('emailEdit').value;
                let petition = document.getElementById('petitionEdit').value;
                let status = 1;
                const data = {
                    "Name": name,
                    "Cellphone": cellPhone,
                    "Email": email,
                    "Petition": petition,
                    "Status": status,
                    "ChannelId": arrayChannels,
                    "ChannelIdDel": arrayChannelsDel
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
        if (this.deleteContacts != null) {
            for (let index = 0; index < this.deleteContacts.length; index++) {
                this.deleteContacts[index].addEventListener('click', () => {
                    let id = this.deleteContacts[index].getAttribute('key');
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

    channelsList()
    {
        let channelsList = document.getElementById('channelsList');
        if (channelsList != null) {
            Fetch('https://nativacrm.api.local/api/v1/channels', null, 'GET').then(response => {
                if (!response.error) {
                    channelsList.innerHTML = '<hr /><label class="mt-2" >Asigna los canales correspondientes</label>';
                    for (let index = 0; index < response.message.length; index++) {
                        channelsList.innerHTML +=
                            `<ul class="list-group" >
                                <li class="list-group-item" >
                                    <input class="form-check-input  channels ml-1" type="checkbox" value="${response.message[index].Id}" id="channel" /><span class="ml-4" >${response.message[index].Name}</span>
                                </li>
                            </ul>`;
                    }
                    this.typesChannelsList(response);
                    this.store();
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    typesChannelsList(response)
    {
        channelsList.innerHTML += '<div id="typesChannels" ></div>';
        let channels = document.getElementsByClassName('channels');
        for (let index = 0; index < channels.length; index++) {
            channels[index].addEventListener('click', () => {
                if (channels[index].checked == true) {
                    if (response.message[index].types_channels.length > 0) {
                        document.getElementById('typesChannels').innerHTML += `<label class="mt-3 label_${response.message[index].Id}"> Selecciona el tipo de canal para ${response.message[index].Name}</label>`;
                        for (let i in response.message[index].types_channels) {
                            document.getElementById('typesChannels').innerHTML +=
                                `<ul class="list-group list_${response.message[index].Id} " >
                                                <li class="list-group-item" >
                                                    <input class="form-check-input  typeschannels ml-1" type="checkbox" value="${response.message[index].types_channels[i].Id}" id="channel" /><span class="ml-4" >${response.message[index].types_channels[i].Name}</span>
                                                </li>
                                            </ul>`;
                        }
                    }
                } else {
                    const child = document.getElementsByClassName('list_' + response.message[index].Id);
                    const childLabel = document.getElementsByClassName('label_' + response.message[index].Id);
                    for (let index = child.length - 1; index >= 0; --index) {
                        child[index].remove();
                    }
                    for (let index = childLabel.length - 1; index >= 0; --index) {
                        childLabel[index].remove();
                    }
                }
            });
        }
    }

    dataTable()
    {
        if (document.getElementById('tableContacts') != null) {
            $('#tableContacts').DataTable({
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
(new Contacts()).index().channelsList().show().edit();