"use strict";

var edge = require('edge');
var adodb = edge.func('adodb.cs');

function nodeEdgeOledb(query, callback){
	if (!(this instanceof nodeEdgeOledb)) return new nodeEdgeOledb();

	adodb(query, function(error, result){
		var data = {}
		if (error){
			data.valid = false 
			data.error = error.message
			if (error && error.hasOwnProperty('InnerException')){
				if (error.InnerException && error.InnerException.hasOwnProperty('Message')){
					data.errorMessage = error.InnerException.Message
				}else{
					data.errorMessage = util.inspect(error.InnerException)
				}
			}else{
				data.errorMessage = util.inspect(error);
			}
		}else{
			data.valid = true 
			data.records = result
		}
		setImmediate(cb, data);
	}
}

module.exports = nodeEdgeOledb;