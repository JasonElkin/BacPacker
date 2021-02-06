(function () {
  'use strict';

  function Socially(editorService, bacPackerExporterResource, notificationsService) {

    var vm = this;

    vm.loading = true;
    vm.exporters = [];

    vm.noExporters = false;

    bacPackerExporterResource.getExporters().then(
      (successResponse) => {
        vm.exporters = successResponse;
      }
    ).finally(() => {
      vm.loading = false;
    });

  }

  angular.module("umbraco").controller("BacPacker.DashboardController", Socially);

})();
