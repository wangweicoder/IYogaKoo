$(document).ready(function () {    
    //国家选择
    $("select[id='CountryID']").change(function () { getProvince(this) });/*当国家的下拉选单改变时*/
    $("select[id='ProvinceID']").change(function () { getCity(this) });/*当省份的下拉菜单改变时*/
    $("select[id='CityID']").change(function () { getDistrict(this) });/*当城市的下拉菜单改变时*/
});
//得到省份          
function getProvince(country) {
    var CountryId = $(country).val(); //取出CountryId                    
    $.post("/Admin/LoadCitys", { CountryId: CountryId }, function (data) {
        var provhtml = $("select[id='ProvinceID'] option").length;
        if (provhtml == 1)
            $("select[id='ProvinceID']").append(data);
        else {
            data = "<option value=''>--请选择--</option>" + data;
            $("select[id='ProvinceID']").html(data);
            $("select[id='CityID']").html(data);
            $("select[id='DistrictID']").html(data);
        }
    });
}
//得到城市
function getCity(provice) {
    var CountryId = $(provice).val();//取出ProvinceId
    $.post("/Admin/LoadCitys", { CountryId: CountryId }, function (data) {
        var cityval = $("select[id='CityID'] option").length;
        if (cityval == 1)
            $("#CityID").append(data);
        else {
            data = "<option value=''>--请选择--</option>" + data;
            $("select[id='CityID']").html(data);
        }
    });
}
//得到城区
function getDistrict(city) {
    var CountryId = $(city).val();//取出CityId
    $.post("/Admin/LoadCitys", { CountryId: CountryId }, function (data) {
        var distval = $("#DistrictID option").length;
        if (distval == 1)
            $("#DistrictID").append(data);
        else {
            data = "<option value=''>--请选择--</option>" + data;
            $("select[id='DistrictID']").html(data);
        }
    });
}
