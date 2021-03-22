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
        // INDEX //
        this.tracings = document.getElementById('tracings');
        // STORE //
        this.tracingsForm = document.getElementById('tracingsForm');
        // SHOW //
        this.detailsId = document.getElementById('detailsId');
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
                                <td>${response.message[index].created_at}</td>
                                <td><a href="Tracings/Details/${response.message[index].Id}" ><button class="btn btn-outline-primary" >Ver</button></a></td>
                            </tr>`;
                    }
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
                    "Observation": observation
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
                    document.getElementById('createdDetails').value = response.message.created_at;
                    document.getElementById('updatedDetails').value = response.message.updated_at;
                    document.getElementById('observationDetails').value = response.message.Observation;
                } else {
                    toastr.error(response.message, 'Ups');
                }
            });
        }
        return this;
    }
}
(new Tracings()).index().store().show();