namespace Badg.ly
{
   using System.Collections.Generic;
   using Newtonsoft.Json;

   public class BadgesResponse
   {
      [JsonProperty("badges")]
      public IList<Badge> Badges { get; set; }
   }
}