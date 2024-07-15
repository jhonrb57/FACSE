
function ListadoFacturacion() {
    $("#ListadoEmisor").jqGrid({
        url: "ListadoGrilla",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['', '', '', '', 'Tipo', 'N. a', 'Prefijo', 'Numero', 'Identificacion', 'Nombre', 'Fecha Emisión', 'Fecha Envio', 'Total', 'Usuario', '', '', ''],
        colModel: [
            { name: 'doc_id', index: 'doc_id', hidden: true },
            { name: 'RutaJson', index: 'RutaJson', hidden: true },
            { name: 'CodigoEstado', index: 'CodigoEstado', hidden: true },
            { name: 'Estado', index: 'Estado', width: 30, align: "center" },
            { key: false, name: 'Tipo', index: 'Tipo', search: false, sortable: false, width: 40 },
            { key: false, name: 'Anexo', index: 'Anexo', search: false, sortable: false, width: 40 },
            { key: false, name: 'doc_prefijo', index: 'doc_prefijo', search: false, sortable: false, width: 50 },
            { key: false, name: 'doc_numero', index: 'doc_numero', search: false, width: 60 },
            { key: false, name: 'Identificacion', index: 'Identificacion', search: false, width: 100 },
            { key: false, name: 'Nombre', index: 'Nombre', search: false },
            { key: false, name: 'doc_fecha_recepcion', index: 'doc_fecha_recepcion', search: false, width: 100 },
            { key: false, name: 'doc_fecha_envio', index: 'doc_fecha_envio', search: false, width: 100 },
            { key: false, name: 'doc_valor_total', index: 'doc_valor_total', formatter: 'number', align: "right", formatoptions: { thousandsSeparator: '.', decimalPlaces: 0 }, search: false, width: 110 },
            { key: false, name: 'doc_usuario', index: 'doc_usuario', search: false, sortable: false, width: 60 },
            { name: 'Json', index: 'Json', width: 30, align: "center" },
            { name: 'Xml', index: 'Xml', width: 30, align: "center" },
            { name: 'Pdf', index: 'Pdf', width: 30, align: "center" }
        ],
        cmTemplate: { sortable: true },
        sortable: true,
        sortcolumn: 'doc_fecha_recepcion',
        loadonce: false,
        pager: jQuery('#pagerListadoEmisor'),
        rowNum: 30,
        height: 'auto',
        viewrecords: true,
        caption: 'Documentos Emisor',
        emptyrecords: 'No hay documentos a mostrar',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: true,
        subGrid: true,
        subGridRowExpanded: function (subgrid_id, row_id) {
            var subgrid_table_id;
            subgrid_table_id = subgrid_id + "_t";
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'  ></table><br/><table id='" + subgrid_table_id + "2' class='scroll'></table><br/><table id='" + subgrid_table_id + "3' class='scroll'></table><br/><table id='" + subgrid_table_id + "4' class='scroll'></table><br/>");
            var dataFromTheRow = jQuery('#ListadoEmisor').jqGrid('getRowData', row_id);

            jQuery("#" + subgrid_table_id).jqGrid({
                url: "SubListadoGrilla?id=" + dataFromTheRow.doc_id,
                datatype: 'json',

                mtype: 'Get',
                sortable: false,
                page: 1,
                colNames: ['Estado'],
                colModel: [
                    { key: false, name: 'edf_descripcion', index: 'edf_descripcion', search: false, sortable: false },
                ],
                height: 'auto',
                caption: 'Estados',

                emptyrecords: 'No hay Estados a mostrar',
                width: '900',
                jsonReader: {
                    root: "rows",
                    repeatitems: false,
                    Id: "0"
                }
            });

            jQuery("#" + subgrid_table_id + "2").jqGrid({
                url: "SubListadoGrillaNotificacion?id=" + dataFromTheRow.doc_id,
                datatype: 'json',
                mtype: 'Get',
                sortable: false,
                page: 1,
                colNames: ['Regla', 'Facse'],
                colModel: [
                    { key: false, name: 'rdi_regla', index: 'rdi_regla', search: false, sortable: false },
                    { key: false, name: 'rdi_descripcion_facse', index: 'rdi_descripcion_facse', search: false, sortable: false },
                ],
                height: 'auto',
                caption: 'Notificaciones',
                emptyrecords: 'No hay Notificaciones a mostrar',
                width: '900',
                jsonReader: {
                    root: "rows",
                    repeatitems: false,
                    Id: "0"
                }
            });

            jQuery("#" + subgrid_table_id + "3").jqGrid({
                url: "SubListadoGrillaCorreo?id=" + dataFromTheRow.doc_id,
                datatype: 'json',
                mtype: 'Get',
                sortable: false,
                page: 1,
                colNames: ['Fecha', 'Correo', 'Estado'],
                colModel: [
                    { key: false, name: 'Fecha', index: 'Fecha', search: false, sortable: false },
                    { key: false, name: 'dco_correo', index: 'dco_correo', search: false, sortable: false },
                    { key: false, name: 'edc_descripcion', index: 'edc_descripcion', search: false, sortable: false },
                ],
                height: 'auto',
                caption: 'Correos',
                emptyrecords: 'No hay Correo a mostrar',
                width: '900',
                jsonReader: {
                    root: "rows",
                    repeatitems: false,
                    Id: "0"
                }
            });

            jQuery("#" + subgrid_table_id + "4").jqGrid({
                url: "SubListadoGrillaAnexo?id=" + dataFromTheRow.doc_id,
                datatype: 'json',
                mtype: 'Get',

                sortable: false,
                page: 1,
                colNames: ['Nombre', ''],
                colModel: [
                    { key: false, name: 'dan_nombre', index: 'dan_nombre', search: false, sortable: false },
                    { name: 'Anexo', index: 'Anexo', width: 30, align: "center" }
                ],
                height: 'auto',
                caption: 'Anexos',
                emptyrecords: 'No hay Anexo a mostrar',
                width: '900',
                jsonReader: {
                    root: "rows",
                    repeatitems: false,
                    Id: "0"
                }
            });
        },
        onPaging: function (which_button) {
            jQuery("#gridid").jqGrid('setGridParam', { datatype: 'json' });
        }
    }).navGrid('#pagerListadoEmisor', { view: false, edit: false, add: false, del: false, search: false, refresh: false },
        {
            // edit options

        },
        {
            // add options

        },
        {
            // delete options

        },
        {
            closeOnEscape: true, multipleSearch: true,
            closeAfterSearch: true
        }, // search options
        {
            //view options
            width: 600
        });
};

function myCustomCheck(value, colname) {
    if (value < 0 || value > 20)
        return [false, "Please enter value between 0 and 20"];
    else
        return [true, ""];
};