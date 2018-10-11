(function () {
    'use strict';

    angular.module('bob').factory('caeceService', function (apiBase, $http) {

        var self = this;

        self.getAllCourses = function () {
            return $http.get(apiBase + 'courses')
                .then(function (result) {
                    return result.data;
                });
        };

        self.getCourse = function (courseId) {
            return $http.get(apiBase + 'course/' + courseId + '/full')
                .then(function (result) {
                    return result.data;
                });
        };

        return this;
    });
})();