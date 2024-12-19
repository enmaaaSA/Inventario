const modelBase = {
    idUsuario: 0,
    nombre: "",
    apellido: "",
    documento: "",
    numeroDocumento: "",
    correo: "",
    idRol: 0,
    estado: 1
}

const modelBase1 = {
    idUsuario: 0,
    nombre: "",
    apellido: "",
    documento: "",
    numeroDocumento: "",
    correo: "",
    idRol: 0,
    estado: 1
}

const modelBase2 = {
    idUsuario: 0,
    nombre: "",
    apellido: "",
    documento: "",
    numeroDocumento: "",
    correo: "",
    idRol: 0,
    estado: 1
}

let tablaData;
$(document).ready(function () {
    fetch("/Usuario/ListaRol")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboRol").append(
                        $("<option>").val(item.idRol).text(item.nombre)
                    )
                })
            }
        })
    fetch("/Usuario/ListaRol")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboRol1").append(
                        $("<option>").val(item.idRol).text(item.nombre)
                    )
                })
            }
        })
    fetch("/Usuario/ListaRol")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboRol2").append(
                        $("<option>").val(item.idRol).text(item.nombre)
                    )
                })
            }
        })
    tablaData = $('#tbdata').DataTable({ //nombre de tabla
        responsive: true,
        "ajax": {
            "url": '/Usuario/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idUsuario", "visible": false, "searchable": false },
            { "data": "nombre" },
            { "data": "apellido" },
            { "data": "documento", "visible": false, "searchable": false },
            { "data": "numeroDocumento"},
            { "data": "correo" },
            { "data": "rol" },
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
        lenguage: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
    
    //FILTRO ACTIVO VISTA
    //tablaData.on('xhr', function () {
        // Una vez que se carguen los datos, filtrar la columna de estado
        //tablaData.column(7).search("^Activo$", true, false).draw();
    //});
})
function mostrarModal(modelo = modelBase) {
    $("#txtId").val(modelo.idUsuario)
    $("#txtNombre").val(modelo.nombre)
    $("#txtApellido").val(modelo.apellido)
    $("#cboDocumento").val(modelo.documento == 0 ? $("#cboDocumento option:first").val() : modelo.documento)
    $("#txtNumeroDocumento").val(modelo.numeroDocumento)
    $("#txtCorreo").val(modelo.correo)
    $("#cboRol").val(modelo.idRol == 0 ? $("#cboRol option:first").val() : modelo.idRol)
    $("#modalData").modal("show")
}

function mostrarModal1(modelo = modelBase1) {
    $("#txtId1").val(modelo.idUsuario)
    $("#txtNombre1").val(modelo.nombre)
    $("#txtApellido1").val(modelo.apellido)
    $("#cboDocumento1").val(modelo.documento == 0 ? $("#cboDocumento1 option:first").val() : modelo.documento)
    $("#txtNumeroDocumento1").val(modelo.numeroDocumento)
    $("#txtCorreo1").val(modelo.correo)
    $("#cboRol1").val(modelo.idRol == 0 ? $("#cboRol1 option:first").val() : modelo.idRol)
    $("#cboEstado1").val(modelo.estado)
    $("#modalData1").modal("show")
}

function mostrarModal2(modelo = modelBase2) {
    $("#txtId2").val(modelo.idUsuario)
    $("#txtNombre2").val(modelo.nombre)
    $("#txtApellido2").val(modelo.apellido)
    $("#cboDocumento2").val(modelo.documento == 0 ? $("#cboDocumento2 option:first").val() : modelo.documento)
    $("#txtNumeroDocumento2").val(modelo.numeroDocumento)
    $("#txtCorreo2").val(modelo.correo)
    $("#cboRol2").val(modelo.idRol == 0 ? $("#cboRol2 option:first").val() : modelo.idRol)
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

    const nombre = $("#txtNombre").val();
    const last = $("#txtApellido").val();
    const number = $("#txtNumeroDocumento").val();
    const correo = $("#txtCorreo").val();

    if (!/^[a-zA-Z\s]+$/.test(nombre)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtNombre").focus();
        return;
    }

    if (!/^[a-zA-Z\s]+$/.test(last)) {
        toastr.warning("", 'El apellido solo debe contener letras.');
        $("#txtApellido").focus();
        return;
    }

    if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(correo)) {
        toastr.warning("", 'Por favor ingresa un correo electrónico válido.');
        $("#txtCorreo").focus();
        return;
    }

    if (!/^\d{8,12}$/.test(number)) {
        toastr.warning("", 'El número debe contener entre 8 y 12 dígitos y solo números.');
        $("#txtNumeroDocumento").focus(); // Colocar el cursor en el campo
        return; // Detener la ejecución
    }

    const modelo = structuredClone(modelBase);
    modelo["idUsuario"] = parseInt($("#txtId").val())
    modelo["nombre"] = $("#txtNombre").val()
    modelo["apellido"] = $("#txtApellido").val()
    modelo["documento"] = $("#cboDocumento").val()
    modelo["numeroDocumento"] = $("#txtNumeroDocumento").val()
    modelo["correo"] = $("#txtCorreo").val()
    modelo["idRol"] = $("#cboRol").val()

    const formData = new FormData();
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    fetch("/Usuario/Crear", {
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
                swal("Listo!", "El usuario fue creado", "success")
            } else {
                swal("Los sentimos", responseJson.mensaje, "error")
            }
        })
})

$("#btnEditar").click(function () {
    const modelo = structuredClone(modelBase1);
    modelo["idUsuario"] = parseInt($("#txtId1").val())
    modelo["nombre"] = $("#txtNombre1").val()
    modelo["apellido"] = $("#txtApellido1").val()
    modelo["documento"] = $("#cboDocumento1").val()
    modelo["numeroDocumento"] = $("#txtNumeroDocumento1").val()
    modelo["correo"] = $("#txtCorreo1").val()
    modelo["idRol"] = $("#cboRol1").val()
    modelo["estado"] = $("#cboEstado1").val()

    const formData = new FormData();
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData1").find("div.modal-content").LoadingOverlay("show");

    fetch("/Usuario/Editar", {
        method: "PUT",
        body: formData
    })
        .then(response => {
            $("#modalData1").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.estado) {
                tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                filaSeleccionada = null;
                $("#modalData1").modal("hide")
                swal("Listo!", "El usuario fue modificado", "success")
            } else {
                swal("Los sentimos", responseJson.mensaje, "error")
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