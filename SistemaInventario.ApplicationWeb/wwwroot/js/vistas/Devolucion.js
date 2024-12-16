$(document).ready(function () {
    $("#buscarVenta").click(function () {
        let numeroVenta = $("#numeroVenta").val()
        const number1 = $("#numeroVenta").val().trim();

        if (number1 == "" || number1 == null) {
            toastr.warning("", "Debe completa el campo : descripcion")
            $("#numeroVenta").focus()
            return;
        }
        console.log(number1);

        fetch(`/Venta/HistorialVenta1?numeroVenta=${numeroVenta}`)
            .then(response => {
                console.log(response);
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                $("#test tbody").html("");

                if (responseJson.length > 0) {

                    responseJson.forEach((venta) => {

                        $("#test tbody").append(
                            $("<tr>").append(
                                $("<td>").text(venta.idVenta).addClass("testeo"),
                                $("<td>").text(venta.numeroVenta),
                                $("<td>").text(venta.total).addClass("testeo1"),
                                $("<td>").append(
                                    $("<button>").addClass("btn btn-info btn-sm").append(
                                        $("<i>").addClass("fas fa-eye")
                                    ).data("venta", venta)
                                )
                            )
                        )
                    })
                }
            })
    })
})

$("#test tbody").on("click", ".btn-info", function () {

    let d = $(this).data("venta")

    $("#txtFechaRegistro").val(d.fecha)
    $("#txtNumVenta").val(d.numeroVenta)
    $("#txtUsuarioRegistro").val(d.usuario)
    $("#txtTipoDocumento").val(d.tipoDocumentoVenta)
    $("#txtDocumentoCliente").val(d.numeroDocumentoCliente)
    $("#txtNombreCliente").val(d.nombreCliente)
    $("#txtSubTotal").val(d.subTotal)
    $("#txtIGV").val(d.impuestoTotal)
    $("#txtTotal").val(d.total)


    $("#test tbody").html("");

    d.detalleVenta.forEach((item) => {

        $("#test tbody").append(
            $("<tr>").append(
                $("<td>").text(item.nombreProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precioVenta),
                $("<td>").text(item.total),
            )
        )

    })

    $("#linkImprimir").attr("href", `/Venta/MostrarPDFVenta?numeroVenta=${d.numeroVenta}`)

    $("#modalData").modal("show");

})

$("#testbtn").click(function () {

    const modelo = {
        
        numeroVenta: $("#numeroVenta").val(),
        motivo: $("#motivo").val(),
        montoDevuelto: $(".testeo1").val().trim(),
    };

    let numeroVenta = $("#numeroVenta").val();

    if (numeroVenta == "" || numeroVenta == null) {

        fetch("/Mantenimiento/RegistrarDevolucion", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {

                    tablaData.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo!", "La marca fue creada", "success")
                } else {
                    swal("Los sentimos", responseJson.mensaje, "error")
                }
            })
    }
})

$("#testbtn2").click(function () {

    const modelo = {

        numeroVenta: $("#numeroVenta").val(),
        motivo: $("#motivo").val(),
        montoDevuelto: $(".testeo1").val(),
    };

    console.log(modelo);
})