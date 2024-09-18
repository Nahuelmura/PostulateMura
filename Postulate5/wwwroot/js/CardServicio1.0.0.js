function CardServicios() {
    
    $.ajax({
        url: '/Servicios/CardServicios',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (tiposProfesionMostrar) {
            console.log(tiposProfesionMostrar);

            let contenidoCard = ``;

            contenidoCard += `<div class="row">`; // Usamos row para aplicar el sistema de grilla
            
            $.each(tiposProfesionMostrar, function (index, tipoProfesion) {
                $.each(tipoProfesion.listadoPersonas, function (index, persona) {
                    contenidoCard += `
                    <div class="col-sm-12 col-md-6 col-lg-4 mb-3"> <!-- Configuración responsiva de las columnas -->
                        <div class="card-container card-hoover tamanio-card" id="card-${persona.servicioID}">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title text-center">${tipoProfesion.nombre}</h5> <!-- Título de la profesión sobre la tarjeta -->
                                    <a href="javascript:cargarPerfil(${persona.servicioID})" class="text-decoration-none text-dark">
                                        <h5>${persona.nombrePersona} ${persona.apellidoPersona}</h5>
                                        <p><strong>Teléfono:</strong> ${persona.telefonoPersona}</p>
                                    </a>
                                    <div class="card-action mt-3">
                                        <button type="button" class="btn btn-success me-2" onclick="EditarServicio(${persona.servicioID})">
                                            <i class="fa-regular fa-pen-to-square"></i> Editar
                                        </button>
                                        <button type="button" class="btn btn-danger me-2" onclick="EliminarServicio(${persona.servicioID})">
                                            <i class="fa-regular fa-trash-can"></i> Eliminar
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;
                });
            });
            
            contenidoCard += `</div>`; // Cerramos el row
            
            
            document.getElementById("contenedorCards").innerHTML = contenidoCard;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al cargar los servicios');
        }
    });
}
// Llama a la función para cargar las tarjetas al cargar la página
document.addEventListener("DOMContentLoaded", CardServicios);

function EliminarServicio(servicioID) {
    $.ajax({
        // la URL para la petición
        url: '../../Servicios/EliminarServicio',
        data: { servicioID: servicioID },
        type: 'POST',
        dataType: 'json',
        success: function (Respuesta) {
            CardServicios();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para eliminado');
        }
    });
}

function cargarPerfil(servicioID) {

    window.location.href = `/Servicios/VistaServicio/${servicioID}`;
}