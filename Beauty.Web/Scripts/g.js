function ScrollToBottom() {
    window.scrollTo(0, document.body.scrollHeight);
}

function ScrollToTop() {
    window.scrollTo(0, 0);
}

(function ($) {
    m = {
        '\b': '\\b',
        '\t': '\\t',
        '\n': '\\n',
        '\f': '\\f',
        '\r': '\\r',
        '"': '\\"',
        '\\': '\\\\'
    },
	$.toJSON = function (value, whitelist) {
	    var a,          // The array holding the partial texts.
			i,          // The loop counter.
			k,          // The member key.
			l,          // Length.
			r = /["\\\x00-\x1f\x7f-\x9f]/g,
			v;          // The member value.

	    switch (typeof value) {
	        case 'string':
	            return r.test(value) ?
                    '"' + value.replace(r, function (a) {
                        var c = m[a];
                        if (c) {
                            return c;
                        }
                        c = a.charCodeAt();
                        return '\\u00' + Math.floor(c / 16).toString(16) + (c % 16).toString(16);
                    }) + '"' :
                    '"' + value + '"';

	        case 'number':
	            return isFinite(value) ? String(value) : 'null';

	        case 'boolean':
	        case 'null':
	            return String(value);

	        case 'object':
	            if (!value) {
	                return 'null';
	            }
	            if (typeof value.toJSON === 'function') {
	                return $.toJSON(value.toJSON());
	            }
	            a = [];
	            if (typeof value.length === 'number' &&
                        !(value.propertyIsEnumerable('length'))) {
	                l = value.length;
	                for (i = 0; i < l; i += 1) {
	                    a.push($.toJSON(value[i], whitelist) || 'null');
	                }
	                return '[' + a.join(',') + ']';
	            }
	            if (whitelist) {
	                l = whitelist.length;
	                for (i = 0; i < l; i += 1) {
	                    k = whitelist[i];
	                    if (typeof k === 'string') {
	                        v = $.toJSON(value[k], whitelist);
	                        if (v) {
	                            a.push($.toJSON(k) + ':' + v);
	                        }
	                    }
	                }
	            } else {
	                for (k in value) {
	                    if (typeof k === 'string') {
	                        v = $.toJSON(value[k], whitelist);
	                        if (v) {
	                            a.push($.toJSON(k) + ':' + v);
	                        }
	                    }
	                }
	            }
	            return '{' + a.join(',') + '}';
	    }
	};

})(jQuery);

function ShowMsg(msgcontent, handler) {

    $('#dialog-message').html(msgcontent).dialog({
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if (handler != null && handler != undefined) {
                    handler();
                }
            }
        }});
   
}

function ShowConfirmMsg(msgcontent, handler) {

    $('#confirm-dialog-message').html(msgcontent).dialog({
        modal: true,
        buttons: {
            "确定": function () {
                if (handler != null && handler != undefined) {
                    handler();
                }
                $(this).dialog("close");
               
            },
            "取消": function () {
                $(this).dialog("close");

            }
        }
    });

}


(function () {
    $("#dialog-message").dialog({
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#confirm-dialog-message").dialog({
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
  
   
})

function NoRightClick() {
   
    window.event.returnValue = false;
}

$(document).ready(function () {
    window.oncontextmenu = NoRightClick;
})