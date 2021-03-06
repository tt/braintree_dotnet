#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Braintree
{
    public class QueryString
    {
        protected StringBuilder builder;

        public QueryString()
        {
            builder = new StringBuilder();
        }

        public virtual QueryString Append(KeyValuePair<string, string> pair)
        {
            return Append(pair.Key.Replace('-', '_'), pair.Value);
        }

        public virtual QueryString Append(String key, Object value)
        {
            if (value == null)
            {
                return this;
            }
            if (value is Request)
            {
                return AppendRequest(key, (Request)value);
            }
            if (value is Boolean)
            {
                return AppendString(key, value.ToString().ToLower());
            }
            else if (value is Dictionary<String, String>)
            {
                foreach (KeyValuePair<String, String> pair in (Dictionary<String, String>)value)
                {
                    AppendString(String.Format("{0}[{1}]", key, pair.Key), pair.Value);
                }
                return this;
            }

            return AppendString(key, value.ToString());
        }

        protected virtual QueryString AppendString(String key, String value)
        {
            if (key != null && !(key == "") && value != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append(EncodeParam(key, value));
            }
            return this;
        }

        protected virtual QueryString AppendRequest(String parent, Request request)
        {
            if (request == null)
            {
                return this;
            }
            String requestQueryString = request.ToQueryString(parent);
            if (requestQueryString.Length > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }

                builder.Append(requestQueryString);
            }
            return this;
        }


        protected virtual String EncodeParam(String key, String value)
        {
            return HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value);
        }

        public override String ToString()
        {
            return builder.ToString();
        }
    }
}
