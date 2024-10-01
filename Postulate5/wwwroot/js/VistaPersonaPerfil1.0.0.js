window.onload = function () {
    RecuperarPerfilPersonaLogeada();
};

function RecuperarPerfilPersonaLogeada() {
    $.ajax({
        url: '../../Persona/RecuperarPerfilPersonaLogeada',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (Personas) {
            $("#perfilContainer").empty();
            let contenidoPerfil = `<div class="row">`;

            $.each(Personas, function (index, perfil) {
                contenidoPerfil += `
                    <div class="col-md-12">
                        <div class="card mb-4">
                            <div class="card-header bg-secondary text-white ">
                                Datos personales
                            </div>
                            <div class="card-body">
                            <p class="texto3"><strong>Localidad de residencia:</strong> ${perfil.nombreLocalidad}</p>
                                <p class="texto3"><strong>Nombre:</strong> ${perfil.nombre}</p>
                                <p class="texto3"><strong>Apellido:</strong> ${perfil.apellido}</p>
                                <p class="texto3"><strong>Teléfono:</strong> ${perfil.telefono}</p>
                                <p class="texto3"><strong>Edad:</strong> ${perfil.edad}</p>
                                <p class="texto3"><strong>Documento:</strong> ${perfil.documento}</p>
                                <p class="texto3"><strong>Email:</strong> ${perfil.email}</p>
                                <div class="button-group mt-3">
                                    <button type="button" class="btn btn-primary" onclick="EditarPefil(${perfil.personaID})">
                                        Editar Cuenta
                                    </button>
                                    <button type="button" class="btn btn-danger" onclick="EliminarPersona(${perfil.personaID})">
                                        Eliminar Cuenta
                                    </button>
                                </div>
                            </div>

                        </div>
                    </div>`;

                    
            });

            contenidoPerfil += `</div>`; 
            $("#perfilContainer").html(contenidoPerfil);
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al recuperar el perfil.');
        }
    });
}




function EditarPefil(personaID) {

    document.getElementById("PersonaID").value = personaID;

    $.ajax({
        url: '../../Persona/RecuperarPerfilPersonaLogeada',
        data: { id: personaID },
        type: 'POST',
        dataType: 'json',
        success: function (personas) {
            let Persona = personas[0];
            document.getElementById("PersonaID").value = Persona.personaID;
            document.getElementById("nombre").value = Persona.nombre;
            document.getElementById("apellido").value = Persona.apellido;
            document.getElementById("edad").value = Persona.edad;
            document.getElementById("documento").value = Persona.documento;
            document.getElementById("telefono").value = Persona.telefono;
            document.getElementById("LocalidadID").value = Persona.nombreLocalidad;

            $("#ModalVistaPersona").modal("show");
            RecuperarPerfilPersona();

        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema ');
        }
    });
}

function GuardarPerfil() {
    var personaID = $('#PersonaID').val();
    var nombre = $('#nombre').val();
    var apellido = $('#apellido').val();
    var edad = $('#edad').val();
    var telefono = $('#telefono').val();
    var documento = $('#documento').val();
    var correo = ""; 
    var localidadId =$('#LocalidadID').val();

    $.ajax({
        url: '/Persona/GuardarPerfilLogeada',
        type: 'POST', 
        data: {
            personaID: personaID,
            nombre: nombre,
            apellido: apellido,
            edad: edad,
            telefono: telefono,
            documento: documento,
            correo: correo,
            localidadId: localidadId
        },
        success: function (response) {
            alert(response);
            // Puedes agregar lógica adicional para actualizar la tabla o cerrar el modal
    
            $("#ModalVistaPersona").modal("hide");
            RecuperarPerfilPersona();

            
        },
        error: function (error) {
            console.log(error);
            alert('Ocurrió un error al guardar los datos.');
        }
    });


    
}
function EliminarPersona(personaID) {
    // Primero mostramos el SweetAlert para la confirmación
    Swal.fire({
        title: "¿Estás seguro?",
        text: "¡No podrás revertir esto!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, ¡eliminarlo!"
    }).then((result) => {
  
        if (result.isConfirmed) {
         
            $.ajax({
                url: '../../Persona/EliminarPersona', // La URL para la petición
                data: { personaID: personaID },
                type: 'POST',
                dataType: 'json',
                success: function (respuesta) {
                    if (respuesta.success) {
                       
                        Swal.fire({
                            title: "¡Eliminado!",
                            text: "El registro ha sido eliminado exitosamente.",
                            icon: "success"
                        }).then(() => {
                       
                            window.location.href = '../Identity/Account/Login'; // Cambia la ruta según tu configuración
                        });
                    } else {
                     
                        Swal.fire({
                            title: "Error",
                            text: "Hubo un problema al eliminar el registro.",
                            icon: "error"
                        });
                    }
                },
                error: function (xhr, status) {
                    // Mostramos el error si ocurre un problema con la petición AJAX
                    Swal.fire({
                        title: "Error",
                        text: "Disculpa, hubo un problema al intentar eliminar el registro.",
                        icon: "error"
                    });
                }
            });
        }
    });
}





