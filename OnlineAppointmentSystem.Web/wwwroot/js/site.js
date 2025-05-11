// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Confirm delete
function confirmDelete(event, message) {
    if (!confirm(message || 'Bu kaydı silmek istediğinizden emin misiniz?')) {
        event.preventDefault();
        return false;
    }
    return true;
}

// Confirm cancel
function confirmCancel(event, message) {
    if (!confirm(message || 'Bu randevuyu iptal etmek istediğinizden emin misiniz?')) {
        event.preventDefault();
        return false;
    }
    return true;
}

// Format date inputs
$(document).ready(function () {
    // Bootstrap tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Format date inputs
    $('.date-picker').datepicker({
        format: 'dd.mm.yyyy',
        autoclose: true,
        language: 'tr',
        todayHighlight: true
    });

    // Format time inputs
    $('.time-picker').timepicker({
        timeFormat: 'HH:mm',
        stepMinute: 15,
        hourMin: 8,
        hourMax: 18
    });

});