using Microsoft.Extensions.Configuration;

namespace Repository.Services.Infrastructure
{
    public class AppConfiguration
    {
        /// <summary>
        ///  Globle Varible Declaration   
        /// </summary>
        //public readonly static string _conn_cricket_Master = string.Empty;
        //public readonly static string _conn_cricket_Normal = string.Empty;
        public readonly static string Host = string.Empty;
        public readonly static string UserName = string.Empty;
        public readonly static string Password = string.Empty;
        public readonly static string Port = string.Empty;
        public readonly static string FromEmail = string.Empty;
        public readonly static string WebAppPath = string.Empty;
        public readonly static string IDProtectAdminEmail = string.Empty;
        public readonly static string APIWebsitePath = string.Empty;
        public readonly static string APIHost = string.Empty;
        public readonly static string AdministratorEmail = string.Empty;
        public readonly static string AzureStorageBaseURL = string.Empty;
        public readonly static string AzureStorageConnectionstring = string.Empty;

        public readonly static string azureOpenAIKey = string.Empty;
        public readonly static string azureOpenAIEndpoint = string.Empty;
        public readonly static string ComputerVisionKey = string.Empty;
        public readonly static string ComputerVisionEndpoint = string.Empty;

        static AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            //configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            Host = root.GetSection("AppSettings").GetSection("MailSettings:Host").Value;
            UserName = root.GetSection("AppSettings").GetSection("MailSettings:UserName").Value;
            Password = root.GetSection("AppSettings").GetSection("MailSettings:Password").Value;
            Port = root.GetSection("AppSettings").GetSection("MailSettings:Port").Value;
            FromEmail = root.GetSection("AppSettings").GetSection("MailSettings:FromEmail").Value;
            IDProtectAdminEmail = root.GetSection("AppSettings").GetSection("IDProtectAdminEmail").Value;
            WebAppPath = root.GetSection("WebAppPath").Value;

            APIWebsitePath = root.GetSection("AppSettings").GetSection("APIWebsitePath").Value;
            APIHost = root.GetSection("AppSettings").GetSection("APIHost").Value;
            AdministratorEmail = root.GetSection("AppSettings").GetSection("AdminEmail").Value;
            AzureStorageBaseURL = root.GetSection("AppSettings").GetSection("BlobConnectionStrings:BaseUrl").Value;
            AzureStorageConnectionstring = root.GetSection("AppSettings").GetSection("BlobConnectionStrings:AzureBlobStorage").Value;

            azureOpenAIKey = root.GetSection("AppSettings").GetSection("AzureAISearch:azureOpenAIKey").Value;
            azureOpenAIEndpoint = root.GetSection("AppSettings").GetSection("AzureAISearch:azureOpenAIEndpoint").Value;
            ComputerVisionKey = root.GetSection("AppSettings").GetSection("AzureAISearch:ComputerVisionKey").Value;
            ComputerVisionEndpoint = root.GetSection("AppSettings").GetSection("AzureAISearch:ComputerVisionEndpoint").Value;
        }

    }
}
