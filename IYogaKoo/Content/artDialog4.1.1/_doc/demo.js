document.title += ' ' + art.dialog.fn.version;

// 运行代码
$.fn.runCode = function () {
	var getText = function(elems) {
		var ret = "", elem;

		for ( var i = 0; elems[i]; i++ ) {
			elem = elems[i];
			if ( elem.nodeType === 3 || elem.nodeType === 4 ) {
				ret += elem.nodeValue;
			} else if ( elem.nodeType !== 8 ) {
				ret += getText( elem.childNodes );
			};
		};

		return ret;
	};
	
	var code = getText(this);
	new Function(code).call(window);
	
	return this;
};

$(function(){
	// 按钮触发代码运行
	$(document).bind('click', function(event){
		var target = event.target,
			$target = $(target);

		if ($target.hasClass('runCode')) {
			$('#' + target.name).runCode();
		};
	});
	
	// 跳转到头部
	var $footer = $('#footer');
	if ($footer[0]) $footer.bind('click', function () {
		window.scrollTo(0, 0);
		return false;
	}).css('cursor', 'pointer')[0].title = '回到页头';
});

// 皮肤选择	
window._demoSkin = function () {
	art.dialog({
		id: 'demoSkin',
		padding: '15px',
		title: 'artDialog皮肤展示',
		content: _demoSkin.tmpl
	});
};
_demoSkin.tmpl = function (data) {
	var html = ['<table class="zebra" style="width:480px"><tbody>'];
	for (var i = 0, length = data.length; i < length; i ++) {
		html.push('<tr class="');
		html.push(i%2 ? 'odd' : '');
		html.push('"><th style="width:7em"><a href="?demoSkin=');
		html.push(data[i].name);
		html.push('">');
		html.push(data[i].name);
		html.push('</a></th><td>');
		html.push(data[i].about);
		html.push('</td></tr>');
	};
	html.push('</tbody></table>');
	return html.join('');
}([
	{name: 'default', about: 'artDialog默认皮肤，简洁，纯CSS设计，无图片，采用css3渐进增强'},
	{name: 'aero', about: 'artDialog 2+标志性的皮肤，windows7毛玻璃风格。提供PSD源文件 <a href="http://code.google.com/p/artdialog/downloads/detail?name=aero.psd&can=2&q=" target="_blank">下载</a>'},
	{name: 'chrome', about: 'chrome浏览器(xp)风格'},
	{name: 'opera', about: 'opera 11浏览器内置对话框风格'},
	{name: 'simple', about: '简单风格，无图片，不显示标题'},
	{name: 'idialog', about: '苹果风格，iPad Safari或Mac Safari关闭按钮将在左边显示'},
	{name: 'twitter', about: 'twitter风格，无图片'},
	{name: 'blue', about: '蓝色风格'},
	{name: 'black', about: '黑色风格'},
	{name: 'green', about: '绿色风格'}
]);

$(function () {
	var $skin = $('#nav-skin');
	$skin[0] && $skin.bind('click', function () {
		_demoSkin();
		return false;
	});
});

// firebug
(function () {
var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
ga.src = 'http://getfirebug.com/firebug-lite-beta.js';
var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();

// google-analytics
var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-19823759-2']);
_gaq.push(['_setDomainName', '.planeart.cn']);
_gaq.push(['_trackPageview']);
(function() {
var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();
