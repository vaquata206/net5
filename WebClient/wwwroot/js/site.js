// Show loading overlay the element
function showLoading() {
    $element = $("body");
    if (!$element.hasClass("card")) {
        $element.addClass("card");
    }

    if ($element.children(".overlay").length === 0) {
        $element.append('<div class="overlay dark" style="z-index: 9999"><i class="fas fa-2x fa-sync-alt fa-spin"></i></div>');
    }
}


// hide loading overlay the element
function hideLoading() {
    $element = $("body");
    $element.removeClass("card");
    if ($element.children(".overlay").length > 0) {
        $element.children(".overlay").remove();
    }
}

var modalHelper = function () {
    var $modal = $("#modal-default-confirm");
    var defaultModal = {
        title: "Xác thực",
        body: null,
        buttons: {
            submit: "Đồng ý",
            close: "Đóng"
        },
        submit: function () { },
        close: function () { }
    };

    var controls = {
        $title: $modal.find(".modal-title"),
        $body: $modal.find(".modal-default-body-message"),
        $submit: $modal.find(".modal-default-submit"),
        $close: $modal.find(".modal-default-close")
    };

    function showModal(options) {
        options = $.extend({}, defaultModal, options);
        emptyModal();

        controls.$title.text(options.title);
        if (options.body) {
            controls.$body.append(options.body);
        }

        controls.$submit.text(options.buttons.submit);
        controls.$submit.click((e) => {
            var result = options.submit(e);
            if (result === false) {
                e.preventDefault();
                e.stopPropagation();
            } else {
                emptyModal();
            }
        });
        controls.$close.text(options.buttons.close);
        controls.$close.click((e) => {
            options.close(e);
            emptyModal();
        });
        $modal.modal("show");
    }

    function emptyModal() {
        controls.$submit.unbind();
        controls.$submit.text(defaultModal.buttons.submit);
        controls.$close.unbind();
        controls.$close.text(defaultModal.buttons.close);
        controls.$body.empty();
        controls.$title.text("");
    }

    var publicInterfaces = {
        showModal: showModal
    };

    return publicInterfaces;
}();

var alertHelper = function () {
    var AlertTypes = {
        Info: 0,
        Success: 1,
        Error: 2,
        Warning: 3
    };

    var defaultOption = {
        title: "Thông báo",
        type: AlertTypes.Info,
        timer: 3000
    };

    function Init() {
        // show alerts that were send from system
        var $alertSytem = $(".alerts-system");
        if ($alertSytem.length > 0) {
            var $spMessage = $alertSytem.find("span");
            var type = $spMessage.data("type");
            var title = $spMessage.text() || "";
            var timer = $spMessage.data("timer");
            if (title.length > 0) {
                showAlert({
                    type: type,
                    timer: timer,
                    title: title
                })
            }
        }
    }

    function showAlert(option) {
        option = $.extend({}, defaultOption, option);
        if ((option.title || "").length === 0) {
            // no content
            return;
        }

        var icon = "";
        switch (option.type) {
            case AlertTypes.Info: icon = "info"; break;
            case AlertTypes.Success: icon = "success"; break;
            case AlertTypes.Error: icon = "error"; break;
            case AlertTypes.Warning: icon = "warning"; break;
            default:
                icon = "question"; break;
        }

        var swalOpt = {
            toast: true,
            title: option.title,
            icon: icon,
            showConfirmButton: false,
            position: 'top-end'
        };

        if (option.timer > 0) {
            swalOpt.timer = option.timer
        }


        Swal.fire(swalOpt);
    }

    Init();
    var publicInterfaces = {
        AlertTypes: AlertTypes,
        showAlert: showAlert
    };
    return publicInterfaces;
}();

var DataTable = {
    language: {
        "sEmptyTable": "Không có dữ liệu",
        "sInfo": "Hiển thị từ <strong>_START_</strong> đến <strong>_END_</strong> của <strong>_TOTAL_</strong> kết quả",
        "sInfoEmpty": "Hiển thị từ <strong>0</strong> đến <strong>0</strong> của <strong>0</strong> kết quả",
        "sLengthMenu": 'Hiển thị <select name="tb_length" aria-controls="tb" class="form-control input-sm">' +
            '<option value="10">10</option>' +
            '<option value="20">20</option>' +
            '<option value="50">50</option>' +
            '<option value="100">100</option>' +
            '<option value="200">200</option>' +
            '</select> hàng',
        "sProcessing": "Đang xử lý...",
        "oPaginate": {
            "sNext": ">",
            "sPrevious": "<"
        }
    }
};

var moneyHelper = function () {
    function FormatMoney(n) {
        if (n) {
            n = Math.round(n * 100) / 100;
        }
        n += "";
        var d = n.split(".");
        if (d.length === 0) {
            return "";
        }

        var f = d[0].replace(/./g, function (c, i, a) {
            return i > 0 && c !== "," && (a.length - i) % 3 === 0 ? "," + c : c;
        });

        var strMoney = f;
        if (d.length === 2) {
            strMoney += "." + d[1];
        }

        return strMoney;
    }

    function ConvertMoneyToText(money) {
        money = money + "";
        var t3 = ["", "NGHÌN", "TRIỆU", "TỈ"];
        var iT3 = 0;
        var textMoney = "ĐỒNG";

        if (money === "0") {
            return "KHÔNG " + textMoney;
        }

        for (var i = money.length - 1; i > -3; i -= 3) {
            var str = money.substring(i - 2 >= 0 ? i - 2 : 0, i + 1);

            var text = "";
            for (var j = str.length - 1; j >= 0; j--) {
                if (str[j] !== "0") {
                    text = NumberToText(j, str) + " " + text;
                }
            }

            if (text) {
                textMoney = (text.trim() + " " + t3[iT3]).trim() + " " + textMoney;
            }

            iT3++;
            if (iT3 > t3.length) {
                iT3 = 1;
            }
        }

        return textMoney.charAt(0).toUpperCase() + textMoney.slice(1).toLowerCase();
    }

    function NumberToText(index, str) {
        var number = parseInt(str[index]);
        var t1 = ["", "MƯƠI", "TRĂM"];
        var text = index === 1 && number === 1 ? "" : (" " + t1[str.length - index - 1]);

        var isLe = false;
        if (str.length === 3 && index === 2 && str[1] === '0' && str[0] !== '0') {
            isLe = true;
        }
        switch (number) {
            case 1:
                if (index === str.length - 2) text = "MƯỜI";
                else if (index === str.length - 1 && str.length > 1 && !isLe && str[str.length - 2] !== '1') text = "MỐT" + text;
                else text = "MỘT" + text;
                break;
            case 2: text = "HAI" + text; break;
            case 3: text = "BA" + text; break;
            case 4: text = "BỐN" + text; break;
            case 5: text = (index === str.length - 1 && str.length > 1 && !isLe ? "LĂM" : "NĂM") + text; break;
            case 6: text = "SÁU" + text; break;
            case 7: text = "BẢY" + text; break;
            case 8: text = "TÁM" + text; break;
            case 9: text = "CHÍN" + text; break;
        }

        return (isLe ? "LẺ " : "") + text;
    }

    var publicInterfaces = {
        FormatMoney: FormatMoney,
        ConvertMoneyToText: ConvertMoneyToText
    };

    return publicInterfaces;
}();

var dateTimeHelper = function () {
    $.fn.datepicker.dates['vi'] = {
        days: ["Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 5", "Thứ 7", "Chủ nhật"],
        daysShort: ["CN", "T2", "T3", "T4", "T5", "T6", "T7", "CN"],
        daysMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7", "CN"],
        months: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
        monthsShort: ["T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12"],
        today: "Hôm nay"
    };
    $.fn.datepicker.defaults.format = "dd/mm/yyyy";
    $.fn.datepicker.defaults.language = "vi";

    $('.datetimepicker-input').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true
    });

    $('.monthpicker').datepicker({
        autoclose: true,
        format: "mm/yyyy",
        startView: "months",
        minViewMode: "months"
    }).datepicker("setDate", 'now');

    function formatDateToString(d) {
        if (!d) {
            return "";
        }

        if (typeof d === "string") {
            d = new Date(d);
        }

        return ("0" + d.getDate()).slice(-2) + "/" + ("0" + (d.getMonth() + 1)).slice(-2) + "/" + d.getFullYear();
    }

    function formatDateTimeToString(d) {
        if (!d) {
            return "";
        }

        if (typeof d === "string") {
            d = new Date(d);
        }

        return formatDateToString(d) + " " + ("0" + d.getHours()).slice(-2) + ":" + ("0" + d.getMinutes()).slice(-2);
    }

    var publicInterfaces = {
        dateToString: formatDateToString,
        dateTimeToString: formatDateTimeToString
    };

    return publicInterfaces
}();

var formHelper = function () {
    $("form").submit(function () {
        var $form = $(this);
        if ($form.find("input[name='__RequestVerificationToken']").length === 0) {
            var $defaultRequestVarificationToken = $(".wrapper > input[name='__RequestVerificationToken']").clone();
            $form.append($defaultRequestVarificationToken);
        }
    });
}();

function isNumeric(value) {
    return /^-?\d+$/.test(value);
}