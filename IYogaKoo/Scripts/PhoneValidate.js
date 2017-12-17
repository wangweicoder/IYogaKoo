       
//发送验证码
function checkUserName() { 
    if ($("#Uphone").val() != "") {
        var captcha = $("#phone_eCAPTCHA").val(); 
        if ($.trim(captcha)!="") { 
            jQuery.ajax({
                url: "/Login/RegemailCAPTCHA",
                type: "GET",
                data: { "eCAPTCHA": captcha },
                dataType: "text",
                success: function (data) {
                    if (data == "true") {
                        $("#phone_eCAPTCHA").val("");
                        $("#phone_img4").attr("src", "/Shared/check?r=" + Math.random());

                        jQuery.ajax({
                            url: "/Login/getyzm",
                            type: "GET",
                            data: { "phone": $("#Uphone").val() },
                            contentType: "application/x-www-form-urlencoded",
                            dataType: "text",
                            success: function (relust, status) {
                                if (relust == "0") {

                                    layer.alert("短信发送成功", { "closeBtn": 0 }, function (l) {
                                        layer.close(l);
                                        CountDown();
                                    });
                                 
                                } else {
                                    layer.msg("出现错误，请稍后再试，带来不便，敬请谅解");
                                }
                                //else { alert(relust); }
                                //if (status == "success")

                                // timedCount();              
                                //CountDown();
                            },
                            error: function (xhr, error, ex) {
                                var div = document.createElement("div");
                                div.innerText = "出现错误，请稍后再试，带来不便，敬请谅解";
                                document.getElementsByTagName("body")[0].appendChild(div);
                            }
                        });
                    }
                    else {
                        layer.alert("验证码错误");
                    }
                }
            });
        } else {
            layer.alert("请输入图形验证码");
            return false;
        }
    }
    else { layer.alert("请先填写手机号！"); }
    //使用jquery 的ajax发送 请求
}
 
var time = 60, ti;
function CountDown() {    
      
        Execute(time);
}
function angerEx()
{    
    if ($("#Uphone").val() != "") {
        layer.alert("验证码重新发送", function (data) {
            layer.close(data);
            CountDown();
        });
        
    }
}

function Execute(time) {  
    var t = document.getElementById('button');
    document.getElementById("button").disabled = true;
    t.value = "请稍等" + time + "秒";   
    ti=setTimeout("Execute(" + --time + ")", 1000);
    if (time <= 0) {
        clearTimeout(ti);       
        document.getElementById("button").disabled = false;       
        document.getElementById('button').value = "获取验证码";
    }  
}
//-----

    $(function () {        
        jQuery.validator.addMethod("NickName", function (value, element) {
            var tel = /^[a-zA-Z0-9\u4e00-\u9fa5]{3,16}$/;
            return tel.test(value) || this.optional(element);
        },"请输入 3 - 16 个字母或汉字字符。");
        $("#reg_form").validate({
            rules: {
                NickName: {
                    remote: {
                        url: "../Login/nickname",
                        beforeSend: function (xhr) {
                            $("#reg_form #NickName").parent().children(".error_field").children("label").html("正在检查是否可用…");
                            $("#reg_form #NickName").parent().children(".error_field").addClass("intput_wait");
                        }
                    }
                },
                UEmail: {
                    remote: {
                        url: "../Login/email",
                        beforeSend: function (xhr) {
                            $("#reg_form #UEmail").parent().children(".error_field").children("label").html("正在检查是否可用…");
                            $("#reg_form #UEmail").parent().children(".error_field").addClass("intput_wait");
                        }
                    }
                },               
                rpassword: {
                    required: true,
                    minlength: 6,
                    equalTo: '#password'
                },
                eCAPTCHA: {
                    remote: {
                        url: "../Login/RegemailCAPTCHA",
                        beforeSend: function (xhr)
                        {
                            $("#reg_form #eCAPTCHA").parent().children(".error_field").children("label").html("正在检查是否正确");
                            $("#reg_form #eCAPTCHA").parent().children(".error_field").addClass("intput_wait");
                        }
                    }
                       
                },                

            },
            messages: {
                NickName: {
                    required: "请输入昵称。",
                    nickname: "由字母、汉字组成。",
                    maxlength: "请输入 3 - 16 个字母或汉字字符。",
                    minlength: "请输入 3 - 16 个字母或汉字字符。",
                    remote: jQuery.format("很遗憾，昵称已被占用。")
                },
                UEmail: {
                    required: "请输入电子邮箱，以便接收邮件。",
                    email: "电邮地址好像不对。再试一次？",
                    remote: jQuery.format("很遗憾，邮箱已被注册。")
                },                
                password: {
                    required: "请输入6-16位密码。",
                    maxlength: "请输入6-16位密码。",
                    minlength: "请输入6-16位密码。"
                },
                rpassword: {
                    required: "请再次输入密码。",
                    maxlength: "请输入6-16位密码。",
                    minlength: "请输入6-16位密码。", 
                    equalTo: "输入的密码和确认密码要一致。"
                },
                eCAPTCHA: {
                    required: "请输入验证码。",
                    maxlength: "请输入4位验证码。",
                    minlength: "请输入4位验证码。",
                    remote: jQuery.format("验证码输入错误。")
                }
            },
            errorPlacement: function (error, element) {
                $(element).before("<div class='error_field'></div>");
                error.appendTo(element.prev(".error_field"));
                $(element).parent().children(".error_field").removeClass("intput_right").removeClass("intput_wait").addClass("intput_error");
            },
            success: function (label) {
                label.parent().addClass("intput_right");
                label.text($(label).parent().next().attr("data-right"))
            },
            highlight: function (element, errorClass, validClass) {
                $(element).parent().children(".error_field").removeClass("intput_right").removeClass("intput_wait").addClass("intput_error");
            }
        });
        $("#btnReset").click(function (e) {
            e.preventDefault();
            // 这里使用了插件form的resetForm()函数，恢复到第一次加载页面的状态 
            $("#reg_form").resetForm();
        });
    });
