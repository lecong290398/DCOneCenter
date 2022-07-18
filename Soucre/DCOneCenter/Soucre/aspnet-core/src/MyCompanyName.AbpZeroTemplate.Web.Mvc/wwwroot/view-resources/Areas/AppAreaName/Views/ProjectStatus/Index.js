(function () {
  $(function () {
    var _$projectStatusTable = $('#ProjectStatusTable');
    var _projectStatusService = abp.services.app.projectStatus;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.ProjectStatus.Create'),
      edit: abp.auth.hasPermission('Pages.ProjectStatus.Edit'),
      delete: abp.auth.hasPermission('Pages.ProjectStatus.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/ProjectStatus/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/ProjectStatus/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProjectStatuModal',
    });

    var _viewProjectStatuModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/ProjectStatus/ViewprojectStatuModal',
      modalClass: 'ViewProjectStatuModal',
    });

    var getDateFilter = function (element) {
      if (element.data('DateTimePicker').date() == null) {
        return null;
      }
      return element.data('DateTimePicker').date().format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if (element.data('DateTimePicker').date() == null) {
        return null;
      }
      return element.data('DateTimePicker').date().format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$projectStatusTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _projectStatusService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProjectStatusTableFilter').val(),
            nameStatusFilter: $('#NameStatusFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                iconStyle: 'far fa-eye mr-2',
                action: function (data) {
                  _viewProjectStatuModal.open({ id: data.record.projectStatu.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.projectStatu.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProjectStatu(data.record.projectStatu);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'projectStatu.nameStatus',
          name: 'nameStatus',
        },
        {
          targets: 3,
          data: 'projectStatu.sumaryStatus',
          name: 'sumaryStatus',
        },
      ],
    });

    function getProjectStatus() {
      dataTable.ajax.reload();
    }

    function deleteProjectStatu(projectStatu) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _projectStatusService
            .delete({
              id: projectStatu.id,
            })
            .done(function () {
              getProjectStatus(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewProjectStatuButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _projectStatusService
        .getProjectStatusToExcel({
          filter: $('#ProjectStatusTableFilter').val(),
          nameStatusFilter: $('#NameStatusFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditProjectStatuModalSaved', function () {
      getProjectStatus();
    });

    $('#GetProjectStatusButton').click(function (e) {
      e.preventDefault();
      getProjectStatus();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProjectStatus();
      }
    });
  });
})();
