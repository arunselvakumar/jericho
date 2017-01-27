var webpack = require('webpack');
var fs = require('fs');

module.exports = [
  {
    entry: {
      core: './node_modules/core-js/client/shim.min.js',
      zone: './node_modules/zone.js/dist/zone.js',
      reflect: './node_modules/reflect-metadata/Reflect.js',
      system: './node_modules/systemjs/dist/system.src.js'
    },
    output: {
      filename: './Client/js/[name].js'
    },
    target: 'web',
    node: {
      fs: "empty"
    }
  },
  {
    entry: {
      app: './Client/app/main.ts'
    },
    output: {
      filename: './Client/app/bundle.js'
    },
    devtool: 'source-map',
    resolve: {
      extensions: ['', '.ts', '.js', '.json', '.css', '.scss', '.html', '.eot', '.ttf', '.svg', '.woff', '.woff2', '.otf']
    },
    module: {
      loaders: [
        { test: /\.ts$/, loader: 'ts-loader' },
        { test: /\.html$/, loader: 'html' }
      ]
    }
  }];