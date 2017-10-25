let path = require('path');

let _root = path.resolve(__dirname, '..');
let root = path.join.bind(path, _root);

exports.root = root;
