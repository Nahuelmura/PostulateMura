


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


function LimpiarModal(){
    // document.getElementById("PersonaID").value = 0;
    // document.getElementById("ProfesionID").value = 0;
    // document.getElementById("herramientas").value = "";
    // document.getElementById("descripcion").value = "";
    // document.getElementById("descripcion").value = "";
    // document.getElementById("descripcion").value = "";
    // document.getElementById("descripcion").value = "";

}
