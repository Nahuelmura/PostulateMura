$(document).ready(function() {
    $("#formulario").on("submit", function(event) {
        event.preventDefault();
        if (Guardar()) {
            enviarFormularioAjax();
        }
    });
});

function Guardar() {
    // Limpiar mensajes de error anteriores
    $(".error-message").remove();

    let isValid = true;

    if ($("#ProvinciaID").val() === "") {
        $("#ProvinciaID").after('<div class="error-message text-danger">El campo Provincia no puede estar vacío.</div>');
        $("#ProvinciaID").focus();
        isValid = false;
    }
    if ($("#LocalidadID").val() === "") {
        $("#LocalidadID").after('<div class="error-message text-danger">El campo Localidad no puede estar vacío.</div>');
        if (isValid) $("#LocalidadID").focus(); 
    }
    if ($("#nombre").val() === "") {
        $("#nombre").after('<div class="error-message text-danger">El campo Nombre no puede estar vacío.</div>');
        if (isValid) $("#nombre").focus();
        isValid = false;
    }
    if ($("#apellido").val() === "") {
        $("#apellido").after('<div class="error-message text-danger">El campo Apellidos no puede estar vacío.</div>');
        if (isValid) $("#apellido").focus();
        isValid = false;
    }
    if ($("#edad").val() === "") {
        $("#edad").after('<div class="error-message text-danger">El campo Edad no puede estar vacío.</div>');
        if (isValid) $("#edad").focus();
        isValid = false;
    }
    if ($("#telefono").val() === "") {
        $("#telefono").after('<div class="error-message text-danger">El campo Teléfono no puede estar vacío.</div>');
        if (isValid) $("#telefono").focus();
        isValid = false;
    }
    if ($("#documento").val() === "") {
        $("#documento").after('<div class="error-message text-danger">El campo Documento no puede estar vacío.</div>');
        if (isValid) $("#documento").focus();
        isValid = false;
    }

    return isValid;
}

function enviarFormularioAjax() {
    let formulario = $("#formulario").serialize(); // Serializa el formulario
    console.log(formulario); // Agrega esta línea para verificar el contenido del formulario

    $.ajax({
        url: '../../Persona/Guardar',
        data: formulario,
        type: 'POST',
        dataType: 'json',
        success: function(resultado) {
            console.log("Formulario guardado exitosamente");
            // Mostrar mensaje de éxito
            $("#formulario").fadeOut("slow", function() {
                $("#mensajeExito").html('<div class="alert  Felicitaciones" role="alert">Felicidades, usted es un nuevo usuario de Postulate.Com</div>');
                $("#mensajeExito").fadeIn("slow");
            });
        },
        error: function(xhr, status) {
            alert('Disculpe, existió un problema al guardar el formulario');
        }
    });
}








