document.addEventListener('DOMContentLoaded', onLoad);

var onLoad = function () {
    jQuery("#sendToPayment").click(function () {
        window.location = "Payment\Card?amount";
    });
};