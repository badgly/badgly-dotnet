namespace Badg.ly
{
   using Newtonsoft.Json;

   public class Earned
   {
      [JsonProperty("badge_id")]
      public string BadgeId { get; set; }
      [JsonProperty("times")]
      public int Times { get; set; }
   }
}