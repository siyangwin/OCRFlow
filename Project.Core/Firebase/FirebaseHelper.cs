using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Project.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core
{
    public class FirebaseHelper
    {
        static FirebaseHelper()
        {
            // [START access_services_default]
            // Initialize the default app
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("Config/google-firebase.json"),
            });
            //Console.WriteLine(defaultApp.Name); // "[DEFAULT]"

            // Retrieve services by passing the defaultApp variable...
            //var defaultAuth = FirebaseAuth.GetAuth(defaultApp);

            // ... or use the equivalent shorthand notation
            //defaultAuth = FirebaseAuth.DefaultInstance;
            // [END access_services_default]
        }
        /// <summary>
        /// 创建主题并像主题中添加用户
        /// </summary>
        /// <param name="topic">主题名称</param>
        /// <param name="registrationTokens">推送的token</param>
        /// <returns></returns>
        public static bool SubscribeToTopicAsync(string topic, List<string> registrationTokens, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            // [START subscribe_to_topic]
            // These registration tokens come from the client FCM SDKs.
            //var registrationTokens = new List<string>()
            //{
            //    "YOUR_REGISTRATION_TOKEN_1",
            //    // ...
            //    "YOUR_REGISTRATION_TOKEN_n",
            //};

            // Subscribe the devices corresponding to the registration tokens to the
            // topic

            Task<TopicManagementResponse> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
              registrationTokens, topic);
                if (response.Result.FailureCount > 0)
                {
                    flg = false;
                }
                result = JsonConvert.SerializeObject(response.Result);
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "PushToken订阅到谷歌推送主题", ReqParameter = "topic:" + topic + ",token:" + JsonConvert.SerializeObject(registrationTokens), ResParameter = result });
                }
            }

            return flg;
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            //Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");
            // [END subscribe_to_topic]
        }
        /// <summary>
        /// 从主题中删除用户
        /// </summary>
        /// <param name="topic">主题名称</param>
        /// <param name="registrationTokens">推送的token</param>
        /// <returns></returns>
        public static bool UnsubscribeFromTopicAsync(string topic, List<string> registrationTokens, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            // [START unsubscribe_from_topic]
            // These registration tokens come from the client FCM SDKs.
            //var registrationTokens = new List<string>()
            //{
            //    "YOUR_REGISTRATION_TOKEN_1",
            //    // ...
            //    "YOUR_REGISTRATION_TOKEN_n",
            //};

            // Unsubscribe the devices corresponding to the registration tokens from the
            // topic
            Task<TopicManagementResponse> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(
               registrationTokens, topic);
                if (response.Result.FailureCount > 0)
                {
                    flg = false;
                }
                result = JsonConvert.SerializeObject(response.Result);
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "从谷歌推送主题中删除PushToken", ReqParameter = "topic:" + topic + "token:" + JsonConvert.SerializeObject(registrationTokens), ResParameter = result });
                }
            }
            return flg;
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            //Console.WriteLine($"{response.SuccessCount} tokens were unsubscribed successfully");
            // [END unsubscribe_from_topic]
        }

        /// <summary>
        /// 向主题组合发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendToConditionAsync(Message message, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            // [START send_to_condition]
            // Define a condition which will send to devices which are subscribed
            // to either the Google stock or the tech industry topics.
            var condition = "'stock-GOOG' in topics || 'industry-tech' in topics";

            // See documentation on defining a message payload.
            //var message = new Message()
            //{
            //    Notification = new Notification()
            //    {
            //        Title = "$GOOG up 1.43% on the day",
            //        Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
            //    },
            //    Condition = condition,
            //};

            // Send a message to devices subscribed to the combination of topics
            // specified by the provided condition.
            Task<string> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = response = FirebaseMessaging.DefaultInstance.SendAsync(message);
                result = response.Result;
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "谷歌推送SendToConditionAsync", ReqParameter = JsonConvert.SerializeObject(message), ResParameter = result });
                }
            }
            return flg;
            // Response is a message ID string.
            //Console.WriteLine("Successfully sent message: " + response);
            // [END send_to_condition]
            //return response;
        }
        /// <summary>
        /// 向主题推送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendToTopicAsync(Message message, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            // [START send_to_topic]
            // The topic name can be optionally prefixed with "/topics/".
            //var topic = "highScores";

            // See documentation on defining a message payload.
            //var message = new Message()
            //{
            //    Notification = new Notification()
            //    {
            //        //Title = item.MessageTitle,
            //        //Body = item.MessageContent
            //    },
            //    //数据消息内容
            //    //Data = new Core.FireBase.TransmissionPushData()
            //    //{
            //    //    AlterTitle = item.MessageTitle,
            //    //    AlertBody = item.MessageContent,
            //    //    Type = (int)PushType.OpenPage,
            //    //    TypeName = EnumExtension.GetEnumDesc(PushType.OpenPage, ""),
            //    //    AndroidAppPageName = EnumExtension.GetEnumDesc(MeesageJumpAddressEnum.AndroidOrderpage, ""),
            //    //    IOSAppPageName = EnumExtension.GetEnumDesc(MeesageJumpAddressEnum.IosOrderpage, ""),
            //    //    AppPageData = JsonConvert.SerializeObject(new { orderid = item.OrderNo })
            //    //},
            //    Topic = topic,
            //};

            // Send a message to the devices subscribed to the provided topic.

            Task<string> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = FirebaseMessaging.DefaultInstance.SendAsync(message);
                //response.Wait();
                result = response.Result;
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "谷歌推送SendToTopicAsync", ReqParameter = JsonConvert.SerializeObject(message), ResParameter = result });
                }
            }

            return flg;
            // Response is a message ID string.
            //Console.WriteLine("Successfully sent message: " + response);
            // [END send_to_topic]
        }
        /// <summary>
        /// 向特定用户发送消息
        /// </summary>
        /// <returns></returns>
        public static bool SendToTokenAsync(Message message, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            // [START send_to_token]
            // This registration token comes from the client FCM SDKs.
            //var registrationToken = "YOUR_REGISTRATION_TOKEN";

            // See documentation on defining a message payload.
            //var message = new Message()
            //{
            //    Data = new Dictionary<string, string>()
            //    {
            //        { "score", "850" },
            //        { "time", "2:45" },
            //    },
            //    Token = registrationToken,
            //};

            // Send a message to the device corresponding to the provided
            // registration token.
            Task<string> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = FirebaseMessaging.DefaultInstance.SendAsync(message);
                //response.Wait();
                result = response.Result;
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "谷歌推送SendToTokenAsync", ReqParameter = JsonConvert.SerializeObject(message), ResParameter = result });
                }
            }

            return flg;
            // Response is a message ID string.
            //Console.WriteLine("Successfully sent message: " + response);
            // [END send_to_token]
        }
        /// <summary>
        /// 向多个设备发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendMulticastAsync(MulticastMessage message, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            // [START send_multicast]
            // Create a list containing up to 500 registration tokens.
            // These registration tokens come from the client FCM SDKs.
            //var registrationTokens = new List<string>()
            //{
            //    "YOUR_REGISTRATION_TOKEN_1",
            //    // ...
            //    "YOUR_REGISTRATION_TOKEN_n",
            //};
            //var message = new MulticastMessage()
            //{
            //    Tokens = registrationTokens,
            //    Data = new Dictionary<string, string>()
            //    {
            //        { "score", "850" },
            //        { "time", "2:45" },
            //    },
            //};
            Task<BatchResponse> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                //response.Wait();
                if (response.Result.FailureCount > 0)
                {
                    flg = false;
                }
                result = JsonConvert.SerializeObject(response.Result);
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "谷歌推送SendMulticastAsync", ReqParameter = JsonConvert.SerializeObject(message), ResParameter = result });
                }
            }
            return flg;
            // See the BatchResponse reference documentation
            // for the contents of response.
            //Console.WriteLine($"{response.Result.SuccessCount} messages were sent successfully");
            // [END send_multicast]
            //return response;
        }
        /// <summary>
        /// 一次发送多条消息
        /// </summary>
        /// <returns></returns>
        public static bool SendAllAsync(List<Message> messages, Microsoft.AspNetCore.Http.HttpContext httpContent = null)
        {
            //var registrationToken = "YOUR_REGISTRATION_TOKEN";
            //// [START send_all]
            //// Create a list containing up to 500 messages.
            //var messages = new List<Message>()
            //{
            //    new Message()
            //    {
            //        Notification = new Notification()
            //        {
            //            Title = "Price drop",
            //            Body = "5% off all electronics",
            //        },
            //        Token = registrationToken,
            //    },
            //    new Message()
            //    {
            //        Notification = new Notification()
            //        {
            //            Title = "Price drop",
            //            Body = "2% off all books",
            //        },
            //        Topic = "readers-club",
            //    },
            //};
            Task<BatchResponse> response = null;
            string result = "";
            bool flg = true;
            try
            {
                response = FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
                //response.Wait();
                if (response.Result.FailureCount > 0)
                {
                    flg = false;
                }
                result = JsonConvert.SerializeObject(response.Result);
            }
            catch (Exception ex)
            {
                flg = false;
                result = ex.Message;
            }
            finally
            {
                if (httpContent != null)
                {
                    //写入日志
                    GlobalConfig.SystemLogService.LocalAndSqlLogAdd(new SystemLog { Guid = httpContent.Request.Headers["GuidPwd"].ToString(), ClientType = httpContent.Request.Headers["ClientType"].ToString(), APIName = httpContent.Request.Path, UserId = httpContent.Request.Headers["UserId"].ToString() == "" ? 0 : Convert.ToInt32(httpContent.Request.Headers["UserId"]), DeviceId = httpContent.Request.Headers["DeviceId"].ToString() == "" ? "0" : httpContent.Request.Headers["DeviceId"].ToString(), Instructions = "谷歌推送SendAllAsync", ReqParameter = JsonConvert.SerializeObject(messages), ResParameter = result });
                }
            }
            return flg;
            //return response;
            // See the BatchResponse reference documentation
            // for the contents of response.
            //Console.WriteLine($"{response.Result.SuccessCount} messages were sent successfully");
            // [END send_all]

        }
        /// <summary>
        /// 发送空运行异步消息
        /// </summary>
        /// <returns></returns>
        public static Task SendDryRunAsync(Message message)
        {
            //var message = new Message()
            //{
            //    Data = new Dictionary<string, string>()
            //    {
            //        { "score", "850" },
            //        { "time", "2:45" },
            //    },
            //    Token = "token",
            //};

            // [START send_dry_run]
            // Send a message in the dry run mode.
            var response = FirebaseMessaging.DefaultInstance.SendAsync(
                message, dryRun: true);
            // Response is a message ID string.
            //Console.WriteLine("Dry run successful: " + response);
            // [END send_dry_run]
            return response;
        }

        /// <summary>
        /// 发送多个设备并处理并返回响应的结果
        /// </summary>
        /// <returns></returns>
        internal static async Task SendMulticastAndHandleErrorsAsync()
        {
            // [START send_multicast_error]
            // These registration tokens come from the client FCM SDKs.
            var registrationTokens = new List<string>()
            {
                "YOUR_REGISTRATION_TOKEN_1",
                // ...
                "YOUR_REGISTRATION_TOKEN_n",
            };
            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            if (response.FailureCount > 0)
            {
                var failedTokens = new List<string>();
                for (var i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        // The order of responses corresponds to the order of the registration tokens.
                        failedTokens.Add(registrationTokens[i]);
                    }
                }

                Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
            }

            // [END send_multicast_error]
        }

        internal static Message CreateAndroidMessage()
        {
            // [START android_message]
            var message = new Message
            {
                Android = new AndroidConfig()
                {
                    TimeToLive = TimeSpan.FromHours(1),
                    Priority = Priority.Normal,
                    Notification = new AndroidNotification()
                    {
                        Title = "$GOOG up 1.43% on the day",
                        Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
                        Icon = "stock_ticker_update",
                        Color = "#f45342",
                    },
                },
                Topic = "industry-tech",
            };
            // [END android_message]
            return message;
        }

        internal static Message CreateAPNSMessage()
        {
            // [START apns_message]
            var message = new Message
            {
                Apns = new ApnsConfig()
                {
                    Headers = new Dictionary<string, string>()
                    {
                        { "apns-priority", "10" },
                    },
                    Aps = new Aps()
                    {
                        Alert = new ApsAlert()
                        {
                            Title = "$GOOG up 1.43% on the day",
                            Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% "
                                + "on the day.",
                        },
                        Badge = 42,
                    },
                },
                Topic = "industry-tech",
            };
            // [END apns_message]
            return message;
        }

        internal static Message CreateWebpushMessage()
        {
            // [START webpush_message]
            var message = new Message
            {
                Webpush = new WebpushConfig()
                {
                    Notification = new WebpushNotification()
                    {
                        Title = "$GOOG up 1.43% on the day",
                        Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
                        Icon = "https://my-server/icon.png",
                    },
                },
                Topic = "industry-tech",
            };
            // [END webpush_message]
            return message;
        }

        internal static Message CreateMultiPlatformsMessage()
        {
            // [START multi_platforms_message]
            var message = new Message
            {
                Notification = new Notification()
                {
                    Title = "$GOOG up 1.43% on the day",
                    Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
                },
                Android = new AndroidConfig()
                {
                    TimeToLive = TimeSpan.FromHours(1),
                    Notification = new AndroidNotification()
                    {
                        Icon = "stock_ticker_update",
                        Color = "#f45342",
                    },
                },
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        Badge = 42,
                    },
                },
                Topic = "industry-tech",
            };
            // [END multi_platforms_message]
            return message;
        }

    }
}
