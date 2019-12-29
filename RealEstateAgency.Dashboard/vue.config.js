let apiProxy = 'http://localhost:5000';

module.exports = {
  lintOnSave: false,
  runtimeCompiler: true,
  publicPath: '/',
  devServer: {
    proxy: {
      '/api': {
        target: apiProxy,
        ws: true,
        changeOrigin: true
      }
    }
  }
};
