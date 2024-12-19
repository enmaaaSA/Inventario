const modelBase = {
    idProducto: 0,
    idUsuario: 0,
    codigoBarra: "",
    nombre: "",
    descripcion: "",
    idMarca: 0,
    idCategoria: 0,
    stock: 0,
    precioCompra: 0,
    precioVenta: 0,
    estado: 1,
}

const modelBase1 = {
    idProducto: 0,
    idUsuario: 0,
    codigoBarra: "",
    nombre: "",
    descripcion: "",
    idMarca: 0,
    idCategoria: 0,
    stock: 0,
    precioCompra: 0,
    precioVenta: 0,
    estado: 1,
}

const modelBase2 = {
    idProducto: 0,
    idUsuario: 0,
    codigoBarra: "",
    nombre: "",
    descripcion: "",
    idMarca: 0,
    idCategoria: 0,
    stock: 0,
    precioCompra: 0,
    precioVenta: 0,
    estado: 1,
}

let tablaData;

$(document).ready(function () {

    fetch("/Categoria/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                const categoriasActivas = responseJson.data.filter(item => item.estado === 1);
                if (categoriasActivas.length > 0) {
                    categoriasActivas.forEach((item) => {
                        $("#cboCategoria").append(
                            $("<option>").val(item.idCategoria).text(item.nombre)
                        );
                    });
                }
            }
        })
    fetch("/Categoria/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                const categoriasActivas = responseJson.data.filter(item => item.estado === 1);
                if (categoriasActivas.length > 0) {
                    categoriasActivas.forEach((item) => {
                        $("#cboCategoria1").append(
                            $("<option>").val(item.idCategoria).text(item.nombre)
                        );
                    });
                }
            }
        })
    fetch("/Categoria/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                const categoriasActivas = responseJson.data.filter(item => item.estado === 1);
                if (categoriasActivas.length > 0) {
                    categoriasActivas.forEach((item) => {
                        $("#cboCategoria2").append(
                            $("<option>").val(item.idCategoria).text(item.nombre)
                        );
                    });
                }
            }
        })

    fetch("/Marca/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                const marcasActivas = responseJson.data.filter(item => item.estado === 1);
                if (marcasActivas.length > 0) {
                    marcasActivas.forEach((item) => {
                        $("#cboMarca").append(
                            $("<option>").val(item.idMarca).text(item.nombre)
                        );
                    });
                }
            }
        })
    fetch("/Marca/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                const marcasActivas = responseJson.data.filter(item => item.estado === 1);
                if (marcasActivas.length > 0) {
                    marcasActivas.forEach((item) => {
                        $("#cboMarca1").append(
                            $("<option>").val(item.idMarca).text(item.nombre)
                        );
                    });
                }
            }
        })
    fetch("/Marca/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                const marcasActivas = responseJson.data.filter(item => item.estado === 1);
                if (marcasActivas.length > 0) {
                    marcasActivas.forEach((item) => {
                        $("#cboMarca2").append(
                            $("<option>").val(item.idMarca).text(item.nombre)
                        );
                    });
                }
            }
        })


    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Producto/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idProducto", "visible": false, "searchable": false },
            { "data": "codigoBarra" },
            { "data": "marca" },
            { "data": "nombre" },
            { "data": "categoria" },
            { "data": "stock" },
            { "data": "precioCompra" },
            { "data": "precioVenta" },
            {
                "data": "estado", render: function (data) {
                    return data == 1
                        ? '<span class="badge badge-info">Activo</span>'
                        : '<span class="badge badge-danger">Inactivo</span>';
                
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-edit"></i></button>' +
                    '<button class="btn btn-info btn-eliminar btn-sm"><i class="fas fa-info px-1"></i></button>',
                "orderable": false,
                "searchable": false,
            }
        ],
        order: [[0, "desc"]],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
    //FILTRO ACTIVO VISTA
    //tablaData.on('xhr', function () {
        // Una vez que se carguen los datos, filtrar la columna de estado
        //tablaData.column(8).search("^Activo$", true, false).draw();
    //});

})


function mostrarModal(modelo = modelBase) {
    $("#txtId").val(modelo.idProducto)
    $("#txtNombre").val(modelo.nombre)
    $("#txtCodigoBarra").val(modelo.codigoBarra)
    $("#cboMarca").val(modelo.idMarca == 0 ? $("#cboMarca option:first").val() : modelo.idMarca)
    $("#txtDescripcion").val(modelo.descripcion)
    $("#cboCategoria").val(modelo.idCategoria == 0 ? $("#cboCategoria option:first").val() : modelo.idCategoria)
    $("#txtStock").val(modelo.stock)
    $("#txtPrecioCompra").val(modelo.precioCompra)
    $("#txtPrecio").val(modelo.precioVenta)
    $("#modalData").modal("show")
}

function mostrarModal1(modelo = modelBase1) {
    $("#txtId1").val(modelo.idProducto)
    $("#txtNombre1").val(modelo.nombre)
    $("#txtCodigoBarra1").val(modelo.codigoBarra)
    $("#cboMarca1").val(modelo.idMarca == 0 ? $("#cboMarca1 option:first").val() : modelo.idMarca)
    $("#txtDescripcion1").val(modelo.descripcion)
    $("#cboCategoria1").val(modelo.idCategoria == 0 ? $("#cboCategoria1 option:first").val() : modelo.idCategoria)
    $("#txtStock1").val(modelo.stock)
    $("#txtPrecioCompra1").val(modelo.precioCompra)
    $("#txtPrecio1").val(modelo.precioVenta)
    $("#cboEstado1").val(modelo.estado)
    $("#modalData1").modal("show")
}

function mostrarModal2(modelo = modelBase2) {
    $("#txtId2").val(modelo.idProducto)
    $("#txtNombre2").val(modelo.nombre)
    $("#txtCodigoBarra2").val(modelo.codigoBarra)
    $("#cboMarca2").val(modelo.idMarca == 0 ? $("#cboMarca2 option:first").val() : modelo.idMarca)
    $("#txtDescripcion2").val(modelo.descripcion)
    $("#cboCategoria2").val(modelo.idCategoria == 0 ? $("#cboCategoria2 option:first").val() : modelo.idCategoria)
    $("#txtStock2").val(modelo.stock)
    $("#txtPrecioCompra2").val(modelo.precioCompra)
    $("#txtPrecio2").val(modelo.precioVenta)
    $("#cboEstado2").val(modelo.estado)
    $("#modalData2").modal("show")
}

$("#btnNuevo").click(function () {
    mostrarModal();
})

$("#btnGuardar").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    const codigoBarra = $("#txtCodigoBarra").val();
    if (!/^\d+$/.test(codigoBarra)) {
        toastr.warning("", 'El codigo de Barra solo debe contener números.');
        $("#txtCodigoBarra").focus();
        return;
    }

    const precio = $("#txtPrecio").val();
    if (!/^\d+(\.\d+)?$/.test(precio)) {
        toastr.warning("", 'El costo debe ser un número válido.');
        $("#txtPrecioCompra").focus();
        return;
    }

    const preciocompra = $("#txtPrecioCompra").val();
    if (!/^\d+(\.\d+)?$/.test(preciocompra)) {
        toastr.warning("", 'El precio debe ser un número válido.');
        $("#txtPrecioCompra").focus();
        return;
    }

    const modelo = structuredClone(modelBase);
    modelo["idProducto"] = parseInt($("#txtId").val())
    modelo["codigoBarra"] = $("#txtCodigoBarra").val()
    modelo["nombre"] = $("#txtNombre").val()
    modelo["idMarca"] = $("#cboMarca").val()
    modelo["descripcion"] = $("#txtDescripcion").val()
    modelo["idCategoria"] = $("#cboCategoria").val()
    modelo["stock"] = $("#txtStock").val()
    modelo["precioCompra"] = $("#txtPrecioCompra").val()
    modelo["precioVenta"] = $("#txtPrecio").val()

    const formData = new FormData();


    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

        fetch("/Producto/Crear", {
            method: "POST",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {
                    tablaData.row.add(responseJson.objeto).draw(false);
                    $("#modalData").modal("hide");
                    swal("Listo!", "El producto fue creado", "success");
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
})

$("#btnEditar").click(function () {

    const inputs = $("input.input-validar1").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    const codigoBarra = $("#txtCodigoBarra1").val();
    if (!/^\d+$/.test(codigoBarra)) {
        toastr.warning("", 'El codigo de Barra solo debe contener números.');
        $("#txtCodigoBarra1").focus();
        return;
    }

    const precio = $("#txtPrecio1").val();
    if (!/^\d+(\.\d+)?$/.test(precio)) {
        toastr.warning("", 'El costo debe ser un número válido.');
        $("#txtPrecioCompra1").focus();
        return;
    }

    const preciocompra = $("#txtPrecioCompra1").val();
    if (!/^\d+(\.\d+)?$/.test(preciocompra)) {
        toastr.warning("", 'El precio debe ser un número válido.');
        $("#txtPrecioCompra1").focus();
        return;
    }

    const modelo = structuredClone(modelBase1);
    modelo["idProducto"] = parseInt($("#txtId1").val())
    modelo["codigoBarra"] = $("#txtCodigoBarra1").val()
    modelo["nombre"] = $("#txtNombre1").val()
    modelo["idMarca"] = $("#cboMarca1").val()
    modelo["descripcion"] = $("#txtDescripcion1").val()
    modelo["idCategoria"] = $("#cboCategoria1").val()
    modelo["stock"] = $("#txtStock1").val()
    modelo["precioCompra"] = $("#txtPrecioCompra1").val()
    modelo["precioVenta"] = $("#txtPrecio1").val()
    modelo["estado"] = $("#cboEstado1").val()

    const formData = new FormData();

    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData1").find("div.modal-content").LoadingOverlay("show");

        fetch("/Producto/Editar", {
            method: "PUT",
            body: formData
        })
            .then(response => {
                $("#modalData1").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {
                    $("#modalData1").modal("hide");
                    swal("Listo!", "El producto fue modificado", "success");
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
})

let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }
    const data = tablaData.row(filaSeleccionada).data();
    mostrarModal1(data);
});

let filaSeleccionada1;
$("#tbdata tbody").on("click", ".btn-eliminar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada1 = $(this).closest("tr").prev();
    } else {
        filaSeleccionada1 = $(this).closest("tr");
    }
    const data = tablaData.row(filaSeleccionada1).data();
    mostrarModal2(data);
})