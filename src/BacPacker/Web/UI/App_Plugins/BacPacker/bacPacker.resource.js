(function () {
  'use strict';

  function PostsResource($http) {
    return {
      getExporters: () => {
        return $http.get("/Umbraco/backoffice/api/BacPacker/GetExporters")
          .then((response) => response.data);
      }
    }
  }

  angular.module("umbraco.resources").factory("bacPackerExporterResource", PostsResource);

})();
