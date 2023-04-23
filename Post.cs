using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetwayProject
{
    // Post myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Post
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        //public string Title { get; set; }
        public object Tittle { get; internal set; }
    }


}
