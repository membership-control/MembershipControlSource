using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.Interface;
using System.IO;
using Google.Apis.Drive.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Util.Store;

namespace Core.Services.Service
{
   public  class ServerStatusMobile:IServerStatusMobile
    {
       public bool WriteToLogFile(string path, string content)
       {
           try
           {
               if (!System.IO.File.Exists(path))
               {
                   using (StreamWriter sw = System.IO.File.CreateText(path))
                   {
                       sw.WriteLine(content);
                   }
               }
               else
               {
                   using (StreamWriter sw = System.IO.File.AppendText(path))
                   {
                       sw.WriteLine(content);
                   }
               }
               return true;
           }
           catch (Exception ex)
           {
               return false;
           }
       }
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        public bool  uploadFile(string _uploadFile)
        {
            DriveService _service = googleauth();
            if (_service == null)
                return false;
            string _parent = "root", _fileId = "";
            if (System.IO.File.Exists(_uploadFile))
            {
                Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
                body.Title = System.IO.Path.GetFileName(_uploadFile);
                body.Description = "File updated by Control Tower";
                body.MimeType = GetMimeType(_uploadFile);
                body.Parents = new List<ParentReference> { new ParentReference() { Id = _parent } };

                FilesResource.ListRequest listrequest = _service.Files.List();
                FileList files = listrequest.Execute();

                while (files.Items != null)
                {
                    foreach (Google.Apis.Drive.v2.Data.File item in files.Items)
                    {
                        if (item.OriginalFilename == body.Title)
                            _fileId = item.Id;
                    }
                    if (files.NextPageToken == null)
                    {
                        break;
                    }
                    listrequest.PageToken = files.NextPageToken;
                    files = listrequest.Execute();
                }

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    if (_fileId != "")
                    {
                        FilesResource.UpdateMediaUpload request = _service.Files.Update(body, _fileId, stream, GetMimeType(_uploadFile));
                        request.Upload();
                        //return request.ResponseBody;
                        return true;
                    }
                    else
                    {
                        FilesResource.InsertMediaUpload request = _service.Files.Insert(body, stream, GetMimeType(_uploadFile));
                        request.Upload();
                        //return request.ResponseBody;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    string logfilename = "c:\\controltowermobileuat\\ControlTowerLog_" + DateTime.Now.Month.ToString() + ".txt";
                    WriteToLogFile(logfilename, e.Message+" upload");
                    Console.WriteLine("An error occurred: " + e.Message);
                    return false;
                }
            }
            else
            {
                string logfilename = "c:\\controltowermobileuat\\ControlTowerLog_" + DateTime.Now.Month.ToString() + ".txt";
                WriteToLogFile(logfilename, "File does not exist: " + _uploadFile);
                return false;
            }

        }
        private DriveService googleauth()
        {
             try
             {
                string[] scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
                var clientId = "409362331008-ro3sm034ci83dcn5amthv36bf751p3o8.apps.googleusercontent.com";
                var clientSecret = "MnEEod0Y8xUjNSCWEZm2NoA6";
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                    scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    null).Result;

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API",
                });
                return service;
             }
             catch (Exception e)
             {
                 string logfilename = "c:\\controltowermobileuat\\ControlTowerLog_" + DateTime.Now.Month.ToString() + ".txt";
                 WriteToLogFile(logfilename, e.Message + " auth");
                 WriteToLogFile(logfilename, e.InnerException.Message+ " auth");
                return null;
             }
        }
    }
}
