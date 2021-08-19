
function bytesToHex(b) {
  /** @type {!Array} */
  var n = [];
  /** @type {number} */
  var bi = 0;
  for (; bi < b.length; bi++) {
    n.push((b[bi] >>> 4).toString(16));
    n.push((15 & b[bi]).toString(16));
  }
  return n.join("");
};

function bytesToString(bytes) {
  return decodeURIComponent(escape(n.bin.bytesToString(bytes)));
};

function rotl(b, g) {
  return b << g | b >>> 32 - g;
};

function endian(data) {
  if (data.constructor == Number) {
    return 16711935 & rotl(data, 8) | 4278255360 & rotl(data, 24);
  }
  /** @type {number} */
  var i = 0;
  for (; i < data.length; i++) {
    data[i] = endian(data[i]);
  }
  return data;
};

function first(t, d, a, b, expression, n, s) {
  var x = t + (d & a | ~d & b) + (expression >>> 0) + s;
  return (x << n | x >>> 32 - n) + d;
};

function second(b, g, j, i, n, r, s) {
  var t = b + (g & i | j & ~i) + (n >>> 0) + s;
  return (t << r | t >>> 32 - r) + g;
};

function third(req, res, next, err, done, n, s) {
  var x = req + (res ^ next ^ err) + (done >>> 0) + s;
  return (x << n | x >>> 32 - n) + res;
};

function fourth(a, b, c, d, x, s, ch) {
  var n = a + (c ^ (b | ~d)) + (x >>> 0) + ch;
  return (n << s | n >>> 32 - s) + b;
};

function bytesToWords(value) {
  /** @type {!Array} */
  var input = [];
  /** @type {number} */
  var j = 0;
  /** @type {number} */
  var i = 0;
  for (; j < value.length; j++, i = i + 8) {
    input[i >>> 5] |= value[j] << 24 - i % 32;
  }
  return input;
};

function wordsToBytes(words) {
  /** @type {!Array} */
  var bytes = [];
  /** @type {number} */
  var i = 0;
  for (; i < 32 * words.length; i = i + 8) {
    bytes.push(words[i >>> 5] >>> 24 - i % 32 & 255);
  }
  return bytes;
};

function stringToBytesS(o_content) {
  return stringToBytes(unescape(encodeURIComponent(o_content)));
};

function stringToBytes(s) {
  /** @type {!Array} */
  var bytes = [];
  /** @type {number} */
  var i = 0;
  for (; i < s.length; i++) {
    bytes.push(255 & s.charCodeAt(i));
  }
  return bytes;
};

function r(value, options) {
  if (value.constructor == String) {
    value = options && "binary" === options.encoding ? stringToBytesS(value) : stringToBytesS(value);
  } else {
    if (i(value)) {
      /** @type {!Array<?>} */
      value = Array.prototype.slice.call(value, 0);
    } else {
      if (!Array.isArray(value)) {
        value = value.toString();
      }
    }
  }
  var x = bytesToWords(value);
  /** @type {number} */
  var len = 8 * value.length;
  /** @type {number} */
  var b = 1732584193;
  /** @type {number} */
  var c = -271733879;
  /** @type {number} */
  var d = -1732584194;
  /** @type {number} */
  var a = 271733878;
  /** @type {number} */
  var i = 0;
  for (; i < x.length; i++) {
    /** @type {number} */
    x[i] = 16711935 & (x[i] << 8 | x[i] >>> 24) | 4278255360 & (x[i] << 24 | x[i] >>> 8);
  }
  x[len >>> 5] |= 128 << len % 32;
  /** @type {number} */
  x[14 + (len + 64 >>> 9 << 4)] = len;
  var FF = e._ff;
  var GG = e._gg;
  var HH = e._hh;
  var II = e._ii;
  /** @type {number} */
  i = 0;
  for (; i < x.length; i = i + 16) {
    /** @type {number} */
    var tintB = b;
    /** @type {number} */
    var y = c;
    /** @type {number} */
    var curr_photo = d;
    /** @type {number} */
    var up = a;
    
    b = first(b, c, d, a, x[i + 0], 7, -680876936);
    a = first(a, b, c, d, x[i + 1], 12, -389564586);
    d = first(d, a, b, c, x[i + 2], 17, 606105819);
    c = first(c, d, a, b, x[i + 3], 22, -1044525330);
    b = first(b, c, d, a, x[i + 4], 7, -176418897);
    a = first(a, b, c, d, x[i + 5], 12, 1200080426);
    d = first(d, a, b, c, x[i + 6], 17, -1473231341);
    c = first(c, d, a, b, x[i + 7], 22, -45705983);
    b = first(b, c, d, a, x[i + 8], 7, 1770035416);
    a = first(a, b, c, d, x[i + 9], 12, -1958414417);
    d = first(d, a, b, c, x[i + 10], 17, -42063);
    c = first(c, d, a, b, x[i + 11], 22, -1990404162);
    b = first(b, c, d, a, x[i + 12], 7, 1804603682);
    a = first(a, b, c, d, x[i + 13], 12, -40341101);
    d = first(d, a, b, c, x[i + 14], 17, -1502002290);
    c = first(c, d, a, b, x[i + 15], 22, 1236535329);
    b = second(b, c, d, a, x[i + 1], 5, -165796510);
    a = second(a, b, c, d, x[i + 6], 9, -1069501632);
    d = second(d, a, b, c, x[i + 11], 14, 643717713);
    c = second(c, d, a, b, x[i + 0], 20, -373897302);
    b = second(b, c, d, a, x[i + 5], 5, -701558691);
    a = second(a, b, c, d, x[i + 10], 9, 38016083);
    d = second(d, a, b, c, x[i + 15], 14, -660478335);
    c = second(c, d, a, b, x[i + 4], 20, -405537848);
    b = second(b, c, d, a, x[i + 9], 5, 568446438);
    a = second(a, b, c, d, x[i + 14], 9, -1019803690);
    d = second(d, a, b, c, x[i + 3], 14, -187363961);
    c = second(c, d, a, b, x[i + 8], 20, 1163531501);
    b = second(b, c, d, a, x[i + 13], 5, -1444681467);
    a = second(a, b, c, d, x[i + 2], 9, -51403784);
    d = second(d, a, b, c, x[i + 7], 14, 1735328473);
    c = second(c, d, a, b, x[i + 12], 20, -1926607734);
    b = third(b, c, d, a, x[i + 5], 4, -378558);
    a = third(a, b, c, d, x[i + 8], 11, -2022574463);
    d = third(d, a, b, c, x[i + 11], 16, 1839030562);
    c = third(c, d, a, b, x[i + 14], 23, -35309556);
    b = third(b, c, d, a, x[i + 1], 4, -1530992060);
    a = third(a, b, c, d, x[i + 4], 11, 1272893353);
    d = third(d, a, b, c, x[i + 7], 16, -155497632);
    c = third(c, d, a, b, x[i + 10], 23, -1094730640);
    b = third(b, c, d, a, x[i + 13], 4, 681279174);
    a = third(a, b, c, d, x[i + 0], 11, -358537222);
    d = third(d, a, b, c, x[i + 3], 16, -722521979);
    c = third(c, d, a, b, x[i + 6], 23, 76029189);
    b = third(b, c, d, a, x[i + 9], 4, -640364487);
    a = third(a, b, c, d, x[i + 12], 11, -421815835);
    d = third(d, a, b, c, x[i + 15], 16, 530742520);
    c = third(c, d, a, b, x[i + 2], 23, -995338651);
    b = fourth(b, c, d, a, x[i + 0], 6, -198630844);
    a = fourth(a, b, c, d, x[i + 7], 10, 1126891415);
    d = fourth(d, a, b, c, x[i + 14], 15, -1416354905);
    c = fourth(c, d, a, b, x[i + 5], 21, -57434055);
    b = fourth(b, c, d, a, x[i + 12], 6, 1700485571);
    a = fourth(a, b, c, d, x[i + 3], 10, -1894986606);
    d = fourth(d, a, b, c, x[i + 10], 15, -1051523);
    c = fourth(c, d, a, b, x[i + 1], 21, -2054922799);
    b = fourth(b, c, d, a, x[i + 8], 6, 1873313359);
    a = fourth(a, b, c, d, x[i + 15], 10, -30611744);
    d = fourth(d, a, b, c, x[i + 6], 15, -1560198380);
    c = fourth(c, d, a, b, x[i + 13], 21, 1309151649);
    b = fourth(b, c, d, a, x[i + 4], 6, -145523070);
    a = fourth(a, b, c, d, x[i + 11], 10, -1120210379);
    d = fourth(d, a, b, c, x[i + 2], 15, 718787259);
    c = fourth(c, d, a, b, x[i + 9], 21, -343485551);
    /** @type {number} */
    b = b + tintB >>> 0;
    /** @type {number} */
    c = c + y >>> 0;
    /** @type {number} */
    d = d + curr_photo >>> 0;
    /** @type {number} */
    a = a + up >>> 0;
  }
  return endian([b, c, d, a]);
};

function morph(e, options) {
  
   var digestbytes = r(e, options);
   var next = wordsToBytes(digestbytes);
   var hex = bytesToHex(next);
  //var digestbytes = [235,178,157,29,39,28,79,223,188,51,173,131,24,97,27,248];
  return options && options.asBytes ? next : options && options.asString ? bytesToString(next) : bytesToHex(next);
};
var e = "{\"$gpb\":\"badoo.bma.BadooMessage\",\"body\":[{\"message_type\":377,\"server_person_profile_edit_form\":{\"type\":[2,10,12,22,3,8,4,6,7,5,9]}}],\"message_id\":55,\"message_type\":377,\"version\":1,\"is_background\":false}whitetelevisionbulbelectionroofhorseflying";
var n = undefined;
var s = morph(e,n)

function go(e){


}

