namespace Badg.ly
{
   using System;

   public interface IBadglyConfiguration
   {
      IBadglyConfiguration WithTimeout(int timeout);
      IBadglyConfiguration WithTimeout(TimeSpan timeout);

      /// <summary>
      /// Used by the test library
      /// </summary>      
      IBadglyConfiguration ForceUrlTo(string url);
   }

   public class BadglyConfiguration : IBadglyConfiguration
   {
      private static ConfigurationData _data = new ConfigurationData();
      private static readonly BadglyConfiguration _configuration = new BadglyConfiguration();

      protected BadglyConfiguration()
      {
      }

      public static IConfigurationData Data
      {
         get { return _data; }
      }

      public IBadglyConfiguration WithTimeout(int timeout)
      {
         _data.Timeout = timeout;
         return this;
      }

      public IBadglyConfiguration WithTimeout(TimeSpan timeout)
      {
         return WithTimeout((int) timeout.TotalMilliseconds);
      }

      /// <summary>
      /// Used by the test library
      /// </summary>
      public IBadglyConfiguration ForceUrlTo(string url)
      {
         _data.ForcedUrl = url;
         return this;
      }

      public static void Configure(Action<IBadglyConfiguration> action)
      {
         action(_configuration);
      }

      /// <summary>
      /// Used by the test library
      /// </summary>
      public static void ResetToDefaults()
      {
         _data = new ConfigurationData();
      }
   }
}