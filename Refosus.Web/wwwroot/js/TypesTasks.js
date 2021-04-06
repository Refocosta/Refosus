class TypesTasks
{
    constructor()
    {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
        };

        // ROUTES //
        this.route = '/types-tasks';
        // INDEX //
        this.typesTaskList = document.getElementById('typesTaskList');
        // STORE //
        this.typesTasksForm = document.getElementById('typesTasksForm');
        // SHOW //
        this.detailsId = document.getElementById('detailsId');
        // EDIT //
        this.editId = document.getElementById('editId');
        // UPDATE //
        this.editTypesTasks = document.getElementById('editTypesTasks');
        // DISABLE //
        this.deleteTypesTasks = document.getElementsByClassName('deleteTypesTasks');
    }

    index()
    {
        if (this.typesTaskList != null) {
            Fetch(this.route, null, 'GET').then(response => {
                if (!response.error) {
                    for (let index = 0; index < response.message.length; index++) {
                        this.typesTaskList.innerHTML +=
                            `<tr>
                                <td>${response.message[index].Id}</td>
                                <td>${response.message[index].Name}</td>
                                <td><span class="badge bg-success">Activo</span></td>
                                <td>${response.message[index].created_at.replace('T', ' ').slice(0, 19)}</td>
                                <td><a href="TypesTasks/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                                <td><a href="TypesTasks/Edit/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Editar</button></a></td>
                                <td><button key="${response.message[index].Id}" class="btn btn-outline-primary deleteTypesTasks" >Eliminar</button></td>
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
        if (this.typesTasksForm != null) {
            this.typesTasksForm.addEventListener('submit', event => {
                event.preventDefault();
                let name = document.getElementById('name').value;
                let status = 1;
                const data = {
                    "Name": name,
                    "Status": status
                };
                Fetch(this.route, data, 'POST').then(response => {
                    if (!response.error) {
                        toastr.success(`Se ha registrado el contacto ${response.message.Name}`, 'OK');
                        setTimeout(() => {
                            location.replace("/TypesTasks");
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
            let id = this.detailsId.getAttribute('key');
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
        if (this.editId != null) {
            let id = this.editId.getAttribute('key');
            Fetch(`${this.route}/${id}`, null, 'GET').then(response => {
                if (!response.error) {
                    document.getElementById('nameEdit').value = response.message.Name;
                    document.getElementById('createdEdit').value = response.message.created_at.replace('T', ' ').slice(0, 19);
                    document.getElementById('updatedEdit').value = response.message.updated_at.replace('T', ' ').slice(0, 19);
                    this.update(id);
                } else {
                    toastr.error(response.message, 'Ups');
                }
            })
        }
        return this;
    }

    update(id)
    {
        if (this.editTypesTasks != null) {
            this.editTypesTasks.addEventListener('click', () => {
                let name = document.getElementById('nameEdit').value;
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
        if (this.deleteTypesTasks != null) {
            for (let index = 0; index < this.deleteTypesTasks.length; index++) {
                this.deleteTypesTasks[index].addEventListener('click', () => {
                    let id = this.deleteTypesTasks[index].getAttribute('key');
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
                })
            }
        }
        return this;
    }

    dataTable()
    {
        if (document.getElementById('tableTypesTasks') != null) {
            $('#tableTypesTasks').DataTable({
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
(new TypesTasks()).index().store().show().edit();