//$(document).ready(function () {
//    //国家选择
//    $("select[id='Teacher_CountryID']").change(function () { getProvince(this) });/*当国家的下拉选单改变时*/
//    $("select[id='ProvinceID']").change(function () { getCity(this) });/*当省份的下拉菜单改变时*/
//    $("select[id='CityID']").change(function () { getDistrict(this) });/*当城市的下拉菜单改变时*/
//});
////得到省份          
//function getProvince(country) {
//    var CountryId = $(country).val(); //取出CountryId                    
//    $.post("/Admin/LoadCitys", { CountryId: CountryId }, function (data) {       
//        var provhtml = $("select[id='ProvinceID'] option").length;        
//        if (provhtml == 1 && $("select[id='ProvinceID'] option").val()=="")
//            $("select[id='ProvinceID']").append(data);
//        else
//        {
//            data = "<option value=''>--请选择--</option>" + data;
//            $("select[id='ProvinceID']").html(data);
//            $("select[id='CityID']").html(data);
//            $("select[id='DistrictID']").html(data);
//        }
//    });
//}
////得到城市
//function getCity(provice) {
//    var CountryId = $(provice).val();//取出ProvinceId
//    $.post("/Admin/LoadCitys", { CountryId: CountryId }, function (data) {
//        var cityval = $("select[id='CityID'] option").length;
//        if (cityval == 1 && $("select[id='CityID'] option").val() == "")
//            $("#CityID").append(data);
//        else {
//            data = "<option value=''>--请选择--</option>" + data;
//            $("select[id='CityID']").html(data);          
//            $("select[id='DistrictID']").html(data);
//        }       
//    });
//}
////得到城区
//function getDistrict(city) {
//    var CountryId = $(city).val();//取出CityId
//    $.post("/Admin/LoadCitys", { CountryId: CountryId }, function (data) {
//        var distval = $("#DistrictID option").length;
//        if (distval == 1 && $("select[id='DistrictID'] option").val() == "")
//            $("#DistrictID").append(data);
//        else {
//            data = "<option value=''>--请选择--</option>" + data;
//            $("select[id='DistrictID']").html(data);
//        }
//    });
//}
//地区选择
$(document).on('change', '.select-area', function () {
    var index = $('.select-area').index($(this));
    $('.select-area:gt(' + index + ')').remove();
    $(this).after('<select class="select-area"></select>');
    BindArea(this, true);
});

//绑定位置信息
function BindArea(ele, forChild) {    
    var id = $(ele).val();
    alert(id);
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
//ajax common
function Loading() {
    layer.msg('加载中', { icon: 16, time: 10000 });
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