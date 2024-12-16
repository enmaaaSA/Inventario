const modelBase = {
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
                filename: 'Reporte Categorias',
                exportOptions: {
                    columns: [1, 2]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})
function mostrarModal(modelo = modelBase) {
    $("#txtId").val(modelo.idMarca)
    $("#txtDescripcion").val(modelo.nombre)
    //$("#cboEstado").val(modelo.esActivo)

    $("#modalData").modal("show")
}

$("#btnNuevo").click(function () {
    mostrarModal()
})


$("#btnGuardar").click(function () {


    if ($("#txtDescripcion").val().trim() == "") {
        toastr.warning("", "Debe completa el campo : descripcion")
        $("#txtDescripcion").focus()
        return;
    }


    const modelo = structuredClone(modelBase);
    modelo["idMarca"] = parseInt($("#txtId").val())
    modelo["nombre"] = $("#txtDescripcion").val()
    modelo["estado"] = $("#cboEstado").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idMarca == 0) {

        fetch("/Marca/Crear", {
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
    } else {
        fetch("/Marca/Editar", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
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
                    swal("Listo!", "La marca fue modificada", "success")
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
