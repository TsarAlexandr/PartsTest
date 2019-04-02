/// <reference path="../module/scripts/module.js" />
let path = require('path');

const CleanWebpackPlugin = require('clean-webpack-plugin');


const bundleFolder = "wwwroot/bundle/";

module.exports = {

    entry: { main: '../module/scripts/module.js' },


    output: {
        filename: 'script.js',
        path: path.resolve(__dirname, bundleFolder),
        publicPath: path.resolve(__dirname, bundleFolder)
    },
    plugins: [
        new CleanWebpackPlugin()
    ]
};