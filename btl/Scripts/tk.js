$(document).ready(function () {
    $('#txttimkiem').keyup(function () {
        if ($('#txttimkiem').val() == "") {
            $('#sp').html('');
            return;
        }
        $('#loading').show();
        var s = $('#txttimkiem').val();
        console.log($('#txttimkiem').val()),
        $.ajax({
            type: "POST",
            url: '/SoSanhSanPham/TimKiem?name='+s,
            //data: JSON.stringify({
            //    name: $('#txttimkiem').val(),
            //}),
            contentType: 'application/html; charset=utf-8',
            dataType : 'text',
            success: function (data) {
                $('#loading').hide();
                //var lst = "";
                //var str = data;
                //while (str.search("data-id") != -1) {
                //    lst += str.slice(str.search("data-id") + 2, str.search("data-id") + 4);
                //};

                $('#sp').html(data);
            },
            error: function (err) {
                $('#loading').hide();
                console.log(err);
            }
            
        });
       
    });
})