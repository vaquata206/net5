jQuery.extend(jQuery.validator.messages, {
    required: "Trường này bắt buộc.",
    remote: "Please fix this field.",
    email: "Vui lòng nhập đúng định dạng email!",
    url: "Please enter a valid URL.",
    date: "Please enter a valid date.",
    dateISO: "Please enter a valid date (ISO).",
    number: "Vui lòng chỉ nhập số.",
    digits: "Please enter only digits.",
    creditcard: "Please enter a valid credit card number.",
    equalTo: "Giá trị không khớp.",
    accept: "Please enter a value with a valid extension.",
    maxlength: jQuery.validator.format("Vui lòng nhập giá trị nhỏ hơn {0} ký tự."),
    minlength: jQuery.validator.format("Vui lòng nhập ít nhất {0} ký tự."),
    rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
    range: jQuery.validator.format("Please enter a value between {0} and {1}."),
    max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
    min: jQuery.validator.format("Vui lòng nhập giá trị lớn hơn hoặc bằng {0}.")
});
jQuery.validator.addMethod("testphone", function (value, element) {
    var reg = /^0(1\d{9}|9\d{8})$/;
    return this.optional(element) || reg.test(value);
}, 'Please enter a valid phone.');
jQuery.validator.addMethod("invoice_number", function (value, element) {
    var reg = /^\d{0,7}$/;
    return this.optional(element) || reg.test(value);
}, 'Không đúng định dạng mã số');
jQuery.validator.addMethod("testToaDo", function (value, element) {
    var reg1 = /^\d*,?\d*$/;
    var reg2 = /^\d*\.?\d*$/;
    return this.optional(element) || reg1.test(value) || reg2.test(value);
}, 'Không đúng định dạng kinh độ, vĩ độ');

$(function () {
    var $forms = $("form[validate=true]");
    var count = $forms.length;
    for (var i = 0; i < count; i++) {
        var $form = $($forms[i]);
        $form.validate({
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });
        $form.submit(function (event) {
            var $f = $(this);
            var submitNoValid = $f.attr("submit-no-valid");
            if (!submitNoValid && !$f.valid()) {
                event.preventDefault();
            }
            else if ($f.attr("show-loader")) {
                showLoading();
            }
        });
    }
});