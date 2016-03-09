// app-gas.js 
(function() {

	"use strict";

	// Creating the Module
	angular.module("app-gas", ["simpleControls", "ngRoute"])
		.config(function($routeProvider) {

			$routeProvider.otherwise({ redirectTo: "/" });

		});

})();