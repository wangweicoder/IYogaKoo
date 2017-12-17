function btnLogin() {
    //alert(window.location.href); 

    //var b = new Base64();

    var url = "http://localhost:54929";//http://www.iyogakoo.org
    var strUrl = window.location.href;
    var urlpath = strUrl.replace(url, "");
   
    //var str = b.encode(urlpath);
 
    window.location.href = "/Login/Login?ReturnUrl=" + urlpath;

}