﻿/*! layPage v1.2 - 分页插件 By 贤心 http://sentsin.com/layui/laypage MIT License */
; !function () { "use strict"; function a(d) { var e = "laypagecss"; a.dir = "dir" in a ? a.dir : f.getpath + "/skin/laypage.css", new f(d), a.dir && !b[c](e) && f.use(a.dir, e) } var b, c, d, e, f; a.v = "1.2", b = document, c = "getElementById", d = "getElementsByTagName", e = 0, f = function (a) { var b = this, c = b.config = a || {}; c.item = e++, b.render(!0) }, f.on = function (a, b, c) { return a.attachEvent ? a.attachEvent("on" + b, function () { c.call(a, window.even) }) : a.addEventListener(b, c, !1), f }, f.getpath = function () { var a = document.scripts, b = a[a.length - 1].src; return b.substring(0, b.lastIndexOf("/") + 1) }(), f.use = function (c, e) { var f = b.createElement("link"); f.type = "text/css", f.rel = "stylesheet", f.href = a.dir, e && (f.id = e), b[d]("head")[0].appendChild(f), f = null }, f.prototype.type = function () { var a = this.config; return "object" == typeof a.cont ? void 0 === a.cont.length ? 2 : 3 : void 0 }, f.prototype.view = function () { var b = this, c = b.config, d = [], e = {}; c.pages = 0 | c.pages, c.curr = 0 | c.curr || 1, c.groups = "groups" in c ? 0 | c.groups : 5, c.first = "first" in c ? c.first : 1, c.last = "last" in c ? c.last : c.pages, c.prev = "prev" in c ? c.prev : "上一页", c.next = "next" in c ? c.next : "下一页", c.groups > c.pages && (c.groups = c.pages), e.index = Math.ceil((c.curr + (c.groups > 1 && c.groups !== c.pages ? 1 : 0)) / (0 === c.groups ? 1 : c.groups)), c.curr > 1 && c.prev && d.push('<a href="javascript:;" class="laypage_prev" data-page="' + (c.curr - 1) + '">' + c.prev + "</a>"), e.index > 1 && c.first && 0 !== c.groups && d.push('<a href="javascript:;" class="laypage_first" data-page="1"  title="首页">' + c.first + "</a><span>…</span>"), e.poor = Math.floor((c.groups - 1) / 2), e.start = e.index > 1 ? c.curr - e.poor : 1, e.end = e.index > 1 ? function () { var a = c.curr + (c.groups - e.poor - 1); return a > c.pages ? c.pages : a }() : c.groups, e.end - e.start < c.groups - 1 && (e.start = e.end - c.groups + 1); for (; e.start <= e.end; e.start++) e.start === c.curr ? d.push('<span class="laypage_curr" ' + (/^#/.test(c.skin) ? 'style="background-color:' + c.skin + '"' : "") + ">" + e.start + "</span>") : d.push('<a href="javascript:;" data-page="' + e.start + '">' + e.start + "</a>"); return c.pages > c.groups && e.end < c.pages && c.last && 0 !== c.groups && d.push('<span>…</span><a href="javascript:;" class="laypage_last" title="尾页"  data-page="' + c.pages + '">' + c.last + "</a>"), e.flow = !c.prev && 0 === c.groups, (c.curr !== c.pages && c.next || e.flow) && d.push(function () { return e.flow && c.curr === c.pages ? '<span class="page_nomore" title="已没有更多">' + c.next + "</span>" : '<a href="javascript:;" class="laypage_next" data-page="' + (c.curr + 1) + '">' + c.next + "</a>" }()), '<div name="laypage' + a.v + '" class="laypage_main laypageskin_' + (c.skin ? function (a) { return /^#/.test(a) ? "molv" : a }(c.skin) : "default") + '" id="laypage_' + b.config.item + '">' + d.join("") + function () { return c.skip ? '<span class="laypage_total"><label>到第</label><input type="number" min="1" onkeyup="this.value=this.value.replace(/\\D/, \'\');" class="laypage_skip"><label>页</label><button type="button" class="laypage_btn">确定</button></span>' : "" }() + "</div>" }, f.prototype.jump = function (a) { var i, j, b = this, c = b.config, e = a.children, g = a[d]("button")[0], h = a[d]("input")[0]; for (i = 0, j = e.length; j > i; i++) "a" === e[i].nodeName.toLowerCase() && f.on(e[i], "click", function () { var a = 0 | this.getAttribute("data-page"); c.curr = a, b.render() }); g && f.on(g, "click", function () { var a = 0 | h.value.replace(/\s|\D/g, ""); a && a <= c.pages && (c.curr = a, b.render()) }) }, f.prototype.render = function (a) { var d = this, e = d.config, f = d.type(), g = d.view(); 2 === f ? e.cont.innerHTML = g : 3 === f ? e.cont.html(g) : b[c](e.cont).innerHTML = g, e.jump && e.jump(e, a), d.jump(b[c]("laypage_" + e.item)), e.hash && !a && (location.hash = "!" + e.hash + "=" + e.curr) }, "function" == typeof define ? define(function () { return a }) : "undefined" != typeof exports ? module.exports = a : window.laypage = a }();
function InitPage(data) {
    //分页
    $('#pageBox').html("");
    laypage({
        cont: $('#pageBox'), //容器。值支持id名、原生dom对象，jquery对象,
        curr: data.Index,
        pages: data.PageCount, //总页数
        skip: false, //是否开启跳页
        skin: 'molv',
        groups: 7, //连续显示分页数
        jump: function (e) {
            if (pageData.enablePage != undefined) {
                pageData.data.page = e.curr;
                SearchList(pageData);
            }
            else
                pageData.enablePage = true;
        }
    });
}
function NoSearchResult(info) {
    if ($('#noSearchResult').length==0) {
        var html = '<div class="alert alert-warning" role="alert" id="noSearchResult">不好意思!' + info + '</div>';
        $(pageData.contentContainer).hide().after(html);
        $('#pageBox').hide();
    }
}
function SearchList(fromPage) {
    $('#noSearchResult').remove();
    pageData = fromPage;
    Loading();
    $.post(pageData.url, pageData.data, function (result) {
        Loaded();
        if (result.Code == 0 && result.Objects.length>0) {
            $(pageData.contentContainer).html('');
            var trs = "";
            for (var i = 0; i < result.Objects.length; i++) {
                trs += pageData.row(result.Objects[i]);
            }
            $(pageData.contentContainer).html(trs);
            $(pageData.container).show();
            if (pageData.enablePage==undefined) {
                InitPage(result);
            }
            $('#pageBox').show();
        }
        else {
            NoSearchResult(pageData.msg);
        }
    }, 'json');
}
//保存查询参数
var pageData = {
    url: '',
    data: {},
    row: function () { },
    msg: '',
    container: '.data-table tbody',
    contentContainer: '.data-table'
};