namespace Badg.ly
{
   using System;
   using System.Collections.Generic;
   using System.Net;
   using System.Text;
   using System.Web;
   using Newtonsoft.Json;

   public class Communicator
   {
      public const string GET = "GET";
      public const string POST = "POST";

      private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
                                                                     {
                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                     };

      public void SendPayload<T>(string method, string endPoint, IDictionary<string, object> partialPayload, Action<Response<T>> callback)
      {
         var url = BadglyConfiguration.Data.ForcedUrl ?? "http://api.badg.ly/api/" + endPoint;
         var payload = FinalizePayload(partialPayload);
         if (method == GET)
         {
            url += '?' + payload;
         }
         var request = (HttpWebRequest) WebRequest.Create(url);
         request.Method = method;
         request.UserAgent = "badgly-dotnet";
         request.Timeout = BadglyConfiguration.Data.Timeout;
         request.ReadWriteTimeout = BadglyConfiguration.Data.Timeout;
         request.KeepAlive = false;

         if (method != GET)
         {
            request.BeginGetRequestStream(GetRequestStream<T>, new RequestState<T> {Request = request, Payload = Encoding.UTF8.GetBytes(payload), Callback = callback});
         }
         else 
         {
            request.BeginGetResponse(GetResponseStream<T>, new RequestState<T> {Request = request, Callback = callback});
         }         
      }

      private static void GetRequestStream<T>(IAsyncResult result)
      {
         var state = (RequestState<T>) result.AsyncState;
         if (state.Payload != null)
         {
            using (var requestStream = state.Request.EndGetRequestStream(result))
            {
               requestStream.Write(state.Payload, 0, state.Payload.Length);
               requestStream.Flush();
               requestStream.Close();
            }
         }
         state.Request.BeginGetResponse(GetResponseStream<T>, state);
      }

      private static void GetResponseStream<T>(IAsyncResult result)
      {
         var state = (ResponseState<T>) result.AsyncState;
         try
         {
            using (var response = (HttpWebResponse) state.Request.EndGetResponse(result))
            {
               if (state.Callback != null)
               {
                  state.Callback(Response<T>.CreateSuccess(GetResponseBody(response)));
               }
            }
         }
         catch (Exception ex)
         {
            if (state.Callback != null)
            {
               state.Callback(Response<T>.CreateError(HandleException(ex)));
            }
         }
      }


      private static string FinalizePayload(IEnumerable<KeyValuePair<string, object>> payload)
      {
         var sb = new StringBuilder();
         foreach (var kvp in payload)
         {
            sb.Append(kvp.Key);
            sb.Append("=");
            sb.Append(HttpUtility.UrlEncode(kvp.Value.ToString()));
            sb.Append("&");
         }
         return sb.ToString();
      }

      private static string GetResponseBody(WebResponse response)
      {
         using (var stream = response.GetResponseStream())
         {
            var sb = new StringBuilder();
            int read;
            var bufferSize = response.ContentLength == -1 ? 2048 : (int) response.ContentLength;
            if (bufferSize == 0)
            {
               return null;
            }
            do
            {
               var buffer = new byte[2048];
               read = stream.Read(buffer, 0, buffer.Length);
               sb.Append(Encoding.UTF8.GetString(buffer, 0, read));
            } while (read > 0);
            return sb.ToString();
         }
      }

      private static ErrorMessage HandleException(Exception exception)
      {
         if (exception is WebException)
         {
            var body = GetResponseBody(((WebException) exception).Response);
            try
            {
               var message = JsonConvert.DeserializeObject<ErrorMessage>(body, _jsonSettings);
               message.InnerException = exception;
               return message;
            }
            catch (Exception)
            {
               return new ErrorMessage {Message = body, InnerException = exception};
            }
         }
         return new ErrorMessage {Message = "Unknown Error", InnerException = exception};
      }
      
      private class RequestState<T> : ResponseState<T>
      {
         public byte[] Payload { get; set; }
      }
      
      private class ResponseState<T>
      {
         public HttpWebRequest Request { get; set; }
         public Action<Response<T>> Callback { get; set; }
      }
   }
}