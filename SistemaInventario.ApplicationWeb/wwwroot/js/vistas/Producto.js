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

let tablaData;

$(document).ready(function () {

    fetch("/Categoria/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log(responseJson)
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

    fetch("/Marca/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log(responseJson)
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
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Productos',
                exportOptions: {
                    columns: [2, 3, 4, 5, 6, 7]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
    //FILTRO ACTIVO VISTA
    tablaData.on('xhr', function () {
        // Una vez que se carguen los datos, filtrar la columna de estado
        tablaData.column(8).search("^Activo$", true, false).draw();
    });

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
    $("#cboEstado").val(modelo.estado)

    $("#modalData").modal("show")
    console.log(modelo)
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

    const nombre = $("#txtNombre").val();
    if (!/^[a-zA-Z\s]+$/.test(nombre)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtNombre").focus();
        return;
    }

    const descripcion = $("#txtDescripcion").val();
    if (!/^[a-zA-Z\s]+$/.test(descripcion)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtDescripcion").focus();
        return;
    }

    const codigoBarra = $("#txtCodigoBarra").val();
    if (!/^\d+$/.test(codigoBarra)) {
        toastr.warning("", 'El codigo de Barra solo debe contener números.');
        $("#txtCodigoBarra").focus();
        return;
    }

    const precio = $("#txtPrecio").val();
    if (!/^\d+$/.test(precio)) {
        toastr.warning("", 'El precio solo debe contener números.');
        $("#txtPrecio").focus();
        return;
    }

    const preciocompra = $("#txtPrecioCompra").val();
    if (!/^\d+$/.test(preciocompra)) {
        toastr.warning("", 'El precio solo debe contener números.');
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
    modelo["estado"] = $("#cboEstado").val()

    const formData = new FormData();


    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idProducto == 0) {

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
                    tablaData.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo!", "El producto fue creado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {
        fetch("/Producto/Editar", {
            method: "PUT",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {
                    tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                    filaSeleccionada = null;
                    $("#modalData").modal("hide")
                    swal("Listo!", "El producto fue modificado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    }


})


let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();

    mostrarModal(data);

})