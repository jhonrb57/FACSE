function ListadoFacturacion() {
    tableInicio = $("#ListadoEmisor").DataTable({

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
                className: "hiddenClass", "render": function (data, type, row) {
                    return '<input value="' + row.doc_id + '" id="doc_id" name="doc_id" type="hidden"><input value="' + row.CodigoEstado + '" id="CodigoEstado" name="CodigoEstado" type="hidden"><input value="' + row.CorreoReceptor + '" id="CorreoReceptor" name="CorreoReceptor" type="hidden">';
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<input type="checkbox" name="rowSelectCheckBox" />';
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) { return (full.CodigoEstado == "1" ? '<img src="/Content/Images/verde.png" title="Aprobado por la Dian" />' : full.CodigoEstado == "2" ? '<img src="/Content/Images/rojo.png" title="En proceso de Validación" />' : full.CodigoEstado == "10" ? '<img src="/Content/Images/aprobacion_novedad.png" title="Aprobado con Novedad" />' : '<img src="/Content/Images/amarillo.png" title="Sin Aprobacion de la Dian" />') }
            },
            {
                "className": 'details-control', "orderable": false, "render": function (data, type, full, meta) {
                    return '<a class="tooltip" style="cursor:pointer" ><img title=' + full.edf_descripcion + ' src=' + ((full.edf_ruta_imagen != null) && (full.edf_ruta_imagen != "") ? full.edf_ruta_imagen.replace('~', '') : "/Content/Images/blanco.png") + ' /></a>';
                }
            },
            {
                "className": 'details-controlNot', "orderable": false, "render": function (data, type, full, meta) {
                    return (full.notificacion > 0 ? '   <a class="tooltip" style="cursor:pointer"; title="Notificacion"><img src = "/Content/Images/notificacion.png" /></a>' : '');
                }
            },
            {
                "className": 'details-controlAne', "orderable": false, "render": function (data, type, full, meta) {
                    return (full.anexo > 0 ? '  <a class="tooltip" style="cursor:pointer" title="Anexo"><img src = "/Content/Images/adjuntar.png" /></a>' : '');
                }
            },
            {
                "data": "Tipo", "name": "Tipo", "orderable": false
            },
            { "data": "doc_prefijo", "name": "doc_prefijo", "orderable": false },
            { "data": "doc_numero", "name": "doc_numero" },
            { "data": "Identificacion", "name": "Identificacion" },
            { "data": "Nombre", "name": "Nombre", "autoWidth": true },
            { "data": "doc_fecha_recepcion", "name": "doc_fecha_recepcion" },
            { "data": "doc_fecha_envio", "name": "doc_fecha_envio" },
            { "data": "doc_valor_total", "name": "doc_valor_total", className: "text-right", render: $.fn.dataTable.render.number(",", ".", 0, '$ ') },
            { "data": "doc_usuario", "name": "doc_usuario", "orderable": false },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return (full.Session == false ? '<a target="_blank" href="/EmisorInicio/ExportarJson?gDocumento=' + full.doc_id + '&sTipoDocumento=' + full.Tipo + '&sNitEmisor=' + full.Identificacion + '&sPrefijo=' + full.doc_prefijo + '&sNumeroFactura=' + full.doc_numero + '})" title="JSON"><img src="/Content/Images/descargar.png" /></a>' : '');
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<a target="_blank" href=' + full.Xml + ' title="Xml"><img src="/Content/Images/xml.png" /></a>';
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<a target="_blank" href="/EmisorInicio/ExportarPdf?gDocumento=' + full.doc_id + '" title = "PDF" ><img src="/Content/Images/pdf.png" /></a>';
                }
            },
            {
                "className": 'details-controlCor', "orderable": false, "render": function (data, type, full, meta) {
                    return (full.correo > 0 ? ' <a class="tooltip" style="cursor:pointer"><img src = "/Content/Images/email.png" /></a>' : '');
                }
            }
        ]

    });

    $('#ListadoEmisor tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = $('#ListadoEmisor').DataTable().row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    }
    );



    $('#ListadoEmisor tbody').on('click', 'td.details-controlNot', function () {
        var tr = $(this).closest('tr');
        var row = $('#ListadoEmisor').DataTable().row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(formatNoti(row.data())).show();
            tr.addClass('shown');
        }
    }
    );


    $('#ListadoEmisor tbody').on('click', 'td.details-controlAne', function () {
        var tr = $(this).closest('tr');
        var row = $('#ListadoEmisor').DataTable().row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(formatAnex(row.data())).show();
            tr.addClass('shown');
        }
    }
    );


    $('#ListadoEmisor tbody').on('click', 'td.details-controlCor', function () {
        var tr = $(this).closest('tr');
        var row = $('#ListadoEmisor').DataTable().row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(formatCorr(row.data())).show();
            tr.addClass('shown');
        }
    }
    );
}


function format(d) {
    var tbody = "";
    $.ajax({
        type: 'GET',
        url: "SubListadoGrilla?id=" + d.doc_id,
        dataType: "json",
        async: false,
        success: function (data) {
            var list = JSON.stringify(data);
            tbody = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left: 50px;"><tr><th>Estado</th></tr>';
            var j = 0;
            $.each(data, function (i, item) {
                tbody += '<tr><td> ' + item.edf_descripcion + '</td></tr>';
                j = j + 1;
            });
            tbody += '</table>';

            if (j == 0) {
                tbody = '';
            }

        },
        error: function (result) {
            var error = JSON.stringify(result);
            throw "Error...";
        }
    });

    return tbody;
}


function formatNoti(d) {
    var tbody = "";
    $.ajax({
        type: 'GET',
        url: "SubListadoGrillaNotificacion?id=" + d.doc_id,
        dataType: "json",
        async: false,
        success: function (data) {
            var list = JSON.stringify(data);
            tbody = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left: 50px;"><tr><th>Regla</th><th>Facse</th></tr>';
            var j = 0;
            $.each(data, function (i, item) {
                tbody += '<tr><td> ' + item.rdi_regla + '</td><td> ' + item.rdi_descripcion_facse + '</td></tr>';
                j = j + 1;
            });
            tbody += '</table>';

            if (j == 0) {
                tbody = '';
            }

        },
        error: function (result) {
            var error = JSON.stringify(result);
            throw "Error...";
        }
    });

    return tbody;
}


function formatAnex(d) {
    var tbody = "";
    $.ajax({
        type: 'GET',
        url: "SubListadoGrillaAnexo?id=" + d.doc_id,
        dataType: "json",
        async: false,
        success: function (data) {
            var list = JSON.stringify(data);
            tbody = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left: 50px;"><tr><th>Adjunto</th></tr>';
            var j = 0;
            $.each(data, function (i, item) {
                tbody += '<tr><td><a target="_blank" href=' + item.dan_directorio + ' title = "Adjunto" >' + item.dan_nombre + '</a></td></tr>';
                j = j + 1;
            });
            tbody += '</table>';

            if (j == 0) {
                tbody = '';
            }

        },
        error: function (result) {
            var error = JSON.stringify(result);
            throw "Error...";
        }
    });

    return tbody;
}


function formatCorr(d) {
    var tbody = "";
    $.ajax({
        type: 'GET',
        url: "SubListadoGrillaCorreo?id=" + d.doc_id,
        dataType: "json",
        async: false,
        success: function (data) {
            var list = JSON.stringify(data);
            tbody = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left: 50px;"><tr><th>Fecha</th><th>Correo</th><th>Estado</th></tr>';
            var j = 0;
            $.each(data, function (i, item) {
                tbody += '<tr><td> ' + item.Fecha + '</td><td> ' + item.dco_correo + '</td><td> ' + item.edc_descripcion + '</td></tr>';
                j = j + 1;
            });
            tbody += '</table>';

            if (j == 0) {
                tbody = '';
            }

        },
        error: function (result) {
            var error = JSON.stringify(result);
            throw "Error...";
        }
    });

    return tbody;
}