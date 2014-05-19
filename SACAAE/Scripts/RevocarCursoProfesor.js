$(document).ready(function() {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltCursosImpartidos").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltCursosImpartidos = "<option>Seleccione Profesor</option>";


    $("#sltCursosImpartidos").html(itemsltCursosImpartidos);


    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltProfesor").change(function () {

        var route = "/CursoProfesor/Profesor/Cursos/" + $('select[name="sltProfesor"]').val();
        //alert(route);

        $.getJSON(route, function (data) {
            var items = "";
            for (var i = 0; i < data.length; i++) {
                items += "<option value=" + data[i]["Id"] + ">"+data[i]["Codigo"]+" - "+data[i]["Nombre"]+"</option>"
            }

            if (items != "") {
                $("#sltCursosImpartidos").html(items);
                $("#sltCursosImpartidos").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");
                $("#sltCursosImpartidos").prop("disabled", false);
                $("#Revocar").prop("disabled", false);
            }
            else {
                $("#sltCursosImpartidos").html("<option>No hay cursos impartidos por ese profesor.</option>");
            }
        });
    });
});