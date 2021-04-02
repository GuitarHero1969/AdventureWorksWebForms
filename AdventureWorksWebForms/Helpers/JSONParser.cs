using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace AdventureWorksWebForms.Helpers
{
    public class JSONParser
    {
        private static string output;

        // C# 4.0 introduced named and optional arguments for methods, indexers, constructors and delegates...

        /// <summary>
        /// Recursive JSON string parser.
        /// Pass the JSON in string format and request the field name to return the field value.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="fieldName"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string GetValue(string jsonString, string fieldName, int top = 0)
        {
            JObject ser = JObject.Parse(jsonString);

            JToken[] data;
            JToken[] jT;

            if (top == 0)
            {
                data = ser.Children().ToArray();
                jT = data[0].First.ToArray();
            }
            else
                jT = ser.Children().ToArray();

            foreach (JProperty itm in jT)
            {
                if (itm.Name == fieldName)
                {
                    output = itm.Value.ToString();
                }
                if (itm.Value.Type == JTokenType.Array)
                {
                    string str = itm.First.First.ToString();
                    GetValue(str, fieldName, 1);
                }
                if (itm.Value.Type == JTokenType.Object)
                {
                    string str = itm.First.ToString();
                    GetValue(str, fieldName, 1);
                }
            }

            return output;
        }
    }
}