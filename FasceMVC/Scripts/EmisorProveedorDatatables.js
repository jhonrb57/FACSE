function ListadoEmisorProveedor() {
    table = $("#ListadoEmisor").DataTable({

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
                    return '<input value="' + row.dpr_id + '" id="dpr_id" name="dpr_id" type="hidden">';
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<input type="checkbox" name="rowSelectCheckBox" />';
                }
            },
            {
                "className": 'details-control', "orderable": false, "render": function (data, type, full, meta) {
                    return '<a class="tooltip" style="cursor:pointer" ><img title=' + full.edf_descripcion + ' src=' + ((full.edf_ruta_imagen != null) && (full.edf_ruta_imagen != "") ? full.edf_ruta_imagen.replace('~', '') : "/Content/Images/blanco.png") + ' /></a>';
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
            { "data": "dpr_prefijo", "name": "dpr_prefijo", "orderable": false },
            { "data": "dpr_numero", "name": "dpr_numero" },
            { "data": "Identificacion", "name": "Identificacion" },
            { "data": "Nombre", "name": "Nombre", "autoWidth": true },
            { "data": "dpr_fecha_documento", "name": "dpr_fecha_documento" },
            { "data": "dpr_fecha_envio", "name": "dpr_fecha_envio" },
            { "data": "dpr_fecha_recibido", "name": "dpr_fecha_recibido" },
            { id: "prueba", "data": "dpr_valor_total", "name": "dpr_valor_total", className: "text-right", render: $.fn.dataTable.render.number(",", ".", 0, '$ ') },
            { "data": "dpr_usuario", "name": "dpr_usuario", "orderable": false },
            {
                "orderable": false, "render": function (data, type, full, meta) { return (full.dpr_acuse == true ? '<input type="checkbox" disabled checked /><label>' + full.dpr_fecha_acuse + '</label>' : '<input type="checkbox" disabled />') }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return (full.edf_codigo == "01" ? '<a id="aceptar' + meta.row + '" style="cursor: pointer" onclick="AceptarRechazar(\'' + full.dpr_id + '\',true,this.id)" title = "Aceptar" ><img src="/Content/Images/verde.png" /></a><a id="rechazado' + meta.row + '" style="cursor: pointer"  onclick="AceptarRechazar(\'' + full.dpr_id + '\',false,this.id)" title = "Rechazar" ><img src="/Content/Images/cancelar.png" /></a>' : '');
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<a target="_blank" href=' + full.Xml + ' title="Xml"><img src="/Content/Images/xml.png" /></a>';
                }
            },
            {
                "orderable": false, "render": function (data, type, full, meta) {
                    return '<a target="_blank" href="/EmisorProveedor/ExportarPdf?gDocumento=' + full.dpr_id + '&sRuta=' + full.dpf_pdf + '" title = "PDF" ><img src="/Content/Images/pdf.png" /></a>';
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
            row.child(formatProveedor(row.data())).show();
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
            row.child(formatAnexProveedor(row.data())).show();
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


function formatProveedor(d) {
    var tbody = "";
    $.ajax({
        type: 'GET',
        url: "SubListadoGrilla?id=" + d.dpr_id,
        dataType: "json",
        async: false,
        success: function (data) {
            var list = JSON.stringify(data);
            tbody = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left: 50px;"><tr><th>Estado</th><th>Fecha</th></tr>';
            var j = 0;
            $.each(data, function (i, item) {
                tbody += '<tr><td> ' + item.edf_descripcion + '</td><td>' + item.dpe_fecha + '</td></tr>';
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


function formatAnexProveedor(d) {
    var tbody = "";
    $.ajax({
        type: 'GET',
        url: "SubListadoGrillaAnexo?id=" + d.dpr_id,
        dataType: "json",
        async: false,
        success: function (data) {
            var list = JSON.stringify(data);
            tbody = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left: 50px;"><tr><th>Adjunto</th></tr>';
            var j = 0;
            $.each(data, function (i, item) {
                tbody += '<tr><td><a target="_blank" href=' + item.dpa_directorio + ' title = "Adjunto" >' + item.dpa_nombre + '</a></td></tr>';
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