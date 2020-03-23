let dataTable;

$(document).ready(function() {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/nationalparks/getallnationalparks",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "state", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/nationalparks/upsert/${data}" class="btn btn-success text-white" style="cursor: pointer;">
                               <i class="fa fa-edit"></i></a>
                               &nbsp;
                            <a onclick="Delete('/nationalparks/delete/${data}')" class="btn btn-danger text-white" style="cursor: pointer;">
                               <i class="fa fa-trash-alt"></i></a>
                    `
                },
                "width": "30%"
            },
        ]
    });
}

function Delete(url) {
    swal({
        title: "Delete forever?",
        text: "You will not be able to restore the data",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else
                    {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}