const path = require('path');
const webpack = require('webpack');
const { AureliaPlugin } = require('aurelia-webpack-plugin');
const bundleOutputDir = './wwwroot/dist';
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const extractCss = false;
const cssRules = [
    { loader: 'css-loader' },
    {
        loader: 'postcss-loader',
        options: { plugins: () => [require('autoprefixer')({ browsers: ['last 2 versions'] })] }
    }
]

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const analyze = (env && env.analyze);
    return [{
        mode: 'development',
        performance: {
            hints: false
        },
        optimization: {
            // See this blog post for more info on optimisation:
            // https://hackernoon.com/the-100-correct-way-to-split-your-chunks-with-webpack-f8a9df5b7758

            runtimeChunk: 'single',
            splitChunks: {
                chunks: 'all',
            }
        },
        stats: { modules: false },
        entry: { 'app': 'aurelia-bootstrapper' },
        resolve: {
            extensions: ['.tsx', '.jsx', '.ts', '.js'],
            modules: ['ClientApp', 'node_modules'],
        },
        output: {
            path: path.resolve(bundleOutputDir),
            publicPath: '/dist/',
            filename: '[name].js'
        },
        module: {
            rules: [
                // CSS required in JS/TS files should use the style-loader that auto-injects it into the website
                // only when the issuer is a .js/.ts file, so the loaders are not applied inside html templates
                {
                    test: /\.css$/i,
                    issuer: [{ not: [{ test: /\.html$/i }] }],
                    use: extractCss ? ExtractTextPlugin.extract({
                        fallback: 'style-loader',
                        use: cssRules,
                    }) : ['style-loader', ...cssRules],
                },
                {
                    test: /\.css$/i,
                    issuer: [{ test: /\.html$/i }],
                    // CSS required in templates cannot be extracted safely
                    // because Aurelia would try to require it again in runtime
                    use: cssRules,
                },
                { test: /\.scss$/i, use: isDevBuild ? ['css-loader', 'sass-loader'] : ['css-loader?minimize', 'sass-loader'] },

                { test: /\.html$/i, use: 'html-loader' },
                { test: /\.(ts|tsx|jsx)$/i, include: /ClientApp/, use: 'ts-loader?silent=true' },
                { test: /\.json$/i, loader: 'json-loader' },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                { test: /\.woff2(\?v=[0-9]\.[0-9]\.[0-9])?$/i, loader: 'url-loader', options: { limit: 10000, mimetype: 'application/font-woff2' } },
                { test: /\.woff(\?v=[0-9]\.[0-9]\.[0-9])?$/i, loader: 'url-loader', options: { limit: 10000, mimetype: 'application/font-woff' } },

                // load these fonts normally, as files:
                { test: /\.(ttf|eot|svg|otf)(\?v=[0-9]\.[0-9]\.[0-9])?$/i, loader: 'file-loader' }
            ]
        },
        externals: {
            stripe: {
                root: ['Stripe', 'stripe']
            },
            jquery: 'jQuery'
        },
        plugins: [
            new webpack.DefinePlugin({ IS_DEV_BUILD: JSON.stringify(isDevBuild) }),
            new AureliaPlugin({ aureliaApp: 'boot' }),

            /// Uncomment the following to generate a pretty graph that
            /// visualises the bundle. This will create a file named report.html
            /// in the output directory, and automatically open it in your
            /// browser.

            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]')  // Point sourcemap entries to the original file locations on disk
            }),
        ].concat(
            (analyze) ? [ new BundleAnalyzerPlugin() ] : [],
        ),
    }];
}
