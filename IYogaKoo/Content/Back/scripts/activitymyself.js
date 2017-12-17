$(function () {
    $("#select-teacher").click(function () {
        ShowBox('/class/_PartialTeacherWall', "选择上课导师", null, function () {
            LoadClassTeacher(1);
        });
    });
    $("#add-teacher").click(function () {
        ShowBox('/class/_PartialAddClassTeacher', "添加上课导师", null, function () {
            var setting = uploadifySetting;
            setting.onUploadSuccess = function (file, data, response) {
                if (response) {
                    $('#add-class-teacher #Avatar').val(data);
                    $('#add-class-teacher #avatar-img').attr('src', data);
                    $('#progress').html('100 %');
                }
            };
            setting.post_params = { 'category': '/class/teacher' };
            $('#add-class-teacher #select-file').uploadify(uploadifySetting);
        });
    });
    //兴趣
    $(document).on('click', '.addInterest', function () {
        var classId = $('#classId').val();
        var btnInterest = this;
        if (classId > 0)
            Class.AddInterest(classId, function (result) {
                if (result.Id > 0)
                    $(btnInterest).toggleClass('interested');
            });
        else
            ShowError('没有活动');
    });
    $('body').on("keypress", ".search-teacher input:text", function () {
        if (event.keyCode == 13)
            GetTeachers();
    });
    var selectTeacherLayIndex;
    var addSapceLayIndex;
    $('body').on("click", ".teacher-wall .teacher", function () {
        var teacher = {};
        teacher.ID = $(this).attr("id").split('_')[1];
        teacher.Name = $(this).find("h5").text();
        teacher.Avatar = $(this).find(".avatar").attr("src");
        layer.close(selectTeacherLayIndex);
        if ($(".teachers .teacher img").length > 0) {
            $(".teachers .teacher img").attr("src", teacher.Avatar);
            $(".teachers .teacher img").attr("title", teacher.Name);
            $("#TeacherID").val(teacher.ID);
        }
        else {
            $(".teachers .teacher").append("<img src='" + teacher.Avatar + "' title='" + teacher.Name + "' />");
            $("#TeacherID").val(teacher.ID);
        }
        $(".teachers .teacher").removeClass("noteacher");
        return false;
    });
    //标签，选择
    //多选
    //$('.tags .tag').click(function () {
    //    if ($(this).hasClass('selected'))
    //        $(this).removeClass('selected');
    //    else
    //        $(this).addClass('selected');
    //    var tags = $('.tags .tag.selected');
    //    var tagids = "";
    //    for (var i = 0; i < tags.length; i++) {
    //        if (i == 0)
    //            tagids = $(tags[i]).attr('id').split('_')[1];
    //        else
    //            tagids += "," + $(tags[i]).attr('id').split('_')[1];
    //    }
    //    $('#TopicIds').val(tagids);
    //});
    $('.tags .tag').click(function () {
        $('.tags .tag').removeClass('selected');
        $(this).addClass('selected');
        var tags = $('.tags .tag.selected');
        var tagids = "";
        for (var i = 0; i < tags.length; i++) {
            if (i == 0)
                tagids = $(tags[i]).attr('id').split('_')[1];
            else
                tagids += "," + $(tags[i]).attr('id').split('_')[1];
        }
        $('#TopicIds').val(tagids);
    });
    //地区选择
    $(document).on('change', '.select-area', function () {
        var index = $('.select-area').index($(this));
        $('.select-area:gt(' + index + ')').remove();
        $(this).after('<select class="select-area"></select>');
        BindArea(this, true);
    });
    $('body').on("click", ".teachers .teacher .remove", function (event) {
        $(".teachers .teacher img").remove();
        $(".teachers .teacher").addClass("noteacher");
        $("#TeacherID").val(0);
        return false;
    });
    $('body').on("submit", "form#add-class-teacher", function () {
        var options = {
            success: function (json) {
                if (json.Id > 0) {
                    layer.closeAll();
                    HtmlClassTeacher(json);
                }
                else {
                    $('.error').html("添加失败，请重试");
                }
            },
            url: '/class/_PartialAddClassTeacher',
            dataType: 'json'
        };
        $('form#add-class-teacher').ajaxSubmit(options);
        return false;
    });
    $('body').on('click', '.teacher-close', function () {
        $(this).parent().slideUp('fast', function () {
            $(this).remove();
        });
    });
    $('body').on('click', '.Center-close', function () {
        $(this).parent().slideUp('fast', function () {
            $(this).remove();
        });
    });
    $('#add-activity').submit(function () {
        if (!CheckActivityInput())
            return false;
        $('#AreaID').val($('.select-area:last').val());
        var options = {
            success: function (json) {
                if (json.Id > 0) {
                    window.location.href = "/manage/class";
                }
                else {
                    $('.error').html("添加失败，请重试");
                }
            },
            url: '/manage/class/AddActivity',
            dataType: 'json'
        };
        $('#add-activity').ajaxSubmit(options);
        return false;
    });
    $('#edit-activity').submit(function () {
        var tags = $('.tags .tag.selected');
        var tagids = "";
        for (var i = 0; i < tags.length; i++) {
            if (i == 0)
                tagids = $(tags[i]).attr('id').split('_')[1];
            else
                tagids += "," + $(tags[i]).attr('id').split('_')[1];
        }
        $('#TopicIds').val(tagids);
        if (!CheckActivityInput())
            return false;
        $('#AreaID').val($('.select-area-edit:last').val());
        var options = {
            success: function (json) {
                if (json.Id > 0) {
                    window.location.href = "/manage/class";
                }
                else {
                    $('.error').html("修改失败，请重试");
                }
            },
            url: '/manage/class/EditActivity',
            dataType: 'json'
        };
        $('#edit-activity').ajaxSubmit(options);
        return false;
    });
    $('#save-banner').click(function () {
        var banner = $('#uploaded-image img').attr('src');
        if (banner == undefined || banner == '') {
            ShowError("请上传要展示的海报图片");
            return false;
        }
        PostData('/class/addactivitybanner', { id: $('#classId').val(), "banner": banner }, function (result) {
            if (result.Code == 0)
                window.location.href = "/Class/AddActivityView?id=" + $('#classId').val();
            else
                ShowError(result.Message);
        });
    });
    //选择搜索到的导师
    $('body').on('click', '.teacher-wall .op-add', function () {
        var tds = $(this).parent().parent().children('td');
        if ($('.teachers .teacher[teacherid=' + $(tds[0]).text() + ']').length == 0) {
            var classTeacher = {};
            classTeacher.TeacherId = $(tds[0]).text();
            classTeacher.Avatar = $(tds[1]).find('img').attr('src');
            classTeacher.Name = $(tds[2]).find('div').attr('title');
            classTeacher.Gender = $(tds[3]).text();
            classTeacher.Country = $(tds[4]).text();
            classTeacher.YogaSystem = $(tds[5]).text();
            classTeacher.Info = $(this).parent().parent().attr('title');
            PostData('/class/_PartialAddClassTeacherOfManager', classTeacher, function (result) {
                if (result && result.Id > 0) {
                    classTeacher.Id = result.Id;
                    $('.teachers').append(HtmlClassTeacher(classTeacher));
                }
                else
                    ShowError(result.Message);
            });

        }
        else {
            ShowError('已添加过这个导师了');
        }
    });
    //兴趣列表，取消兴趣关注
    $('body').on('click', '#interest-list .interest .cancel', function () {
        var thisInterest = $(this).parents('.interest');
        var classId = $(this).attr('id').split('-')[1];
        Class.DeleteInterest(classId, function (result) {
            if (result.Code == 0) {
                thisInterest.remove();
            }
            else {
                Msg(result.Message);
            }
        });
    });
});
//common
//activity
var Class = {
    //兴趣
    AddInterest: function (classId, callback) {
        if (!isNaN(classId)) {
            PostData('/class/AddInterest', { "classId": classId }, callback);
        }
    },
    DeleteInterest: function (classId, callback) {
        if (!isNaN(classId)) {
            PostData('/class/DeleteInterest', { "classId": classId }, callback);
        }
    },
    Interests: function (page, size, callback) {
        PostData('', { "page": page, "size": size }, callback);
    },
    Apply: function () {
        var apply = {};
        console.log($('#number').val() == '' || isNaN($('#number').val()) || $('#number').val() == 0);
        if ($('#number').val() == '' || isNaN($('#number').val()) || $('#number').val() == 0) {
            ShowError('参加人数要大于0');
            return;
        }
        else
            apply.Number = $('#number').val();
        if ($('#name').val() == '') {
            ShowError('请输入姓名');
            return;
        }
        else
            apply.Name = $("#name").val();
        if ($('#phone').val() == '' || $('#phone').val().length != 11) {
            ShowError('手机号不正确');
            return;
        }
        else
            apply.Phone = $("#phone").val();
        apply.Description = $('#description').val();
        apply.ClassId = $('#class-id').val();
        PostData('/class/apply', apply, function (json) {
            if (json.Code == 0) {
                window.location.href = "/class/pay?id=" + json.Obj.Id;
            }
            else {
                Msg(json.Message);
            }
        });
    }
};
//ajax common
function Loading() {
    //layer.msg('加载中', { icon: 16, time: 10000 });
}
function Loaded() {
    layer.closeAll('dialog');
}
function ShowError(msg) {
    layer.msg(msg, { icon: 5, time: 1400 });
}
function Msg(msg) {
    layer.msg(msg, { icon: 0, time: 1400 });
}
function GetData(url, param, callback) {
    Loading();
    $.get(url, param, function (result) {
        Loaded();
        if (callback)
            callback(result);
    }, "JSON");
}
function PostData(url, param, callback) {
    Loading();
    $.post(url, param, function (result) {
        Loaded();
        if (callback)
            callback(result);
    }, "JSON");
}
function HtmlOption(ele, items, clear) {
    if (clear == true) {
        $(ele).html('');
    }
    else {
        for (var i = 0; i < items.length; i++) {
            $(ele).append("<option vlaue='" + items[i].Id + "' " + items[i].Selected ? "selected" : "" + ">" + items[i].Name + "</option>")
        }
    }
}
function ShowBox(url, title, param, callback) {
    Loading();
    $.get(url, param, function (html) {
        Loaded();
        layer.open({ type: 1, area: ['660px', '475px'], title: title, shadeClose: false, content: html });
        if (callback != undefined)
            callback();
    });
}
function GetDate(dateStr, format) {
    var date = new Date();
    date.setDate(dateStr);
    return date.Format(format);
}
function ChangeDateFormat(cellval) {
    try {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    } catch (e) {
        return "";
    }
}
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//绑定位置信息
function BindArea(ele, forChild) {
    var id = $(ele).val();
    if (id == null)
        id = $('#city').val();
    GetData('/shared/getarea', { "id": id, "forChild": forChild }, function (data) {
        if (data.length != undefined && data.length > 0) {
            if (forChild == undefined)
                HtmlOptionFor($(ele), data);
            else if (forChild == true)
                HtmlOptionFor($(ele).next('.select-area'), data);
        }
        else {
            if (forChild == true)
                $(ele).next('.select-area').remove();
            else
                HtmlOptionFor($(ele).prev('.select-area'), data);
        }
    });
}


function BindAreaSelect(ele, pid) {
    GetData('/shared/getarea', { "id": pid, "forChild": true }, function (data) {
        if (pid != -1 && data.length != undefined && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                $(ele).append('<option value="' + data[i].Value + '">' + data[i].Text + '</option>');
            }
        }
    });
}
function HtmlOptionFor(ele, data) {
    $(ele).append('<option value="-1">不限</option>');
    for (var i = 0; i < data.length; i++) {
        $(ele).append('<option value="' + data[i].Value + '">' + data[i].Text + '</option>');
    }
}
function GetTeachers() {
    LoadClassTeacher(1);
    return false;
}
function CheckClassTeacher() {
    if ($('#add-class-teacher').val() == '') {
        ShowError("请输入导师名字");
        return false;
    }
    if ($('#add-class-teacher').val() == '') {
        ShowError("请输入导师简介");
        return false;
    }
}
function CheckActivityInput() {
    if ($('#TopicIds').val() == "") {
        ShowError("请选择活动类型");
        $('.tags .tag').focus();
        return false;
    }
    if ($('#Name').val() == "") {
        ShowError("请输入活动标题");
        $('#Name').focus();
        return false;
    }
    if ($('#Start').val() == "") {
        ShowError("请选择活动开始时间");
        $('#Start').focus();
        return false;
    }
    if ($('#Duration').val() == "") {
        ShowError("请选择活动开始时间");
        $('#Start').focus();
        return false;
    }
    if ($('.select-area:last').val() == "-1") {
        ShowError("请选择活动地区");
        $('.select-area:last').focus();
        return false;
    }
    if ($('#Address').val() == "") {
        ShowError("请输入活动详细地址");
        $('#Address').focus();
        return false;
    }
    if ($('#Summary').val() == "") {
        ShowError("请输入活动概述");
        $('#Summary').focus();
        return false;
    }
    if ($('#Price').val() == "") {
        ShowError("请输入活动费用");
        $('#Price').focus();
        return false;
    }
    if ($('#Summary').val() == "") {
        ShowError("请输入活动概述");
        $('#Summary').focus();
        return false;
    }
    if ($('#Max').val() == "") {
        ShowError("请输入活动报名人数");
        $('#Max').focus();
        return false;
    }
    var teachers = $('.teachers .teacher');
    var teacherids = '';
    for (var i = 0; i < teachers.length; i++) {
        if (i == 0)
            teacherids =  $(teachers[i]).attr('id').split('_')[1] ;
        else
            teacherids += ',' +  $(teachers[i]).attr('id').split('_')[1];
    }
    if (teacherids == "") {
        ShowError("必须选择一个导师");
        return false;
    }
    $('#TeacherIds').val(teacherids);


    var centers = $('.centers .Center');
    var centerids = '';
    for (var i = 0; i < centers.length; i++) {
        if (i == 0)
            centerids = $(centers[i]).attr('id').split('_')[1];
        else
            centerids += ',' + $(centers[i]).attr('id').split('_')[1];
    }
    $('#CenterID').val(centerids);
    return true;
}
var uploadifySetting = {
    'swf': '/content/uploadify/uploadify-v3.1/uploadify.swf',
    'buttonImage': '/content/uploadify/uploadify-v3.1/uploadify-class.png',
    'queueSizeLimit': 1,
    'uploader': '/class/upload',
    'width': 135,
    'height': 40,
    'post_params': { 'category': '/class/banner/' },
    'fileTypeDesc': 'Image Files',
    'fileTypeExts': '*.gif; *.jpg; *.png; *.jpeg; *.bmp',
    'onUploadProgress': function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
        var per = Math.floor(totalBytesUploaded * 100 / totalBytesTotal);
        $('#progress').html(per + ' %');
    },
    'onUploadSuccess': function (file, data, response) {
        if (response) {
            $('#uploaded-image img').attr('src', data);
            $('#progress').html('100 %');
        }
    }
};
//选择活动导师
function LoadClassTeacher(page) {
    SearchList({
        url: '/class/getyogis', data: { text: $('.search-teacher input:text').val(), page: page, size: 10 }, row: function (dataItem) {
            var html = '<tr title="' + (dataItem.Info == null ? '' : dataItem.Info) + '">';
            html += '<td>' + dataItem.TeacherId + '</td>';
            html += '<td><img class="small-avatar" src="' + dataItem.Avatar + '"/></td>';
            html += '<td><div style="width:120px;overflow:hidden;" title="' + dataItem.Name + '">' + dataItem.Name.substring(0, 12) + (dataItem.Name.length > 12 ? '...' : '') + '</div></td>';
            html += '<td>' + dataItem.Gender + '</td>';
            html += '<td>' + dataItem.Country + '</td>';
            html += '<td>' + dataItem.YogaSystem + '</td>';
            html += '<td><a class="op-add">添加</a></td>';
            html += '</tr>';
            return html;
        }, msg: '没有找到导师', container: '.data-table', contentContainer: '.data-table tbody'
    });
}
//加载兴趣
function LoadInterests(page) {
    SearchList({
        url: '/class/interests', data: { page: page, size: 12 }, row: function (dataItem) {
            var html = '<div class="col-md-3 interest" id="interest-' + dataItem.Id + '">';
            html += '<div class="thumbnail">';
            html += '<img src="' + dataItem.Class.Banner + '" />';
            html += '<div class="caption">';
            html += '<h3>' + dataItem.Class.Name + '</h3>';
            html += '<p>' + dataItem.Class.Summary + '</p>';
            html += '<p><a href="/class/viewactivity?id=' + dataItem.ClassId + '" class="btn btn-success" role="button">查看活动</a> <a href="#" class="btn btn-default cancel" role="button" id="cancel-' + dataItem.ClassId + '">取消关注</a></p>';
            html += '</div></div></div>';
            html += '</div>';
            return html;
        }, msg: '还没有添加感兴趣的活动，或者登陆后再来看看', container: '#interest-list', contentContainer: '#interest-list'
    });
}
//加载活动报道
function LoadClass(page, code, args) {
    SearchList({
        url: '/class/index', data: { page: page, size: 12, code: code, args: args }, row: function (dataItem) {
            var html = '<div class="lb_School_index_index_left_index class-item">';
            html += '<div class="lb_School_index_index_left_index_left">';
            html += '<a href="/class/viewactivity?id=' + dataItem.Id + '" class="banner"><img src="' + dataItem.Banner + '"></a>';
            html += '</div>';
            html += '<div class="lb_School_index_index_left_index_right">';
            html += '<h3>' + dataItem.Name + '</h3>';
            var areas = dataItem.NoPassMsg.split('-');
            html += '<p><b>活动时间： </b><span>' + dataItem.Start + '</span><b>活动地点：</b> <span>' + areas[3] + areas[4] + '</span><b>活动费用：</b> <span>' + dataItem.Price + '</span></p>';
            html += '<div class="lb_School_index_index_left_index_p">';
            html += '<p>' + dataItem.Summary + '</p>';
            html += '</div>';
            html += '<div class="lb_School_index_index_left_index_right_b">';
            html += '<p><p class="p1">' + dataItem.User.NickName + '   <a>3天前发布</a></p></p>';
            html += '<p><p class="p2"><a href="">' + dataItem.InterestCount + '</a>人感兴趣 <a href="">132</a>人参加</p></p>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            return html;
        }, msg: '木有找到活动', container: '#class-list', contentContainer: '#class-list'
    });
}
//加载活动
function LoadReport(page, code, args) {
    SearchList({
        url: '/class/Reports', data: { page: page, size: 12, code: code, args: args }, row: function (dataItem) {
            var html = '<div class="lb_lb_hd_index_show_img_img">';
            html += '<div class="lb_lb_hd_index_show_img2_top">';
            html += '<a href="/class/viewactivity?id=' + dataItem.Id + '"><img src="' + dataItem.Banner + '"></a>';
            html += '</div>';
            html += '<div class="lb_lb_hd_index_show_img2_bott">';
            html += '<p><a href="/class/viewactivity?id=' + dataItem.Id + '">' + dataItem.Name + '</a></p>';
            html += '<p><a href="/class/viewactivity?id=' + dataItem.Id + '">' + dataItem.Content + '&nbsp;' + dataItem.Start + '</a><span><a>42</a>人参加</span></p>';
            html += '</div>';
            html += '</div>';
            return html;
        }, msg: '木有找到活动报道', container: '#class-list', contentContainer: '#class-list'
    });
}
function HtmlClassTeacher(obj) {
    var html = '<li class="teacher" id="teacher_' + obj.Id + '" teacherid=' + obj.TeacherId + '><div class="teacher-info"><div class="name"><img src="' + obj.Avatar + '"/> ' + obj.Name.substring(0, 12) + ' <span>' + obj.Gender + '</span> <span>' + obj.Country + '</span> <span>' + obj.YogaSystem + '</span></div></div><div class="teacher-close">&nbsp;</div></li>';
    $('.teachers').append(html);
}
