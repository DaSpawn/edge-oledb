node-edge-oledb enables OLE DB communications on Windows via C# System.Data.OleDb and [Edge.js](https://github.com/tjanczuk/edge).  

There was a need for an interface to an old system written in FoxPro, and the only available nodejs adodb/oledb modules at the time were slow/problematic/used external programs

Since this module does not use any other external processes to run ole commands and instead uses C# and System.Data.OleDb, so it is extremely fast

```javascript
var oledb = require('node-edge-oledb');

var options = {
	dsn: "Provider=vfpoledb.1; Data Source=C:/mydb/mydb.dbc; Mode=ReadWrite|Share Deny None;",
	query: "SELECT * FROM customers WHERE type = 'C'"
}

oledb(options, function(error, result){
	if (error) throw error;
	console.log(result);
});	

```


### Installation
```bash
$ npm install node-edge-oledb
```


### More Info
  * https://en.wikipedia.org/wiki/OLE_DB
  * https://github.com/tjanczuk/edge
  * https://www.connectionstrings.com/


### People
Written by [Brian Taber](https://github.com/DaSpawn) [![DaSpawn's Gratipay][gratipay-image-daspawn]][gratipay-url-daspawn]


### License
  [MIT](LICENSE)


[gratipay-url-daspawn]: https://gratipay.com/~DaSpawn
[gratipay-image-daspawn]: https://img.shields.io/gratipay/team/daspawn.svg
