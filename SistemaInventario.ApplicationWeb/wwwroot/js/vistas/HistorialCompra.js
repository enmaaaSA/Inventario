const VISTA_BUSQUEDA = {

    busquedaFecha: () => {

        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    }, busquedaVenta: () => {

        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").hide()
        $(".busqueda-venta").show()
    }
}

$(document).ready(function () {
    VISTA_BUSQUEDA["busquedaFecha"]()

    $.datepicker.setDefaults($.datepicker.regional["es"])

    $("#txtFechaInicio").datepicker({ dateFormat: "dd/mm/yy" })
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy" })

})

$("#cboBuscarPor").change(function () {

    if ($("#cboBuscarPor").val() == "fecha") {
        VISTA_BUSQUEDA["busquedaFecha"]()
    } else {
        VISTA_BUSQUEDA["busquedaVenta"]()
    }

})


$("#btnBuscar").click(function () {

    if ($("#cboBuscarPor").val() == "fecha") {

        if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") {
            toastr.warning("", "Debe ingresar fecha inicio y fin")
            return;
        }
    } else {

        if ($("#txtNumeroVenta").val().trim() == "") {
            toastr.warning("", "Debe ingresar el numero de venta")
            return;
        }
    }

    let numeroVenta = $("#txtNumeroVenta").val()
    let fechaInicio = $("#txtFechaInicio").val()
    let fechaFin = $("#txtFechaFin").val()


    $(".card-body").find("div.row").LoadingOverlay("show");

    fetch(`/Compra/HistorialCompra?numeroCompra=${numeroVenta}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`)
        .then(response => {
            $(".card-body").find("div.row").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            $("#tbventa tbody").html("");

            if (responseJson.length > 0) {

                responseJson.forEach((compra) => {

                    $("#tbventa tbody").append(
                        $("<tr>").append(
                            $("<td>").text(compra.fecha),
                            $("<td>").text(compra.numeroCompra),
                            $("<td>").text(compra.tipoDocumentoCompra),
                            $("<td>").text(compra.documentoProveedor),
                            $("<td>").text(compra.proveedor),
                            $("<td>").text(compra.total),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-info btn-sm").append(
                                    $("<i>").addClass("fas fa-eye")
                                ).data("compra", compra)
                            )
                        )
                    )

                })

            }

        })

})

$("#tbventa tbody").on("click", ".btn-info", function () {

    let d = $(this).data("compra")

    $("#txtFechaRegistro").val(d.fecha)
    $("#txtNumVenta").val(d.numeroCompra)
    $("#txtUsuarioRegistro").val(d.usuario)
    $("#txtTipoDocumento").val(d.tipoDocumentoCompra)
    $("#txtDocumentoCliente").val(d.proveedor)
    $("#txtNombreCliente").val(d.nombreProveedor)
    $("#txtSubTotal").val(d.subTotal)
    $("#txtIGV").val(d.impuestoTotal)
    $("#txtTotal").val(d.total)


    $("#tbProductos tbody").html("");

    d.detalleCompra.forEach((item) => {

        $("#tbProductos tbody").append(
            $("<tr>").append(
                $("<td>").text(item.nombreProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precioCompra),
                $("<td>").text(item.total),
            )
        )

    })

    $("#linkImprimir").attr("href", `/Venta/MostrarPDFVenta?numeroVenta=${d.numeroVenta}`)

    $("#modalData").modal("show");

})