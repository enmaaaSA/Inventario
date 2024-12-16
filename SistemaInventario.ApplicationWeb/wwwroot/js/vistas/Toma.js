$("#cboBuscarProducto").select2({
    ajax: {
        url: "/Compra/ObtenerProductos",
        dataType: 'json',
        delay: 250,
        data: function (params) {
            return { busqueda: params.term };
        },
        processResults: function (data) {
            return {
                results: data.map((item) => (
                    {
                        id: item.idProducto,
                        text: item.nombre,
                        marca: item.marca,
                        categoria: item.categoria,
                        codigo: item.codigoBarra,
                        descripcion: item.descripcion,
                        stock: item.stock,
                        precioCompra: item.precioCompra,
                        precioVenta: item.precioVenta,
                        estado: item.estado,
                        idMarca: item.idMarca,
                        idCategoria: item.idCategoria,
                    }
                ))
            };
        }
    },
    placeholder: 'Buscar Producto...',
    templateResult: formatoResultados,
});

function formatoResultados(data) {
    //esto es por defecto, ya que muestra el "buscando..."
    if (data.loading)
        return data.text;
    var contenedor = $(
        `<table width="100%">
            <tr>
                <td class="col-3" style="font-size: 0.7rem">ID
                    <p style="font-weight: bolder;margin:2px;font-size:1rem">${data.id}</p>
                </td>
                <td class="col-4" style="font-size: 0.7rem">Codigo de Barras
                    <p style="font-weight: bolder;margin:2px;font-size:1rem">${data.codigo}</p>
                </td>
                <td class="col-5" style="font-size: 0.7rem">Nombre
                    <p style="font-weight: bolder;margin:2px; font-size:1rem">${data.text}</p>
                </td>
            </tr>
         </table>`
    );
    //console.log(data.codigo);
    return contenedor;
}
$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();
})

$("#cboBuscarProducto").on("select2:select", function (e) {
    var data = e.params.data; // Obtiene los datos del producto seleccionado
    agregarProductoATabla(data); // Llama a una función para agregar a la tabla
});

function agregarProductoATabla(data) {
    // Verifica si el producto ya está en la tabla
    if ($(".tbProducto tbody tr").filter(function () {
        return $(this).find("td:eq(0)").text() == data.id;
    }).length > 0) {
        alert("El producto ya está en la tabla.");
        return;
    }

    // Crea una fila con los datos del producto
    var fila = `
        <tr>
            <td>${data.id}</td>
            <td>${data.codigo}</td>
            <td>${data.text}</td>
            <td class="txtStockAnterior" id="txtStockAnterior">${data.stock}</td>
            <td><input style="width: 80%" type="number" class="txtStock" id="txtStock"/></td>
            <td><button class="btn btn-danger btn-sm" onclick="eliminarFila(this)">Eliminar</button></td>

        </tr>
    `;

    // Añade la fila al cuerpo de la tabla
    $("#detalle1").append(fila);
}

$("#detalle1").on("input", ".txtStock", function () {
    let nuevoStock = $(this).val();
    if (nuevoStock < 0) {
        alert("El stock no puede ser negativo.");
        $(this).val(''); // Resetea el campo si el valor es inválido
    }
});
function agregarProductoATabla(data) {
    // Verifica si el producto ya está en la tabla
    if ($("#detalle1 tr[data-id='" + data.id + "']").length > 0) {
        alert("El producto ya está en la tabla.");
        return;
    }

    // Crea una fila con los datos del producto
    var fila = `
        <tr data-id="${data.id}">
            <td>${data.id}</td>
            <td>${data.codigo}</td>
            <td>${data.text}</td>
            <td class="txtStockAnterior">${data.stock}</td>
            <td><input style="width: 80%" type="number" class="txtStock" id="txtStock" min="0" required/></td>
            <td><button class="btn btn-danger btn-sm" onclick="eliminarFila(this)">Eliminar</button></td>
        </tr>
    `;

    // Añade la fila al cuerpo de la tabla
    $("#detalle1").append(fila);
}
$(document).on("click", ".btn-danger", function () {
    $(this).closest("tr").remove();
});

$("#btnMantenimiento").click(function (e) {
    e.preventDefault();

    let productos = [];

    // Recorremos las filas de la tabla 'detalle1'
    $("#detalle1 tr").each(function () {
        let fila = $(this);

        let producto = {
            idProducto: parseInt(fila.find("td:eq(0)").text()),
            stockAnterior: parseInt(fila.find(".txtStockAnterior").text()),
            stockNuevo: parseInt(fila.find(".txtStock").val() || 0),
            razon: $("#txtRazon").val(),
            documentoEncargado: $("#DocumentoPersonaDesignada").val(),
            numeroDocumentoEncargado: $("#NumeroDocumentoPersonaDesignada").val(),
            nombreEncargado: $("#PersonaDesignada").val(),
            fechaToma: $("#FechaRealizada").val()
        };

        productos.push(producto);
    });

    if (productos.length === 0) {
        alert("No hay productos para registrar.");
        return;
    }

    // Validar campos vacíos
    if (!$("#txtRazon").val()) {
        alert("Debes proporcionar una razón para la modificación.");
        return;
    }

    // Enviar datos al controlador
    $.ajax({
        url: "/TomaInventario/Create",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(productos),
        success: function (response) {
            if (response.estado) {
                alert("Toma de inventario registrada correctamente.");
                location.reload();
            } else {
                alert(response.mensaje);
            }
        },
        error: function (error) {
            console.error("Error:", error);
            alert("Ocurrió un error al registrar la toma de inventario.");
        }
    });
});
