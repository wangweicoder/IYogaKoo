
$(function () {
    //保存个人信息
    $('#butnSave').click(function () {       
        $("#hYogisDepict").val(UE.getEditor('editor').getContent());
        var strInfo = $("#hidinfo").val();//从审核页面到个人信息页面：保存成功返回审核页面
        //alert(strInfo);
        var data = $("#form1").serialize();
        $.ajax({
            url: '/Login/Method?t=' + Math.random(),
            type: 'POST',
            data: data,
            success: function (data) {
                if (data.code == 0) {
                    layer.msg("修改成功！");
                    if (strInfo == "1")
                    {
                        layer.confirm('是否返回到审核页面？', {
                            btn: ['是','否'] //按钮
                        }, function(){
                            //layer.msg('的确很重要', {icon: 1});
                            window.location.href = '/YogisModels/Audit';
                        }, function(){
                            //layer.msg('也可以这样', {
                            //    time: 20000, //20s后自动关闭
                            //    btn: ['明白了', '知道了']
                            //});
                            window.location.href = location.href;
                        });
                    }  
                    else
                        window.location.href = location.href;
                } else {
                    layer.msg("修改失败！");
                }

            }, error: function () {
                layer.msg("修改失败！");
            }
        });
        return false;
    });
    //end保存个人信息
    // 国家事件
    $('#ddlCountryID').click(function () {

        var strGuoJia = $(this).val();
        if (strGuoJia != "0") {
            document.getElementById("ddlProvinceID").innerHTML = "";
            //加载省
            $.ajax({
                url: '/Login/GetAreaInfo?id=' + strGuoJia,
                type: 'post',
                datatype: 'json',
                success: function (data) {
                    $("#ddlProvinceID").html("<option value='0'>请选择</option>");
                    $("#ddlCityID").html("<option value='0'>请选择</option>");
                    $("#ddlDistrictID").html("<option value='0'>请选择</option>");
                    for (var i = 0; i < data.length; i++) {

                        $("#ddlProvinceID").append("<option value='" + data[i].ID + "'>" + data[i].ItemName + "</option>");

                    };
                }
            });
        }
    });
    //省事件
    $("#ddlProvinceID").click(function () {

        var strSheng = $(this).val();
        if (strSheng != "0") {
            document.getElementById("ddlCityID").innerHTML = "";
            //加载市
            $.ajax({
                url: '/Login/GetAreaInfo?id=' + strSheng,
                type: 'post',
                datatype: 'json',
                success: function (data) {
                    $("#ddlCityID").html("<option value='0'>请选择</option>");
                    $("#ddlDistrictID").html("<option value='0'>请选择</option>");
                    for (var i = 0; i < data.length; i++) {

                        $("#ddlCityID").append("<option value='" + data[i].ID + "'>" + data[i].ItemName + "</option>");

                    };
                }
            });
        }
    });
    //城市事件
    $("#ddlCityID").click(function () {

        var strShi = $(this).val();
        if (strShi != "0") {
            document.getElementById("ddlDistrictID").innerHTML = "";
            //加载城区
            $.ajax({
                url: '/Login/GetAreaInfo?id=' + strShi,
                type: 'post',
                datatype: 'json',
                success: function (data) {
                    $("#ddlDistrictID").html("<option value='0'>请选择</option>");
                    for (var i = 0; i < data.length; i++) {

                        $("#ddlDistrictID").append("<option value='" + data[i].ID + "'>" + data[i].ItemName + "</option>");

                    };
                }
            });
        }
    });
});

function delCoverImg(id) {

    if (confirm("是否删除!")) {

        $.ajax({
            url: '/Login/delCoverImgModels/' + id,
            type: 'POST',
            data: {
                id: id,
                iType: 1
            },
            success: function (data) {

                if (data.code == 0) {
                    $("#idpCoverImg").attr("src", "");
                    location.href = "/Login/Edit"
                } else {
                    layer.msg("删除失败！");

                }

            }, error: function () {
                layer.msg("删除失败！");

            }
        });

    }
}
//修改密码
function checkpwdNull() {
    var oldpwd = $("#OldPwd").val();
    var spwd = $("#NewPwd").val();
    var spwd2 = $("#NewPwd2").val();
    var bl = true;
    if (oldpwd.trim().length == 0)
    {
        layer.msg("密码不能为空");
        bl = false;
    }
    else if (spwd.length < 6)
    {
        layer.msg("密码长度不能少于6位");
        bl = false;
    }
    else if (spwd != spwd2) {
        layer.msg("两次密码输入的不一致，请重新输入！");
        bl = false;
    }
    return bl;
}
$(function () {
    $('#btnpwd').click(function () {
        var NewPwd = $("#NewPwd").val();
        var NewPwd2 = $("#NewPwd2").val();
        if (checkpwdNull()) {

            $.ajax({
                url: '/Login/UpdatePwd?t=' + Math.random(),
                type: 'POST',
                data: {
                    OldPwd: $("#OldPwd").val(),
                    NewPwd: NewPwd
                },
                success: function (data) {

                    if (data.code == 0) {
                        layer.msg("密码修改成功！");
                        $("#NewPwd").val("");
                        $("#NewPwd2").val("");
                        $("#OldPwd").val("");
                    }
                    else if (data.code == 2) {
                        layer.msg("原密码错误！");
                    }
                    else {
                        layer.msg("密码修改失败！");
                    }

                }, error: function () {
                    layer.msg("密码修改失败" + data.code);
                }
            });

        }

    });

    //证件信息
    function checkIdTypeNull() {
        var IdType = $("#IdType").val();
        var IdCardNum = $("#IdCardNum").val();
        var bl = false;
        if (IdType != "0" && IdCardNum != "") {
            bl = true;
        }
        return bl;
    }
    $('#btnIdType').click(function () {
        var IdType = $("#IdType").val();
        var IdCardNum = $("#IdCardNum").val();
        
        if (checkIdTypeNull()) {
            $.ajax({
                url: '/Login/UpdateIdType?t=' + Math.random(),
                type: 'POST',
                data: {
                    IdType: IdType,
                    IdCardNum: IdCardNum
                },
                success: function (data) {

                    if (data.code == 0) {
                        layer.msg("证件信息修改成功！");
                        // window.location = '/Login/UpdatePwd';

                    } else {
                        layer.msg("证件信息修改失败！");
                    }

                }, error: function () {
                    layer.msg("证件信息修改失败");
                }
            });
        }
        else {
            layer.msg("请输入正确的证件信息！");
        }
        return false;
    });

    //
});
    //end修改密码

//级别认证
        $(function () { 
            $("#btn_submit").click(function () {
                $.ajax({
                    type: "POST",
                    url: "/YogisModels/LevelAuthentication",
                    data: $("#authform").serialize(),
                    success: function (result) {
                        if (result >= 1)
                        {
                            var level = "";
                            if (result == 1) {
                                level = "初级";
                            }
                            else if (result == 2) {
                                level = "中级";
                            }
                            else if (result == 3) {
                                level = "高级";
                            }
                            else {
                                layer.alert("升级失败，", { icon: 6 });
                            }
                            UpgradeEvent(level);
                        }
                        else if (result == 0) {
                            layer.alert("自评分数不足以升级，升级失败", { icon: 6 });
                        }
                        else {
                            layer.alert("级别异常，无法升级", { icon: 6 });
                        }
                        
                    },
                    error: function (result) {
                        layer.alert("提交失败",{icon:6});
                    }
                });

            });
            // UpgradeEvent("大师");
        });
function UpgradeEvent(dslevel)
{
    layer.confirm("您好！根据您的自评选项，综合评测，你现已达到" + dslevel + "导师级别。请您选择是否上传资质证明，进一步完成级别的认证。", {
        btn: ["提交", "完成"],title:"提示",icon:3
    }, function () {
        //资质认证界面               
        location.href = "/YogisModels/AuditEdit";
    }, function () {
        //完成
        UpgradeEvent2();
    });
}
function UpgradeEvent2() {
    layer.confirm("如果不进行一下步提交上传资料，直接完成您的级别状态将不会晋升，是否继续？", {
        btn: ["继续提交", "完成"], title: "提示", icon: 3
    }, function () {
        //资质认证界面
        location.href = "/YogisModels/AuditEdit";
    });
}

//end级别认证