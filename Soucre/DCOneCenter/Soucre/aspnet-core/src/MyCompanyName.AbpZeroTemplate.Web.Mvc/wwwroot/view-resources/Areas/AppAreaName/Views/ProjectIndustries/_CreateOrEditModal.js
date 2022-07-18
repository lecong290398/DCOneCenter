(function ($) {
    app.modals.CreateOrEditProjectIndustrieModal = function () {
        var _projectIndustriesService = abp.services.app.projectIndustries;

        var _modalManager;
        var _$projectIndustrieInformationForm = null;

        var _fileUploading = [];
        var _logoToken;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            });

            _$projectIndustrieInformationForm = _modalManager.getModal().find('form[name=ProjectIndustrieInformationsForm]');
            _$projectIndustrieInformationForm.validate();
        };

        this.save = function () {
            if (!_$projectIndustrieInformationForm.valid()) {
                return;
            }

            if (_fileUploading != null && _fileUploading.length > 0) {
                abp.notify.info(app.localize('WaitingForFileUpload'));
                return;
            }

            var projectIndustrie = _$projectIndustrieInformationForm.serializeFormToObject();

            projectIndustrie.logoToken = _logoToken;

            _modalManager.setBusy(true);
            _projectIndustriesService
                .createOrEdit(projectIndustrie)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditProjectIndustrieModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('#ProjectIndustrie_Logo').change(function () {
            var file = $(this)[0].files[0];
            if (!file) {
                _logoToken = null;
                return;
            }

            var formData = new FormData();
            formData.append('file', file);
            _fileUploading.push(true);

            var url = abp.appPath + 'AppAreaName/ProjectIndustries/UploadLogoFile';


            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
            })
                .done(function (resp) {
                    if (resp.success && resp.result.fileToken) {
                        _logoToken = resp.result.fileToken;
                    } else {
                        this.message.error(resp.result.message);
                    }
                })
                .always(function () {
                    _fileUploading.pop();
                });
        });

        $('#ProjectIndustrie_Logo_Remove').click(function () {
            abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    var ProjectIndustrie = _$projectIndustrieInformationForm.serializeFormToObject();
                    _projectIndustriesService
                        .removeLogoFile({
                            id: ProjectIndustrie.id,
                        })
                        .done(function () {
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                            _$projectIndustrieInformationForm.find('#div_current_file').css('display', 'none');
                        });
                }
            });
        });

        $('#ProjectIndustrie_Logo').change(function () {
            var fileName = app.localize('ChooseAFile');
            if (this.files && this.files[0]) {
                fileName = this.files[0].name;
            }
            $('#ProjectIndustrie_LogoLabel').text(fileName);
        });
    };
})(jQuery);
