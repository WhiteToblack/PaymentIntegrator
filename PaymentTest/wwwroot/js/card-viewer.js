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
        cvcInput: 'input[name="CardInformation.Cvc"]',
        nameInput: 'input[name="User.NameSurname"]'
    }
});