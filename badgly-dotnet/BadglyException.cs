namespace Badg.ly
{
   using System;

   public class BadglyException : Exception
   {
      public BadglyException()
      {
      }

      public BadglyException(string message) : base(message)
      {
      }

      public BadglyException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}