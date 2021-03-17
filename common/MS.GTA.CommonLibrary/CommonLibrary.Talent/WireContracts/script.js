/*
    generate the index.ts file
*/
var fs = require('fs');
var files = fs.readdirSync('.');

(function createIndex() {

    var exportContent = `
//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//---------------------------------------------------------------------------
/* tslint:disable:no-trailing-whitespace */
        `;

    try {
        fs.writeFileSync('index.ts', exportContent);
        console.log('Index.ts is successfully generated.');
    } catch (err) {
        console.error('Error', err);
    }

    files.forEach(function (file) {
        if (file.indexOf('.ts') < 0 || file.indexOf('index.ts') === 0 || file.indexOf('ContractGenerator.tst') === 0) {
            return;
        }

        try {
            var data = fs.readFileSync(file);
        } catch (err) {
            console.error('read file' + file + ' error: ' + err);
        }
        var cleanData = data.toString().split('\n').reduce(function (acc, str) {
            if (str.indexOf('//') >= 0 || str.indexOf('/*') >= 0) return acc;
            return acc + str;
        }, '');

        try {
            fs.appendFileSync('index.ts', cleanData);
        } catch (err) {
            console.error('write file error: ' + err);
        }
    });

})();
