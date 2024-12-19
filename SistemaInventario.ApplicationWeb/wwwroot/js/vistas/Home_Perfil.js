
$(document).ready(function () {


    $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/ObtenerUsuario")
        .then(response => {
            $(".container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {


            if (responseJson.estado) {
                const d = responseJson.objeto

                $("#txtNombre").val(d.nombre)
                $("#txtApellido").val(d.apellido)
                $("#txtCorreo").val(d.correo)
                $("#txtRol").val(d.rol)
                $("#txtDocumento").val(d.documento)
                $("#txtNumberDocumento").val(d.numeroDocumento)

            } else {
                swal("Los sentimos", responseJson.mensaje, "error")

            }
        })


})

$("#btnCambiarClave").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    if ($("#txtClaveNueva").val().trim() != $("#txtConfirmarClave").val().trim() ) {
        toastr.warning("", "Las contraseñas no coinciden")
        return;
    }

    let modelo = {
        claveActual: $("#txtClaveActual").val().trim(),
        claveNueva: $("#txtClaveNueva").val().trim()
    }


    fetch("/Home/CambiarClave", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(modelo)
    })
        .then(response => {
            $(".showSweetAlert").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {

                swal("Listo!", "Su contraseña  fue actualizada", "success")
                $("input.input-validar").val("");
            } else {
                swal("Los sentimos", responseJson.mensaje, "error")
            }
        })

})