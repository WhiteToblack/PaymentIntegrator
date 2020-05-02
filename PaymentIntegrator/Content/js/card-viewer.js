$('Form').card({
    container: '.card-wrapper',
    placeholders: {
        number: '**** **** **** ****',
        name: 'Name Surname',
        expiry: '**/****',
        cvc: '***',
        form: 'CardForm'
    },
    formSelectors: {
        numberInput: 'input[name="CardInformation.CardNumber"]',
        expiryInput: 'input[name="CardInformation.ExpiryDate"]',
        cvcInput: 'input[name="CardInformation.Cvv"]',
        nameInput: 'input[name="CardInformation.HolderName"]'
    }
});
document.addEventListener('DOMContentLoaded', function () {
    var amountId = 0;
    window.amountOnChange = function (e) {
        var selectedValue = event.currentTarget.value;
        amountId = selectedValue;
        document.location.href = "#" + amountId;
        var totalAmount = $('#totalAmount');
        var amountList = window.amountList;
        for (var i = 0; i < amountList.length; i++) {
            if (amountList[i].Id == selectedValue) {
                totalAmount.html(amountList[i].TotalAmount);
                if (amountList[i].UseInstallment) {
                    $('#installments').css('display', 'flex');
                } else {
                    $('#installments').css('display', 'none');
                }
                totalAmount.addClass('glow');
                this.setTimeout(function () {
                    totalAmount.removeClass('glow');
                }, 3000);
                return;
            }
        }
    };

    $(function () {
        $("form").submit(function () {
            var formSelectors = {
                numberInput: 'input[name="CardInformation.CardNumber"]',
                expiryInput: 'input[name="CardInformation.ExpiryDate"]',
                cvcInput: 'input[name="CardInformation.Cvv"]',
                nameInput: 'input[name="CardInformation.HolderName"]'
            };

            if ($(formSelectors.numberInput).val().length < 16) {
                alert("Lütfen kart numarasını giriniz.");
                return false;
            }

            if ($(formSelectors.expiryInput).val().length < 4) {
                alert("Lütfen kart geçerlilik süresini giriniz.");
                return false;
            }

            if ($(formSelectors.cvcInput).val().length < 3) {
                alert("Lütfen kart arka yüzündeki cvv kodunu giriniz.");
                return false;
            }

            if ($(formSelectors.nameInput).val().length < 1) {
                alert("Lütfen kartın üzerinde bulunan ismi giriniz.");
                return false;
            }

            if ($(formSelectors.nameInput).val().length < 1) {
                alert("Lütfen kartın üzerinde bulunan ismi giriniz.");
                return false;
            }

            var selectedAmountType = $('#PaymentInformation_SelectedAmountId').val();
            if (selectedAmountType === '') {
                alert("Lütfen ödeme tipi seçiniz.");
                return false;
            }

            return true;

        });
    });
});
