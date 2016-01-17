$(".toggle-more-detail").on("click", function () {
    $(this).parent().parent().nextAll('.quotation-detail:first').toggle();
});