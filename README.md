## node-edge-oledb
====

This module allows OLE DB communications via C# System.Data.OleDb and [Edge.js](https://github.com/tjanczuk/edge).  

There was a need for an interface to an old system written in FoxPro, and the only available nodejs adodb/oledb modules at the time were slow/problematic/used external programs

Since this module does not use any other external processes to run ole/ado commands and instead uses C# and System.Data.OleDb, it is extremely fast

Usage example:

```javascript
var oledb = require('node-edge-oledb');

var query = {
	dsn: "Provider=vfpoledb.1; Data Source=C:/mydb/mydb.dbc; Mode=ReadWrite|Share Deny None;",
	query: "SELECT * FROM customers WHERE type = 'C'"
}

oledb(query, function(error, result){
	if (error) throw error;
	console.log(result);
});	

```

## Installation

```bash
$ npm install node-edge-oledb
```

## More Info
---
  * https://github.com/tjanczuk/edge
  * https://www.connectionstrings.com/


## People
Written by [Brian Taber](https://github.com/DaSpawn) [![DaSpawn's Gratipay][gratipay-image-daspawn]][gratipay-url-daspawn]


## License

  [MIT](LICENSE)
  
[gratipay-url-daspawn]: https://gratipay.com/~DaSpawn
[gratipay-image-daspawn]: https://img.shields.io/gratipay/team/daspawn.svg
