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
            { "data": "numeroDocumento", "visible": false, "searchable": false },
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
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [{
            text: 'Exportar Excel',
            extend: 'excelHtml5',
            title: '',
            filename: 'Reporte',
            exportOptions: {
                columns: [0,1,2,3,4,5,6,7]
            }
        }, 'pageLength'
        ],
        lenguage: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });

    //FILTRO ACTIVO VISTA
    tablaData.on('xhr', function () {
        // Una vez que se carguen los datos, filtrar la columna de estado
        tablaData.column(7).search("^Activo$", true, false).draw();
    });
})
function mostrarModal(modelo = modelBase) {
    $("#txtId").val(modelo.idUsuario)
    $("#txtNombre").val(modelo.nombre)
    $("#txtApellido").val(modelo.apellido)
    $("#cboDocumento").val(modelo.documento)
    $("#txtNumeroDocumento").val(modelo.numeroDocumento)
    $("#txtCorreo").val(modelo.correo)
    $("#cboRol").val(modelo.idRol == 0 ? $("#cboRol option:first").val() : modelo.idRol)
    $("#modalData").modal("show")
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
    if (!/^[a-zA-Z\s]+$/.test(nombre)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtNombre").focus();
        return;
    }

    const last = $("#txtApellido").val();
    if (!/^[a-zA-Z\s]+$/.test(last)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtApellido").focus();
        return;
    }

    const correo = $("#txtCorreo").val();
    if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(correo)) {
        toastr.warning("", 'Por favor ingresa un correo electrónico válido.');
        $("#txtCorreo").focus();
        return;
    }

    const telefono = $("#txtNumeroDocumento").val().trim();
    const regexPeru = /^(9\d{8}|(\d{2})\d{7})$/;
    if (!regexPeru.test(telefono)) {
        toastr.warning("", 'El número de teléfono no es válido. Referencia, Peru');
        $("#txtNumeroDocumento").focus();
        return;
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

    if (modelo.idUsuario == 0) {

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
    } else {
        fetch("/Usuario/Editar", {
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
                    swal("Listo!", "El usuario fue modificado", "success")
                } else {
                    swal("Los sentimos", responseJson.mensaje, "error")
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

$("#tbdata tbody").on("click", ".btn-eliminar", function () {

    let fila;
    if ($(this).closest("tr").hasClass("child")) {
        fila = $(this).closest("tr").prev();
    } else {
        fila = $(this).closest("tr");
    }

    const data = tablaData.row(fila).data();

    swal({
        title: "¿Está seguro?",
        text: `Eliminar al usuario "${data.nombre}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (respuesta) {

            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show");

                fetch(`/Usuario/Eliminar?IdUsuario=${data.idUsuario}`, {
                    method: "DELETE"
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            tablaData.row(fila).remove().draw()

                            swal("Listo!", "El usuario fue eliminado", "success")
                        } else {
                            swal("Los sentimos", responseJson.mensaje, "error")
                        }
                    })


            }
        }
    )


})