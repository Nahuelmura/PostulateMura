window.onload = ListadoProfesion();

function ListadoProfesion() {
    $.ajax({
        url: '../../TiposProfesion/ListadoProfesion',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (TiposProfesion) {

            // $("#ModalTipoEjercicio").modal("hide");
            LimpiarModal();

            let contenidoTabla = ``;


            $.each(TiposProfesion, function (index, Profesion) {

                {


                    contenidoTabla += `
                    <tr>
                        <td>${Profesion.nombre}</td>
                        <td>${Profesion.matricula}</td>
                        <td>
                        <button type="button" class="edit-button" onclick="AbrirModalEditar(${Profesion.profesionID})">  <svg class="edit-svgIcon" viewBox="0 0 512 512">
                    <path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path>
                    </svg></button></td>
                        <td>
                        <button type="button" class="delete-button" onclick="EliminarProfesion(${Profesion.profesionID})">  <svg class="delete-svgIcon" viewBox="0 0 448 512">
                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path>
                    </svg></button></td>
                    </tr>`;
                }
            });

            document.getElementById("tbody-Profesion").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al deshabilitar');
        }

    });



}

function GuardarProfesion() {


    // valor del modal
    let ProfesionID = document.getElementById("ProfesionID").value;
    let nombreProfesion = document.getElementById("nombreProfesion").value;
    let matricula = document.getElementById("matricula").value;


    $.ajax({
        url: '../../TiposProfesion/GuardarProfesion',
        data: { ProfesionID: ProfesionID, nombre: nombreProfesion, matricula: matricula },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {



            ListadoProfesion();
            $("#ModalProfesion").modal("hide");
        },
        error: function (xhr, status) {

            alert('Disculpe, existió un problema al guardar la actividad');
        }
    });
}
function AbrirModalEditar(ProfesionID) {
    $.ajax({
        url: '../../TiposProfesion/ListadoProfesion',
        data: { id: ProfesionID },
        type: 'POST',
        dataType: 'json',
        success: function (profesiones) {
            let profesion = profesiones[0];

            document.getElementById("ProfesionID").value = ProfesionID;
            document.getElementById("nombreProfesion").value = profesion.nombre;
            document.getElementById("matricula").value = profesion.matricula;
            $('#ModalProfesion').modal('show');
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema');
        }
    });
}

document.getElementById('btnGuardar').addEventListener('click', function () {
    let ProfesionID = document.getElementById("ProfesionID").value;
    let nombreProfesion = document.getElementById("nombreProfesion").value;
    let matricula = document.getElementById("matricula").value;

    $.ajax({
        url: '../../TiposProfesion/GuardarProfesion',
        data: {
            ProfesionID: ProfesionID,
            nombreProfesion: nombreProfesion,
            matricula: matricula
        },
        type: 'POST',
        dataType: 'json',
        success: function (respuesta) {
            $('#ModalProfesion').modal('hide');
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Your work has been saved",
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                ListadoProfesion(); // Actualiza la lista de profesiones
            });
        },
        error: function (xhr, status) {
            Swal.fire({
                title: "Error",
                text: "Disculpe, existió un problema al guardar los datos.",
                icon: "error"
            });
        }
    });
});

function EliminarProfesion(profesionID) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "¡No podrás revertir esto!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "¡Sí, eliminar!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../TiposProfesion/EliminarProfesion',
                data: { profesionID: profesionID },
                type: 'POST',
                dataType: 'json',
                success: function (Respuesta) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        text: "Tu archivo ha sido eliminado.",
                        icon: "success"
                    }).then(() => {
                        ListadoProfesion();
                    });
                },
                error: function (xhr, status) {
                    Swal.fire({
                        title: "Error",
                        text: "Disculpe, existió un problema al eliminar el registro.",
                        icon: "error"
                    });
                }
            });
        }
    });
}



function LimpiarModal() {
    document.getElementById("ProfesionID").value = "0";
    document.getElementById("nombreProfesion").value = "";
    document.getElementById("matricula").value = "";
}
