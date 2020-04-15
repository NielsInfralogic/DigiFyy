using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models
{
    [JsonObject]
    public class SocialMediaUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

    }
    public class FacebookEmail
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public Picture Picture { get; set; }
    }

    public class Picture
    {
        public Data Data { get; set; }

    }
    public class Data
    {
        public string Height { get; set; }
        public string Is_Silhouette { get; set; }
        public string Url { get; set; }
        public string Width { get; set; }
    }

}

