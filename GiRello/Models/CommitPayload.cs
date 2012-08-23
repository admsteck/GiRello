using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GiRello.Models
{
    [DataContract]
    public class CommitPayload
    {
        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CommitPayload));

        [DataMember(Name = "repository")]
        public Repository repository { get; set; }

        public static CommitPayload Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static CommitPayload Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (CommitPayload)jsonSerializer.ReadObject(jsonStream);
        }

    }

}