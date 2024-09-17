function CardServicios() {
    let profesionID = $("#profesionBuscarID").val(); 
    
    $.ajax({
        url: '/VistaServicio/CardServicios',
        data: {
            ProfesionID: profesionID 

        },
        type: 'POST',
        dataType: 'json',
        success: function (tiposProfesionMostrar) {
            console.log(tiposProfesionMostrar);

            let contenidoCard = ``;
            $.each(tiposProfesionMostrar, function (index, tipoProfesion) {
                contenidoCard += `
                    <div class="profesion-group">
                        <h3>${tipoProfesion.nombre}</h3>
                        <div class="row justify-content-start">`; // Justificación al inicio
            
       
            
                $.each(tipoProfesion.listadoPersonas, function (index, persona) {
                    contenidoCard += `
                    <div class="col-md-6 col-lg-4 mb-3 d-flex align-items-stretch card-container card-hoover tamanio-card" id="card-${persona.servicioID}">
                        <div class="card mb-3">
                            <div class="row g-0">
                                <div class="col-md-4">
                                    <div class="card-bg color-Card" style="width: 15em; height: 100%;"></div>
                                </div>
                                <div class="col-md-8">
                                    <div class="card-body">
                                        <a href="javascript:cargarPerfil(${persona.servicioID})" class="text-decoration-none text-dark">
                                            <h5 class="">${persona.nombrePersona} ${persona.apellidoPersona}</h5>
                                            <p class=""><strong>Teléfono:</strong> ${persona.telefonoPersona}</p>
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="herramienta-${persona.servicioID}" disabled ${persona.herramienta ? 'checked' : ''}>
                                                <label class="form-check-label" for="herramienta-${persona.servicioID}">
                                                    ¿Este usuario cuenta con herramientas?
                                                </label>
                                            </div>
                                        </a>
                                        <div class="card-action mt-3">
                                            <button type="button" class="btn btn-danger me-2" onclick="EliminarServicio(${persona.servicioID})">
                                                <i class="fa-regular fa-trash-can"></i> Eliminar
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;
                });
                contenidoCard += `</div></div>`;;
            });
            
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
        url: '../../VistaServicio/EliminarServicio',
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