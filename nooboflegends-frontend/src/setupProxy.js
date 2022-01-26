﻿const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/helloworld",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7168',
        secure: false
    });

    app.use(appProxy);
};
