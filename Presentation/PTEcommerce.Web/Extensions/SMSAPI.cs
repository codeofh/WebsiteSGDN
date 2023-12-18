using Framework.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PTEcommerce.Web.Extensions
{
    public static class SMSAPI
    {
        private static readonly string token  = Config.GetConfigByKey("tokenSMS");
        private static int TYPE_BRANDNAME = 3;
        private static readonly string rootURL = "https://api.speedsms.vn/index.php";
        private static string EncodeNonAsciiCharacters(string value)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string getUserInfo()
        {
            string url = rootURL + "/user/info";
            NetworkCredential myCreds = new NetworkCredential(token, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }

        public static string sendSMS(string[] phones, string content, int type, string sender)
        {
            string url = rootURL + "/sms/send";
            if (phones.Length <= 0)
                return "";
            if (content.Equals(""))
                return "";

            if (type == TYPE_BRANDNAME && sender.Equals(""))
                return "";

            NetworkCredential myCreds = new NetworkCredential(token, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string builder = "{\"to\":[";

            for (int i = 0; i < phones.Length; i++)
            {
                builder += "\"" + phones[i] + "\"";
                if (i < phones.Length - 1)
                {
                    builder += ",";
                }
            }
            builder += "], \"content\": \"" + Uri.EscapeDataString(content) + "\", \"type\":" + type + ", \"sender\": \"" + sender + "\"}";

            string json = builder.ToString();
            return client.UploadString(url, json);
        }

        public static string sendMMS(string[] phones, string content, string link, string sender)
        {
            string url = rootURL + "/mms/send";
            if (phones.Length <= 0)
                return "";
            if (content.Equals(""))
                return "";

            NetworkCredential myCreds = new NetworkCredential(token, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string builder = "{\"to\":[";

            for (int i = 0; i < phones.Length; i++)
            {
                builder += "\"" + phones[i] + "\"";
                if (i < phones.Length - 1)
                {
                    builder += ",";
                }
            }
            builder += "], \"content\": \"" + Uri.EscapeDataString(content) + "\", \"link\": \"" + link + "\", \"sender\": \"" + sender + "\"}";

            string json = builder.ToString();
            return client.UploadString(url, json);
        }
    }
}