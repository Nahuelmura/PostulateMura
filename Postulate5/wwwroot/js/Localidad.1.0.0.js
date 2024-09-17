window.onload = ListadoLocalidad();

function ListadoLocalidad() {
    $.ajax({
        url: '../../Localidad/ListadoLocalidad',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (Localidades) {

             $("#ModalLocalidad").modal("hide");
             LimpiarModal();

            let contenidoTabla = ``;
            


            $.each(Localidades, function (index, Localidad) { 
                
                { 
                    
                
                contenidoTabla += `
                    <tr>
                        <td>${Localidad.provinciaNombre}</td>
                        <td>${Localidad.nombre}</td>
                        <td>${Localidad.codigoPostal}</td>
                        <td><button type="button" class="edit-button" onclick="AbrirModalEditar(${Localidad.localidadID})"><svg class="edit-svgIcon" viewBox="0 0 512 512">
                    <path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path>
                    </svg></button></td>
                        <td><button type="button" class="delete-button" onclick="EliminarLocalidad(${Localidad.localidadID})"><svg class="delete-svgIcon" viewBox="0 0 448 512">
                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path>
                    </svg></button></td>
                    </tr>`;
                }
            });
            document.getElementById("tbody-Localidades").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al deshabilitar');
        }

    });



}

function GuardarLocalidad() {
   
    let localidadID = document.getElementById("LocalidadID").value;
    let provinciaID = document.getElementById("ProvinciaID").value; 
    let nombreLocalidad = document.getElementById("nombreLocalidad").value;
    let codigoPostal = document.getElementById("CodigoPostal").value;


    $.ajax({
        url: '../../Localidad/GuardarLocalidad',
        data: {
            localidadID: localidadID,
            provinciaID: provinciaID,
            nombre: nombreLocalidad,
            codigoPostal: codigoPostal
        },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            ListadoLocalidad();
             $("#ModalLocalidad").modal("hide");
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al guardar la actividad');
        }
    });
}
function AbrirModalEditar(LocalidadID) {
    $.ajax({
        url: '../../Localidad/ListadoLocalidad',
        data: { id: LocalidadID },
        type: 'POST',
        dataType: 'json',
        success: function (localidades) {
            let localidad = localidades[0];

            document.getElementById("LocalidadID").value = LocalidadID;
            // $("#ModalTitulo").text("Editar Tipo De ejercico");
            document.getElementById("nombreLocalidad").value = localidad.nombre;
            document.getElementById("CodigoPostal").value = localidad.codigoPostal;
            $('#ModalLocalidad').modal('show');
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema ');
        }
    });
}

document.getElementById("btnGuardarLocalidad").addEventListener("click", function() {
   

    Swal.fire({
        position: "top-end",
        icon: "success",
        title: "Your work has been saved",
        showConfirmButton: false,
        timer: 1500
    });
});

    function EliminarLocalidad(LocalidadID) {
        Swal.fire({
            title: '¿Está seguro?',
            text: "¡No podrá revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminarlo'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '../../Localidad/EliminarLocalidad',
                    data: { LocalidadID: LocalidadID },
                    type: 'POST',
                    dataType: 'json',
                    success: function (Respuesta) {
                        ListadoLocalidad();
                        Swal.fire(
                            '¡Eliminado!',
                            'La localidad ha sido eliminada.',
                            'success'
                        );
                    },
                    error: function (xhr, status) {
                        console.log('Disculpe, existió un problema al consultar el registro para eliminado');
                        Swal.fire(
                            'Error',
                            'Hubo un problema al eliminar la localidad.',
                            'error'
                        );
                    }
                });
            }
        });
    }
function LimpiarModal() {
    document.getElementById("LocalidadID").value = 0;
    document.getElementById("ProvinciaID").value = 0;
    document.getElementById("nombreLocalidad").value = "";
    document.getElementById("CodigoPostal").value = "";
}
