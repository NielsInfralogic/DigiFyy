using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Transfer;
using DigiFyy.DataService;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DigiFyy.Helpers
{
    public static class Utils
    {
        public static string GenerateTimeStamp()
        {
            DateTime t = DateTime.UtcNow;
            return string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
        }

       /* public async static Task<string> DownloadFileS3(string url)
        {
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                "eu-west-2:7c7c1872-0bb0-4802-9de1-355ca7fb121a", // Identity pool ID
                RegionEndpoint.EUWest2  // Region
            );

            var fileCopyName = Path.Combine(FileSystem.CacheDirectory, Path.GetFileName(url));

            try
            {
                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.EUWest2);
                var transferUtility = new TransferUtility(s3Client);
                await transferUtility.DownloadAsync(fileCopyName, Constants.S3BucketName, Path.GetFileName(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }

            return fileCopyName;
        }*/

        public async  static Task<string> UploadFileS3(string fileName)
        {            
           /*CognitoAWSCredentials credentials = new CognitoAWSCredentials(               
                "eu-west-2:7c7c1872-0bb0-4802-9de1-355ca7fb121a", // Identity pool ID
                RegionEndpoint.EUWest2  // Region
            ); */
         
            try
            {
                string k1 = Utils.DecodeBase64(Constants.SomeKey);
                string k2 = Utils.DecodeBase64(Constants.SomeOtherKey);
                // var s3Client = new AmazonS3Client(credentials, RegionEndpoint.EUWest2);
                 var s3Client = new AmazonS3Client(k1, k2, RegionEndpoint.EUWest2);
                 var transferUtility = new TransferUtility(s3Client);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    BucketName = Constants.S3BucketName,
                    Key = Path.GetFileName(fileName), 
                    FilePath = fileName,
                   CannedACL = S3CannedACL.PublicReadWrite, 
                    //StorageClass = S3StorageClass.Standard
                };
                await transferUtility.UploadAsync(uploadRequest);
//                await transferUtility.UploadAsync(fileName, Constants.S3BucketName);

                return Constants.S3BucketUrl + "/" + Path.GetFileName(fileName);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
        }

        public async static Task<string> UploadPhoto(string fileName)
        {
            string url = $"{Constants.ImageBaseUrl}/{Helpers.Utils.GenerateTimeStamp()}-{Path.GetFileName(fileName)}";

            try
            {
                byte[] upfilebytes = File.ReadAllBytes(fileName);
                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent imageContent = new ByteArrayContent(upfilebytes);
                content.Add(imageContent, "Image", Path.GetFileName(fileName));

                string ext = Path.GetExtension(fileName).ToLower().Replace(".", "");
             //   if (ext == "jpg" ||ext == "jpeg")
            //       imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            //    else if (ext == "png")
            //        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
                imageContent.Headers.Add("Content-Type", "application/octet-stream");
                //upload MultipartFormDataContent content async and store response in response var
                //var response = await client.PostAsync(url, content);

                try
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
                    {
                        Content = content
                    };
                    requestMessage.Headers.ExpectContinue = false;

                    HttpResponseMessage httpRequest = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                    var responseContent = await httpRequest.Content.ReadAsStringAsync();
              
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                //read response result as a string async into json var
              //  var responsestr = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                //debug
                Debug.WriteLine("Exception Caught: " + e.ToString());

                return "";
            }

            return url;

        }

        public static string DecodeBase64(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(data); 
        }
    }
}
