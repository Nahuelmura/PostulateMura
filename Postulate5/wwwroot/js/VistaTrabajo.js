function CardTrabajos() {
    let profesionID = $("#profesionBuscarID").val(); 

    $.ajax({
        url: '/VistaTrabajo/CardTrabajos',
        data: { ProfesionID: profesionID },  
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
                                <div class="col-md-4 mb-3" id="card-${persona.trabajoID}">
                                    <div class="card h-100">
                                        <div class="row g-0 h-100">
                                            <div class="col-md-4">
                                                <div class="card-bg color-Card" style="width: 1em; height: 100%;"></div>
                                            </div>
                                            <div class="col-md-8 d-flex flex-column">
                                                <div class="card-body flex-grow-1 overflow-auto" style="max-height: 400px;">
                                                    <p><strong>Nombre:</strong> ${persona.nombrePersona}</p>
                                                    <p><strong>Apellido:</strong> ${persona.apellidoPersona}</p>
                                                    <p><strong>Teléfono:</strong> ${persona.telefonoPersona}</p>
                                                    <p><strong>Dirección:</strong> ${persona.direccion}</p>
                                                    <p><strong>Descripción:</strong> ${persona.descripcion}</p>
                                                    <p><strong>Hora:</strong> ${persona.hora}</p>
                                                    <p><strong>Fecha de inicio:</strong> ${persona.fecha}</p>
                                                    <p><strong>Comentario:</strong> ${persona.comentario}</p>
                                                </div>
                                                <div class="card-action mt-3">
                                                    <button type="button" class="btn btn-danger me-2" onclick="EliminarTrabajo(${persona.trabajoID})">
                                                        <i class="fa-regular fa-trash-can"></i> Eliminar
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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



    let formData = new FormData();

    formData.append("TrabajoID", trabajoID);
    formData.append("PersonaID", personaID);
    formData.append("ProfesionID", profesionID);
    formData.append("descripcion", descripcion);
    formData.append("direccion", direccion);
    formData.append("hora", hora);
    formData.append("fecha", fecha);
    formData.append("comentario", comentario);



    $.ajax({
        // URL para la petición
        url: '/VistaTrabajo/AgregarTrabajo',
        // Información a enviar
        data: formData,
        // Especifica si será una petición POST
        type: 'POST',
        // Tipo de información que se espera de respuesta
        dataType: 'json',
        // Necesario para enviar archivos
        processData: false,
        contentType: false,
        // Código a ejecutar si la petición es satisfactoria
        success: function (response) {
            if (response.success) {
                alert("Trabajo guardado exitosamente");

                $('#agregarTrabajo').modal('hide');

                CardTrabajos();
            } else {
                alert("Error al guardar el Trabajo: " + response.message);
            }
        },
        // Código a ejecutar si la petición falla
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el Trabajo');
        }
    });


}

// function EditarTrabajo(TrabajoID) {
//     $.ajax({
//         url: '/Trabajo/CardTrabajos',
//         data: { id: TrabajoID },
//         type: 'GET',
//         dataType: 'json',
//         success: function (trabajo) {
//             if (trabajo) {
//                 document.getElementById("TrabajoID").value = trabajo.TrabajoID;
//                 document.getElementById("PersonaID").value = trabajo.PersonaID;
//                 document.getElementById("ProfesionID").value = trabajo.ProfesionID;
//                 document.getElementById("descripcion").value = trabajo.Descripcion;
//                 document.getElementById("hora").value = trabajo.Hora;
//                 document.getElementById("fecha").value = trabajo.Fecha;
//                 document.getElementById("direccion").value = trabajo.Direccion;
//                 document.getElementById("comentario").value = trabajo.Comentario;
//                 $('#agregarTrabajo').modal('show');
//             }
//         },
//         error: function (xhr, status) {
//             console.log('Disculpe, existió un problema al cargar el servicio para editar');
//         }
//     });
// }

function EditarTrabajo(trabajoID) {
    $.ajax({
        url: '/VistaTrabajo/CardTrabajos',
        data: { id: trabajoID },
        type: 'POST',
        dataType: 'json',
        success: function (vistaTrabajoPersonas) {
            let trabajo = vistaTrabajoPersonas[0];


            document.getElementById("TrabajoID").value = trabajoID;

            document.getElementById("PersonaID").value = trabajo.personaID;
            document.getElementById("ProfesionID").value = trabajo.profesionID;
            document.getElementById("descripcion").value = trabajo.descripcion;
            document.getElementById("hora").value = trabajo.hora;
            document.getElementById("fecha").value = trabajo.fecha;
            document.getElementById("direccion").value = trabajo.direccion;
            document.getElementById("comentario").value = trabajo.comentario;
            $('#agregarTrabajo').modal('show');

        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el servicio para editar');
        }
    });
}

function EliminarTrabajo(trabajoID) {
    $.ajax({
        // la URL para la petición
        url: '../../VistaTrabajo/EliminarTrabajo',
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