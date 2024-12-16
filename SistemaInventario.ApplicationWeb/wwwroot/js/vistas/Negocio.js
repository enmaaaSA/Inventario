$(document).ready(function () {
    $(".card-body").LoadingOverlay("show");
    fetch("/Negocio/Obtener")
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.estado) {
                const d = responseJson.objeto
                $("#txtDocumento").val(d.documento)
                $("#txtNumeroDocumento").val(d.numeroDocumento)
                $("#txtRazonSocial").val(d.nombre)
                $("#txtCorreo").val(d.correo)
                $("#txtDireccion").val(d.direccion)
                $("#txtImpuesto").val(d.impuesto)
                $("#txtSimboloMoneda").val(d.simboloMoneda)
            } else {
                swal("Los sentimos", responseJson.mensaje, "error")

            }
        })
})

$("#btnGuardarCambios").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    const modelo = {
        documento: $("#txtDocumento").val(),
        numeroDocumento: $("#txtNumeroDocumento").val(),
        nombre: $("#txtRazonSocial").val(),
        correo: $("#txtCorreo").val(),
        direccion: $("#txtDireccion").val(),
        impuesto: $("#txtImpuesto").val(),
        simboloMoneda: $("#txtSimboloMoneda").val()
    }
    const formData = new FormData()
    formData.append("modelo", JSON.stringify(modelo))

    $(".card-body").LoadingOverlay("show");
    fetch("/Negocio/GuardarCambio", {
        method: "POST",
        body: formData
    })
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {
                swal("Listo!", "El negocio fue modificado", "success")

            } else {
                swal("Los sentimos", responseJson.mensaje, "error")

            }
        })
})