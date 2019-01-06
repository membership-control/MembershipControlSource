using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using Core.Data.EF;
using Core.Data.Model;
using Core.Services.Service;
using Firebase.Database;
using Newtonsoft;
using Firebase;
using Firebase.Database.Query;
using Firebase.Auth;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WebMembership.Hubs
{
    [CustomAuthorize]
    [Microsoft.AspNet.SignalR.Hubs.HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private static int k = 0;

        [Microsoft.AspNet.SignalR.Hubs.HubMethodName("Send")]
        public void Send(string message)
        {
            var user = Context.User;
            string name = user.Identity.Name;
            ChartModel chartmodel = new ChartModel();
            chartmodel.datetime = DateTime.Now;
            chartmodel.message = message;

           // System.IO.File.WriteAllText(@"C:\track\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_shipmentsearch.txt", message);
 //           SendNotification("AIzaSyBp0pvJpj8tU9YuOjiZTJQVfRWGOsHNwXo", "327184979278-br7c387t0ok0n4een490ob0nhouaf4jr.apps.googleusercontent.com", "e9X2g8kokFk:APA91bHnzx2dxxmXpz0ZpvoR5Io-dp6HU-veFmKruM9RxCgZhcpYps3DXazUje-EbxzEHay3brQWKE7O2vsWf0-Ael-5b3u6H-mlnfcPSdW9KKC7QMXHT5PTB527NOEAI5HCr4eY_HVb", "YOU ARE DEAD");


            Clients.All.addNewMessageToPage(name, chartmodel);            
        }
        public class AndroidFCMPushNotificationStatus
        {
            public bool Successful
            {
                get;
                set;
            }

            public string Response
            {
                get;
                set;
            }
            public Exception Error
            {
                get;
                set;
            }
        }
        public void insertFirebaseDBEXE(string message)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = @"C:\UploadToFirebase\UploadToFirebase.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "ServerStatusLog \"" + message + "\"";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
        }

        public async Task insertFirebaseDB(string message)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCg5EgrWJtB7bI_JqT6a5pYHsddhHOJ5HA"));
           
            var auth = await authProvider.SignInWithEmailAndPasswordAsync("controltowertoll@gmail.com", "C0ntr0lT0w3r");

            var firebase = new FirebaseClient("https://controltower-1369.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                });

            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
            SSLog a = new SSLog();
            a.LogDetail = message;
            a.Timestamp = timestamp;
            await firebase.Child("ServerStatus").Child(timestamp).PutAsync(a);
        }
        public AndroidFCMPushNotificationStatus SendNotification(string serverApiKey, string senderId, string deviceId, string message)
        {
            AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();

            try
            {
                result.Successful = false;
                result.Error = null;

                var value = message;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                   //to = deviceId,
                    to = "/topics/test",
                    notification = new
                    {
                        
                        body = message ,
                        title = "Control Tower Alert",
                        sound = "default",
                        icon = "ic_launcher"
                    },
                    priority = "high",
                    content_available = true
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverApiKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                                System.IO.File.WriteAllText(@"C:\track\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_shipmentsearch.txt", result.Error + "  " +"TRUE");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"C:\track\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_shipmentsearch.txt", ex + "  " + "FALSE");
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }

            return result;
        }
        [Microsoft.AspNet.SignalR.Hubs.HubMethodName("serverstatus")]
        public void SendServerStatus(object message)
        {
           //System.IO.File.WriteAllText(@"C:\track\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_shipmentsearch.txt", message.ToString());

            string errorMsg = message.ToString().Substring(message.ToString().IndexOf("AlertMessgae") + 16);
            errorMsg = errorMsg.Substring(0, errorMsg.IndexOf('"'));
            if (errorMsg != null && errorMsg != "")
            {
                try
                {
                    //string logfilename = "c:\\controltowermobileuat\\ControlTowerLog_" + DateTime.Now.Month.ToString() + ".txt";

                    //ServerStatusMobile ssm = new ServerStatusMobile();
                    //ssm.WriteToLogFile(logfilename, (errorMsg.Replace(@" %/n","")).Replace("  "," "));
                    //ssm.uploadFile(logfilename);
                    //Connect();
                    Guid guid = Guid.NewGuid(); 
                    using (TGF_IntegrationEntities ef = new TGF_IntegrationEntities())
                    {
                        Core.Data.EF.TGF_GI_ControlTower_Alert_Log r = new Core.Data.EF.TGF_GI_ControlTower_Alert_Log();
                        r.Alert_Detail = errorMsg;
                        r.Server_Name = errorMsg.Substring(0, errorMsg.IndexOf('?'));
                        r.insert_user = "HUBS";
                        r.insert_date = DateTime.Now;
                        r.TGF_GI_ControlTower_Alert_Log_PK = guid;
                        ef.TGF_GI_ControlTower_Alert_Log.Add(r);
                        ef.SaveChanges();
                    }
//                    AndroidFCMPushNotificationStatus a = SendNotification("AIzaSyBp0pvJpj8tU9YuOjiZTJQVfRWGOsHNwXo", "327184979278-br7c387t0ok0n4een490ob0nhouaf4jr.apps.googleusercontent.com", "e9X2g8kokFk:APA91bHnzx2dxxmXpz0ZpvoR5Io-dp6HU-veFmKruM9RxCgZhcpYps3DXazUje-EbxzEHay3brQWKE7O2vsWf0-Ael-5b3u6H-mlnfcPSdW9KKC7QMXHT5PTB527NOEAI5HCr4eY_HVb", errorMsg);
//                    AndroidFCMPushNotificationStatus a = SendNotification("AIzaSyCg5EgrWJtB7bI_JqT6a5pYHsddhHOJ5HA", "327184979278", "e9X2g8kokFk:APA91bHnzx2dxxmXpz0ZpvoR5Io-dp6HU-veFmKruM9RxCgZhcpYps3DXazUje-EbxzEHay3brQWKE7O2vsWf0-Ael-5b3u6H-mlnfcPSdW9KKC7QMXHT5PTB527NOEAI5HCr4eY_HVb", errorMsg);
 //                   System.IO.File.WriteAllText(@"C:\track\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_ERROR.txt", errorMsg);
                  //  insertFirebaseDB(errorMsg).Wait();
                    insertFirebaseDBEXE(guid.ToString() + "?"+errorMsg);
                }
                catch(Exception ex)
                {

                }
            }

            k = k + 1;
            Clients.Others.addServerStatus(message);
            //Clients.Group("chart").addNewMessageToPage(name, message);
        }

        private bool Connect()
        {
            bool Flag = false;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
               // proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "C:\\GOOGEDRIVETEST\\GoogleDriveTesting.exe >> c:\\templog.txt";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }

                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (String.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return true;
        }

        public override  System.Threading.Tasks.Task OnConnected()
        {
            //Groups.Add(Context.ConnectionId, "chart");
            System.Diagnostics.Debug.WriteLine("OnConnected");
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            //Groups.Remove(Context.ConnectionId, "chart");
            System.Diagnostics.Debug.WriteLine("OnDisconnected");
            return base.OnDisconnected(stopCalled);
        }
        public override System.Threading.Tasks.Task OnReconnected()
        {
            System.Diagnostics.Debug.WriteLine("OnReconnected");
            return base.OnReconnected();
        }
    }

}