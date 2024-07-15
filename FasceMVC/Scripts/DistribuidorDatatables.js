function DistribuidorInicio() {
    table = $("#ListadoDistribuidor").DataTable({

        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 50,
        "searching": false,

        "ajax": {
            "url": "ListadoGrillaDatatable",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<button type="button" role="button" aria-expanded="false" aria-controls="collapseExample" class="btn btn-link editar" onclick="EditarEmisor(\'' + full.emi_id + '\')"></button>';
                }
            },
            {
                "data": "tid_descripcion", "name": "tid_descripcion"
            },
            { "data": "emi_identificacion", "name": "emi_identificacion" },
            { "data": "emi_nombre", "name": "emi_nombre" },
            { "data": "dep_nombre", "name": "dep_nombre" },
            { "data": "mun_nombre", "name": "mun_nombre" },
            { "data": "emi_correo", "name": "emi_correo" },
            { "data": "emi_telefono", "name": "emi_telefono" },
            { "data": "emi_correo_automatico", "name": "emi_correo_automatico"},
            { "data": "emi_fecha_creacion", "name": "emi_fecha_creacion" }
        ]
    });
}