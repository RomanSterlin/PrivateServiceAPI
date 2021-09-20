using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadooAPI.Utills
{
    public static class XPingGenerator
    {
        static string bytesToHex(List<long> b)
        {
            /** @type {!ArrayList} */
            var input = new List<string>();
            /** @type {number} */
            var bi = 0;
            var prov = 16;
            for (; bi < b.Count; bi++)
            {
                input.Add(((uint)b[bi] >> 4).ToString("x"));
                input.Add((15 & b[bi]).ToString("x"));
            }

            return string.Join("", input);
        }

        //void bytesToString(int[] bytes)
        //{
        //    return decodeURIComponent(Uri.EscapeDataString(n.bin.bytesToString(bytes)));
        //}

        static long rotl(long b, int g)
        {
            var q = ((int)b << g) | (uint)b >> (32 - g);
            var ww = ((int)b << g);
            var rr = 32 - g;
            var tt = (uint)b >> rr;
            var rty = ww | rr;
            return q;
        }

        static dynamic endian(dynamic data)
        {
            if (data is long)
            {
                var q = (16711935 & rotl(data, 8));
                var t = rotl(data, 24);
                var y = (int)(4278255360 & t);
                var w = (int)(4278255360 & rotl(data, 24));
                var x = q | w;
                return x;
            }
            var i = 0;

            for (; i < data.Length; i++)
            {
                data[i] = endian(data[i]);
            }

            /** @type {number} */
            return data;
        }
        static long endian2(long data)
        {
            return (16711935 & rotl(data, 8)) | (4278255360 & rotl(data, 24));

        }
        static long first(long t, long d, long a, long b, long? expression, int n, int s)
        {
            //  var zz = ((int)~d & b) + (int)((uint)expression >> 0) + ((int)s);
            // var x = q | zz;
            //var x = ((int)t) + (((int)d & a) | ((int)~d & b))+ ((uint)(expression >> 0)) + ((int)s);
            //var w = ((int)~d & b);
            //var e = (((int)d & a) | ((int)~d & b));
            //var test2 = (int)((uint)expression >> 0);
            //var r = ((int)t) + (((int)d & a) | ((int)~d & b));  
            //var y = ((uint)(expression >> 0)) + ((int)s);
            //var yy = test2 + ((uint)(expression >> 0)) + ((int)s);
            //var zzzz = ((int)x) << ((int)n);
            //var test3 = test2 + ((uint)s);
            //var zx = ((int)((uint)x >> (32 - n)));
            //var zxx = zzzz | zx;
            //var zxxx = zxx + d;
            var x = ((int)t) + (((int)d & a) | ((int)~d & b)) + (int)((uint)(expression >> 0)) + ((int)s);
            var f = ((int)x) << ((int)n);
            var sec = ((int)((uint)x >> (32 - n)));
            var rr = f | sec;
            var v = (f | sec) + d;

            var q = ((int)x) << ((int)n);
            var w = ((int)((uint)x >> (32 - n)));
            t = q | w;
            var sy = (long)t + d; // <-----I NEED THIS VALUE, LONG
            var tt = ((long)(-1291243181) + (-1113402670));

            return sy;
        }

        static long second(long b, long g, long j, long i, long n, int r, int s)
        {
            var t = b + (((int)(g & i)) | (((j & (~i))))) + (int)((uint)(n >> 0)) + ((int)s);

            return (((int)t << r) | ((int)((uint)t >> (32 - r)))) + g;
        }

        static long third(long req, long res, long next, long err, long done, int n, int s)
        {
            var x = req + (int)(res ^ next ^ err) + ((int)((uint)(done >> 0))) + s;
            return ((int)x << n | (int)((uint)x >> 32 - n)) + res;
        }
        /// /// /// 
        static long fourth(long a, long b, long c, long d, long x, int s, int ch)
        {
            var n = a + (int)(c ^ (b | (~d))) + ((int)((uint)(x >> 0))) + ch;
            var xx = ((int)n << s | (int)((uint)n >> 32 - s)) + b;
            return ((int)n << s | (int)((uint)n >> 32 - s)) + b;
        }

        public static List<long> bytesToWords(List<long> value)
        {

            /** @type {!Array} */
            List<long> input = new List<long>();
            input.Add(0);
            /** @type {number} */
            var j = 0;
            /** @type {number} */
            var i = 0;
            long r = 0;
            for (; j < value.Count; j++, i = i + 8)
            {

                var q = (int)((uint)i >> 5);

                if (q >= input.Count)
                {
                    input.Add(0);
                    r = input[(int)((uint)i >> 5)];
                    r = 0;
                }
                else
                {
                    r = input[(int)((uint)i >> 5)];
                }

                int t = (int)((uint)i >> 5);
                long result = r | value[j] << 24 - i % 32;
                input[t] = result;
            }
            return input;
        }

        static int[] wordsToBytes(int[] words)
        {
            /** @type {!Array} */
            var bytes = new int[words.Length];
            /** @type {number} */
            var i = 0;
            for (; i < 32 * words.Length; i = i + 8)
            {
                bytes[i] = words[i >> 5] >> 24 - i % 32 & 255;
            }
            return bytes;
        }
        static List<long> wordsToBytes2(long[] words)
        {
            /** @type {!Array} */
            var bytes = new List<long>();
            /** @type {number} */
            var i = 0;
            for (; i < 32 * words.Length; i = i + 8)
            {

                var q = words[(uint)i >> 5];
                var w = 24 - i;
                var y = (uint)q >> w;
                var e = (32 & 255);
                var r = w & e;
                var t = y - r;

                var final = (uint)words[(uint)i >> 5] >> 24 - i % 32 & 255;

                bytes.Add(final);
            }
            return bytes;
        }

        static List<long> stringToBytesS(string o_content)
        {

            return stringToBytes(o_content);
        }

        static List<long> stringToBytes(string s)
        {
            /** @type {!Array} */
            var bytes = new List<long>();
            /** @type {number} */

            for (var i = 0; i < s.Length; i++)
            {
                var f = 255 & s[i];
                bytes.Add(255 & s[i]);
            }

            return bytes;
        }

        static long[] r(string value)
        {

            var gfgf = value.Length;
            var v = stringToBytesS(value);

            var x = bytesToWords(v);
            /** @type {number} */
            var len = 8 * v.Count;
            /** @type {number} */
            long b = 1732584193;
            /** @type {number} */
            long c = -271733879;
            /** @type {number} */
            long d = -1732584194;
            /** @type {number} */
            long a = 271733878;
            /** @type {number} */
            var i = 0;
            for (; i < x.Count; i++)
            {
                /** @type {number} */
                x[i] = 16711935 & (((int)x[i]) << 8 | ((int)(uint)x[i] >> 24)) | 4278255360 & (((int)x[i]) << 24 | ((int)(uint)x[i] >> 8));

                //var o = x[i];
                //var f = (int)o << 8;
                //var sec = (int)((uint)x[i] >> 24);
                //var fin1 = f | sec;
                //var f1 = 16711935 & fin1;
                //var third = x[i] << 24;
                //var fourth = ((int)(uint)x[i] >> 8);
                //var f2 = third | fourth;
                //var sec2 = 4278255360 & f2;
                //var final1 = f1 | f2;

                //var re = x[1];
                //var r = (int)((uint)x[i] >> 8);
            }

            var q = (int)(uint)len >> 5;
            var w = 128 << (int)(uint)len % 32;
            if (q == x.Count)
            {
                x.Add(0);
            }
            x[q] |= w;


            var r = 14 + (len + 64 >> 9 << 4);
            if (r > x.Count)
            {
                var ci = r - x.Count + 1;
                for (int j = 0; j < ci; j++)
                {
                    x.Add(0);
                }

                var yy = x.Count;

            }
            x[14 + (len + 64 >> 9 << 4)] = len;
            var ttt = 14 + (len + 64 >> 9 << 4);


            var indx = ((int)(uint)len >> 5);

            var val1 = (int)x[((int)(uint)len >> 5)];
            var vrs = ((int)128 << len % 32);

            var comb1 = val1 | vrs;

            /** @type {number} */
            // x.Insert(indx, comb1);



            /** @type {number} */
            var first1 = 14 + ((int)((uint)len + 64 >> 9) << 4);
            //for (int j = x.Count; j < first1 + 1; j++)
            //{
            //    x.Add(0);
            //}
            //x[first1] = len;
            //for (int ii = 0; ii < 20; ii++)
            //{
            //    x.Add(0);
            //}
            //x[282] = 0; //custom add, so it will be undefined like JS script

            /** @type {number} */
            i = 0;
            for (; i < x.Count; i = i + 16)
            {

                /** @type {number} */
                var tintB = b;
                /** @type {number} */
                var y = c;
                /** @type {number} */
                var curr_photo = d;
                /** @type {number} */
                var up = a;


                try
                {
                    b = first(b, c, d, a, x[i + 0], 7, -680876936);
                }
                catch (Exception)
                {
                    b = first(b, c, d, a, 0, 7, -680876936);
                }
                try
                {
                    a = first(a, b, c, d, x[i + 1], 12, -389564586);
                }
                catch (Exception)
                {
                    a = first(a, b, c, d, 0, 12, -389564586);
                }
                try
                {
                    d = first(d, a, b, c, x[i + 2], 17, 606105819);
                }
                catch (Exception)
                {
                    d = first(d, a, b, c, 0, 17, 606105819);
                }
                try
                {
                    c = first(c, d, a, b, x[i + 3], 22, -1044525330);
                }
                catch (Exception)
                {
                    c = first(c, d, a, b, 0, 22, -1044525330);
                }
                try
                {
                    b = first(b, c, d, a, x[i + 4], 7, -176418897);
                }
                catch (Exception)
                {
                    b = first(b, c, d, a, 0, 7, -176418897);
                }
                try
                {
                    a = first(a, b, c, d, x[i + 5], 12, 1200080426);
                }
                catch (Exception)
                {
                    a = first(a, b, c, d, 0, 12, 1200080426);
                }
                try
                {
                    d = first(d, a, b, c, x[i + 6], 17, -1473231341);
                }
                catch (Exception)
                {
                    d = first(d, a, b, c, 0, 17, -1473231341);
                }
                try
                {
                    c = first(c, d, a, b, x[i + 7], 22, -45705983);
                }
                catch (Exception)
                {
                    c = first(c, d, a, b, 0, 22, -45705983);
                }
                try
                {
                    b = first(b, c, d, a, x[i + 8], 7, 1770035416);
                }
                catch (Exception)
                {
                    b = first(b, c, d, a, 0, 7, 1770035416);
                }
                try
                {
                    a = first(a, b, c, d, x[i + 9], 12, -1958414417);
                }
                catch (Exception)
                {
                    a = first(a, b, c, d, 0, 12, -1958414417);
                }
                try
                {
                    d = first(d, a, b, c, x[i + 10], 17, -42063);
                }
                catch (Exception)
                {
                    d = first(d, a, b, c, 0, 17, -42063);
                }
                try
                {
                    c = first(c, d, a, b, x[i + 11], 22, -1990404162);
                }
                catch (Exception)
                {
                    c = first(c, d, a, b, 0, 22, -1990404162);
                }
                try
                {
                    b = first(b, c, d, a, x[i + 12], 7, 1804603682);
                }
                catch (Exception)
                {
                    b = first(b, c, d, a, 0, 7, 1804603682);
                }
                try
                {
                    a = first(a, b, c, d, x[i + 13], 12, -40341101);
                }
                catch (Exception)
                {
                    a = first(a, b, c, d, 0, 12, -40341101);
                }
                try
                {
                    d = first(d, a, b, c, x[i + 14], 17, -1502002290);
                }
                catch (Exception)
                {
                    d = first(d, a, b, c, 0, 17, -1502002290);
                }
                try
                {
                    c = first(c, d, a, b, x[i + 15], 22, 1236535329);

                }
                catch (Exception)
                {
                    c = first(c, d, a, b, 0, 22, 1236535329);


                }
                b = second(b, c, d, a, x[i + 1], 5, -165796510);
                try
                {
                    a = second(a, b, c, d, x[i + 6], 9, -1069501632);
                }
                catch (Exception)
                {
                    a = second(a, b, c, d, 0, 9, -1069501632);
                }
                try
                {
                    d = second(d, a, b, c, x[i + 11], 14, 643717713);
                }
                catch (Exception)
                {
                    d = second(d, a, b, c, 0, 14, 643717713);
                }
                c = second(c, d, a, b, x[i + 0], 20, -373897302);
                try
                {
                    b = second(b, c, d, a, x[i + 5], 5, -701558691);
                }
                catch (Exception)
                {
                    b = second(b, c, d, a, 0, 5, -701558691);
                }
                try
                {
                    a = second(a, b, c, d, x[i + 10], 9, 38016083);
                }
                catch (Exception)
                {
                    a = second(a, b, c, d, 0, 9, 38016083);
                }
                try
                {
                    d = second(d, a, b, c, x[i + 15], 14, -660478335);
                }
                catch (Exception)
                {

                    d = second(d, a, b, c, 0, 14, -660478335);

                }
                try
                {
                    c = second(c, d, a, b, x[i + 4], 20, -405537848);
                }
                catch (Exception)
                {
                    c = second(c, d, a, b, 0, 20, -405537848);
                }
                try
                {
                    b = second(b, c, d, a, x[i + 9], 5, 568446438);
                }
                catch (Exception)
                {
                    b = second(b, c, d, a, 0, 5, 568446438);
                }
                try
                {
                    a = second(a, b, c, d, x[i + 14], 9, -1019803690);
                }
                catch (Exception)
                {
                    a = second(a, b, c, d, 0, 9, -1019803690);
                }
                try
                {
                    d = second(d, a, b, c, x[i + 3], 14, -187363961);
                }
                catch (Exception)
                {
                    d = second(d, a, b, c, 0, 14, -187363961);
                }
                try
                {
                    c = second(c, d, a, b, x[i + 8], 20, 1163531501);
                }
                catch (Exception)
                {
                    c = second(c, d, a, b, 0, 20, 1163531501);
                }
                try
                {
                    b = second(b, c, d, a, x[i + 13], 5, -1444681467);
                }
                catch (Exception)
                {
                    b = second(b, c, d, a, 0, 5, -1444681467);
                }
                try
                {
                    a = second(a, b, c, d, x[i + 2], 9, -51403784);
                }
                catch (Exception)
                {
                    a = second(a, b, c, d, 0, 9, -51403784);
                }
                try
                {
                    d = second(d, a, b, c, x[i + 7], 14, 1735328473);
                }
                catch (Exception)
                {
                    d = second(d, a, b, c, 0, 14, 1735328473);
                }
                try
                {
                    c = second(c, d, a, b, x[i + 12], 20, -1926607734);
                }
                catch (Exception)
                {
                    c = second(c, d, a, b, 0, 20, -1926607734);
                }
                try
                {
                    b = third(b, c, d, a, x[i + 5], 4, -378558);
                }
                catch (Exception)
                {
                    b = third(b, c, d, a, 0, 4, -378558);
                }
                try
                {
                    a = third(a, b, c, d, x[i + 8], 11, -2022574463);
                }
                catch (Exception)
                {
                    a = third(a, b, c, d, 0, 11, -2022574463);
                }
                try
                {
                    d = third(d, a, b, c, x[i + 11], 16, 1839030562);
                }
                catch (Exception)
                {
                    d = third(d, a, b, c, 0, 16, 1839030562);
                }
                try
                {
                    c = third(c, d, a, b, x[i + 14], 23, -35309556);
                }
                catch (Exception)
                {
                    c = third(c, d, a, b, 0, 23, -35309556);
                }
                try
                {
                    b = third(b, c, d, a, x[i + 1], 4, -1530992060);
                }
                catch (Exception)
                {
                    b = third(b, c, d, a, 0, 4, -1530992060);
                }
                try
                {
                    a = third(a, b, c, d, x[i + 4], 11, 1272893353);
                }
                catch (Exception)
                {
                    a = third(a, b, c, d, 0, 11, 1272893353);
                }
                try
                {
                    d = third(d, a, b, c, x[i + 7], 16, -155497632);
                }
                catch (Exception)
                {
                    d = third(d, a, b, c, 0, 16, -155497632);
                }
                try
                {
                    c = third(c, d, a, b, x[i + 10], 23, -1094730640);
                }
                catch (Exception)
                {
                    c = third(c, d, a, b, 0, 23, -1094730640);
                }
                try
                {
                    b = third(b, c, d, a, x[i + 13], 4, 681279174);
                }
                catch (Exception)
                {
                    b = third(b, c, d, a, 0, 4, 681279174);
                }
                try
                {
                    a = third(a, b, c, d, x[i + 0], 11, -358537222);
                }
                catch (Exception)
                {
                    a = third(a, b, c, d, 0, 11, -358537222);
                }
                try
                {
                    d = third(d, a, b, c, x[i + 3], 16, -722521979);
                }
                catch (Exception)
                {
                    d = third(d, a, b, c, 0, 16, -722521979);
                }
                try
                {
                    c = third(c, d, a, b, x[i + 6], 23, 76029189);
                }
                catch (Exception)
                {
                    c = third(c, d, a, b, 0, 23, 76029189);
                }
                try
                {
                    b = third(b, c, d, a, x[i + 9], 4, -640364487);
                }
                catch (Exception)
                {
                    b = third(b, c, d, a, 0, 4, -640364487);
                }
                try
                {
                    a = third(a, b, c, d, x[i + 12], 11, -421815835);
                }
                catch (Exception)
                {
                    a = third(a, b, c, d, 0, 11, -421815835);
                }
                try
                {
                    d = third(d, a, b, c, x[i + 15], 16, 530742520);
                }
                catch (Exception)
                {

                    d = third(d, a, b, c, 0, 16, 530742520);

                }
                try
                {
                    c = third(c, d, a, b, x[i + 2], 23, -995338651);
                }
                catch (Exception)
                {
                    c = third(c, d, a, b, 0, 23, -995338651);
                }
                try
                {
                    b = fourth(b, c, d, a, x[i + 0], 6, -198630844);
                }
                catch (Exception)
                {
                    b = fourth(b, c, d, a, 0, 6, -198630844);
                }
                try
                {
                    a = fourth(a, b, c, d, x[i + 7], 10, 1126891415);
                }
                catch (Exception)
                {
                    a = fourth(a, b, c, d, 0, 10, 1126891415);
                }
                try
                {
                    d = fourth(d, a, b, c, x[i + 14], 15, -1416354905);
                }
                catch (Exception)
                {
                    d = fourth(d, a, b, c, 0, 15, -1416354905);
                }
                try
                {
                    c = fourth(c, d, a, b, x[i + 5], 21, -57434055);
                }
                catch (Exception)
                {
                    c = fourth(c, d, a, b, 0, 21, -57434055);
                }
                try
                {
                    b = fourth(b, c, d, a, x[i + 12], 6, 1700485571);
                }
                catch (Exception)
                {
                    b = fourth(b, c, d, a, 0, 6, 1700485571);
                }
                try
                {
                    a = fourth(a, b, c, d, x[i + 3], 10, -1894986606);
                }
                catch (Exception)
                {
                    a = fourth(a, b, c, d, 0, 10, -1894986606);
                }
                try
                {
                    d = fourth(d, a, b, c, x[i + 10], 15, -1051523);
                }
                catch (Exception)
                {
                    d = fourth(d, a, b, c, 0, 15, -1051523);
                }
                try
                {
                    c = fourth(c, d, a, b, x[i + 1], 21, -2054922799);
                }
                catch (Exception)
                {
                    c = fourth(c, d, a, b, 0, 21, -2054922799);
                }
                try
                {
                    b = fourth(b, c, d, a, x[i + 8], 6, 1873313359);
                }
                catch (Exception)
                {
                    b = fourth(b, c, d, a, 0, 6, 1873313359);
                }
                try
                {
                    a = fourth(a, b, c, d, x[i + 15], 10, -30611744);
                }
                catch (Exception)
                {
                    a = fourth(a, b, c, d, 0, 10, -30611744);

                }
                try
                {
                    d = fourth(d, a, b, c, x[i + 6], 15, -1560198380);
                }
                catch (Exception)
                {
                    d = fourth(d, a, b, c, 0, 15, -1560198380);
                }
                try
                {
                    c = fourth(c, d, a, b, x[i + 13], 21, 1309151649);
                }
                catch (Exception)
                {
                    c = fourth(c, d, a, b, 0, 21, 1309151649);
                }
                try
                {
                    b = fourth(b, c, d, a, x[i + 4], 6, -145523070);
                }
                catch (Exception)
                {
                    b = fourth(b, c, d, a, 0, 6, -145523070);
                }
                try
                {
                    a = fourth(a, b, c, d, x[i + 11], 10, -1120210379);
                }
                catch (Exception)
                {
                    a = fourth(a, b, c, d, 0, 10, -1120210379);
                }
                try
                {
                    d = fourth(d, a, b, c, x[i + 2], 15, 718787259);
                }
                catch (Exception)
                {
                    d = fourth(d, a, b, c, 0, 15, 718787259);
                }
                try
                {
                    c = fourth(c, d, a, b, x[i + 9], 21, -343485551);
                }
                catch (Exception)
                {
                    c = fourth(c, d, a, b, 0, 21, -343485551);
                }
                var xp = (int)b + (uint)(tintB >> 0);
                /** @type {number} */
                b = (uint)b + (uint)(tintB >> 0);
                /** @type {number} */

                var xx = c + y;
                c = (uint)xx >> 0;
                /** @type {number} */

                d = (uint)(d + curr_photo) >> 0;
                /** @type {number} */
                a = (uint)((uint)a + (up >> 0));
                var t = 0;
            }

            var arr = new long[4] { b, c, d, a };
            return endian(arr);
        }

        public static string GenerateXPing(string e)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(e);
            sb.AppendLine("whitetelevisionbulbelectionroofhorseflying");
            var t = sb.ToString().Replace("\r\n", string.Empty);

            try
            {
                long[] digestbytes = r(t);
                var next = wordsToBytes2(digestbytes);
                var ss = bytesToHex(next);
                return ss;
            }
            catch (Exception)
            {

                return "";
            }
            //FIXME_VAR_TYPE digestbytes= [235,178,157,29,39,28,79,223,188,51,173,131,24,97,27,248];
           // return "";
        }
    }
}
