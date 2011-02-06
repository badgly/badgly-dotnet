namespace Badg.ly
{
   using System;
   using System.Collections.Generic;
   using System.Security.Cryptography;
   using System.Text;
   using Newtonsoft.Json;

   public class Badgly : IBadgly, IRequestContext
   {
      public string Key { get; private set; }
      public int ApiVersion { get { return 1; }}
      public string Secret { get; private set; }

      public Badgly(string key, string secret)
      {
         Key = key;
         Secret = secret;
      }

      public void GetBadge(string badgeId, Action<Response<BadgeResponse>> callback)
      {
         var payload = BuildPayload(new Dictionary<string, object>(3) { { "id", badgeId } }, false);
         new Communicator().SendPayload<BadgeResponse>(Communicator.GET, "badge", payload, r =>
         {
            if (r.Success)
            {
               r.Data = JsonConvert.DeserializeObject<BadgeResponse>(r.Raw);
            }
            callback(r);
         });
      }

      public void GetBadges(Action<Response<BadgesResponse>> callback)
      {
         var payload = BuildPayload(null, false);
         new Communicator().SendPayload<BadgesResponse>(Communicator.GET, "badges", payload, r =>
         {
            if (r.Success)
            {
               r.Data = JsonConvert.DeserializeObject<BadgesResponse>(r.Raw);
            }
            callback(r);
         });
      }


      public void GetUser(string userId, Action<Response<UserResponse>> callback)
      {
         var payload = BuildPayload(new Dictionary<string, object>(3) {{"uid", userId}}, false);
         new Communicator().SendPayload<UserResponse>(Communicator.GET, "user", payload, r =>
         {
            if (r.Success)
            {
               r.Data = JsonConvert.DeserializeObject<UserResponse>(r.Raw);
            }
            callback(r);
         });
      }

      public void Earned(string badgeId, string userId, Action<Response<EarnedResponse>> callback)
      {
         var payload = BuildPayload(new SortedDictionary<string, object> { { "id", badgeId}, {"uid", userId } }, true);
         new Communicator().SendPayload<EarnedResponse>(Communicator.GET, "earned", payload, r =>
         {
            if (r.Success)
            {
               r.Data = JsonConvert.DeserializeObject<EarnedResponse>(r.Raw);
            }
            callback(r);
         });
      }


      private IDictionary<string, object> BuildPayload(IDictionary<string, object> payload, bool sign)
      {         
         if (payload == null)
         {
            payload = new Dictionary<string, object>(2);
         }
         payload.Add("key", Key);
         payload.Add("v", ApiVersion);
         if (sign)
         {
            payload.Add("sig", SignPayload(payload));
         }
         return payload;
      }

      private string SignPayload(IEnumerable<KeyValuePair<string, object>> payload)
      {
         var sb = new StringBuilder();         
         foreach(var kvp in payload)
         {
            sb.Append(kvp.Key);
            sb.Append("|");
            sb.Append(kvp.Value);
            sb.Append("|");
         }
         sb.Append(Secret);
         using (var hasher = new SHA1Managed())
         {
            var bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var data = new StringBuilder(bytes.Length * 2);
            for (var i = 0; i < bytes.Length; ++i)
            {
               data.Append(bytes[i].ToString("x2"));
            }
            return data.ToString();
         }         
      }
   }
}