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
                    <div class="col-md-6">
                        <label>Fecha limite de la tarea</label>
                        <input id="deadline" class="form-control deadline" type="date" required />
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-md-12">
                        <label>Descripcion</label>
                        <textarea id="description" class="form-control description" required></textarea>
                    </div>
                </div>
                <hr />
                <button class="btn btn-outline-success addTasks" >Añadir tarea</button>
            </form>
            <hr />
            <button key="${i}" class="btn btn-outline-primary float-right removeTasks" >Remover tarea</button>
        </div>
    </div>
    `;
    return string;
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