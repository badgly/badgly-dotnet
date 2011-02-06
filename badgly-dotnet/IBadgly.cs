namespace Badg.ly
{
   using System;
   
   public interface IBadgly
   {
      /// <summary>
      /// Get's a badge object
      /// </summary>
      /// <param name="badgeId">the id of the badge to get</param>
      /// <param name="callback">the callback to execute when the badge information has been received</param>
      void GetBadge(string badgeId, Action<Response<BadgeResponse>> callback);

      /// <summary>
      /// Get's all of the bdges
      /// </summary>      
      /// <param name="callback">the callback to execute when the badge information has been received</param>
      void GetBadges(Action<Response<BadgesResponse>> callback);

      /// <summary>
      /// Get's all of the bdges
      /// </summary>      
      /// <param name="userId">the id of the user</param>
      /// <param name="callback">the callback to execute when the earned information has been received</param>
      void GetUser(string userId, Action<Response<UserResponse>> callback);

      /// <summary>
      /// Grants a user a badge
      /// </summary>      
      /// <param name="userId">the id of the user</param>      
      /// <param name="callback">the callback to execute when the earned information has been received</param>
      void Earned(string badgeId, string userId, Action<Response<EarnedResponse>> callback);
   }

}