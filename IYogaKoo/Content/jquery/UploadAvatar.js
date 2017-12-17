var swfu;
window.onload = function () {
    swfu = new SWFUpload({
        upload_url: "/photo/UploadPhoto",
        post_params: { "swfuserId": document.getElementById('UID').value, "uptype": 1 },
        flash_url: "../../Content/SWFUpload/Flash/swfupload.swf",
        file_size_limit: "4 MB",
        file_types: "*.*",
        file_types_description: "All Files",
        file_upload_limit: 1,
        file_queue_limit: 0,
        debug: false,
        auto_upload: false,
        //Button settings
        button_image_url: "/Content/img/avatar.png",
        button_width: "50",
        button_height: "24",
        button_placeholder_id: "spanButtonPlaceHolder",
        button_text: '<span class="theFont"></span>',
        button_text_style: ".theFont { font-size: 16; }",
        button_text_left_padding: 12,
        button_text_top_padding: 3,


        file_dialog_complete_handler: function (numFilesSelected, numFilesQueued) {
        },
        file_queued_handler: function (file) {
            if (swfu.getStats().files_queued > 0) {
                //alert(swfu.getStats().files_queued);
                $("#fileProgress").dialog({
                    modal: true,
                    width: 400,
                    open: function () {
                        $("#progressbar").progressbar({
                            value: 0
                        });
                        swfu.startUpload();

                    }
                });
            }
        },
        upload_progress_handler: function (file, complete, total) {
            var value = complete / total * 100;
            $("#progressbar").progressbar("value", value);
            $("#CurrentSpeed").html(SWFUpload.speed.formatBPS(file.currentSpeed));
        },
        upload_success_handler: function (file, data) {
                        alert(data);
            if (data!=="false") {
                var appendhtml = '<img src=' + data + ' id="PhotoPath" alt="" height="64px;" width="64px"/>';
                $('#UploadPhotoImg').append(appendhtml);
            }
                           // alert("头像上传成功");
            $("#fileProgress").dialog("Progress");
            alert("头像上传成功");
        }

    });

    $("#cancelButton").click(function () {
        swfu.stopUpload();
        //刷新网页
        window.location.reload(false);
        $('#UploadPhotoImg').remove();
        $("#fileProgress").dialog("close");
    });
};



function savePhoto() {
    var PhotoName = $('#PhotoName').val().trim();
    var PhotoContent = $('#PhotoComtent').val().trim();
//    var CommentLimite = $('#CommentLimite').val().trim();
    var photoOriginal = $('#PhotoPath').attr('src');
    var uid = $('#UserId').val().trim();
    var url = "/photo/ajax_SavePhoto";
    $.post(
        url,
        {
            PhotoName: PhotoName,
            PhotoContent: PhotoContent,
//            CommentLimite: CommentLimite,
            photoOriginal: photoOriginal,
            userId: uid
        },
        function (date) {
            if (date.Code == 0) {
                alert(date.Message);
                //刷新网页
                window.location.reload(true);
            }
            else {
                alert(date.Message);
            }
        }
    );
}