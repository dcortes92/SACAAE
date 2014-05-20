$(document).ready(function() {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltProyectos").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltProyectos = "<option>Seleccione Profesor</option>";


    $("#sltProyectos").html(itemsltProyectos);


    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltProfesor").change(function () {

        var route = "/ProfesoresProyectos/Profesor/Proyectos/" + $('select[name="sltProfesor"]').val();
        //alert(route);

        $.getJSON(route, function (data) {
            var items = "";
            for (var i = 0; i < data.length; i++) {
                items += "<option value=" + data[i]["ID"] + ">"+data[i]["Nombre"]+"</option>"
            }

            if (items != "") {
                $("#sltProyectos").html(items);
                $("#sltProyectos").prepend("<option value='' selected='selected'>-- Seleccionar Comisi&oacute;n --</option>");
                $("#sltProyectos").prop("disabled", false);
                $("#Revocar").prop("disabled", false);
            }
            else {
                $("#sltProyectos").html("<option>No hay comisiones a las que pertenezca el profesor.</option>");
            }
        });
    });
});