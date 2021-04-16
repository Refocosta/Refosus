class Channels
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        this.route = '/channels';
        
        //INDEX//
        this.bodyChannels = document.getElementById("crmchannels");
        //STORE//
        this.formChannels = document.getElementById("channelsform");
        //SHOW//
        this.details = document.getElementById('detailsId');
        //EDIT///
        this.editChannel = document.getElementById('editChannel');
        //DISABLE//
        this.deleteChannels = document.getElementsByClassName('deleteChannel');
    }

    index()
    {
        if (this.bodyChannels != null) {
            Fetch(this.route, null, 'GET').then(response => {
                if (!response.error) {
                    for (let index = 0; index < response.message.length; index++) {
                        this.bodyChannels.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].Name}</td>
                                <td><span class="badge bg-success">Activo</span></td>
                                <td>${response.message[index].created_at.replace('T', ' ').slice(0, 19)}</td>
                                <td><a href="Channels/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary">Ver</button></a></td>
                                <td><a href="Channels/Edit/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Editar</button></a></td>
                                <td><button key="${response.message[index].Id}" class="btn btn-outline-primary deleteChannel" >Eliminar</button></td>
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
        if (this.formChannels != null) {
            this.formChannels.addEventListener('submit', event => {
                event.preventDefault();
                let name = document.getElementById('name').value;
                let status = 1;
                const data = {
                    "Name": name,
                    "Status": status
                };
                Fetch(this.route, data, 'POST').then(response => {
                    if (!response.error) {
                        toastr.success(`Se ha registrado el canal ${response.message.Name}`, 'OK');
                        setTimeout(() => {
                            location.replace("/Channels");
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
        
        if (this.details != null) {
            const id = this.details.getAttribute('key');
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    document.getElementById('nameDetails').value = response.message.Name;
                    document.getElementById('createdDetails').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('updatedDetails').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }

    edit()
    {
        if (this.editChannel != null) {
            const id = this.details.getAttribute('key');
            this.editChannel.addEventListener('click', () => {
                let name = document.getElementById('nameDetails').value;
                let status = 1;
                const data = {
                    "Name": name,
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
        if (this.deleteChannels != null) {
            for (let index = 0; index < this.deleteChannels.length; index++) {
                this.deleteChannels[index].addEventListener('click', () => {
                    let id = this.deleteChannels[index].getAttribute('key');
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
        if (document.getElementById('tableChannels') != null) {
            $('#tableChannels').DataTable({
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
(new Channels()).index().store().show().edit();