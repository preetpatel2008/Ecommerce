using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Net.Mail;

namespace Repository.Services.Infrastructure
{
    public class UtilityFunctions
    {
        #region Globle variable Declaration
        private static int Port;
        private static string Host = string.Empty;
        private static string UserName = string.Empty;
        private static string Password = string.Empty;
        private static string FromEmail = string.Empty;

        private static string azureOpenAIKey = string.Empty;
        private static string azureOpenAIEndpoint = string.Empty;
        private static string ComputerVisionKey = string.Empty;
        private static string ComputerVisionEndpoint = string.Empty;
        #endregion

        static UtilityFunctions()
        {
            Port = Convert.ToInt32(AppConfiguration.Port);
            Host = AppConfiguration.Host;
            UserName = AppConfiguration.UserName;
            Password = AppConfiguration.Password;
            FromEmail = AppConfiguration.FromEmail;

            azureOpenAIKey = AppConfiguration.azureOpenAIKey;
            azureOpenAIEndpoint = AppConfiguration.azureOpenAIEndpoint;
            ComputerVisionKey = AppConfiguration.ComputerVisionKey;
            ComputerVisionEndpoint = AppConfiguration.ComputerVisionEndpoint;
                
        }

        private static byte[] _buffer = new byte[10];
        public static T BDNullFromDB<T>(object value)
        {
            return (value == DBNull.Value || value == null) ? default(T) : (T)value;
        }

        public static object DBNullToDB<T>(T value)
        {
            return value == null ? (object)DBNull.Value : value;
        }

        public static string DBNullToDB_Empty(string value)
        {
            return value == null ? string.Empty : value;
        }

        #region Method to send email
        public static bool SendEmail(string ToEmailIDs, string strFromEmail, string CCEmailIDs, string BCCEmailIDs, string strSubject, string strBody, List<string> strAttachmentFileName = null)
        {
            try
            {
                //creating the object of MailMessage
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromEmail); //From Email Id
                mailMessage.Subject = strSubject; //Subject of Email
                mailMessage.Body = strBody; //body or message of Email
                mailMessage.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(ToEmailIDs))
                {
                    string[] ToEmailMulitipleId = ToEmailIDs.Split(',');
                    foreach (string ToEMailId in ToEmailMulitipleId)
                    {
                        mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                    }
                }

                if (!string.IsNullOrEmpty(CCEmailIDs))
                {
                    string[] ccEmailIDs = CCEmailIDs.Split(',');
                    foreach (string CCEmail in ccEmailIDs)
                    {
                        mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                    }
                }

                if (!string.IsNullOrEmpty(BCCEmailIDs))
                {
                    string[] bccids = BCCEmailIDs.Split(',');
                    foreach (string bccEmailId in bccids)
                    {
                        mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id
                    }
                }

                // Added for Attachment starts
                if (strAttachmentFileName != null)
                {
                    foreach (string fileName in strAttachmentFileName)
                    {
                        if (File.Exists(fileName))
                        {
                            mailMessage.Attachments.Add(new Attachment(fileName));
                        }
                    }
                }


                SmtpClient smtp = new SmtpClient();  // creating object of smptpclient
                smtp.Host = Host;              //host of emailaddress for example smtp.gmail.com etc

                //network and security related credentials
                // client.EnableSsl = true;
                if (smtp.Host.Contains("gmail"))
                {
                    smtp.EnableSsl = true;
                }

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                smtp.Port = Convert.ToInt32(Port);
                smtp.Send(mailMessage); //sending Email
                smtp.Dispose();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        public static string GetRandomPassword(int length, bool DigitsOnly = false)
        {
            char[] chars = "abcdefghjkmnopqrstuvwxyz1234567890ABCDEFGHJKMNOPQRSTUVWXYZ".ToCharArray(); //29.06.2012: Removed I & L both capital and small as per client request
            if (DigitsOnly)
                chars = "1234567890".ToCharArray();
            string password = string.Empty;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);
                //Don't Allow Repetation of Characters
                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }
            return password;
        }

        public static Object SafeDbObject<T>(T input)
        {
            if (input is DateTime && DateTime.MinValue.Equals(input))
            {
                return DBNull.Value;
            }
            if (input == null)
            {
                return DBNull.Value;
            }
            else
            {
                return input;
            }
        }
        //    public async Task<string> GetImageDescriptionByAzureOpenAI(List<IFormFile> imageFiles, string searchText)
        //    {
        //        try
        //        {
        //            List<dynamic> lstImagedescription = new List<dynamic>();
        //            foreach (var imageFile in imageFiles)
        //            {
        //                using (var stream = imageFile.OpenReadStream())
        //                {
        //                    var ComputerVisionclient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(ComputerVisionKey))
        //                    {
        //                        Endpoint = ComputerVisionEndpoint
        //                    };
        //                    // Define the visual features to extract
        //                    var visualFeatures = new List<VisualFeatureTypes?>
        //                        {
        //                            VisualFeatureTypes.Description,
        //                            VisualFeatureTypes.Tags,
        //                            VisualFeatureTypes.Categories,
        //                            VisualFeatureTypes.Adult,
        //                            VisualFeatureTypes.Objects,
        //                            VisualFeatureTypes.Brands,
        //                            VisualFeatureTypes.ImageType,
        //                            VisualFeatureTypes.Color
        //                        };

        //                    // Analyze the image
        //                    var result_ = await ComputerVisionclient.AnalyzeImageInStreamAsync(stream, visualFeatures);

        //                    foreach (var caption in result_.Description.Captions)
        //                    {
        //                        lstImagedescription.Add(new { caption.Text });
        //                    }
        //                    foreach (var tag in result_.Tags)
        //                    {
        //                        lstImagedescription.Add(new { tag.Name });
        //                    }
        //                    foreach (var Object in result_.Objects)
        //                    {
        //                        lstImagedescription.Add(new { Object });
        //                    }
        //                }
        //            }


        //            //var prompt = $"You are a medical practitioner and an expert in analyzing medical-related images working for a very reputed hospital. " +
        //            //        "You will be provided with images and you need to identify anomalies, any diseases, or health issues. " +
        //            //        "Please also take into account the following search term when analyzing the images: " + searchText +
        //            //        "You only need to respond if the image is related to a human body and health issues. " +
        //            //        "Here is a description of the image you are analyzing: " + lstImagedescription +
        //            //        "You must respond, but also write a disclaimer saying 'Consult with a doctor before making any decisions.' Remember, " +
        //            //        "if certain aspects are not clear from the image, it's okay to state 'Unable to determine based on the provided image.' " +
        //            //        "Please generate the detailed result under the paragraph Symptoms, Diagnoses, Next steps, Recommendations.";

        //            var prompt = "explain the image the components are image is " + string.Join(", ", lstImagedescription) + "using " + searchText +
        //                        "identify anomalies, any diseases, or health issues. Describe the image and Please generate the detailed result under the paragraph Symptoms, Diagnoses, Diagnoses codes, Next steps, Recommendations.";

        //            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
        //            {
        //                Messages =
        //                {
        //                   new ChatMessage(ChatRole.System, "You are a helpful assistant."),
        //                   new ChatMessage(ChatRole.User, prompt)
        //                },
        //                Temperature = (float)0.7,
        //                MaxTokens = 800,
        //                NucleusSamplingFactor = (float)0.95,
        //                FrequencyPenalty = 0,
        //                PresencePenalty = 0
        //            };
        //            OpenAIClient client = new OpenAIClient(new Uri(azureOpenAIEndpoint), new AzureKeyCredential(azureOpenAIKey));
        //            var result = await client.GetChatCompletionsAsync("DermatixAzureOpenAIModel", chatCompletionsOptions);

        //            return result.Value.Choices[0].Message.Content.ToString();

        //        }
        //        catch (Exception ex)
        //        {
        //            //_logger.LogError("Error:GetImageDescriptionByAI ~ " + ex.Message);
        //            return $"Error processing images: {ex.Message}";
        //        }
        //    }


    }
}
