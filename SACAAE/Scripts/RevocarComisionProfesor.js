$(document).ready(function() {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltComisiones").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltComisiones = "<option>Seleccione Profesor</option>";


    $("#sltComisiones").html(itemsltComisiones);


    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltProfesor").change(function () {

        var route = "/ComisionProfesor/Profesor/Comisiones/" + $('select[name="sltProfesor"]').val();
        //alert(route);

        $.getJSON(route, function (data) {
            var items = "";
            for (var i = 0; i < data.length; i++) {
                items += "<option value=" + data[i]["ID"] + ">"+data[i]["Nombre"]+"</option>"
            }

            if (items != "") {
                $("#sltComisiones").html(items);
                $("#sltComisiones").prepend("<option value='' selected='selected'>-- Seleccionar Comisi&oacute;n --</option>");
                $("#sltComisiones").prop("disabled", false);
                $("#Revocar").prop("disabled", false);
            }
            else {
                $("#sltComisiones").html("<option>No hay comisiones a las que pertenezca el profesor.</option>");
            }
        });
    });
});