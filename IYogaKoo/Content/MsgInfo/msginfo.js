$(function () {
    
//1.添加留言 
    $("#btnMsg").click(function () { 
    var totype = $(this).attr("totype");
    var hhid = $("#hhid").val();
    
    if (hhid != '' && hhid != 0) {
        // this.disabled="disabled";
        var str = $("#sContent").val();
        if (str.indexOf('em') != -1) {
            $("#sContent").val(replace_em(str));
        }

        var content = $("#sContent").val();
        if (content.length > 10 && content.length < 2000) {
            $.ajax({
                url: "/YogaUserDetail/AddMessageInfo",
                type: "post",
                // dataType: "json",
                data: {
                    sContent: content,
                    hidid: $("#hidid").val(),
                    totype: totype,
                    hidType: $("#hidType").val()
                },
                success: function (data) {
                    // alert(parseInt(data.code));
                    if (parseInt(data.code) == 0) {
                        //location.href = '/YogaUserDetail/Details/' + $("#hidid").val();
                        window.location.reload();
                        $("#sContent").val("");
                    } if (parseInt(data.code) === 2) {
                        
                        layer.msg('该信息已经存在', {
                            time: 1600
                        });
                    }
                    else {
                        // alert("评论失败！");
                    }
                },
                async: true,//同步操作，同步请求将锁住浏览器，用户其它操作必须等待请求完成才可以执行
            });
        } else {
            
            layer.msg('请正确评论', {
                time: 1600 
            });
            $("#sContent").focus();

        }
    }
    else {
        if (confirm("登录后才可以留言!")) {
            //window.open("/Login/Login");// 来打开新窗口
            window.location = "/Login/Login?ReturnUrl=" + '/YogaUserDetail/Details/' + $("#hidid").val();// 原来窗口
        }

    }
})
    //2.
    $('.emotion').qqFace({
        id: 'facebox', //表情盒子的ID
        assign: 'sContent', //给那个控件赋值
        path: 'face/'	//表情存放的路径
    });

    //3.//字符长度判断
    var counter = $("#sContent").val().length; //获取文本域的字符串长度
    $(".lb_School_textarea_div_right a").text(counter);

    $("#sContent").keyup(function () {
        var text = $(this).val();
        var counter = text.length;
        if (counter > 500) {
            $(this).val($(this).val().substr(0, 500));
        }
        counter = $(this).val().length;
        $(".lb_School_textarea_div_right a").text(counter);    //每次减去字符长度
    });

});

//查看结果
function replace_em(str) {
    str = str.replace(/\</g, '&lt;');
    str = str.replace(/\>/g, '&gt;');
    str = str.replace(/\n/g, '<br/>');
    str = str.replace(/\[em_([0-9]*)\]/g, ' <img src="../../Content/Front/js/face/$1.gif" border="0" />');
    //alert(str);
    return str;
}


function over(obj) {
    obj.style.background = "#ffaa00";
}

function out(obj) {
    obj.style.background = "#FFFFFF";
}
function selface(obj) {
    parent.InsertFace(obj); //插入表情
    parent.HideFaceSheet(); //隐藏表情框
}
// 函数： InsertFace
// 参数： obj
// 功能： 表情
function InsertFace(obj) {
    var imgpath = obj.src;
    var imgalt = obj.alt;
    var txtEdit = $('#sContent');
    //var strHtml = "<img src='" + imgpath + "' alt='" + imgalt + "'/>";
    var msg = $('#sContent').html();
    txtEdit.html(msg);
    txtEdit.focus();
}
  
//回复发表
function btnFaBiao(FromUid, id) {
    var hhid = $("#hhid").val();
     var totype = $(this).attr("totype");
    if (hhid != '' && hhid != 0) {
        var itemid = "#FabiaoContent" + id;
        $.ajax({
            url: "/YogaUserDetail/AddFaBiaoInfo",
            type: "post",
            // dataType: "json",
            data: {
                Uid: FromUid,
                sContent: $(itemid).val(),
                parentID: id,
                totype: totype
            },
            success: function (data) {
                // alert(parseInt(data.code));
                if (parseInt(data.code) == 0) {
                    $(itemid).val("");
                    window.location.reload();
                    //location.href = '/YogaUserDetail/Details/' + $("#hidid").val();
                } if (parseInt(data.code) === 2) {
                   
                    layer.msg('该信息已经存在', {
                        time: 1600
                    });
                }
                else {
                    // alert("评论失败！");
                }
            },
            async: true,//同步操作，同步请求将锁住浏览器，用户其它操作必须等待请求完成才可以执行
        });
    } else {
         
        layer.confirm('登录后才可以回复？', {
            btn: ['登录', '不登录'] //按钮
        }, function () {
            window.location = "/Login/Login?ReturnUrl=" + '/YogaUserDetail/Details/' + $("#hidid").val();// 原来窗口
        });
    }
}