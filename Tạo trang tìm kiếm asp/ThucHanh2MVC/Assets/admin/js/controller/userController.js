var user = {
    init: function () {
        user.registerEvent();
    },
    registerEvent: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            var id = $(this).data('id')
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type:"POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('kích hoạt');
                    }
                    else {
                        btn.text('khóa');
                    }
                }
            });
        });
    }
}
user.init();