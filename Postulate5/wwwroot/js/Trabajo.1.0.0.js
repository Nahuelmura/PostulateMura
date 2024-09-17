function CardTrabajos() {
    $.ajax({
        url: '/Trabajo/CardTrabajos',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (tiposProfesionMostrar) {
            console.log(tiposProfesionMostrar);

            let contenidoCard = ``;

            $.each(tiposProfesionMostrar, function (index, tipoProfesion) {
                contenidoCard += `
                    <div class="profesion-group">
                        <h3 class="text-center">${tipoProfesion.nombre}</h3>
                        <div class="row justify-content-start">`;
                             $.each(tipoProfesion.listadoPersonas, function (index, persona) {
                            contenidoCard += `
                                <div class="" id="card-${persona.trabajoID}">
                              
                                        <div class="card mb-3 h-100" >
                                            <div class="row g-0 h-100">
                                                <div class="col-md-4">
                                                    <div class="card-bg color-Card" style="width: 1em; height: 100%;"></div>
                                                </div>
                                                <div class="col-md-8 d-flex flex-column ">
                                                    <div class="card-body flex-grow-1 overflow-auto" style="max-height: 400px;">
                                                        <p class=""><strong>Nombre:</strong> ${persona.nombrePersona}</p>
                                                        <p class=""><strong>Apellido:</strong> ${persona.apellidoPersona}</p>
                                                        <p class=""><strong>Teléfono:</strong> ${persona.telefonoPersona}</p>
                                                        <p class=""><strong>Dirección:</strong> ${persona.direccion}</p>
                                                        <p class=""><strong>Descripción:</strong> ${persona.descripcion}</p>
                                                        <p class=""><strong>Hora:</strong> ${persona.hora}</p>
                                                        <p class=""><strong>Fecha de inicio:</strong> ${persona.fecha}</p>
                                                        <p class=""><strong>Comentario:</strong> ${persona.comentario}</p>
                                                    </div>
                                                    <div class="card-action mt-3">
                                                        <button type="button" class="btn btn-success me-2" onclick="EditarTrabajo(${persona.trabajoID})">
                                                            <i class="fa-regular fa-pen-to-square"></i> Editar
                                                        </button>
                                                        <button type="button" class="btn btn-danger me-2" onclick="EliminarTrabajo(${persona.trabajoID})">
                                                            <i class="fa-regular fa-trash-can"></i> Eliminar
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>`;
                        });

                contenidoCard += `</div></div>`;
            });

            document.getElementById("contenedorCards").innerHTML = contenidoCard;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al cargar los trabajos');
        }
    });
}


// Llama a la función para cargar las tarjetas al cargar la página
document.addEventListener("DOMContentLoaded", CardTrabajos);



function agregarTrabajo() {
    let personaID = document.getElementById("PersonaID").value;
    let trabajoID = document.getElementById("TrabajoID").value;
    let profesionID = document.getElementById("ProfesionID").value;
   
    let descripcion = document.getElementById("descripcion").value;
    let direccion = document.getElementById("direccion").value;
    let hora = document.getElementById("hora").value;
    let fecha = document.getElementById("fecha").value;
    let comentario = document.getElementById("comentario").value;


    // Crear un objeto FormData para enviar archivos
    let formData = new FormData();

    formData.append("PersonaID", personaID);
    formData.append("TrabajoID", trabajoID);
    formData.append("ProfesionID", profesionID);

    formData.append("descripcion", descripcion);
    formData.append("direccion", direccion);
    formData.append("hora", hora);
    formData.append("fecha", fecha);
    formData.append("comentario", comentario);


    $.ajax({
        url: '/Trabajo/AgregarTrabajo',
        data: formData,
        type: 'POST',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                alert("Trabajo guardado exitosamente");
                $('#agregarTrabajo').modal('hide');
                CardTrabajos();
            } else {
                alert("Error al guardar el Trabajo: " + response.message);
            }
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el Trabajo');
        }
    });
}


 function EditarTrabajo(TrabajoID) {
    $.ajax({
        url: '/Trabajo/RecuperarTrabajo',
        data: { id: TrabajoID },
        type: 'POST',
        dataType: 'json',
        success: function (trabajos) {
            if (trabajos && trabajos.length > 0) {
                let trabajo = trabajos[0];

                
                ProfesionID
                document.getElementById("ProfesionID").value = trabajo.profesionID;
                document.getElementById("TrabajoID").value = trabajo.trabajoID;
                document.getElementById("descripcion").value = trabajo.descripcion;
                document.getElementById("hora").value = trabajo.hora;
                document.getElementById("fecha").value = trabajo.fecha;
                document.getElementById("direccion").value = trabajo.direccion;
                document.getElementById("comentario").value = trabajo.comentario;

                $('#agregarTrabajo').modal('show');
            } else {
                alert("No se encontró el trabajo especificado.");
            }
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el servicio para editar');
        }
    });
}


function EliminarTrabajo(trabajoID) {
    $.ajax({
        // la URL para la petición
        url: '../../Trabajo/EliminarTrabajo',
        data: { trabajoID: trabajoID },
        type: 'POST',
        dataType: 'json',
        success: function (Respuesta) {
            CardTrabajos();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para eliminado');
        }
    });
}