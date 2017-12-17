// JavaScript Document

$(document).ready(function(){
	$('.lb_rig_div2 input').focus(function(){
		$(this).val(' ');
	}),
	$('.lb_rig_div2 input').blur(function(){
		var name=document.getElementById('lb_input').value;
		if(name==" "){
			$('#lb_input').val('瑜伽人、瑜伽机构、活动、文章');
		}
	})
	$('.lb_School_qbpl_div_d_bottom_d a').click(function () { 
	    var name = $(this).html();
	    var eid = $(this).attr("eid"); 
	    if (eid == undefined) {
	        if (name == "回复") {
	            $('.lb_School_qbpl_div_d_bottom_d').hide();
	            $('.lb_School_qbpl_div_d_right_hide').slideDown();
	        }
	    }
	    else {
	        if (name == "回复") {
	            $('#lb_School_qbpl_div_d_bottom_d'+eid).hide();
	            $('#lb_School_qbpl_div_d_right_hide'+eid).slideDown();
	        }
	    }
	})

	$('.lb_School_qbpl_div_d_right_hide input').click(function () {
	    var vals = $(this).val();  
	    if (vals == "取消") {
	        $('.lb_School_qbpl_div_d_bottom_d').show();
	        $('.lb_School_qbpl_div_d_right_hide').slideUp();
	    }
	})

    	 
	

})
$(document).ready(function () {
      //$('.zh').click(function () {
       //    getreg();        
	   //		$('.lb_dv , .lb_dc_dv, .lb_zhuce').show();
	   //		$('.lb_login').hide();
	   //	}),
		//$('.login').click(function(){
		//	$('.lb_dv , .lb_dc_dv, .lb_login ,.lb_login_bott').show();
		//	$('.lb_zhuce').hide();
		//}),
		//$('.lb_login_top a').click(function(){
		//		$('.lb_dv').hide();
		//		$('.lb_dc_dv').hide();
		//}),
		//$('.lb_login_bott dl dd p a').click(function(){
		//		$('.lb_login').hide();
		//		$('.lb_zhuce').show();
    //})   
		$('.lb_login_bott dd input').focus(function(){
			$(this).css('border','1px solid red');
		}),
		$('.lb_login_bott dd input').blur(function(){
			$(this).css('border','1px solid #ccc');
		});
		$('#ljzc').focus(function(){
			$(this).css('border','1px solid #54B746');
		}),
		$('#lbb_input').focus(function(){
			$(this).css('border','1px solid #54B746');
		})


       //loadfooter();
		
})

/* 登陆验证 */
function  check(){
    var name = document.getElementById('Email').value;
	var pwd=document.getElementById('pwd').value;
	if (name==''){
		alert('账号不能为空！');
		return false;
	}
	if(pwd==''){
		alert('密码不能为空！');
		return false;
	}
	return true ;
}

/* 表单验证 */
function check2(){
	var name=document.getElementById('nicheng').value;
	var email=document.getElementById('email').value;
	var password=document.getElementById('password').value;
	var pwsd=document.getElementById('pwsd').value;
	var ema= /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;//验证邮箱
	if(name==''){
		alert('昵称不能为空！');
		return false;
	}
	if(email==''){
		alert('邮箱不能为空！');
	}else if(!ema.test(email)){
		alert('您输入的用户名不合法！');
		return false;
	}
	if(password==''){
		alert('密码不能为空！')
		return false;
	}else if(password!=pwsd){
		alert('两次输入的密码不一致！重新输入！');
		$('#password').val('');
		$('#pwsd').val('');
		return false;
	}
	
	return true;
}

function check3(){
	var name2=document.getElementById('nicheng2').value;
	var email2=document.getElementById('email2').value;
	var password2=document.getElementById('password2').value;
	var pwsd2=document.getElementById('pwsd2').value;
	var phone=/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/; //验证手机
		if(name2==''){
		alert('昵称不能为空！');
		return false;
	}
	if(email2==''){
		alert('手机号不能为空！');
		return false;
	}else if(!phone.test(email2)){
		alert('您输入的手机号不合法！');
		return false;
	}
	if(password2==''){
		alert('密码不能为空！')
		return false;
	}else if( pwsd2==''){
		alert('两次输入的密码不一致！')
		return false;
	}else if(password2!=pwsd2){
		alert('两次输入的密码不一致！重新输入！');
		$('#password2').val('');
		$('#pwsd2').val('');
		return false;
	}
}


/*  头部导航js */
 function times(){
	var height=window.pageYOffset;
	 if(height>=125){
		 $('.lb_index2').show();
	 }else{
		  $('.lb_index2').hide();
	 }
}
 window.setInterval('times()',1)

    //var Time=60,    t;
    //var c=Time
    //function timedCount(){
    //    document.getElementById('button').value="请稍等(" + c + ")";
    //    document.getElementById('button').disabled="disabled";
	//	$('#botton').css('background','#ccc');
    //    c=c-1;
    //    t=setTimeout("timedCount()",1000)
    //    if(c<0){
    //        c=Time;
    //        stopCount();
    //        document.getElementById('button').value="获取验证码";
    //        document.getElementById('button').removeAttribute("disabled");
	//		$('#botton').css('background','#54B746');
    //    }
    //}
    //function stopCount(){
    //    clearTimeout(t);
    //}
///---20150327
 function getreg() {  
        jQuery.ajax({            
            url: "/register/_PartialRegisterUser",
            type: "GET",       
            contentType: "application/x-www-form-urlencoded",
            dataType: "text",
            success: function (relust) {                
                $('body').append(relust);                
                $(".lb_dv,.lb_dc_dv, .lb_zhuce").show();
                $('.lb_login').hide();                
                tab();
                $(".lb_login").remove();
                $(".a").click(function () {
                    $(".lb_dv, .lb_dc_dv, .lb_zhuce").remove();
                    $('.login').click(function () {
                        $('.lb_dv , .lb_dc_dv, .lb_login ,.lb_login_bott').show();
                        $('.lb_zhuce').hide();
                    });
                });

            }
        })
 }
 function loadfooter() {
     $.ajax({
         url: "/Shared/_ParialFooter",
         type: "get",
         dataType: "text",
         success: function (relust)
         {
             $('body').append(relust);
         }
     });

 }
 $(function(){
     $(".login").click(function () {        
         jQuery.ajax(
             {  
                 url: "/home/islogin",
                 type:"get",
                 success:function (data, success) {                     
                     if (data == "1")//已登录
                     {
                         //alert("已登录");                       
                     }
                     else//-1
                     {
                         getpreg();
                     }
                 },async:false
             });     
     });
     ///测试
     //$("#sjds").click(function()
     //{
     //    $.get("/register/ExistOauth", { openid: "12" }, function (data) {             
     //        if (data == false)
     //            alert("成功");
     //        else
     //            alert(data);        
         
     //    }, "josn");
     //})
     //退出登录
     $("#exit").click(function () {
         $.get("/home/Exitlogin", function (data) {             
             if (data == "True") {
                 getpreg();
             }
             else { }
         }, "josn");
     });
 });
 ///--返回登录view
     function getpreg()
     {
         jQuery.ajax({
             url: "/register/_PartialLogin",
             type: "GET",
             contentType: "application/x-www-form-urlencoded",
             dataType: "text",
             success: function (relust) {               
                 $('body').append(relust);
                 $('.lb_dv , .lb_dc_dv, .lb_login ,.lb_login_bott').show();
                 $('.lb_zhuce').hide();
                 $(".lb_zhuce").remove();
                 $(".lb_login_top a").click(function () {
                     $(".lb_dv, .lb_dc_dv, .lb_login, .lb_login_bott").remove();
                 });                
             }
         })
     }
     function tab()
     {
         $('.lb_login_bott dd p').click(function () {
             $('.lb_login_bott').hide();
             $('.lb_login_bott2').show();
         }),
         $('.lb_login_bott2 dd p').click(function () {
             $('.lb_login_bott2').hide();
             $('.lb_login_bott').show();
         })
     }
///导航跟随
     function times() {
         var heig = window.pageYOffset;
         if (heig > 300) {
             $('.lb_order_idnex_index').css('display', 'block')
         } else {
             $('.lb_order_idnex_index').css('display', 'none')
         }

     }
     window.setInterval('times()', 1);

///好评吐槽
     $(document).ready(function () {
         $('#lb_top li').click(function () {
             $(this).addClass('li').siblings().removeClass('li');
         }),
         $('.lb_tcs').click(function () {
             $('.lb_tc').show();
             $('.lb_hp').hide();
         }),
     $('.li').click(function () {
         $('.lb_hp').show();
         $('.lb_tc').hide();
     }),
     $('.szkc').click(function () {
         $('.lb_teacher_rig_div').show();
         $('.lb_zxhd').hide();
     }),
     $('.zxhd').click(function () {
         $('.lb_teacher_rig_div').hide();
         $('.lb_zxhd').show();
     }),
     $('.lb_tp').click(function () {
         window.open('demo.html', '图库集', 'height=auto,   width=980px,   top=0,   left=24,   toolbar=yes,   menubar=yes,   scrollbars=yes,   resizable=no,location=yes,   status=yes');
     })
     })
//


     $(document).ready(function () {
         $('.lb_login_idnex_bottom_ul li').click(function () {
             $(this).addClass('li').siblings('li').removeClass('li');
             var thisIndex = $(this).index();
             //start -- qiqi 2015-11-26 Add
             $("#preg_form #NickName").val("");
             $("#preg_form #Pwd").val("");
             $("#preg_form #erpwd").val("");
             $("#preg_form #Uphone").val("");
             $("#preg_form #phone_eCAPTCHA").val("");
             $("#preg_form #CAPTCHA").val("");
             
             ////
             $("#reg_form #NickName").val("");
             $("#reg_form #UEmail").val("");
             $("#reg_form #password").val("");
             $("#reg_form #rpassword").val("");
             $("#reg_form #eCAPTCHA").val("");
             
             //end
             $(".lb_login_idnex_bottom_index_left").eq(thisIndex).show().siblings(".lb_login_idnex_bottom_index_left").hide()
         })
     })


     $(".lb_School_index_info_left a").click(function () {//点击按钮
         $(this).addClass("a").siblings("a").removeClass("a");//点击后当前的span按钮样式为span_moren,其他的取消这个span_moren的样式
         //var thisIndex = $(this).index();/ / 获取span的第几个排序，也就是eq
         //$(".lb_index_hdyg_botimg_top").eq(thisIndex).fadeIn('slow').siblings(".lb_index_hdyg_botimg_top其实").hide();//内容div的排序按照按钮排序，对应显示，其他的div隐藏
     });
       