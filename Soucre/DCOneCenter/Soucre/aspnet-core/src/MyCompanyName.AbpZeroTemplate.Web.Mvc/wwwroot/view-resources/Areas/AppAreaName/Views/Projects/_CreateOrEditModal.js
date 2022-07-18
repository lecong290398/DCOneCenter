(function ($) {
  app.modals.CreateOrEditProjectModal = function () {
    var _projectsService = abp.services.app.projects;

    var _modalManager;
    var _$projectInformationForm = null;

    var _ProjectprojectStatuLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/Projects/ProjectStatuLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/AppAreaName/Views/Projects/_ProjectProjectStatuLookupTableModal.js',
      modalClass: 'ProjectStatuLookupTableModal',
    });
    var _ProjectprojectIndustrieLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/Projects/ProjectIndustrieLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/AppAreaName/Views/Projects/_ProjectProjectIndustrieLookupTableModal.js',
      modalClass: 'ProjectIndustrieLookupTableModal',
    });
    var _fileUploading = [];
    var _logoToken;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$projectInformationForm = _modalManager.getModal().find('form[name=ProjectInformationsForm]');
      _$projectInformationForm.validate();
    };

    $('#OpenProjectStatuLookupTableButton').click(function () {
      var project = _$projectInformationForm.serializeFormToObject();

      _ProjectprojectStatuLookupTableModal.open(
        { id: project.projectStatuId, displayName: project.projectStatuNameStatus },
        function (data) {
          _$projectInformationForm.find('input[name=projectStatuNameStatus]').val(data.displayName);
          _$projectInformationForm.find('input[name=projectStatuId]').val(data.id);
        }
      );
    });

    $('#ClearProjectStatuNameStatusButton').click(function () {
      _$projectInformationForm.find('input[name=projectStatuNameStatus]').val('');
      _$projectInformationForm.find('input[name=projectStatuId]').val('');
    });

    $('#OpenProjectIndustrieLookupTableButton').click(function () {
      var project = _$projectInformationForm.serializeFormToObject();

      _ProjectprojectIndustrieLookupTableModal.open(
        { id: project.projectIndustrieId, displayName: project.projectIndustrieNameIndustries },
        function (data) {
          _$projectInformationForm.find('input[name=projectIndustrieNameIndustries]').val(data.displayName);
          _$projectInformationForm.find('input[name=projectIndustrieId]').val(data.id);
        }
      );
    });

    $('#ClearProjectIndustrieNameIndustriesButton').click(function () {
      _$projectInformationForm.find('input[name=projectIndustrieNameIndustries]').val('');
      _$projectInformationForm.find('input[name=projectIndustrieId]').val('');
    });

    this.save = function () {
      if (!_$projectInformationForm.valid()) {
        return;
      }
      if ($('#Project_ProjectStatuId').prop('required') && $('#Project_ProjectStatuId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('ProjectStatu')));
        return;
      }
      if ($('#Project_ProjectIndustrieId').prop('required') && $('#Project_ProjectIndustrieId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('ProjectIndustrie')));
        return;
      }

      if (_fileUploading != null && _fileUploading.length > 0) {
        abp.notify.info(app.localize('WaitingForFileUpload'));
        return;
      }

      var project = _$projectInformationForm.serializeFormToObject();

      project.logoToken = _logoToken;

      _modalManager.setBusy(true);
      _projectsService
        .createOrEdit(project)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProjectModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };

    $('#Project_Logo').change(function () {
      var file = $(this)[0].files[0];
      if (!file) {
        _logoToken = null;
        return;
      }

      var formData = new FormData();
      formData.append('file', file);
      _fileUploading.push(true);
        var url = abp.appPath + 'AppAreaName/Projects/UploadLogoFile';

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

    $('#Project_Logo_Remove').click(function () {
      abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          var Project = _$projectInformationForm.serializeFormToObject();
          _projectsService
            .removeLogoFile({
              id: Project.id,
            })
            .done(function () {
              abp.notify.success(app.localize('SuccessfullyDeleted'));
              _$projectInformationForm.find('#div_current_file').css('display', 'none');
            });
        }
      });
    });

    $('#Project_Logo').change(function () {
      var fileName = app.localize('ChooseAFile');
      if (this.files && this.files[0]) {
        fileName = this.files[0].name;
      }
      $('#Project_LogoLabel').text(fileName);
    });
  };
})(jQuery);
