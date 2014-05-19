$(document).ready(function() {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltPlan").prop("disabled", "disabled");
    $("#sltCurso").prop("disabled", "disabled");
    $("#sltGrupo").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemSltPlan = "<option>Seleccione Sede y Modalidad</option>";
    itemSltCurso = "<option>Seleccione Sede, Modalidad y Plan de Estudio</option>";
    itemSltGrupo = "<option>Seleccione Sede, Modalidad, Plan de Estudio y Curso</option>";

    $("#sltPlan").html(itemSltPlan);
    $("#sltCurso").html(itemSltCurso);
    $("#sltGrupo").html(itemSltGrupo);

    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltModalidad, #sltSede").change(function () {

        var route = "/CursoProfesor/Planes/List/" + $('select[name="sltSede"]').val() + "/" + $('select[name="sltModalidad"]').val();
        //alert(route);

        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {

                items += "<option value='" + plan.Value + "'>" + plan.Text + "</option>";
            });

            if (items != "") {
                $("#sltPlan").html(items);
                $("#sltPlan").prepend("<option value='' selected='selected'>-- Seleccionar Plan --</option>");
                $("#sltPlan").prop("disabled", false);
            }
            else {
                $("#sltPlan").html("<option>No hay planes para esa sede y modalidad</option>");
            }
        });
    });

    $("#sltPlan").change(function () {
        var route = "/CursoProfesor/Cursos/List/" + $('select[name="sltPlan"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            /*$.each(data, function (i, curso) {

                items += "<option value='" + curso.Value + "'>" + curso.Text + "</option>";
            });*/

            for (var i = 0; i < data.length; i++) {
                items += "<option value='" + data[i]["ID"] + "'>" + data[i]["Codigo"] + " - " + data[i]["Nombre"] + "</option>";
            }

            if (items != "") {
                $("#sltCurso").html(items);
                $("#sltCurso").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");
                $("#sltCurso").prop("disabled", false);
            }
            else {
                $("#sltCurso").html("<option>No hay cursos abiertos para ese plan de estudio.</option>")
            }
        });

    });

    $("#sltCurso").change(function () {
        var route = "/CursoProfesor/Grupos/List/" + $('select[name="sltCurso"]').val();       
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, grupo) {                
                items += "<option value='" + grupo.Value + "'>" + grupo.Text + "</option>";
            });

            if (items != "") {                
                $("#sltGrupo").html(items);
                $("#sltGrupo").prepend("<option value='' selected='selected'>-- Seleccionar Grupo --</option>");
                $("#sltGrupo").prop("disabled", false);
            }
            else {
                $("#sltGrupo").html("<option>No hay grupos abiertos para ese curso.</option>")
            }
        });
    });

    $("#sltGrupo").change(function () {
        /*Obtiene la información del curso*/
        var route1 = "/CursoProfesor/Grupos/Info/" + $('select[name="sltGrupo"]').val();
        /*Obtiene la información del horario*/
        var route2;

        /*Items del select de horarios*/
        var items = "";

        /*Variables de info de curso*/
        var cupo = "";
        var aula = "";
        var id = "";

        /*Horas calculadas de acuerdo al horario*/
        var horas = 0;

        $.getJSON(route1, function (data) {
            //alert(data.toSource());
            cupo = data[0]["Cupo"];
            aula = data[0]["Aula"];
            id = data[0]["Curso"];

            //alert("id es " + id);

            if (cupo != "" || aula != "" || id != "") {
                $("#txtCupo").val(cupo)
                $("#txtAula").val(aula);
            }
            else {
                $("#txtCupo").val("No Disponible")
                $("#txtAula").val("No Disponible");
            }

            /* Obtener información del horario */
            route2 = "/CursoProfesor/Horarios/Info/"+ id;

            $.getJSON(route2, function (data) {
                //alert(data.toSource());
                for (var i = 0; i < data.length; i++) {
                    items += data[i]["Dia1"] + " " + data[i]["Hora_Inicio"] + " - " + data[i]["Hora_Fin"] + "\n";
                    //horas += (parseInt(data[i]["Hora_Fin"], 10) - parseInt(data[i]["Hora_Inicio"], 10));
                    var horafin = parseInt(data[i]["Hora_Fin"]);
                    var horainicio = parseInt(data[i]["Hora_Inicio"]);

                    horas += parseInt(horafin - horainicio);
                }

                //alert(horas);
                horas = parseInt(horas / 100, 10);

                if (items != "") {
                    $("#txtHorario").val(items);
                    $("#Asignar").prop("disabled", false);
                    $("#txtHoras").val(horas);
                }
                else {
                    $("#txtHorario").val("No hay horarios para ese curso.")
                }
            });
        });
    });
});