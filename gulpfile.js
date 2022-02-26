"use strict";

const gulp = require("gulp");
const sass = require("gulp-sass")(require("sass"));

function buildStyles() {
  return gulp
    .src("./site.scss")
    .pipe(sass().on("error", sass.logError))
    .pipe(gulp.dest("./wwwroot/css"));
}

exports.buildStyles = buildStyles;
exports.watch = function () {
  gulp.watch("./sass/**/*.scss", ["sass"]);
};
