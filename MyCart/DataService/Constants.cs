using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.DataService
{
    public static class Constants
    {
        public const string  EmailhtmlBodyResetPassword = @"<html>
<head></head>
<body>
  <h1>Digifyy Account Password Reset</h1>
  <p>This email was sent to you for resetting your password.
    </p>
  <p>Click the following link to reset your password:
    </p>
    <p><a href='https://aws.amazon.com/ses/#1#'>Reset password</a></p>
</body>
</html>";

        public const string EmailhtmlBodyVerifyYourAccount = @"<html>
<head></head>
<body>
  <h1>Digifyy Account Verification</h1>
  <p>This email was sent to you for verifying your account email addrss.
    </p>
<p>Click the following link to verify your account:
    </p>
    <p><a href='https://aws.amazon.com/ses/#1#'>Verify email</a></p>
</body>
</html>";

        public const string EmailhtmlBodyActivationCode = @"<html>
<head></head>
<body>
  <h1>Digifyy Bike owner Activation code</h1>
  <p>This email was sent to you for registering the bike ownership to you</p>
<p>The registration token is: #1#
    </p>
</body>
</html>";

        public const string SmtpUsername = "AKIAVDQTIQUA5BCTA7YB";
        public const string SmtpPassword = "BEKMmT7ECLgld436Gcfh54LRxD5u9VzsWuCqnZLK1oX3";
        public const string SmtpHost = "email-smtp.eu-central-1.amazonaws.com";
        public const int    SmtpPort = 587;

        public const string AWSAccessKey = "AKIAJERXTLCW4EJJYQVQ";
        public const string AWSSecretKey = "3GY5TvFft10anckOGdau67jTqIbzpIQNnh3kHKLC";

        public const string EmailSenderAddress = "support@infralogic.dk";


        public const string FakeUUID = "1234567654323456765432-aasdfgfdsa";

        public static string AppName = "DigiFyy";

        public const string MIME_TYPE = "application/dk.infralogic.digifyy";

        public const string DefaultProfileImage = "digifyy_bikelogo.png";


        public static string ImageBaseUrl = "http://www.infralogic.net/digifyy";

        public static string S3BucketName = "digifyybucket2";
        public static string S3BucketUrl = "https://digifyybucket2.s3.eu-west-2.amazonaws.com";

        
        // Google OAuth

        // For Google login, configure at https://console.developers.google.com/

        public static string GoogleiOSClientId = "134299025694-k5o74o48mv1bvsiorecgmqqckcm9noh0.apps.googleusercontent.com";

        public static string GoogleAndroidClientId = "134299025694-k5o74o48mv1bvsiorecgmqqckcm9noh0.apps.googleusercontent.com";



        // These values do not need changing

        public static string GoogleScope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

        public static string GoogleAuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";

        public static string GoogleAccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";

        public static string GoogleUserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";



        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended

        public static string GoogleiOSRedirectUrl = "<insert IOS redirect URL here>:/oauth2redirect";

        public static string GoogleAndroidRedirectUrl = "com.googleusercontent.apps.134299025694-k5o74o48mv1bvsiorecgmqqckcm9noh0:/oauth2redirect";



        //-------------------------------------------------------------------------------------------------------



        // Facebook OAuth

        // For Facebook login, configure at https://developers.facebook.com

        public static string FacebookiOSClientId = "542042489997006";

        public static string FacebookAndroidClientId = "542042489997006";



        // These values do not need changing

        public static string FacebookScope = "email";

        public static string FacebookAuthorizeUrl = "https://www.facebook.com/dialog/oauth/";

        public static string FacebookAccessTokenUrl = "https://www.facebook.com/connect/login_success.html";

        public static string FacebookUserInfoUrl = "https://graph.facebook.com/me?fields=email&access_token={accessToken}";



        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended

        public static string FacebookiOSRedirectUrl = "<insert IOS redirect URL here>:/oauth2redirect";

        public static string FacebookAndroidRedirectUrl = "https://www.facebook.com/connect/login_success.html";

        //-------------------------------------------------------------------------------------------------------
        public static string RestUrl = "https://f1br1nlc92.execute-api.eu-north-1.amazonaws.com/Test/";
        public static string AzureRestUrl = "https://dbfnazurefunctions.azurewebsites.net/api/";

        public static string EndPoint_LoginUser = "LoginUser";
        public static string ApiKey_LoginUser = "GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";

        public static string AzureEndPoint_LoginUser = "AutheticateUser";
        public static string AzureApiKey_LoginUser = "CMooPpAtRawAL4htaao/noUUEitmZyssH43FsQlt/6iI2q3MIs2dIQ==";


        public static string EndPoint_GetInfo = "GetInfo";
        public static string ApiKey_GetInfo = "GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";


        public static string AzureEndPoint_GetInfo = "GetInfo";
        public static string AzureApiKey_GetInfo = "ea3dyn2ckJlZqiQXlnwJi4UySJtKzd0KVh1aS8I8EsZc6h7ygCXBfQ==";


        public static string EndPoint_RegisterUID = "RegisterUID";
        public static string ApiKey_RegisterUID = "GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";


        public static string EndPoint_UpdateStatus = "UpdateStatus";
        public static string ApiKey_UpdateStatus ="GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";

        public static string AzureEndPoint_UpdateStatus = "UpdateStatus";
        public static string AzureApiKey_UpdateStatus = "awhLQFfjWBirS7ENVulB3BseoE3qOk6zskMNYFs1utBidgfVaAvO5Q==";
       

        public static string EndPoint_GetManufacturers = "GetManufacturers";
        public static string ApiKey_GetManufacturers = "xRlQBaMdpv6I9S34AMPuO8uxzmPANLIL7aCijOow";

        public static string AzureEndPoint_GetManufacturers = "GetManufacturers";
        public static string AzureApiKey_GetManufacturers = "kM/mlW32IrXq43Vui0x6SQR68uSnZQM2UKmvrazjSCqJyox4qBzAOg==";


        public static string EndPoint_GetMessages = "GetMessages";
        public static string ApiKey_GetMessages = "xRlQBaMdpv6I9S34AMPuO8uxzmPANLIL7aCijOow";

        public static string AzureEndPoint_GetMessages = "GetMessages";
        public static string AzureApiKey_GetMessages = "Gem0h4LlTPLJAhimya7fI3AmepDYS4XZHE8SYylg2NmegfESULdVSQ==";


        public static string EndPoint_MarkReadMessages = "MarkReadMessages";
        public static string ApiKey_MarkReadMessages = "xRlQBaMdpv6I9S34AMPuO8uxzmPANLIL7aCijOow";

        public static string AzureEndPoint_MarkReadMessages = "MarkReadMessages";
        public static string AzureApiKey_MarkReadMessages = "iQoLagu2MWYc9ar/wWKraoTE2cXRJyv48G1HjaagQ/aFktbcxA211w==";


        public static string EndPoint_RegisterImage = "RegisterImage";
        public static string ApiKey_RegisterImage = "5siHkngr0970OjlVKAu1afO9f8bcOLf23th1F9t5";

        public static string AzureEndPoint_RegisterImage = "RegisterImage";
        public static string AzureApiKey_RegisterImage = "g49bRG3QuSrpAmARxOqBK9M9x2Y3xUG2h3Ud3hbcHyGM9Ha6QfkqcA==";


        public static string EndPoint_RegisterExtra = "RegisterExtra";
        public static string ApiKey_RegisterExtra = "E6kfdRhddO9zw4JJIMfRR8EVtJr7eJtG6rYpziui";

        public static string AzureEndPoint_RegisterExtra = "RegisterExtra";
        public static string AzureApiKey_RegisterExtra = "ud3DWbqJoOTzhJrTm2Nc1F2q/s1ACSE/823UizAOWNRE27KBzdip2w==";


        public static string EndPoint_GetSpecs = "GetSpecs";
        public static string ApiKey_GetSpecs = "gxOFJ9N0gy8c6Cx6gQ2wD2AFZNaQvDDU9odPypeT";

        public static string AzureEndPoint_GetSpecs = "GetSpecs";
        public static string AzureApiKey_GetSpecs = "viEgq1Ceroeot03KO4MoGyUuxaS6HLlDqvM5hgV8s/0a77HCq7rJ9w==";

        public static string EndPoint_RegisterDocument = "RegisterDocument";
        public static string ApiKey_RegisterDocument = "S7ZmvkJbb25yO4HaXwqlf8UiUpLbjv4QaO5qMDHk";


        public static string EndPoint_GetStatus = "GetStatus";
        public static string ApiKey_GetStatus = "979w9nQ8C63PRY1oJexgK36C7XRC8dib8PXLy42m";
        


    }
}
