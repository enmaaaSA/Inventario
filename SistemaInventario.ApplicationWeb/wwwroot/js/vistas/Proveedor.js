const modelBase = {
    idProveedor: 0,
    tipoDocumento: "",
    numeroDocumento: "",
    nombre: "",
    correo: "",
    direccion: "",
    idUsuario: 0,
    estado: 1
}
const modelBase1 = {
    idProveedor: 0,
    tipoDocumento: "",
    numeroDocumento: "",
    nombre: "",
    correo: "",
    direccion: "",
    idUsuario: 0,
    estado: 1
}
const modelBase2 = {
    idProveedor: 0,
    tipoDocumento: "",
    numeroDocumento: "",
    nombre: "",
    correo: "",
    direccion: "",
    idUsuario: 0,
    estado: 1
}

let tablaData;

$(document).ready(function () {
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Proveedor/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idProveedor", "visible": false, "searchable": false },
            { "data": "tipoDocumento" },
            { "data": "numeroDocumento" },
            { "data": "nombre" },
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
    })
})

function mostrarModal(modelo = modelBase) {
    $("#txtId").val(modelo.idProveedor)
    $("#cboTipoDocumento").val(modelo.tipoDocumento == 0 ? $("#cboTipoDocumento option:first").val() : modelo.tipoDocumento)
    $("#txtNumeroDocumento").val(modelo.numeroDocumento)
    $("#txtNombre").val(modelo.nombre)
    $("#txtCorreo").val(modelo.correo)
    $("#txtDireccion").val(modelo.direccion)
    $("#modalData").modal("show")
}

function mostrarModal1(modelo = modelBase1) {
    $("#txtId1").val(modelo.idProveedor)
    $("#cboTipoDocumento1").val(modelo.tipoDocumento)
    $("#txtNumeroDocumento1").val(modelo.numeroDocumento)
    $("#txtNombre1").val(modelo.nombre)
    $("#txtCorreo1").val(modelo.correo)
    $("#txtDireccion1").val(modelo.direccion)
    $("#cboEstado1").val(modelo.estado)
    $("#modalData1").modal("show")
}

function mostrarModal2(modelo = modelBase2) {
    $("#txtId2").val(modelo.idProveedor)
    $("#cboTipoDocumento2").val(modelo.tipoDocumento)
    $("#txtNumeroDocumento2").val(modelo.numeroDocumento)
    $("#txtNombre2").val(modelo.nombre)
    $("#txtCorreo2").val(modelo.correo)
    $("#txtDireccion2").val(modelo.direccion)
    $("#cboEstado2").val(modelo.estado)
    $("#modalData2").modal("show")
}

$("#btnNuevo").click(function () {
    mostrarModal()
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

    const number = $("#txtNumeroDocumento").val();
    if (!/^\d{8,16}$/.test(number)) {
        toastr.warning("", 'El número debe contener entre 8 y 16 dígitos y solo números.');
        $("#txtNumeroDocumento").focus(); // Colocar el cursor en el campo
        return; // Detener la ejecución
    }
    const correo = $("#txtCorreo").val();
    if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(correo)) {
        toastr.warning("", 'Por favor ingresa un correo electrónico válido.');
        $("#txtCorreo").focus();
        return;
    }

    const modelo = structuredClone(modelBase);
    modelo["idProveedor"] = parseInt($("#txtId").val())
    modelo["tipoDocumento"] = $("#cboTipoDocumento").val()
    modelo["numeroDocumento"] = $("#txtNumeroDocumento").val()
    modelo["nombre"] = $("#txtNombre").val()
    modelo["direccion"] = $("#txtDireccion").val()
    modelo["correo"] = $("#txtCorreo").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

        fetch("/Proveedor/Crear", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {

                    tablaData.row.add(responseJson.objeto).draw(false);
                    $("#modalData").modal("hide");
                    swal("Listo!", "La proveedor fue creado", "success");
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                } else {
                    swal("Los sentimos", responseJson.mensaje, "error")
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

    const numberdocument = $("#txtNumeroDocumento1").val();
    if (!/^\d+$/.test(numberdocument)) {
        toastr.warning("", 'El número de Documento solo debe contener números.');
        $("#txtNumeroDocumento1").focus();
        return;
    }
    const correo = $("#txtCorreo1").val();
    if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(correo)) {
        toastr.warning("", 'Por favor ingresa un correo electrónico válido.');
        $("#txtCorreo1").focus();
        return;
    }

    const modelo = structuredClone(modelBase1);
    modelo["idProveedor"] = parseInt($("#txtId1").val())
    modelo["tipoDocumento"] = $("#cboTipoDocumento1").val()
    modelo["numeroDocumento"] = $("#txtNumeroDocumento1").val()
    modelo["nombre"] = $("#txtNombre1").val()
    modelo["direccion"] = $("#txtDireccion1").val()
    modelo["correo"] = $("#txtCorreo1").val()
    modelo["estado"] = $("#cboEstado1").val()

    $("#modalData1").find("div.modal-content").LoadingOverlay("show");
        fetch("/Proveedor/Editar", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData1").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {
                    $("#modalData1").modal("hide");
                    swal("Listo!", "El proveedor fue editado", "success");
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