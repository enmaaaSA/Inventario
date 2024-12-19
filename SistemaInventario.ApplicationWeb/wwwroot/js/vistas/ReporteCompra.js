let tablaData;
$(document).ready(function () {

    $.datepicker.setDefaults($.datepicker.regional["es"])

    $("#txtFechaInicio").datepicker({ dateFormat: "dd/mm/yy" })
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy" })

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Reporte/ReporteCompra?fechaInicio=01/01/1991&fechaFin=01/01/1991',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "fecha" },
            { "data": "numeroCompra" },
            { "data": "subTotalCompra" },
            { "data": "impuestoTotalCompra" },
            { "data": "totalCompra" },
        ],
        order: [[0, "desc"]],
    });


})

$("#btnBuscar").click(function () {

    if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") {
        toastr.warning("", "Debe ingresar fecha inicio y fin")
        return;
    }

    let fechaInicio = $("#txtFechaInicio").val().trim();
    let fechaFin = $("#txtFechaFin").val().trim();

    let nueva_url = `/Reporte/ReporteCompra?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;

    tablaData.ajax.url(nueva_url).load();


})