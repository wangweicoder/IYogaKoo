function Loading(title) {
    layer.msg((title == undefined ? "加载中" : title), { icon: 16, time: 10000 });
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
function HtmlOption(ele,items,clear) {
    if (clear == true) {
        $(ele).html('');
    }
    else {
        for (var i = 0; i < items.length; i++) {
            $(ele).append("<option vlaue='"+items[i].Id+"' "+items[i].Selected?"selected":""+">"+items[i].Name+"</option>")
        }
    }
}
function ShowBox(url,title, param, callback) {
    Loading();
    $.get(url, param, function (html) {
        Loaded();
        layer.open({ type: 1, area: ['600px', '420px'], title: title, shadeClose: true, content: html });
        if (callback != undefined)
            callback();
    });
}                          
function GetDate(dateStr,format) {
    var date = new Date();
    date.setDate(dateStr);
    return date.Format(format);
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
function ToDate(dateStr) {
    dateStr = dateStr.replace('/Date(', '').replace(')/', '');
    var date = new Date(parseInt(dateStr));
    return date.Format('yyyy-MM-dd hh:mm');
}
var Class = {
    SetClassStatus: function (id,status,text,callback) {
        PostData('/manage/class/SetStatus', { "classId": id, "status": status ,"text":text}, function (json) {
            if (json.Code == 0) {
                if (callback != undefined)
                    callback(json);
            }
            else
                ShowError(json.Message);
        });
    }
};
