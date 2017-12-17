// JavaScript Document
$(document).ready(function () {
	$('#leftMarquee .tempWrap').css('width','560px');
		$('.lb_index_hdyg_top .lb_index_hdyg_top_right input').click(function(){
		$(this).css('border','0px');
		});
	$('.lb_index_scarh_class li ').hover(function(){
			$(this).children('div').slideToggle().parent().siblings('li').children('div').slideUp();
	});//首页的四个分类底下的小分类的显示隐藏
	var china=$('.lb_index_top_right_div .p').html();
	var eng=$('.lb_index_top_right_div .e').html();
	var center;
	$('.lb_index_top_right_div .e').mouseover(function(){
		$('.lb_index_top_right_div .p').show();
		$('.lb_index_top_right_div .p').click(function(){
			center=$(this).html();
			$('.lb_index_top_right_div .e').html($('.lb_index_top_right_div .p').html());
			$('.lb_index_top_right_div .p').html(eng).hide();
		})
	});/* 头部语言切换 */
		
		$(".lb_index_hdyg_center li").click(function(){//点击按钮
		//$(this).addClass("span_moren").siblings("span").removeClass("span_moren");//点击后当前的span按钮样式为span_moren,其他的取消这个span_moren的样式
		var thisIndex = $(this).index();//获取span的第几个排序，也就是eq
		$(".lb_index_hdyg_botimg_top").eq(thisIndex).fadeIn('slow').siblings(".lb_index_hdyg_botimg_top").hide();//内容div的排序按照按钮排序，对应显示，其他的div隐藏
		});
    //导师专业中的留言和推荐切换
		$('.lb_School_pinglun_tab li').click(function () {
		    $(this).addClass('li').siblings().removeClass('li');
		    var thisindexa = $(this).index();
		    $(".lb_School_textarea").eq(thisindexa).fadeIn('slow').siblings(".lb_School_textarea").hide();
		});
    //个人信息页面的左侧导航鼠标路过变色
		$('.lb_left_lbb_left li').mouseover(function () {
		    var num = $(this).index();
		    if (num == 0) {
		        $(this).css('border-top', '1px solid #0E633C');
		       // $(this).children('a').css('color', '#0E633C');
		    }
		    $(this).children('a').css('color', '#0E633C');
		}),
            $('.lb_left_lbb_left li').mouseout(function () {
                $(this).children('a').css('color', '#fff')
            });
    //取个人信息页面的右侧的div的高度 给左侧div赋值
		var heights = $('.lb_qq').outerHeight(true);
		$('.lb_left_lbb_left').css('height', heights);
    //找回密码邮箱手机找回切换
		$('.lb_red_cg2 li').click(function () {
		    $(this).addClass('on').siblings().removeClass('on');
		    var Index_num = $(this).index();
		    $('.lb_red_cg22').eq(Index_num).fadeIn().siblings('.lb_red_cg22').hide();
		}),

    //瑜伽导师个人主页左侧导航点击
    
		//$('.lb_gr_idnex_center_rizhi_left_l_b li').click(function () {
		//    $(this).addClass('on').siblings().removeClass('on')
		//    $(this).children('ul').slideDown();
		//    $(this).siblings('li').children('ul').slideUp();
		//})
		//$('.lb_gr_idnex_center_rizhi_left_l_b ul > li > ul >li').click(function () { 
		//    $(this).removeClass('on');
		//    //$(this).parent().parent().siblings().children().slideUp();
		     
    //});

    //发布活动免费收费
    $('#mianfei').click(function () {
    
        var s2 = $("input[name='select-price']").is(":checked");
       
        if (s2) {
            var sval = $("input[name='select-price']:checked").val();
            if (sval == "free") {
                $('#Price').val(0);
            }             
        }
        
        //if ($('.radio1').attr('checken')==true) {
        //    alert('ss');
      //  }
    })
    //我的瑜伽圈鼠标滑过
		//$('#wrap .info').mouseover(function () {
		//    $(this).children('.lb_gjds').children('a').css('color', '#fff');
		//    $(this).children('.lb_gjds').children('p').css('color', '#fff');
		//    $(this).children('.lb_gjds').css('color', '#fff');
		//});
		//$('#wrap .info').mouseout(function () {
		//    $(this).children('.lb_gjds').children('a').css('color', '#333');
		//    $(this).children('.lb_gjds').children('p').css('color', '#666');
		//    $(this).children('.lb_gjds').css('color', '#999');
		//})
		//$('#lb_add_lb input').on("input",function () {
		//   var v=  $(this).val();
		//    console.log(v);
		//})
	 
		//document.getElementById('txtAddYogaModels').addEventListener("input", function (o) {
		//    $('.lb_add_hi').fadeIn();
           
		//    //doSth  
		//    // ex. alert(myinput.value);                      
		//}, false);
    //关注粉丝列表切换
    $('.lb_add_gz_left li a').click(function () {
        var nal = $(this).parent('li').index();
        //$(this).addClass("a").parent().siblings().children().removeClass('a');
        if (nal == 3) {
            $(this).addClass('a42 a').parent().siblings().children().removeClass('a12 a');
            $('.lb_add_gz_left li').eq(0).children('a').addClass('a1');
        } else if (nal == 0) {
            $(this).addClass('a12 a').parent().siblings().children().removeClass('a42 a');
        } else if (nal == 4 || nal == 5) {
            $(this).addClass("a").parent().siblings().children().removeClass('a');
            $('.lb_add_gz_left li').eq(3).children('a').addClass('a a12');
            $('.lb_add_gz_left li').eq(0).children('a').addClass('a1');
        }
        else {
            $(this).addClass("a").parent().siblings().children().removeClass('a');
            $('.lb_add_gz_left li').eq(0).children('a').addClass('a a12');
            $('.lb_add_gz_left li').eq(3).children('a').removeClass('a42').addClass('a4');
        }

    })

    //机构详情页右侧“我来推荐”切换
    $('.lb_addnew_div_input input').click(function () {
        $('.lb_School_index_info_left_top_div').hide();
        $('.lb_new_add_indexhide').fadeIn();
    })
    $('#divtj #btnEval').click(function () {
        $('.lb_new_add_indexhide').hide();
        $('.lb_School_index_info_left_top_div').fadeIn();
    })

    // 个人专业 对他有影响的导师部分js
    $('.lb_add_img_img_div2 img').mouseover(function () {
        $(this).css('border', '2px solid #65C3A5').parent('a').siblings('.lb_add_new_img_hide2').children('p').css('color','#65c3a5');
    }).mouseout(function () {
        $(this).css('border', '2px solid #fff').parent('a').siblings('.lb_add_new_img_hide2').children('p').css('color', '#333');
    })

    $('#li1').click(function () {
        $(this).addClass('a12 a');
        $('#li2').removeClass('a42 a');
    })
    $('#li2').click(function () {
        $(this).addClass('a42 a');
        $('#li1').addClass('a1').removeClass('a12 a');
    })
    $('#li11 ul li a').click(function () {
       // alert(1)
        var name=$(this).parent().parent('').children('a').html()//.addClass('a1 a')
       // alert(name);
    })
    $('.lb_lb_School_zhuti li').click(function () {
        $(this).addClass('on').siblings().removeClass('on');
    })
    loadfooter();    
})

function loadfooter() {
    $.ajax({
        url: "/Shared/_ParialFooter",
        type: "get",
        dataType: "text",
        success: function (relust) {
            $('body').append(relust);
        }
    });
    



    //点击发表回复框隐藏
    /*$('.lb_School_qbpl_div_d_right_hide input').click(function () {        
        $(this).parent().slideUp();
        //$('.lb_School_qbpl_div_d_bottom_d').slideDown();
        $(this).parent().parent().parent().siblings('.lb_School_qbpl_div_d_bottom').children('.lb_School_qbpl_div_d_bottom_d').slideDown();
    });

    $('.lb_School_qbpl_div_d_bottom_d a').click(function () {
        var name = $(this).val();
        if (name = '回复') {
           $('.lb_School_qbpl_div_d_right_hide').slideDown();
        }
    })*/
    $('.lb_School_qbpl_div_d_bottom_d a').click(function () {
         eventshow(this);
    });
  

    $('.lb_School_qbpl_div_d_right_hide input').click(function () {
        eventhide(this);
    });
 
   
    $('.lb_teach_inde_div p').click(function () {
        $(this).addClass('on1').parent().siblings().children().removeClass('on1');
    });
    //个人信息左侧导航点击
    $('.lb_left_lbb_left li').click(function () {
        $(this).children('ul').slideDown();
        $(this).siblings().children('ul').slideUp();
    }),
    $('.lb_left_lbb_left li li').click(function () {
        var parentindex = $(this).parent().parent().index();
        $(this).addClass('li').siblings().removeClass('li');
        var thisindex = $(this).index();
        $('.lb_index_rightt_div').eq(parentindex).show().siblings('.lb_index_rightt_div').hide();
        $('.lb_index_rightt_div_jibenxx').eq(thisindex).show().siblings('.lb_index_rightt_div_jibenxx').hide();
    })
    $('.lb_left_lbb_left2 li').click(function () {
        $(this).parent().addClass('li')
        $(this).children('ul').slideDown();
        $(this).siblings().children('ul').slideUp();
    }),
$('.lb_left_lbb_left2 li li').click(function () {
    var parentindex = $(this).parent().parent().index();
    $(this).addClass('li').siblings().removeClass('li');
    var thisindex = $(this).index();
    $('.lb_index_rightt_div').eq(parentindex).show().siblings('.lb_index_rightt_div').hide();
    $('.lb_index_rightt_div_jibenxx').eq(thisindex).show().siblings('.lb_index_rightt_div_jibenxx').hide();
})
    $('.lb_left_lbb_left2 li a').click(function () {
        $(this).parent().addClass('li').siblings('li').removeClass('li');
    })
    $('.lb_index_top_right_ul li').mouseover(function () {
        $(this).children('.lb_li_idnex_hide').show().parent().siblings().children('.lb_li_idnex_hide').hide();
    }).mouseout(function () {
        $(this).children('.lb_li_idnex_hide').hide();
    }),
    $('.lb_index_rightt h2').click(function () {
        $(this).next('.lb_index_rightt_div_jibenxx1').slideDown().siblings('.lb_index_rightt_div_jibenxx1').slideUp();
    }),
    $('.lb_index_yujia_xlz_img_img').mouseover(function () {
        var a = $(this).index();
        if (a == 5 || a == 11 || a == 17) {
            $(this).children('.lb_index_yujia_xlz_img2_div').css('left','-100px');
        }
    }),
    //个人管理页面四大功能切换
			$('.lb_gr_index_top li').click(function () {			    
			    $(this).children('a').addClass('on').parent().siblings().children('a').removeClass('on');
			    var indexs = $(this).index() + 1;
			    //  $('.lb_gr_idnex_center_rizhi').eq(indexs).fadeIn().siblings('.lb_gr_idnex_center_rizhi').hide();
			    $("#div"+indexs).show().siblings('div').hide();
			}),
    $('.lb_gr_xc_left li a').click(function () {
        $(this).parent().addClass('on').siblings().removeClass('on');
        var thisIndex = $(this).parent().index();
        $('.wrapper1').eq(thisIndex).fadeIn().siblings('.wrapper1').fadeOut();
        if (thisIndex == 0) {
            $('#con1_1').masonry({
                itemSelector: '.product_list',
                columnWidth: 5
            });
        }
        else if (thisIndex == 1) {
            $('#con1_1bj').masonry({
                itemSelector: '.product_list',
                columnWidth: 5
            });
        }
        else {
            $('#con1_1hd').masonry({
                itemSelector: '.product_list',
                columnWidth: 5
            });
        }
    })
    //$('.lb_gr_idnex_center_rizhi li a').cl
    //个人管理页面的右侧日历
			$('.lb_bc_bottom a').click(function () {
			    $(this).addClass('on').siblings().removeClass('on');
			})
                    var min,max;
                    var d = new Date();
                    var vYear = d.getFullYear();
                    var vYear2 = vYear - 1;
                    var vYear3 = vYear2 - 1;
                    var month = d.getMonth() + 1;             
                    $('.lb_bc_bottom a').each(function () {
                      var thisindex=$(this).index()+1;
                        if(thisindex==month){
                            $(this).addClass('on');                          
                        }
                    })

                    $('.lb_bc_top_c').html(vYear);
                    $('.lb_bc_top_c2').html(vYear2);
                    //$('.lb_bc_top_c3').html(vYear3);
                    //$('.lb_bc_top_l').click(function(){
                    //    var num=$('.lb_bc_top_c').html();
                    //    num=num-1;
                    //    $('.lb_bc_top_c').html(num);
                    //})
                  /*  $('.lb_bc_top_r').click(function(){
                        var year=$('.lb_bc_top_c').html();
                        if(year==vYear){
                            alert('您过得有点快啦！');
                            return false;
                        }
                        var num=$('.lb_bc_top_c').html();
                        num++;
                        $('.lb_bc_top_c').html(num);
                    });

                    $('.lb_bc_top_l2').click(function () {
                        var num = $('.lb_bc_top_c2').html();
                        num = num - 1;
                        $('.lb_bc_top_c2').html(num);
                    })
                    $('.lb_bc_top_r2').click(function () {
                        var year = $('.lb_bc_top_c2').html();
                        if (year == vYear2) {
                            alert('您过得有点快啦！');
                            return false;
                        }
                        var num = $('.lb_bc_top_c2').html();
                        num++;
                        $('.lb_bc_top_c2').html(num);
                    });

                    $('.lb_bc_top_l3').click(function () {
                        var num = $('.lb_bc_top_c3').html();
                        num = num - 1;
                        $('.lb_bc_top_c3').html(num);
                    })
                    $('.lb_bc_top_r3').click(function () {
                        var year = $('.lb_bc_top_c3').html();
                        if (year == vYear3) {
                            alert('您过得有点快啦！');
                            return false;
                        }
                        var num = $('.lb_bc_top_c2').html();
                        num++;
                        $('.lb_bc_top_c2').html(num);
                    });
                    $('.top_jts a').click(function () {
                        
                        var shang = $('.lb_bc_top_c').html();
                        var xia = $('.lb_bc_top_c2').html();
                        if (shang == vYear || xia == vYear) {
                            alert('过得有点快啦！');
                            return false;
                        } else {
                            shang++;
                            xia++;
                        }
                        $('.lb_bc_top_c').html(shang);
                        $('.lb_bc_top_c2').html(xia);
                      
                    });
                    $('.top_jtx a').click(function () {
                        var shang = $('.lb_bc_top_c').html();
                        var xia = $('.lb_bc_top_c2').html();
                        shang2 = shang - 1;
                        xia2 = xia - 1;
                        $('.lb_bc_top_c').html(shang2);
                        $('.lb_bc_top_c2').html(xia2);
                    });
    */
                    $('#lb_bottom2 a').click(function () {
                        var yue = $(this).html();//取下面日历的月份
                        var nian = $(this).parent().siblings().children('.lb_bc_top_c2').html(); //去下面日历的年份
                      
                        formatedate(nian, yue);
                    })
                    $('#lb_bottom a').click(function () {
                        var yue2 = $(this).html();//取上面日历的月份
                        var nian2 = $(this).parent().siblings().children('.lb_bc_top_c').html(); //去上面日历的年份
                        formatedate(nian2, yue2);
                    });
                    $('#lb_bottom3 a').click(function () {
                        var yue2 = $(this).html();//取上面日历的月份
                        var nian2 = $(this).parent().siblings().children('.lb_bc_top_c3').html(); //去上面日历的年份
                        formatedate(nian2, yue2);
                    });
    //个人中心年份点击事件
    $('.lb_idnex_table_l p').click(function () {
        $('.lb_idenx_table_hide').slideToggle();
        $('.lb_idenx_table_hide p').click(function () {
            var vall = $(this).html();
            $('.lb_idnex_table_l p').html(vall);
            $('.lb_idenx_table_hide').hide();
        })
    });
                
    $('.product_list').mouseover(function () {
        $(this).children('.product_list_div').show();
    })
    $('.product_list').mouseout(function () {                     
        $(this).children('.product_list_div').hide();

    })
    $('.product_list_div img').click(function () {
        $(this).parent().siblings('div').slideToggle();
    })
    $('.lb_left_lbb_left2 li a').click(function () {
        var IndexThis = $(this).parent().index();
        $('.lb_add_index_div').eq(IndexThis).fadeIn().siblings().fadeOut();
    })




    //站内信   收到和发出的评论的切换
    $('.lb_znx_index_top li a').click(function () {
        var thisIndex = $(this).index();
        $(this).addClass('on1').c
        if (thisIndex == 1) {
            thisIndex = 0;
            $('.lb_add_znx_index').eq(thisIndex).fadeIn().siblings('.lb_add_znx_index').hide();
        }
        if (thisIndex == 2) {
            thisIndex = 1;
            $('.lb_add_znx_index').eq(thisIndex).fadeIn().siblings('.lb_add_znx_index').hide();
        }

    })
    //站内信左侧导航切换
    $('.lb_znx_index_bottom_left a').click(function () {
        $(this).addClass('on').siblings('a').removeClass('on');
        var thisIndex = $(this).index();
        if (thisIndex == 0) {
            $('.lb_znx_index_top li').eq(thisIndex).show().addClass('on');
            $('.lb_znx_index_top li').eq(1).hide();
            $('.lb_znx_index_top li').eq(2).hide();
        } else if (thisIndex == 3) {
            $('.lb_znx_index_top li').eq(0).hide();
            $('.lb_znx_index_top li').eq(1).show().addClass('on');
            $('.lb_znx_index_top li').eq(2).show();
        } else {
            $('.lb_znx_index_top li').hide();
        }
        $('.lb_znx_index_bottom_right_div').eq(thisIndex).fadeIn().siblings('.lb_znx_index_bottom_right_div').hide();
    })




}
function eventshow(myself) {
    
    var name = $(myself).html();
    var eid = $(myself).attr("eid");
    if (eid == undefined) {
        if (name == "回复") {
            $('.lb_School_qbpl_div_d_bottom_d').hide();
            $('.lb_School_qbpl_div_d_right_hide').slideDown();
        }
    }
    else {
        if (name == "回复") {
            $('#lb_School_qbpl_div_d_bottom_d' + eid).hide();
            $('#lb_School_qbpl_div_d_right_hide' + eid).slideDown();
        }
    }
    return false;
}
function eventhide(myself) {
    var vals = $(myself).val();
    var eid = $(myself).attr("eid"); 
    if (eid == undefined) {
        $('.lb_School_qbpl_div_d_bottom_d').show();
        $('.lb_School_qbpl_div_d_right_hide').slideUp();
    }
    else {
        if (vals == "取消") {
            $('#lb_School_qbpl_div_d_bottom_d' + eid).show();
            $('#lb_School_qbpl_div_d_right_hide' + eid).slideUp();
        }
    }
}




