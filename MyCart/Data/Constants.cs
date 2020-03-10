using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Data
{
    public static class Constants

    {

        public static string AppName = "DigiFyy";

        public const string MIME_TYPE = "application/dk.infralogic.digifyy";


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

        public static string EndPoint_LoginUser = "LoginUser";
        public static string ApiKey_LoginUser = "GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";

        public static string EndPoint_GetInfo = "GetInfo";
        public static string ApiKey_GetInfo = "GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";

        public static string EndPoint_RegisterUID = "RegisterUID";
        public static string ApiKey_RegisterUID = "GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";

        public static string EndPoint_UpdateStatus = "UpdateStatus";
        public static string ApiKey_UpdateStatus ="GSD0L59J7A9raSY2uWoVg2GYzcpeCTzv38iAKZ18";

        public static string EndPoint_GetManufacturers = "GetManufacturers";
        public static string ApiKey_GetManufacturers = "xRlQBaMdpv6I9S34AMPuO8uxzmPANLIL7aCijOow";

        public static string EndPoint_GetMessages = "GetMessages";
        public static string ApiKey_GetMessages = "xRlQBaMdpv6I9S34AMPuO8uxzmPANLIL7aCijOow";

        public static string EndPoint_MarkReadMessages = "MarkReadMessages";
        public static string ApiKey_MarkReadMessages = "xRlQBaMdpv6I9S34AMPuO8uxzmPANLIL7aCijOow";


    }
}
