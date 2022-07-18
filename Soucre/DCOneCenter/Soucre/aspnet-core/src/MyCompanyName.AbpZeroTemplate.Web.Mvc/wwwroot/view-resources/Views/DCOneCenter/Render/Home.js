(function () {
    var url = abp.appPath + 'DCOneCenter/LoadDataProjectIndex';
    $.ajax({
        url: url,
        type: 'POST',
        processData: false,
        contentType: false,
    })
        .done(function (resp) {
            if (resp.success && resp.result.fileToken) {

                debugger;
                console.log("Hazzzz")

            } else {
                this.message.error(resp.result.message);
            }
        })
        .always(function () {
            _fileUploading.pop();
        });

})();