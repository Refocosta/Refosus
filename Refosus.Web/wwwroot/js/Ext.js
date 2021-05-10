const MoreTask = (i) => { 
    let string = `<div id="cardTask_${i}" class="card mt-2 tasks">
        <div class="card-header">
            <div class="card-tools">
                <button type="button" class="btn btn-primary-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
            <center><h3>Añadir tarea</h3></center>
        </div>
        <div class="card-body">
            <form class="moreTask">
                <div class="row">
                    <div class="col-md-4">
                        <label>Fecha limite de la tarea</label>
                        <input id="deadline" class="form-control deadline" type="datetime-local" required />
                    </div>
                    <div class="col-md-4">
                        <label>Tipo de tarea</label>
                        <select id="typesTasksList" class="form-control typesTasksList_${i} typesTasksList" >
                            <option value="">-- Tipo de tarea --</option>
                        </select>
                    </div>
                    <div class="col-md-4">
                        <label>Responsable</label>
                        <select id="responsablesList" class="form-control responsablesList_${i} responsablesList" >
                            <option value="">-- Responsable --</option>
                        </select>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-md-12">
                        <label>Descripcion</label>
                        <textarea id="description" class="form-control description" required></textarea>
                    </div>
                </div>
                <hr />
                <button class="btn btn-outline-success addTasks" >Guardar cambio</button>
            </form>
            <hr />
            <button key="${i}" class="btn btn-outline-primary float-right removeTasks" >Remover tarea</button>
        </div>
    </div>
    `;
    return string;
}

const TypesTasksInTracings = (typesTasks, i) => {
    let typesTasksList = document.getElementsByClassName('typesTasksList_'+ i);
    for (let index = 0; index < typesTasks.length; index++) {
        typesTasksList[0].innerHTML += 
            `<option value="${typesTasks[index].Id}">
                ${typesTasks[index].Name}
            </option>`;
    }
}

const responsableInTracings = (responsables, i) => {
    let responsablesList = document.getElementsByClassName('responsablesList_' + i);
    for (let index = 0; index < responsables.length; index++) {
        responsablesList[0].innerHTML +=
            `<option value="${responsables[index].UserName}">
                ${responsables[index].FirstName + " " + responsables[index].LastName}
            </option>`
    }
}

const TracingsOfContac = (tracings) => {
    let string = 
        `
        <div class="row">
            <div class="col-md-4">
                <label>Tipo de observacion</label>
                <input value="${tracings[1]}" class="form-control" readonly />
            </div>
            <div class="col-md-4">
                <label>Fecha de creación</label>
                <input value="${tracings[2].replace('T', ' ').slice(0, 19)}" class="form-control" readonly />
            </div>
            <div class="col-md-4">
                <label>Canal</label>
                <input value="${tracings[3]}" class="form-control" readonly />
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <label>Valor cotizado</label>
                <input value="${Intl.NumberFormat().format(tracings[4])}" class="form-control" readonly />
            </div>
            <div class="col-md-3">
                <label>Fecha valor cotizado</label>
                <input value="${tracings[6].replace('T', ' ').slice(0, 19)}" class="form-control" readonly />
            </div>
            <div class="col-md-3">
                <label>Valor de venta</label>
                <input value="${Intl.NumberFormat().format(tracings[5])}" class="form-control" readonly />
            </div>
            <div class="col-md-3">
                <label>Fecha valor de venta</label>
                <input value="${tracings[7].replace('T', ' ').slice(0, 19)}" class="form-control" readonly />
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-md-12">
                <label>Observacion</label>
                <textarea class="form-control" readonly>${tracings[0]}</textarea>
            </div>
        </div>
        <hr />
        `;
    return string;
}

const TasksOfTracings = (tasks) => {
    console.log('jeje');
    let string = 
        `
        <div class="row">
            <div class="col-md-3">
                <label>Fecha limite</label>
                <input value="${tasks[2]}" class="form-control" readonly />
            </div>
            <div class="col-md-3">
                <label>Fecha de creacion</label>
                <input value="${tasks[3]}" class="form-control" readonly />
            </div>
            <div class="col-md-3">
                <label>Tipo de tarea</label>
                <input value="${tasks[4]}" class="form-control" readonly />
            </div>
            <div class="col-md-3">
                <label>Estado</label>
                ${tasks[1]}
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-md-12">
                <label>Descripcion</label>
                <textarea class="form-control" readonly>${tasks[0]}</textarea>
            </div>
        </div>
        <hr />
        `;
    return string
};