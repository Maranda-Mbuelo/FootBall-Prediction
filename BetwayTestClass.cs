using Newtonsoft.Json;


namespace BetwayProject
{
    // ...BetwayClass...  myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class BetwayTestClass
    {
        [JsonProperty("MatchNumber")]
        public int MatchNumber { get; set; }

        [JsonProperty("RoundNumber")]
        public int RoundNumber { get; set; }

        [JsonProperty("DateUtc")]
        public string DateUtc { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("HomeTeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("AwayTeam")]
        public string AwayTeam { get; set; }

        [JsonProperty("Group")]
        public string Group { get; set; }

        [JsonProperty("HomeTeamScore")]
        public int HomeTeamScore { get; set; }

        [JsonProperty("AwayTeamScore")]
        public int AwayTeamScore { get; set; }
    }
}


