/*
Copyright (C) 2009 - 2012
Email:		wangking717@qq.com
WebSite:	Http://wangking717.javaeye.com/
Author:		wangking
*/
$(function () {
    $("[name='easyTip']").each(function () {
        $(this).addClass("onShow");
    });
    $("[reg]").blur(function () {
        validate($(this));
    });

    $("[reg]").click(function () {
        $(this).nextAll("[name='easyTip']").eq(0).removeClass();
        $(this).nextAll("[name='easyTip']").eq(0).addClass("onFocus");
    });

    $("form").submit(function () {
        var isSubmit = true;
        $("[reg]").each(function () {
            if (!validate($(this))) {
                isSubmit = false;
            }
        });
        return isSubmit;
    });

});

function validate(obj) {
    var reg = new RegExp(obj.attr("reg"));
    var objValue = obj.attr("value");
    var objClass = obj.attr("class");
    var tipObj = obj.nextAll("[name='easyTip']").eq(0);
    var txtObj = obj.nextAll("[name='easyTxt']").eq(0);
    tipObj.removeClass();

    if (objClass == "txtRePWD") {
        alert($("#Pwd").attr("value"));
        if ($(".txtPWD").val() == "" || $(".txtPWD").val().length < 5) {
            tipObj.addClass("onError");
            txtObj.css("display", "");
            return false;
        } else {
            if ($("#Pwd").val() == $("#rpwd").val()) {
                
                tipObj.addClass("onCorrect");
                txtObj.css("display", "none");
                return true;
            } else {alert("l()");
                tipObj.addClass("onError");
                txtObj.css("display", "");
                return false;
            }
        }
    } else if (objClass == "idNumber") {
        var id = $(".idNumber").val();
        var url = "/Uc/ajaxuc.aspx?code=checkteammemberid&params=" + id;

        var d = "";
        if (id == "" || !reg.test(objValue)) {
            tipObj.addClass("onError");
            txtObj.text("请输入正确的证件号码");
            txtObj.css("display", "");
            return false;
        } else {
            $.get(
    url,
    function (data) {
        if (data.Code == 0) {
            d = "false";
            tipObj.html("");
            tipObj.addClass("onError");
            txtObj.text("证件号码已存在");
            txtObj.css("display", "");
        } else {
            tipObj.addClass("onCorrect");
            txtObj.css("display", "none");
        }
    }, "json"
    );
            if (d == "false") {
                return false;
            } else {
                return true;
            }
        }
    }
    else if (objClass == "txtEmail") {
        var id = $(".txtEmail").val();
        var url = "/Uc/ajaxuc.aspx?code=checkteamemail&params=" + id;

        var d = "";
        if (id == "" || !reg.test(objValue)) {
            tipObj.addClass("onError");
            txtObj.text("请输入正确的邮箱");
            txtObj.css("display", "");
            return false;
        } else {
            $.get(
    url,
    function (data) {
        if (data.Code == 0) {
            d = "false";
            tipObj.html("");
            tipObj.addClass("onError");
            txtObj.text("邮箱已存在");
            txtObj.css("display", "");
        } else {
            tipObj.addClass("onCorrect");
            txtObj.css("display", "none");
        }
    }, "json"
    );
            if (d == "false") {
                return false;
            } else {
                return true;
            }
        }
    }
    else if (objClass == "txtName") {
        var id = $(".txtName").val();
        var url = "/Uc/ajaxuc.aspx?code=checkteamname&params=" + id;

        var d = "";
        if (id == "" || !reg.test(objValue)) {
            tipObj.addClass("onError");
            txtObj.text("请输入正确的单位名称");
            txtObj.css("display", "");
            return false;
        } else {
            $.get(
    url,
    function (data) {
        if (data.Code == 0) {
            d = "false";
            tipObj.html("");
            tipObj.addClass("onError");
            txtObj.text("单位名称已存在");
            txtObj.css("display", "");
        } else {
            tipObj.addClass("onCorrect");
            txtObj.css("display", "none");
        }
    }, "json"
    );
            if (d == "false") {
                return false;
            } else {
                return true;
            }
        }
    }
    else {
        if (!reg.test(objValue)) {
            tipObj.addClass("onError");
            txtObj.css("display", "");
            return false;
        } else {
            tipObj.addClass("onCorrect");
            txtObj.css("display", "none");
            return true;
        }
    }
}

