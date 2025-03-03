using System.Text.Json.Serialization;

namespace Aspire_POS.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Slug { get; set; }
        public AvatarUrls AvatarUrls { get; set; }

        public object Meta { get; set; }

        [JsonPropertyName("is_super_admin")]
        public bool IsSuperAdmin { get; set; }

        [JsonPropertyName("woocommerce_meta")]
        public Dictionary<string, string> WoocommerceMeta { get; set; }

        public List<string> Roles { get; set; }

        [JsonPropertyName("_links")]
        public Links Links { get; set; }
    }

    public class AvatarUrls
    {
        [JsonPropertyName("24")]
        public string Size24 { get; set; }

        [JsonPropertyName("48")]
        public string Size48 { get; set; }

        [JsonPropertyName("96")]
        public string Size96 { get; set; }
    }

    public class Links
    {
        public List<Link> Self { get; set; }
        public List<Link> Collection { get; set; }
    }

    public class Link
    {
        public string Href { get; set; }
        public TargetHints TargetHints { get; set; }
    }

    public class TargetHints
    {
        public List<string> Allow { get; set; }
    }
}
