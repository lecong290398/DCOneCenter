(function () {
  $(function () {
    var _$projectsTable = $('#ProjectsTable');
    var _projectsService = abp.services.app.projects;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Projects.Create'),
      edit: abp.auth.hasPermission('Pages.Projects.Edit'),
      delete: abp.auth.hasPermission('Pages.Projects.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/Projects/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/Projects/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProjectModal',
    });

    var _viewProjectModal = new app.ModalManager({
      viewUrl: abp.appPath + 'AppAreaName/Projects/ViewprojectModal',
      modalClass: 'ViewProjectModal',
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

    var dataTable = _$projectsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _projectsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProjectsTableFilter').val(),
            projectNameFilter: $('#ProjectNameFilterId').val(),
            tokenShortnameFilter: $('#TokenShortnameFilterId').val(),
            totalTokenSupplyFilter: $('#TotalTokenSupplyFilterId').val(),
            releaseYearFilter: $('#ReleaseYearFilterId').val(),
            isPormotedFilter: $('#IsPormotedFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            projectStatuNameStatusFilter: $('#ProjectStatuNameStatusFilterId').val(),
            projectIndustrieNameIndustriesFilter: $('#ProjectIndustrieNameIndustriesFilterId').val(),
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
                  _viewProjectModal.open({ id: data.record.project.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.project.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProject(data.record.project);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'project.projectName',
          name: 'projectName',
        },
        {
          targets: 3,
          data: 'project.tokenShortname',
          name: 'tokenShortname',
        },
        {
          targets: 4,
          data: 'project.totalTokenSupply',
          name: 'totalTokenSupply',
        },
        {
          targets: 5,
          data: 'project.releaseYear',
          name: 'releaseYear',
        },
        {
          targets: 6,
          data: 'project.projectSummary',
          name: 'projectSummary',
        },
        {
          targets: 7,
          data: 'project.projectDescription',
          name: 'projectDescription',
        },
        {
          targets: 8,
          data: 'project.websiteURL',
          name: 'websiteURL',
        },
        {
          targets: 9,
          data: 'project.whitepaper_URL_FAQ',
          name: 'whitepaper_URL_FAQ',
        },
        {
          targets: 10,
          data: 'project.twitterURL',
          name: 'twitterURL',
        },
        {
          targets: 11,
          data: 'project.discord',
          name: 'discord',
        },
        {
          targets: 12,
          data: 'project.reddit',
          name: 'reddit',
        },
        {
          targets: 13,
          data: 'project.facebook',
          name: 'facebook',
        },
        {
          targets: 14,
          data: 'project.telegram',
          name: 'telegram',
        },
        {
          targets: 15,
          data: 'project.yourName',
          name: 'yourName',
        },
        {
          targets: 16,
          data: 'project.yourEmailaddress',
          name: 'yourEmailaddress',
        },
        {
          targets: 17,
          data: 'project.isPormoted',
          name: 'isPormoted',
          render: function (isPormoted) {
            if (isPormoted) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 18,
          data: 'project.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 19,
          data: 'project',
          render: function (project) {
            if (!project.logo) {
              return '';
            }
              return `<a href="/File/DownloadBinaryFile?id=${project.logoFileName}" target="_blank">${project.logoFileName}</a>`;
          },
        },
        {
          targets: 20,
          data: 'projectStatuNameStatus',
          name: 'projectStatuFk.nameStatus',
        },
        {
          targets: 21,
          data: 'projectIndustrieNameIndustries',
          name: 'projectIndustrieFk.nameIndustries',
        },
      ],
    });

    function getProjects() {
      dataTable.ajax.reload();
    }

    function deleteProject(project) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _projectsService
            .delete({
              id: project.id,
            })
            .done(function () {
              getProjects(true);
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

    $('#CreateNewProjectButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _projectsService
        .getProjectsToExcel({
          filter: $('#ProjectsTableFilter').val(),
          projectNameFilter: $('#ProjectNameFilterId').val(),
          tokenShortnameFilter: $('#TokenShortnameFilterId').val(),
          totalTokenSupplyFilter: $('#TotalTokenSupplyFilterId').val(),
          releaseYearFilter: $('#ReleaseYearFilterId').val(),
          isPormotedFilter: $('#IsPormotedFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          projectStatuNameStatusFilter: $('#ProjectStatuNameStatusFilterId').val(),
          projectIndustrieNameIndustriesFilter: $('#ProjectIndustrieNameIndustriesFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditProjectModalSaved', function () {
      getProjects();
    });

    $('#GetProjectsButton').click(function (e) {
      e.preventDefault();
      getProjects();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProjects();
      }
    });
  });
})();
