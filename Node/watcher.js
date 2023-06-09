const fs = require('fs')
const readline = require('readline');
const path = require('path');


function promptUser(query) {
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout,
    });

    return new Promise(resolve => rl.question(query, ans => {
        rl.close();
        resolve(ans);
    }))
}



var watcher;
let watchPath = path.join(__dirname, 'config');
console.log(watchPath);
watcher = fs.watch(watchPath)
watcher.on('change', (event, fileName)=> {
    console.log(event);
    console.log(fileName);
    if(fileName == 'asset-config.js') {
      targetWindow.webContents.send('ASSET_UPDATE', fileName);
    }
  })
  console.log('asset watcher activated');



var result = promptUser("press [Enter] to terminate program.")
