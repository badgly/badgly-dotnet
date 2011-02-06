namespace Badg.ly
{
   public interface IRequestContext
   {
      int ApiVersion { get; }
      string Secret { get; }
      string Key { get; }
   }
}