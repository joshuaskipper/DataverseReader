using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace DataverseReader
{
    public class ODataResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        public List<Contact> value { get; set; }
    }

    public class Contact
    {
        [JsonPropertyName("@odata.etag")]
        public string ODataEtag { get; set; }

        public string fullname { get; set; }
        public string emailaddress1 { get; set; }
        public string contactid { get; set; }
        public string telephone1 { get; set; }
    }
}