$(document).ready(function () {
    $('.datepicker').datepicker({
        dateFormat: 'dd/mm/yy',
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true
    }).on("change", function (e) {
        $(this).valid();
    });

    $('.datepicker').attr("autocomplete", "off");

    $('.datatable').DataTable({
        "order": [],
        "columnDefs": [
            { className: 'dt-body-center', orderable: false, targets: -1 },
            { width: "2%", orderable: false, targets: 0 }
        ]
    });
});

function isEmptyString(str) {
    return ($.trim(str) === "");
}

function renderDate(data, type, val) {
    if (!isEmptyString(data)) {
        return data.substr(0, 10).split('-').reverse().join('-');
    }
    return '';
}

function refreshTable(table, url, callback) {
    if (url) {
        var obj = table.dataTable().fnSettings();
        obj.sAjaxSource = url;
    }
    var dtable = table.dataTable({ bRetrieve: true });
    dtable._fnDraw();
}