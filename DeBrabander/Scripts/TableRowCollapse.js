$(document).ready(function () {
    $(".select").change(function () {
        var id = $(this).attr('data-customer');
        if (this.checked) {
            $(".rowToExpand." + id).show();
        } else {
            $(".rowToExpand." + id).hide();
        }

    });
});