using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Contract;
using Repository.Services.Infrastructure;
using Entity;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using static Repository.Services.Infrastructure.Enums;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repository.Services.Context;
using Microsoft.AspNetCore.Components;


namespace ShoppingCart.Service


{
    public class AuthenticationAppService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly ISecurityRepositoryService _securityRepositoryService;
        private readonly ILogger<AuthenticationAppService> _logger;
        private readonly string BaseURL = AppConfiguration.WebAppPath;

        private readonly NavigationManager _navigationManager;

        //private readonly AppDbContext _context; // Example: Injecting DbContext
        public AuthenticationAppService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration config, ISecurityRepositoryService securityRepositoryService, ILogger<AuthenticationAppService> logger, NavigationManager navigationManager)
        {
            _config = config;
            _securityRepositoryService = securityRepositoryService;
            this._logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _env = webHostEnvironment;

            _navigationManager = navigationManager;
        }


        //public AuthenticationAppService()  // Ensure proper DI dependencies
        //{
        //    _context = context;
        //}

        public static class Constants
        {
            public const string AuthScheme = "Cookies"; // Or use "Bearer" for JWT
        }

        [IgnoreAntiforgeryToken] 
        public async Task<string> Login(AuthModel login)
        {
            try
            {
                if (login != null && !string.IsNullOrWhiteSpace(login.Email) && !string.IsNullOrWhiteSpace(login.Password))
                {

                    long retLoginMasterID = _securityRepositoryService.CheckIsAuthenticate(login.Email.Trim());

                    if (retLoginMasterID > 0)
                    {
                        LoginModel loginUser = await _securityRepositoryService.GetLoginMasterByID(retLoginMasterID);

                        if (loginUser != null)
                        {
                            if (login.Password.Trim() == Repository.Services.Infrastructure.QueryStringEncryptDecrypt.QueryStringDecrypt(loginUser.Password).Trim())
                            {
                                //_securityRepositoryService.InsertLoginDetails(retLoginMasterID, true);
                                UserAuthentication objAuthentication = new UserAuthentication();
                                objAuthentication.LoginMasterID = loginUser.LoginMasterID;
                                objAuthentication.FirstName = loginUser.FirstName;
                                objAuthentication.LastName = loginUser.LastName;
                                objAuthentication.EntityTypeID = loginUser.EntityTypeID;
                                objAuthentication.Email = loginUser.Email;
                                objAuthentication.Area = "Admin";
                                objAuthentication.DisplayName = loginUser.FirstName + " " + loginUser.LastName;
                                var claims = new List<Claim>
                                {
                                    new Claim(type: "DisplayName", value: UtilityFunctions.DBNullToDB(objAuthentication.DisplayName).ToString()),
                                    new Claim(type: "LoginMasterID", value: objAuthentication.LoginMasterID.ToString()),
                                    new Claim(type: "FirstName", value: UtilityFunctions.DBNullToDB(objAuthentication.FirstName).ToString()),
                                   // new Claim(type: "LastName", value: UtilityFunctions.DBNullToDB(objAuthentication.LastName).ToString()),
                                    new Claim(type: "EntityTypeID", value: objAuthentication.EntityTypeID.ToString()),
                                    //new Claim(type: "EntityID", value: Convert.ToString(UtilityFunctions.DBNullToDB(objAuthentication.EntityID))),
                                    new Claim(type: "EntityID", value: UtilityFunctions.DBNullToDB(objAuthentication.EntityID).ToString()),
                                    new Claim(type: "Email", value: objAuthentication.Email.ToString())
                                };

                                string userRole = objAuthentication.EntityTypeID switch
                                {
                                    1 => EntityType.Admin.ToString(),
                                    3 => EntityType.User.ToString(),
                                    //4 => EntityType.Student.ToString(),

                                };      

                                claims.Add(new Claim(ClaimTypes.Role, userRole));
                                /* Add ploicies 
                                 get policy data from table for particuler user and set this policy in claims using for/foreach loop
                                 */

                                //claims.Add(new Claim(UserPolicy.UserPolicy.VIEW_TASK, "true"));

                                var identity = new ClaimsIdentity(claims, Constants.AuthScheme);
                                var principal = new ClaimsPrincipal(identity);


                                //var claims = new List<Claim>
                                //{
                                //   new Claim(ClaimTypes.Name, "John Doe"),
                                //   new Claim(ClaimTypes.Role, "Admin") // Assign roles dynamically
                                //};



                                //var authProperties = new AuthenticationProperties
                                //{
                                //    IsPersistent = login.RememberMe
                                //};
                                //var session = _httpContextAccessor.HttpContext.Session;
                                //session.SetString("CultureID", loginUser.CultureID.ToString());

                                    await _httpContextAccessor.HttpContext.SignInAsync(Constants.AuthScheme, principal);
                                //string cultureType = ((CultureType)loginUser.CultureID).ToString() ?? "en";
                                //var requestCulture = new RequestCulture(cultureType);
                                //var cookieName = CookieRequestCultureProvider.DefaultCookieName;
                                //var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

                                //_httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue);

                                return string.Empty;
                            }
                            else
                            {
                                //_securityRepositoryService.InsertLoginDetails(retLoginMasterID, false);
                                return "Email or Password do not match";
                                //TempData["ErrorMessage"] = "Email or Password do not match"; //Resources.Message.InvalidLogin;
                            }
                        }
                        else
                        {
                            // _securityRepositoryService.InsertLoginDetails(retLoginMasterID, false);
                            return "Email does not exist";
                        }

                    }
                    else if (retLoginMasterID == 0 || retLoginMasterID == -2)
                    {
                        return "Email or Password do not match";//Resources.Message.InvalidLogin;
                    }
                    else if (retLoginMasterID == -1)
                    {
                        return "Your account is Inactive, please contact administrator";// Resources.Message.InactiveAccountMessage;
                    }
                    else
                    {
                        return "Email or Password do not match";
                    }
                }
                else
                {
                    return "Email & Password Required";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:Login ~ " + ex.Message);
                return ex.Message;
            }

            //return View(login);
        }
        public async Task<string> ForgotPassword(string Email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, RegularExpressions.regexEmailValid, RegexOptions.IgnoreCase))
                {
                    return "Invalid Email.";
                }

                int IsEmailExists = _securityRepositoryService.CheckIsEmailExistForgotPassword(Email);
                if (IsEmailExists != 0)
                {
                    long loginMasterID = _securityRepositoryService.UpdateVerificationCode(Email, null);
                    if (loginMasterID > 0)
                    {
                        //string OTP = UtilityFunctions.GetRandomPassword(4, true);
                        //_securityRepositoryService.UpdateVerificationCode(Email, OTP);
                        bool isEmailSend = await SendEmailForForgotPassword(Email, loginMasterID);
                        return "Please check your email for reset password";
                        //return Json(new { success = true, message = "Please check your email for the OTP", loginMasterID = loginMasterID });
                    }
                }
                else
                {
                    return "Email not exists";
                    //return Json(new { success = false, message = "Email not registered or inactivated!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:ForgotPassword ~ " + ex.Message);
                return "Error occured while sending email" + ex.Message;
                //return Json(new { success = false, message = "An error occurred. Please try again later." });
            }
            return "Please check your email for reset password";
        }
        private async Task<bool> SendEmailForForgotPassword(string toEmail, long loginMasterID)
        {
            try
            {
                LoginModel objLogin = await _securityRepositoryService.GetLoginMasterByID(loginMasterID);
                string body = "";
                //string contentRootPath = _host.ContentRootPath;
                //string webRootPath = _host.WebRootPath;
                string contentRootPath = _env.ContentRootPath;
                string webRootPath = _env.WebRootPath;
                using (var sr = new System.IO.StreamReader(contentRootPath + "\\wwwroot\\Content\\EmailTemplate\\AccountActivation.html"))
                {
                    body = sr.ReadToEnd();
                    body = body.Replace("{LogoImagePath}", Path.Combine(Directory.GetCurrentDirectory(), ".\\img\\logo.png"));
                    body = body.Replace("{Salutactation}", "Hello");
                    body = body.Replace("{Name}", objLogin.FirstName);
                    var baseUrl = AppConfiguration.WebAppPath;
                    string LoginMasterID = QueryStringEncryptDecrypt.QueryStringEncrypt(objLogin.LoginMasterID.ToString());
                    string Email = QueryStringEncryptDecrypt.QueryStringEncrypt(toEmail);
                    var confirmEmailUrl = $"{baseUrl}/auth/resetpassword?LoginMasterID={LoginMasterID}&Email={Email}";
                    body = body.Replace("{confirmEmailUrl}", confirmEmailUrl);
                    body = body.Replace("{Regards}", "Warm Regards");
                    body = body.Replace("{FromName}", "Shaqra University");
                }
                string toEmailIDs = toEmail;
                bool isEmailSend = UtilityFunctions.SendEmail(toEmailIDs, "", "", "", "Reset Password", body, null);
                return isEmailSend;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:SendEmailForForgotPassword ~ " + ex.Message);
                return false;
            }
        }
        public async Task<int> ResetPassword(string loginMasterID, string newPassword)
        {
            try
            {
                bool isPasswordchange = _securityRepositoryService.ResetPassword(Convert.ToInt64(loginMasterID), QueryStringEncryptDecrypt.QueryStringEncrypt(newPassword));

                if (isPasswordchange)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:ResetPassword ~ " + ex.Message);
                throw;
            }
        }
        //public APIResponse UpdatedCulture(long LoginMasterID, int CultureID)
        //{
        //    try
        //    {
        //        if (LoginMasterID != null && CultureID != null)
        //        {
        //            _securityRepositoryService.UpdateCulture(LoginMasterID, CultureID);
        //            return APIResponse.GetAPIResponse(false, HttpStatusCode.OK, null, new { LoginMasterID = LoginMasterID });
        //        }
        //        else
        //            return APIResponse.GetAPIResponse(true, HttpStatusCode.BadRequest, null, null);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error:UpdatedCulture ~ " + ex.Message);
        //        return APIResponse.GetAPIResponse(true, HttpStatusCode.BadRequest, ex.Message, null);
        //    }
        //}
        public async Task<long> AddUpdateLoginMaster(string Email, long DepartmentID)
        {
            long LoginMasterID = 0;
            bool IsMailSend = false;
            try
            {
                string RandomPassword = UtilityFunctions.GetRandomPassword(8);
                LoginModel objLoginModel = new LoginModel();
                objLoginModel.Email = Email;
               // objLoginModel.DepartmentID = DepartmentID;
                objLoginModel.Password = QueryStringEncryptDecrypt.QueryStringEncrypt(RandomPassword);
                objLoginModel.EntityTypeID = 3;
                //objLoginModel.CultureID = 1;
               // LoginMasterID = _securityRepositoryService.AddUpdateLoginMaster(objLoginModel); 
                if (LoginMasterID == -1)
                {
                    return LoginMasterID;
                }
                else
                {
                    IsMailSend = await SendEmailToInviteUser(Email, RandomPassword);
                    return LoginMasterID;
                }
                //return LoginMasterID;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:SendMailForAccountActivation ~ " + ex.Message);
                return LoginMasterID;
            }
        }
        private async Task<bool> SendEmailToInviteUser(string toEmail, string RandomPassword)
        {
            try
            {
                string body = "";
                string contentRootPath = _env.ContentRootPath;
                string webRootPath = _env.WebRootPath;
                using (var sr = new System.IO.StreamReader(contentRootPath + "\\wwwroot\\Content\\EmailTemplate\\UserInvitation.html"))
                {
                    body = sr.ReadToEnd();
                    body = body.Replace("{LogoImagePath}", Path.Combine(Directory.GetCurrentDirectory(), ".\\img\\logo.png"));
                    body = body.Replace("{Salutactation}", "Hello");
                    var WebAppURL = $"{BaseURL}/auth/login";
                    body = body.Replace("{WebAppURL}", WebAppURL);
                    body = body.Replace("{Email}", toEmail);
                    body = body.Replace("{Password}", RandomPassword);
                    body = body.Replace("{Regards}", "Warm Regards");
                    body = body.Replace("{FromName}", "Shaqra University");
                }
                string toEmailIDs = toEmail;
                bool isEmailSend = UtilityFunctions.SendEmail(toEmailIDs, "", "", "", "Reset Password", body, null);
                return isEmailSend;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:SendEmailForForgotPassword ~ " + ex.Message);
                return false;
            }
        }
        public List<LoginModel> GetAllInternalUserList(int Type, long EntityTypeID)
        {
            List<LoginModel> lstLoginModel = new List<LoginModel>();
            try
            {
                lstLoginModel = _securityRepositoryService.GetAllInternalUserList(Type, EntityTypeID);
                return lstLoginModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:GetAllInternalUserList ~ " + ex.Message);
                return lstLoginModel;
            }
        }

    }
}