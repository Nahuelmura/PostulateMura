
window.onload = RecuperarPerfilPersona; 

function RecuperarPerfilPersona() {
    $.ajax({
        url: '../../Persona/RecuperarPerfilPersona',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (personas) {
            let contenidoTabla = ``;

            $.each(personas, function (index, persona) {
                let claseEliminado = '';
                let botones = 
                    '<button type="button" onclick="CambiarEstadoPersona(' + persona.personaID + ', 1)" class="btn btn-danger btn-sm">Desactivar</button>';

                // Si la persona está eliminada/desactivada
                if (persona.eliminado) {
                    claseEliminado = 'table-danger';
                    botones = '<button type="button" onclick="CambiarEstadoPersona(' + persona.personaID + ', 0)" class="btn btn-warning btn-sm">Activar</button>';
                    
                    // aca los datos tachados 
                    contenidoTabla += `
                        <tr class="${claseEliminado}">
                            <td class="ocultar-en-550px"><del>${persona.nombreLocalidad}</del></td>
                            <td><del>${persona.nombre}</del></td>
                            <td class="ocultar-en-550px"><del>${persona.apellido}</del></td>
                            <td class="ocultar-en-550px"><del>${persona.telefono}</del></td>
                            <td class="ocultar-en-550px"><del>${persona.edad}</del></td>
                            <td class="ocultar-en-550px"><del>${persona.documento}</del></td>
                            <td><del>${persona.email}</del></td>
                            <td>${botones}</td>
                        </tr>`;
                } else {
                    // esto es oara mostrar los datos sin tachar 
                    contenidoTabla += `
                        <tr>
                            <td class="ocultar-en-550px">${persona.nombreLocalidad}</td>
                            <td>${persona.nombre}</td>
                            <td class="ocultar-en-550px">${persona.apellido}</td>
                            <td class="ocultar-en-550px">${persona.telefono}</td>
                            <td class="ocultar-en-550px">${persona.edad}</td>
                            <td class="ocultar-en-550px">${persona.documento}</td>
                            <td>${persona.email}</td>
                            <td>${botones}</td>
                        </tr>`;
                }
            });

            document.getElementById("tbody-Persona").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al recuperar los perfiles de persona.');
        }
    });
}


function CambiarEstadoPersona(personaID, accion) {

    $.ajax({
        type: "POST",
        url: '../../Persona/DesactivarActivarPersona',
        data: { PersonaID: personaID, Accion: accion },
        success: function (persona) {
       
            RecuperarPerfilPersona();
        },
        error: function (data) {
            alert('Ocurrió un error al cambiar el estado de la persona.');
        }
    });
}
