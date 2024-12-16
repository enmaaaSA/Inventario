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
    razon: "",
}

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
    if ($("#tbProducto tbody tr").filter(function () {
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
            <td>${data.categoria}</td>
            <td>${data.marca}</td>
            <td>${data.precioCompra}</td>
            <td>${data.precioVenta}</td>
            <td hidden>${data.estado}</td>
            <td>${data.stock}</td>
            <td><button class="btn btn-danger btn-sm" onclick="eliminarFila(this)">Eliminar</button></td>

        </tr>
    `;
    var fila2 = `
        <tr>
            <td id="txtId">${data.id}</td>
            <td id="txtCodigo">${data.codigo}</td>
            <td id="txtNombre">${data.text}</td>
            <td id="txtDescripcion" hidden>${data.descripcion}</td>
            <td>${data.categoria}</td>
            <td>${data.marca}</td>
            <td id="txtCategoria" hidden>${data.idCategoria}</td>
            <td id="txtMarca" hidden>${data.idMarca}</td>
            <td id="txtPrecioCompra">${data.precioCompra}</td>
            <td id="txtPrecioVenta">${data.precioVenta}</td>
            <td id="txtEstado" hidden>${data.estado}</td>
            <td><input style="width: 80%" type="number" id="txtStock"/></td>
            <td><button class="btn btn-danger btn-sm" onclick="eliminarFila(this)">Eliminar</button></td>

        </tr>
    `;

    // Añade la fila al cuerpo de la tabla
    $("#detalle1").append(fila);
    $("#detalle2").append(fila2);
    $("#cboBuscarProducto").val("").trigger("change").prop("disabled", true);
}
function eliminarFila() {
    $("#detalle1 tr").remove();
    $("#detalle2 tr").remove();
    $("#cboBuscarProducto").prop("disabled", false);
}


$("#btnMantenimiento0").click(function ()
{
    let modelo = {
        idProducto: $("#idProducto").val(),
        stock: parseInt($("#txtStock").val()),
        razon: $("#txtRazon").val()
    };
    console.log("Llego hasta aqui")
    fetch("/TomaInventario/StockTomaInventario", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(modelo)
    })
        .then(response => {
            console.log("Llego hasta aquix2")
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.estado) {
                swal("Listo!", "El producto fue modificado", "success")
            } else {
                swal("Lo sentimos", responseJson.mensaje, "error")
            }
        })
        .catch(error => {
            console.error("Error en el fetch:", error);
            swal("Error", "Ocurrió un problema con el servidor", "error");
        });
})

$("#btnMantenimiento1").click(function () {
    let idProducto = parseInt($("#txtId").text());
    let modelo = {
        idProducto: idProducto,
        stock: parseInt($("#txtStock").val()),
        razon: $("#txtRazon").val()
    };
    console.log("Modelo enviado al servidor:", modelo);
    fetch("/Producto/Toma", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(modelo)
    })
        .then(response => response.ok ? response.json() : Promise.reject(response))
        .then(responseJson => {
            if (responseJson.estado) {
                swal("¡Listo!", "El stock fue actualizado correctamente.", "success");
            } else {
                swal("Error", responseJson.mensaje, "error");
            }
        })
        .catch(error => {
            console.error("Error en el servidor:", error);
            swal("Error", "No se pudo actualizar el stock.", "error");
        });
});

$("#btnTest").click(function () {
    const modelo = structuredClone(modelBase);
    modelo["idProducto"] = parseInt($("#txtId").text())
    modelo["codigoBarra"] = $("#txtCodigo").text()
    modelo["nombre"] = $("#txtNombre").text()
    modelo["idMarca"] = parseInt($("#txtMarca").text())
    modelo["descripcion"] = $("#txtDescripcion").text()
    modelo["idCategoria"] = parseInt($("#txtCategoria").text())
    modelo["stock"] = parseInt($("#txtStock").val().trim())
    modelo["precioCompra"] = $("#txtPrecioCompra").text()
    modelo["precioVenta"] = $("#txtPrecioVenta").text()
    modelo["estado"] = parseInt($("#txtEstado").text())
    modelo["razon"] = $("#txtRazon").val()
    console.log(modelo)

})
$("#btnMantenimiento").click(function () {
    const modelo = structuredClone(modelBase);
    modelo["idProducto"] = parseInt($("#txtId").text())
    modelo["codigoBarra"] = $("#txtCodigo").text()
    modelo["nombre"] = $("#txtNombre").text()
    modelo["idMarca"] = parseInt($("#txtMarca").text())
    modelo["descripcion"] = $("#txtDescripcion").text()
    modelo["idCategoria"] = parseInt($("#txtCategoria").text())
    modelo["stock"] = parseInt($("#txtStock").val().trim())
    modelo["precioCompra"] = $("#txtPrecioCompra").text()
    modelo["precioVenta"] = $("#txtPrecioVenta").text()
    modelo["estado"] = parseInt($("#txtEstado").text())
    modelo["razon"] = $("#txtRazon").val()
    console.log(modelo)

    fetch("/Producto/Toma", {
        method: "PUT",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(modelo)
    }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            console.log(responseJson);
            if (responseJson.estado) {
                $("#detalle1 tr").remove();
                $("#detalle2 tr").remove();
                $("#cboBuscarProducto").prop("disabled", false);
                swal("Listo!", "La Producto fue modificada Stock", "success")
            } else {
                swal("Los sentimos", responseJson.mensaje, "error")
            }
        })
        .catch(error => {
            console.error("Error en el fetch:", error);
        });
})