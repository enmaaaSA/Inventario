let impuestoValue = 0;
$(document).ready(function ()
{
    fetch("/Venta/ListaTipoDocumentoVenta")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboTipoDocumentoVenta").append(
                        $("<option>").val(item.idTipoDocumentoVenta).text(item.nombre)
                    )
                })
            }
        })
    fetch("/Negocio/Obtener")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {

                const d = responseJson.objeto;

                console.log(d)

                $("#inputGroupSubTotal").text(`Sub total - ${d.simboloMoneda}`)
                $("#inputGroupIGV").text(`IGV(${d.impuesto}%) - ${d.simboloMoneda}`)
                $("#inputGroupTotal").text(`Total - ${d.simboloMoneda}`)

                impuestoValue = parseFloat(d.impuesto)
            }

        })

    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/Venta/ObtenerProductos",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    busqueda: params.term
                };
            },
            processResults: function (data,) {

                return {
                    results: data.map((item) => (
                        {
                            id: item.idProducto,
                            text: item.nombre,
                            precio: parseFloat(item.precioVenta),
                            marca: item.marca,
                            categoria: item.categoria,
                            codigo: item.codigoBarra,
                            descripcion: item.descripcion,
                            stock: item.stock
                        }
                    ))
                };
            }
        },
        language: "es",
        placeholder: 'Buscar Producto...',
        minimumInputLength: 1,
        templateResult: formatoResultados
    });
})

function formatoResultados(data) {

    //esto es por defecto, ya que muestra el "buscando..."
    if (data.loading)
        return data.text;

    var contenedor = $(
        `<table width="100%">
            <tr>
                <td class="col-4">
                    <p style="margin:2px">${data.codigo}</p>
                </td>
                <td class="col-4">
                    <p style="font-weight: bolder;margin:2px">${data.text}</p>
                </td>
                <td class="col-4">
                    <p style="font-weight: bolder;margin:2px">${data.stock}</p>
                </td>
            </tr>
         </table>`
    );

    return contenedor;
}

$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();
})

let ProductosParaVenta = [];
$("#cboBuscarProducto").on("select2:select", function (e) {
    const data = e.params.data;

    let producto_encontrado = ProductosParaVenta.filter(p => p.idProducto == data.id)
    if (producto_encontrado.length > 0) {
        $("#cboBuscarProducto").val("").trigger("change")
        toastr.warning("", "El producto ya fue agregado")
        return false
    }

    swal({
        title: data.text,
        text: data.marca,
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        inputPlaceholder: "Ingrese Cantidad"
    },
        function (valor) {

            if (valor === false) return false;

            if (valor === "") {
                toastr.warning("", "Necesita ingresar la cantidad")
                return false;
            }
            if (isNaN(parseInt(valor))) {
                toastr.warning("", "Debe ingresar un valor númerico")
                return false;
            }

            let producto = {
                idProducto: data.id,
                codigoProducto: data.codigo,
                descripcion: data.descripcion,
                nombreProducto: data.text,
                marcaProducto: data.marca,
                categoriaProducto: data.categoria,
                cantidad: parseInt(valor),
                precioVenta: data.precio.toString(),
                total: (parseFloat(valor) * data.precio).toString()
            }
            ProductosParaVenta.push(producto)
            mostrarProducto_Precios();
            $("#cboBuscarProducto").val("").trigger("change")
            console.log(producto)
            swal.close()
        }
    )

})

function mostrarProducto_Precios() {

    let total = 0;
    let igv = 0;
    let subtotal = 0;
    let porcentaje = impuestoValue / 100;

    $("#tbProducto tbody").html("")

    console.log(ProductosParaVenta)

    ProductosParaVenta.forEach((item) => {

        total = total + parseFloat(item.total)

        $("#tbProducto tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                        $("<i>").addClass("fas fa-trash-alt")
                    ).data("idProducto", item.idProducto)
                ),
                $("<td>").text(item.nombreProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precioVenta),
                $("<td>").text(item.total),
            )
        )
    })

    subtotal = total / (1 + porcentaje);
    igv = total - subtotal;

    $("#txtSubTotal").val(subtotal.toFixed(2))
    $("#txtIGV").val(igv.toFixed(2))
    $("#txtTotal").val(total.toFixed(2))

    console.log(ProductosParaVenta)
}

$(document).on("click", "button.btn-eliminar", function () {

    const _idproducto = $(this).data("idProducto")

    ProductosParaVenta = ProductosParaVenta.filter(p => p.idProducto != _idproducto);

    mostrarProducto_Precios();
})


$("#btnTerminarVenta").click(function () {

    if (ProductosParaVenta.length < 1) {
        toastr.warning("", "Debe ingresar productos")
        return;
    }
    if ($("#txtNombreCliente").val() == "") {
        nombreCliente = "-"
    }
    if ($("#txtDocumentoCliente").val() == "") {
        documentoCliente= "00000000"
    }

    const vmDetalleVenta = ProductosParaVenta;

    const venta = {
        idTipoDocumentoVenta: $("#cboTipoDocumentoVenta").val(),
        documentoCliente: $("#cboDocumento").val(),
        numeroDocumentoCliente: $("#txtDocumentoCliente").val() === "" ? "00000000" : $("#txtDocumentoCliente").val(),
        nombreCliente: $("#txtNombreCliente").val() === "" ? "-" : $("#txtNombreCliente").val(),
        subTotal: $("#txtSubTotal").val(),
        impuestoTotal: $("#txtIGV").val(),
        total: $("#txtTotal").val(),
        DetalleVenta: vmDetalleVenta
    }
    console.log(venta)
    $("#btnTerminarVenta").LoadingOverlay("show");

    fetch("/Venta/RegistrarVenta", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(venta)
    })
        .then(response => {
            $("#btnTerminarVenta").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {
                ProductosParaVenta = [];
                mostrarProducto_Precios();

                $("#txtDocumentoCliente").val("")
                $("#txtNombreCliente").val("")
                $("#cboTipoDocumentoVenta").val($("#cboTipoDocumentoVenta option:first").val())

                swal("Registrado!", `Numero Venta : ${responseJson.objeto.numeroVenta}`, "success")
            } else {
                swal("Lo sentimos!", "No se pudo registrar la venta", "error")
            }
        })

})