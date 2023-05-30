const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:5000';


const onError = (err, req, resp, target) => {
    console.error(`${err.message}`);
}

module.exports = function (app) {
    app.use('/api',
        createProxyMiddleware({
            target: target,
            changeOrigin: true,
            onError: onError,
            secure: false,
            headers: {
                Connection: 'Keep-Alive'
            }
        }));
};