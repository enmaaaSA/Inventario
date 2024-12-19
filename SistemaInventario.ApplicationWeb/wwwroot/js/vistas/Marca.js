const modelBase = {
    idMarca: 0,
    nombre: "",
    idUsuario: 0,
    estado: 1,
}

const modelBase1 = {
    idMarca: 0,
    nombre: "",
    idUsuario: 0,
    estado: 1,
}

const modelBase2 = {
    idMarca: 0,
    nombre: "",
    idUsuario: 0,
    estado: 1,
}

let tablaData;

$(document).ready(function () {
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Marca/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idMarca", "visible": false, "searchable": false },
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
    });
})
function mostrarModal(modelo = modelBase) {
    $("#txtId").val(modelo.idMarca)
    $("#txtDescripcion").val(modelo.nombre)
    $("#modalData").modal("show")
}

function mostrarModal1(modelo = modelBase1) {
    $("#txtId1").val(modelo.idMarca)
    $("#txtDescripcion1").val(modelo.nombre)
    $("#cboEstado1").val(modelo.estado)
    $("#modalData1").modal("show")
}
function mostrarModal2(modelo = modelBase2) {
    $("#txtId2").val(modelo.idMarca)
    $("#txtDescripcion2").val(modelo.nombre)
    $("#cboEstado2").val(modelo.estado)
    $("#modalData2").modal("show")
}

$("#btnNuevo").click(function () {
    mostrarModal()
})


$("#btnGuardar").click(function () {

    if ($("#txtDescripcion").val().trim() == "") {
        toastr.warning("", "Debe completa el campo : Nombre")
        $("#txtDescripcion").focus()
        return;
    }

    const nombre = $("#txtDescripcion").val();
    if (!/^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/.test(nombre)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtDescripcion").focus();
        return;
    }

    const modelo = structuredClone(modelBase);
    modelo["idMarca"] = parseInt($("#txtId").val())
    modelo["nombre"] = $("#txtDescripcion").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");
    fetch("/Marca/Crear", {
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

                    tablaData.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo!", "La marca fue creada", "success");
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                } else {
                    swal("Los sentimos", responseJson.mensaje, "error")
                }
            })
})

$("#btnEditar").click(function () {

    if ($("#txtDescripcion1").val().trim() == "") {
        toastr.warning("", "Debe completa el campo : Nombre")
        $("#txtDescripcion1").focus()
        return;
    }

    const nombre = $("#txtDescripcion1").val();
    if (!/^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/.test(nombre)) {
        toastr.warning("", 'El nombre solo debe contener letras.');
        $("#txtDescripcion1").focus();
        return;
    }

    const modelo = structuredClone(modelBase1);
    modelo["idMarca"] = parseInt($("#txtId1").val())
    modelo["nombre"] = $("#txtDescripcion1").val()
    modelo["estado"] = $("#cboEstado1").val()

    $("#modalData1").find("div.modal-content").LoadingOverlay("show");
    fetch("/Marca/Editar", {
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

                tablaData.row.add(responseJson.objeto).draw(false);
                $("#modalData1").modal("hide");
                swal("Listo!", "La marca fue editada", "success");
                setTimeout(() => {
                    location.reload();
                }, 1000);
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
