const helper = require('./helper.js');
const devServer = require('./dev-server.js');

const definePlugin = require('webpack/lib/DefinePlugin');
const contextReplacementPlugin = require('webpack/lib/ContextReplacementPlugin');
const htmlWebpackPlugin = require('html-webpack-plugin');
const extractTextPlugin = require('extract-text-webpack-plugin');
const commonsChunkPlugin = require('webpack/lib/optimize/CommonsChunkPlugin');
const copyWebpackPlugin = require('copy-webpack-plugin');
const title = 'The Guard project';

module.exports = {
    entry: {
        vendor: './src/vendor.ts',
        styles: './src/public/css/styles.scss',
        polyfills: './src/polyfills.ts',
        app: './src/main.ts'
    },
    output: {
        path: helper.root('dist'),
        filename: '[name].bundle.js',
        chunkFilename: '[id].chunk.js'
    },
    resolve: {
        extensions: ['.ts', '.js', '.json'],
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: [
                    { loader: 'awesome-typescript-loader' },
                    { loader: 'angular2-template-loader' }
                ]
            },
            {
                test: /\.html$/,
                use: 'raw-loader',
                exclude: [helper.root('src/index.html')]
            },
            {
                test: /\.css$/,
                use: ['to-string-loader', 'css-loader'],
                exclude: [helper.root('src', 'public')]
            },
            {
                test: /\.scss$/,
                use: ['to-string-loader', 'css-loader', 'sass-loader'],
                exclude: [helper.root('src', 'public')]
            },
            {
                test: /\.css$/,
                loader: extractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader']
                }),
                include: [helper.root('src', 'public')]
            },
            {
                test: /\.scss$/,
                loader: extractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader', 'sass-loader']
                }),
                include: [helper.root('src', 'public')]
            },
            {
                test: /\.json$/,
                use: 'json-loader'
            },
            {
                test: /\.(jpe|jpg|png|gif|woff|woff2|eot|ttf|svg)(\?.*$|$)/,
                use: 'file-loader'
            }
        ]
    },
    plugins: [
        new commonsChunkPlugin({
            name: ['polyfills', 'vendor'].reverse()
        }),
        new contextReplacementPlugin(
            /angular(\\|\/)core(\\|\/)@angular/,
            helper.root('src'), {}
        ),
        new htmlWebpackPlugin({
            template: 'src/index.html',
            title: title,
            chunksSortMode: 'dependency'
        }),
        new extractTextPlugin({ filename: '[name]-[chunkhash].css', allChunks: true }),
        new definePlugin({
            ENV: JSON.stringify('development')
        })
    ],
    devServer: devServer
}
