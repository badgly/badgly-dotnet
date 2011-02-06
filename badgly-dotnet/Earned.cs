namespace Badg.ly
{
   using System;
   using Newtonsoft.Json;

   public class Earned
   {
      [JsonProperty("badge_id")]
      public string BadgeId { get; set; }      
      public int Times { get; set; }
      public DateTime At { get; set; }
   }
}