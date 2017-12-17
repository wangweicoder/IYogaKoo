
        //判断是否登录
        function isLoginc() {
            var rs = null;
            jQuery.ajax(
                   {
                       url: "/home/islogin",
                       type: "get",
                       success: function (data, success) {
                           if (data == "1")//已登录
                           {
                               rs = true;
                             

                           } else//-1
                           {
                               alert("登录后才可以升级导师！");
                               //getpreg();
                               window.location="/Login/Login";// 原来窗口打开
                               rs = false;
                           }
                       }, async: false//同步请求，返回值才不为null
                   });
            return rs;
        }
$(function() {
    //$("#sjds").click(function () {
    //    if (!isLoginc())
    //        return false;
    //});//点升级导师
           
    teachprof();
    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            $("#btsearch").click();
        }
    });
    
    //------------------------------------------------------------//
    //模板 查询
    $("#btsearchLayOut").click(function () {
        var setext = $("#txtsearchLayOut").val().trim();
        $("#hid_strwhere").val(setext);
        var centertype=$("#hidType").val();//区分导师和结构搜索
        if (centertype == undefined) {
            if (setext != '') {
                 window.open("/Home/Search/?urlContent=" + setext + "&t=" + Math.random(),"_self");           
            }
        } else {
            if (setext != '') {
                var centertypeid = $("#hidiType").val().trim();
                window.open("/Mechanism/Search/?urlContent=" + setext + "&iType=" + centertypeid + "&t=" + Math.random());
            }
        }
        
     
      
    });
    //找活动
    $("#btnActivityLayOut").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();
        if (setext != '') {
            window.open("/Home/Search/?urlContent=" + setext + "&id=3&t=" + Math.random());

        }
    });
    //找导师 
    $("#btnModelsLayOut").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();
        if (setext != '') {
            window.open("/Home/Search/?urlContent=" + setext + "&id=2&t=" + Math.random());
            
        }
    });
    // 找同修
    $("#btnYogaUserLayOut").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();
        if (setext != '') {
            window.open("/Home/Search/?urlContent=" + setext + "&id=1&t=" + Math.random());
           
        }
    }); 
    // 找资料
    $("#btnYogaArticleLayOut").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();
        if (setext != '') {
            window.open("/Home/Search/?urlContent=" + setext + "&id=4&t=" + Math.random());
             
        }
    });

    //------- --习练者.导师.机构.活动 下拉菜单 事件---------------//
    //start：习练者 
    //高级习练者
    $("#ThreeUser").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();
        
        window.open("/Home/Search/?urlContent=" + setext + "&id=1&iType=3&t=" + Math.random());
 
    });
    //中级习练者
    $("#SecondUser").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();

        window.open("/Home/Search/?urlContent=" + setext + "&id=1&iType=2&t=" + Math.random());

    });
    //初级习练者
    $("#Firstser").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();

        window.open("/Home/Search/?urlContent=" + setext + "&id=1&iType=1&t=" + Math.random());

    });
    //end

    //start：  导师
    //大师
    $("#DaoShiModels").click(function () {
       
        var setext = $("#txtsearchLayOut").val().trim();

        window.open("/Home/Search/?urlContent=" + setext + "&id=2&iType=4&t=" + Math.random());

    }); 
    //高级导师
    $("#ThreeModels").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();

        window.open("/Home/Search/?urlContent=" + setext + "&id=2&iType=3&t=" + Math.random());

    });
    //中级导师
    $("#SecondModels").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();

        window.open("/Home/Search/?urlContent=" + setext + "&id=2&iType=2&t=" + Math.random());

    });
    //初级导师
    $("#FirstModels").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();

        window.open("/Home/Search/?urlContent=" + setext + "&id=2&iType=1&t=" + Math.random());

    });
    //机构
    $("#xueyuan,#huiguan,#gongzuoshi").click(function () {

        var setext = $("#txtsearchLayOut").val().trim();
        var jg = $(this).attr("id");
        var centertypeid="1";
        if (jg == "会馆")
            centertypeid = "1";
        else if (jg = "xueyuan")
            centertypeid = "2";
        else
            centertypeid = "3";
        window.open("/Mechanism/Search/?urlContent=" + setext + "&iType=1&t=" + Math.random());

    });
     
    //end
});
//显示审核中
function teachprof() {            
    jQuery.ajax(
                {
                    url: "/home/isTeachProf",
                    data: { uid: $("#hiduid").val() },
                    type: "get",
                    success: function (data, success) {
                        if (data == "1")
                        {
                            //$("#sjds").text("审核中");
                            //$("#sjds").attr("href", "javascript:");
                            $("#sjds").after("<a id='sjds1' href='javascript:;'>审核中</a>");
                            $("#sjds").remove();
                        }
                    }, async: false//
                });
}


$(document).ready(function () {
    $('#sjds1').click(function () {
        //var thisname = $('#sjds1').html();
        //if(thisname=="审核中"){
            //alert('您好！您的资料已提交，我们将会在三个工作日内以邮件的形式告诉您审核结果！');
            window.open("/YogisModels/Audit");
        //}
    })

})


  