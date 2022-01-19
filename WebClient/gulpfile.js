'use strict';
console.log("-------------------------------------");
console.log("START");

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    merge = require('merge-stream'),
    del = require('del'),
    bundleconfig = require('./bundleconfig.json'),
    less = require('gulp-less');

const regex = {
    less: /\.less$/,
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

const version = "";

console.log("-------------------------------------");
console.log("   PUBLISH ENVIROMENT: PRODUCTION");
console.log("   - Resource version:" + version);
console.log("-------------------------------------");

gulp.task('compileless', async function () {
    getBundles(regex.less).map(bundle => {
        (bundle.inputFiles || []).map(inputFile => {
            gulp.src(inputFile.lessfile)
                .pipe(less({
                    modifyVars: inputFile.params || {}
                }))
                .pipe(gulp.dest(bundle.folder + inputFile.output));
        });
    });
});

gulp.task('fonts', async function () {
    gulp.src([
        'wwwroot/node_modules/admin-lte/plugins/fontawesome-free/webfonts/*.{otf,eot,svg,ttf,woff,woff2}'
    ]).pipe(gulp.dest('./wwwroot/webfonts'));
});

gulp.task('min:js', async function () {
    merge(getBundles(regex.js).map(bundle => {
        var path = applyVersion2Path(bundle.outputFileName, version);
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(path))
            .pipe(uglify().on('error', function (e) {
                console.log(e);
            }))
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('min:css', async function () {
    merge(getBundles(regex.css).map(bundle => {
        var path = applyVersion2Path(bundle.outputFileName, version);
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(path))
            .pipe(cssmin())
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('clean', () => {
    return del(bundleconfig.map(bundle => applyVersion2Path(bundle.outputFileName, '*')));
});

gulp.task('min', gulp.series(['clean', 'compileless', 'min:js', 'min:css', 'fonts']));

const getBundles = (regexPattern) => {
    return bundleconfig.filter(bundle => {
        return regexPattern.test(bundle.typefiles);
    });
};

const applyVersion2Path = (path, version) => {
    const arr = (path || "").split('.');
    if (arr.length === 3) {
        return arr[0] + version + '.' + arr[1] + '.' + arr[2];
    }

    return path;
};

gulp.task('default', gulp.series("min"));