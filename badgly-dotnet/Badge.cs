namespace Badg.ly
{
   using Newtonsoft.Json;

   public class Badge
   {
      public string Id { get; set; }      
      public string Name { get; set; }
      [JsonProperty("desc")]
      public string Description { get; set; }    
      public object Meta { get; set; }      
      public int? Points { get; set; }      
      public int? Total { get; set; }      
      public int? Unique { get; set; }
   }
}