node-edge-oledb
====

Please note this repo is currently being filled (code is in production use, working to make node module now)



This module allows OLE DB communications via C# System.Data.OleDb and [Edge.js](https://github.com/tjanczuk/edge).  

There was a need for an interface to an old system written in FoxPro, and the only available nodejs adodb/oledb modules at the time were slow/problematic/used external programs

Since this module does not use any other external processes to run ole/ado commands and instead uses C# and System.Data.OleDb, it is extremely fast

Usage example:

```javascript

var oledb = require('node-edge-oledb');

var query = {
	dsn: "Provider=vfpoledb.1; Data Source=C:/mydb/mydb.dbf; Mode=ReadWrite|Share Deny None;",
	query: "SELECT * FROM customers WHERE type = 'C'"
}

oledb(query, function(error, result){
	if (error) throw error;
	console.log(result);
});	


```
