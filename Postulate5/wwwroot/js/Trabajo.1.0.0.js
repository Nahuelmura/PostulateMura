function CardTrabajos() {
    $.ajax({
        url: '/Trabajo/CardTrabajos',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (tiposProfesionMostrar) {
            console.log(tiposProfesionMostrar);

            let contenidoCard = ``;
            contenidoCard += `<div class="row">`;

            let trabajosEncontrados = false;

            $.each(tiposProfesionMostrar, function (index, tipoProfesion) {

                if (tipoProfesion.listadoPersonas && tipoProfesion.listadoPersonas.length > 0) {
                    trabajosEncontrados = true;
                    $.each(tipoProfesion.listadoPersonas, function (index, persona) {
                        contenidoCard += `
                            <div class="col-md-4 mb-3 cartas_card" id="card-${persona.trabajoID}"> <!-- col-md-4 para tener 3 por fila en pantallas medianas o más grandes -->
                                <div class="card card h-100"> <!-- Establecemos una altura mínima y uso de flex -->
                                    <div class="row g-0 h-100"">
                                        <h5 class="card-title text-center">${tipoProfesion.nombre}</h5> <!-- Título de la profesión dentro de la tarjeta -->
                                        <p><strong><i class="fa-solid fa-person"></i> Nombre:</strong> ${persona.nombrePersona}</p>
                                        <p><strong><i class="fa-solid fa-person"></i> Apellido:</strong> ${persona.apellidoPersona}</p>
                                        <p><strong><i class="fa-solid fa-phone"></i> Teléfono:</strong> ${persona.telefonoPersona}</p>
                                        <p><strong><i class="fa-solid fa-location-dot"></i> Dirección:</strong> ${persona.direccion}</p>
                                        <p><strong><i class="fa-solid fa-list"></i> Descripción:</strong> ${persona.descripcion}</p>
                                        <p><strong><i class="fa-regular fa-clock"></i> Hora:</strong> ${persona.horastring}</p>
                                        <p><strong><i class="fa-regular fa-calendar"></i> Fecha de inicio:</strong> ${persona.fechastring}</p>
                                        <p><strong><i class="fa-solid fa-comment"></i> Comentario:</strong> ${persona.comentario}</p>
                                        <div class="card-action mt-auto"> <!-- Utiliza mt-auto para empujar el contenido al final -->
                                            <button type="button" class="btn btn-success me-2" onclick="EditarTrabajo(${persona.trabajoID})">
                                                <i class="fa-regular fa-pen-to-square"></i> Editar
                                            </button>
                                            <button type="button" class="btn btn-danger me-2" onclick="EliminarTrabajo(${persona.trabajoID})">
                                                <i class="fa-regular fa-trash-can"></i> Eliminar
                                            </button>

                                            <button type="button" class="btn btn-info me-2" onclick="abrirModalPostular(${persona.trabajoID})">
                                                <i class="fa-regular fa-paper-plane"></i> Postular trabajo
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>`;
                    });
                }
            });

            contenidoCard += `</div>`;


            if (!trabajosEncontrados) {
                contenidoCard = `
                    <div class="alert alert-warning text-center" role="alert">
                        No hay trabajos suyos postulados ni asociados a sus servicios.
                    </div>`;
            }

            document.getElementById("contenedorCards").innerHTML = contenidoCard;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al cargar los trabajos');
        }
    });
}




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
    Swal.fire({
        title: "Quiere eliminar este trabajo?",
        text: "Esta accion no podra ser revertida!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, quiero eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../Trabajo/EliminarTrabajo',
                data: { trabajoID: trabajoID },
                type: 'POST',
                dataType: 'json',
                success: function (Respuesta) {

                    CardTrabajos();


                    Swal.fire({
                        title: "!Eliminado",
                        text: "Su trabajo fue eliminado.",
                        icon: "success"
                    });
                },
                error: function (xhr, status) {
                    console.log('Disculpe, existió un problema al consultar el registro para eliminado');
                }
            });
        }
    });
}







// desde aca empiza lo relacionado al postulado de trabajo y solicitud de servicios



function PostularTrabajo(servicioID, trabajoID) {
    console.log("servicioID:", servicioID, "trabajoID:", trabajoID);
    $.ajax({
        url: '/ContratoRespondido/SolicitarServicios',
        type: 'POST',
        data: { servicioID: servicioID, trabajoID: trabajoID },
        dataType: 'json',
        success: function (SolicitarServicio) {
            if (SolicitarServicio.success) {
                alert(SolicitarServicio.mensaje);
                $('#serviciosModal').modal('hide');
            } else {
                alert(SolicitarServicio.mensaje);
            }
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al solicitar el servicio');
        }
    });
}


function abrirModalPostular(trabajoID) {
    $.ajax({
        url: '/ContratoRespondido/BuscarServiciosTrabajo',
        type: 'POST',
        data: { trabajoID: trabajoID },
        success: function (ContratosRespondidoMostrar) {
            console.log(ContratosRespondidoMostrar);
            let contenidoTabla = ``;

            $.each(ContratosRespondidoMostrar, function (index, contrato) {
                contenidoTabla += `
                    <tr>
                        <td>${contrato.nombrePersona} ${contrato.apellidoPersona}</td>
                        <td>
                            <button class="btn btn-primary" onclick="PostularTrabajo(${contrato.servicioID}, ${trabajoID})">Solicitar servicio</button>
                        </td>
                    </tr>`;
            });

            document.getElementById("tbody-Contratos").innerHTML = contenidoTabla;
            $('#serviciosModal').modal('show');
        },
    });
}





// window.onload = ListadoServicioSolicitado();
// function ListadoServicioSolicitado() {
//     $.ajax({
//         url: '../../ContratoRespondido/ListadoServicioSolicitado',
//         type: 'POST',
//         dataType: 'json',
//         success: function (traerServicios) {
//             let contenidoTabla = ``;
        
//             $.each(traerServicios, function (index, traerservicio) {
//                 contenidoTabla += `
//                     <tr>
//                         <td class="ocultar-en-550px">${traerservicio.stringFechaMatch}</td> 
                       
//                         <td class="ocultar-en-550px">${traerservicio.nombrePersona}</td> 
//                         <td>${traerservicio.descripcionTrabajo}</td> 
//                         <td>${traerservicio.comentarioTrabajo}</td> 
//                            <td>${traerservicio.respuestaDesolicitudString}</td> 
//                         <td>
//                             <button type="button" class="btn btn-info me-2" onclick="AceptarRespuesta(${traerservicio.contratoRespondidoID})">Aceptar</button>
//                         </td>
//                         <td>
//                             <button type="button" class="btn btn-danger me-2" onclick="Rechazar(${traerservicio.contratoRespondidoID})">Rechazar</button>
//                         </td>
//                     </tr>`;
//             });
        
//             document.getElementById("solicitudesPendientesBody").innerHTML = contenidoTabla;
//         },
        
//         error: function (xhr, status) {
//             alert('Hubo un problema al cargar las solicitudes.');
//         }
//     });
// }














// function AceptarRespuesta(contratoRespondidoID) {
//     $.ajax({
//         url: '../../ContratoRespondido/CambiarEstado',
//         type: 'POST',
//         dataType: 'json',
//         data: {
//             contratoRespondidoID: contratoRespondidoID,
//             estado: 'aceptar'  // Indicamos que queremos aceptar la solicitud
//         },
//         success: function (respuesta) {
//             ListadoServicioSolicitado();  // Recargar la tabla con la solicitud actualizada
//         },
//         error: function (xhr, status, error) {
//             alert('Hubo un error al aceptar la solicitud: ' + error);
//         }
//     });
// }

// function Rechazar(contratoRespondidoID) {
//     $.ajax({
//         url: '../../ContratoRespondido/CambiarEstado',
//         type: 'POST',
//         dataType: 'json',
//         data: {
//             contratoRespondidoID: contratoRespondidoID,
//             estado: 'rechazar'  // Indicamos que queremos rechazar la solicitud
//         },
//         success: function (respuesta) {
//             ListadoServicioSolicitado();  // Recargar la tabla con la solicitud actualizada
//         },
//         error: function (xhr, status, error) {
//             alert('Hubo un error al rechazar la solicitud: ' + error);
//         }
//     });
// }



window.onload = ListadoTrabajosPostulados;

function ListadoTrabajosPostulados() {
    $.ajax({
        url: '/ContratoRespondido/ListadoTrabajosPostulados', 
        type: 'POST',  
        dataType: 'json',
        success: function (trabajosPostulados) {
            let contenidoTabla = ``; 

            $.each(trabajosPostulados, function (index, trabajo) {
                contenidoTabla += `
                    <tr>
                        <td>${trabajo.descripcionTrabajo}</td>
                        <td>${trabajo.comentarioTrabajo}</td>
                  
                        <td>${trabajo.nombrePersonaServicio} ${trabajo.apellidoPersonaServicio}</td> <!-- Nombre y Apellido de la persona que ofrece el servicio -->
                        <td>${trabajo.estadoSolicitud}</td>
                        <td>${trabajo.fechaPostulacion}</td>
                    </tr>`;
            });

            document.getElementById("tablaTrabajosPostulados").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            alert('Hubo un problema al cargar los trabajos postulados.');
        }
    });
}
