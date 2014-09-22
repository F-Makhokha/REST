﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RESTServer.Utils.Serialization
{
    public class XmlPipelineSerializer : ISerializer
    {
        private XmlSerializer xmlSerializer;


        public async Task<T> Deserialize<T>(string rawBodyData)
        {
            xmlSerializer = new XmlSerializer(typeof(T));
            T result = default(T);
            using (Stream ms = await GenerateStreamFromString(rawBodyData))
            {
                result = (T)xmlSerializer.Deserialize(ms);
            }
            return result;
        }

        public async Task<Byte[]> SerializeAsBytes<T>(T item)
        {
            xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, item);
                ms.Position = 0;
                return ms.ToArray();
            }
        }


        public async Task<string> Serialize<T>(T item)
        {
            xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, item);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }


        private async Task<Stream> GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
