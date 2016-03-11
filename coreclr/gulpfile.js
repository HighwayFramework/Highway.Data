"use strict";

var 
    gulp = require("gulp"),
    dnx = require("gulp-dnx");


gulp.task("default", [ "build-test" ])

gulp.task("build-test", dnx("test", {
   restore: true,
   build: true,
   run: true,
   cwd: './test/Highway.Data.Tests' 
}));

gulp.watch("**/*.cs", ["default"]);