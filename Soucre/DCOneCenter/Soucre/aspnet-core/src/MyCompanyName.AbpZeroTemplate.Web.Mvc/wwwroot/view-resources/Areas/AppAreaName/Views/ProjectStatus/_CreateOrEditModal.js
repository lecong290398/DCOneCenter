(function ($) {
  app.modals.CreateOrEditProjectStatuModal = function () {
    var _projectStatusService = abp.services.app.projectStatus;

    var _modalManager;
    var _$projectStatuInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$projectStatuInformationForm = _modalManager.getModal().find('form[name=ProjectStatuInformationsForm]');
      _$projectStatuInformationForm.validate();
    };

    this.save = function () {
      if (!_$projectStatuInformationForm.valid()) {
        return;
      }

      var projectStatu = _$projectStatuInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _projectStatusService
        .createOrEdit(projectStatu)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProjectStatuModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
