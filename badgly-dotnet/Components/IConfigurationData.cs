namespace Badg.ly
{
   public interface IConfigurationData
   {
      int Timeout { get; }
      string ForcedUrl { get; }
   }

   internal class ConfigurationData : IConfigurationData
   {
      private int _timeout = 10000;

      public int Timeout
      {
         get { return _timeout; }
         set { _timeout = value; }
      }

      public string ForcedUrl { get; set; }
   }
}