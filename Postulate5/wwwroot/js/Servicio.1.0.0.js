


function agregarServicio() {
    let personaID = document.getElementById("PersonaID").value;
    let servicioID = document.getElementById("ServicioID").value;
    let profesionID = document.getElementById("ProfesionID").value;
    // let herramienta = document.getElementById("herramienta").checked;
    let descripcion = document.getElementById("descripcion").value || "Descripción pendiente"; // Valor predeterminado
    let titulo = document.getElementById("titulo").value || "Título temporal"; // Valor predeterminado
    let institucion = document.getElementById("institucion").value || "Institución sin especificar"; // Valor predeterminado

    // Crear un objeto FormData para enviar archivos
    let formData = new FormData();
    formData.append("ServicioID", servicioID);
    formData.append("PersonaID", personaID);
    formData.append("ProfesionID", profesionID);
    // formData.append("Herramienta", herramienta);
    formData.append("Descripcion", descripcion);
    formData.append("Titulo", titulo);
    formData.append("Institucion", institucion);

    $.ajax({
        url: '/Servicios/AgregarServicio',
        data: formData,
        type: 'POST',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                alert("Servicio guardado exitosamente");
                $('#agregarServicio').modal('hide');
               
                    CardServicios();
               
                    CargarPerfilServicio();
                
             
                alert("Error al guardar el servicio: " + response.message);
            }
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el servicio');
        }
    });
}
function EditarServicio() {
    let servicioID = document.getElementById("ServicioID").value;
    {
        $.ajax({
            url: '/Servicios/RecuperarPerfilServicio',
            data: { id: servicioID },
            type: 'POST',
            dataType: 'json',
            success: function (servicios) {
                let servicio = servicios[0]; 

                console.log(servicio.herramienta);
                
                document.getElementById("descripcion").value = servicio.descripcion;
                document.getElementById("herramienta").checked = servicio.herramienta;
                document.getElementById("titulo").value = servicio.titulo;
                document.getElementById("institucion").value = servicio.institucion;
                $('#agregarServicio').modal('show');
                CargarPerfilServicio();
            },



            error: function (xhr, status) {
                console.log('Disculpe, existió un problema al cargar el servicio para editar');
            }
        });
    }


}





window.onload = ListadoServicioSolicitado();
function ListadoServicioSolicitado() {
    $.ajax({
        url: '../../ContratoRespondido/ListadoServicioSolicitado',
        type: 'POST',
        dataType: 'json',
        success: function (traerServicios) {
            let contenidoTabla = ``;
        
            $.each(traerServicios, function (index, traerservicio) {
                contenidoTabla += `
                    <tr>
                        <td class="ocultar-en-550px">${traerservicio.stringFechaMatch}</td> 
                       
                    
                        <td>${traerservicio.descripcionTrabajo}</td> 
                        <td>${traerservicio.comentarioTrabajo}</td> 
                           <td>${traerservicio.direccionTrabajo}</td> 
                           <td>${traerservicio.horaSolicitadaTrabajo}</td> 
                     
                           <td>${traerservicio.fechaSolicitadaTrabajo}</td> 
                     
                     
                           <td>${traerservicio.respuestaDesolicitudString}</td> 
                        <td>
                            <button type="button" class="btn btn-info me-2" onclick="AceptarRespuesta(${traerservicio.contratoRespondidoID})">Aceptar</button>
                        </td>
                        <td>
                            <button type="button" class="btn btn-danger me-2" onclick="Rechazar(${traerservicio.contratoRespondidoID})">Rechazar</button>
                        </td>
                    </tr>`;
            });
        
            document.getElementById("solicitudesPendientesBody").innerHTML = contenidoTabla;
        },
        
        error: function (xhr, status) {
            alert('Hubo un problema al cargar las solicitudes.');
        }
    });
}



function AceptarRespuesta(contratoRespondidoID) {
    $.ajax({
        url: '../../ContratoRespondido/CambiarEstado',
        type: 'POST',
        dataType: 'json',
        data: {
            contratoRespondidoID: contratoRespondidoID,
            estado: 'aceptar'  
        },
        success: function (respuesta) {
            ListadoServicioSolicitado();  
        },
        error: function (xhr, status, error) {
            alert('Hubo un error al aceptar la solicitud: ' + error);
        }
    });
}

function Rechazar(contratoRespondidoID) {
    $.ajax({
        url: '../../ContratoRespondido/CambiarEstado',
        type: 'POST',
        dataType: 'json',
        data: {
            contratoRespondidoID: contratoRespondidoID,
            estado: 'rechazar'  // Indicamos que queremos rechazar la solicitud
        },
        success: function (respuesta) {
            ListadoServicioSolicitado();  // Recargar la tabla con la solicitud actualizada
        },
        error: function (xhr, status, error) {
            alert('Hubo un error al rechazar la solicitud: ' + error);
        }
    });
}





function LimpiarModal(){
    // document.getElementById("PersonaID").value = 0;
    // document.getElementById("ProfesionID").value = 0;
    // document.getElementById("herramientas").value = "";
    // document.getElementById("descripcion").value = "";
    // document.getElementById("descripcion").value = "";
    // document.getElementById("descripcion").value = "";
    // document.getElementById("descripcion").value = "";

}
