(function () {
  $(function () {
    var _$projectIndustriesTable = $('#ProjectIndustriesTable');
    var _projectIndustriesService = abp.services.app.projectIndustries;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.ProjectIndustries.Create'),
      edit: abp.auth.hasPermission('Pages.ProjectIndustries.Edit'),
      delete: abp.auth.hasPermission('Pages.ProjectIndustries.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/ProjectIndustries/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/ProjectIndustries/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProjectIndustrieModal',
    });

    var _viewProjectIndustrieModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/ProjectIndustries/ViewprojectIndustrieModal',
      modalClass: 'ViewProjectIndustrieModal',
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

    var dataTable = _$projectIndustriesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _projectIndustriesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProjectIndustriesTableFilter').val(),
            nameIndustriesFilter: $('#NameIndustriesFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
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
                  _viewProjectIndustrieModal.open({ id: data.record.projectIndustrie.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.projectIndustrie.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProjectIndustrie(data.record.projectIndustrie);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'projectIndustrie.nameIndustries',
          name: 'nameIndustries',
        },
        {
          targets: 3,
          data: 'projectIndustrie.sumaryIndustries',
          name: 'sumaryIndustries',
        },
        {
          targets: 4,
          data: 'projectIndustrie.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 5,
          data: 'projectIndustrie',
          render: function (projectIndustrie) {
            if (!projectIndustrie.logo) {
              return '';
            }
              return `<a href="/view-resources/Views/DCOneCenter/ImagesProjectIndustries/${projectIndustrie.logo}" target="_blank">${projectIndustrie.logoFileName}</a>`;
          },
        },
      ],
    });

    function getProjectIndustries() {
      dataTable.ajax.reload();
    }

    function deleteProjectIndustrie(projectIndustrie) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _projectIndustriesService
            .delete({
              id: projectIndustrie.id,
            })
            .done(function () {
              getProjectIndustries(true);
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

    $('#CreateNewProjectIndustrieButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _projectIndustriesService
        .getProjectIndustriesToExcel({
          filter: $('#ProjectIndustriesTableFilter').val(),
          nameIndustriesFilter: $('#NameIndustriesFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditProjectIndustrieModalSaved', function () {
      getProjectIndustries();
    });

    $('#GetProjectIndustriesButton').click(function (e) {
      e.preventDefault();
      getProjectIndustries();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProjectIndustries();
      }
    });
  });
})();
