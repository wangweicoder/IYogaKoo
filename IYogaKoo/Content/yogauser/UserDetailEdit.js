
function checkr() {
    if ($("#RealName_cn").val().length == 0) {
        layer.msg("请填写中文的真实姓名");
        return false;
    }
    if ($("#ddlCountryID").val() == "-1") {
        layer.msg("请选择国家");
        return false;
    }

}
$(function () {
    
    //保存个人信息
    $("#subuserinfo").click(function () {
        var data = $("#fuserinfo").serialize();
        $.ajax({
            type: "POST",
            url: '/Login/UserDetailEditper' ,
            data: data ,
            success: function (msg) {               
                if (msg == "true")
                {
                    layer.msg("修改成功");
                    window.location.href = location.href;
                }
                else {
                    layer.msg("error");
                }
            },
            error: function (xhr, msg, e) {
                layer.msg("error");
            }
        });
    });
    //end保存个人信息
    //弹出流派选择
    $("#btnYogaTypeid").click(function () {

        art.dialog.open('/Login/LoadYogaTypeidList', {
            title: '选择流派',
            lock: true,
            width: 600,
            hight: 400,
            close: function () {
                var ID = art.dialog.data('ID', ID);
                var ItemName = art.dialog.data('ItemName', ItemName);

                if (ID !== undefined) {
                    document.getElementById('hYogaTypeid').value = ID;
                }
                if (ItemName !== undefined) {
                    document.getElementById('YogaTypeid').value = ItemName;//显示名称
                }
            }
        });
    });
    //end弹出流派选择
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
    //
});
///

///修改密码
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
    $("#BirthYear").bind("focus click", function () {

        WdatePicker({ dateFmt: 'yyyy' })
    });
    $("#BirthMonth").bind("focus click", function () {

        WdatePicker({ dateFmt: 'MM' })
    });
    $("#BirthDay").bind("focus click", function () {

        WdatePicker({ dateFmt: 'dd' })
    });
    $("#StartYear").bind("focus click", function () {

        WdatePicker({ dateFmt: 'yyyy' })
    });

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
    //级别提交
    $("#sublevel").click(function () {
        var data = $("#flevel").serialize();
        var hidlevel = $("#hidlevel").val();//判断当前审核状态
        if (hidlevel == "1") {
            layer.msg("当前状态为审核中，请等待审核");
        }else{
        $.ajax({
            type: "POST",
            url: '/YogaUserDetail/LevelAuthentication1',
            data: data,
            dateType:'text',
            success: function (msg) { 
                if (msg != "-1") {
                    //layer.msg("修改成功");
                    window.location.href = '/YogaUserDetail/PractitionersUpgrade?msg='+msg;
                    //if (msg == "0")
                    //{
                    //    $("#lableLevel").html("基础习练者");
                    //} else if (msg == "1") {
                    //    $("#lableLevel").html("初级习练者");
                    //} else if (msg == "2") {
                    //    $("#lableLevel").html("中级习练者");
                    //} else if (msg == "3") {
                    //    $("#lableLevel").html("高级习练者");
                    //}
                }
                else {
                    layer.msg("修改失败");
                }
            },
            error: function (xhr, msg, e) {
                layer.msg("修改失败:"+msg);
            }
        });
    }
    });
    //end级别提交

    ///隐私提交
    $("#subprivacy").click(function () {
        var data = $("#fprivacy").serialize();
        $.ajax({
            type: "POST",
            url: '/YogaUserDetail/PrivacySettings',
            data: data,
            success: function (msg) {              
                if (msg == "ture") {
                    layer.msg("修改成功");
                }
                else {
                    layer.msg("error");
                }
            },
            error: function (xhr, msg, e) {
                layer.msg("error" + xhr);
            }
        });
    });
    //end隐私提交
});
//