using BaseLibrary.Configurations;
using BaseLibrary.Security;
using Microsoft.IdentityModel.Tokens;

namespace BaseLibrary.Controllers
{

    public class LoginWithKeyVM
    {
        public string Key { get; set; }

        public void TrimAllStrings()
        {
            Key = TrimString.Trim(Key);
        }
    }

    public class JWTTokenRequestVM
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public void TrimAllStrings()
        {
            LoginId = TrimString.Trim(LoginId);
            Password = TrimString.Trim(Password);
            Language = TrimString.Trim(Language);
        }
    }

    [Route("api/[controller]")]
    public class JWTController : BaseLibraryController
    {
        private TimeSpan TokenExpiration;
        private SigningCredentials SigningCredentials;

        #region Static Members
        private const string PrivateKey = "private_key_1234567890";
        public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));

        public AuthOptions AuthOptions { get; }


        #endregion Static Members

        public JWTController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory)
          : base(serviceFactory, loggerFactory.CreateLogger<JWTController>())
        {
            TokenExpiration = TimeSpan.FromMinutes(10000);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            this.AuthOptions = BSF.MicroAppContract.GetAuthOptions();
        }

        [HttpPost("LoginWithId")]
        public IActionResult LoginWithId([FromBody] LoginWithKeyVM loginWithKeyVM)
        {
            try
            {
                //DatabaseMigrator.CreateInitialMainDatabaseIfNotExist(new SqlCommandExecutor());

                if (!ModelState.IsValid) return BadRequest(ModelState);

                loginWithKeyVM.TrimAllStrings();

                var user = BSF.LoginService.GetUserAfterValidatingUserCredential(loginWithKeyVM.Key);

                if (user is not null)
                {
                    BSF.MicroAppContract?.TrackActivity(ActivityTypeBase.UserLoggedIn, string.Empty, user);
                    var jwt = JwtTokenGenerator.GenerateJwtToken(user.Id, "en", user.SessionId.ToString(), AuthOptions.SecureKey, AuthOptions.Issuer, AuthOptions.Audience);
                    //UnitOfWork.Commit();
                    return Ok(jwt);
                }
            }
            catch (Exception exception)
            {
                // TODO: handle errors
                // UnitOfWork.RollbackChanges();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }

            return BadRequest("Invalid username or password.");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] JWTTokenRequestVM jwtTokenRequestVM)
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState);

                ApplicationUser user = BSF.LoginService.GetUserAfterValidatingUserCredential(jwtTokenRequestVM.LoginId, jwtTokenRequestVM.Password);

                if (user != null)
                {
                    BSF.MicroAppContract?.TrackActivity(ActivityTypeBase.UserLoggedIn, string.Empty, user);
                    user.SessionId = Guid.NewGuid();
                    BSF.UserService.UpdateLastLogin(user);
                    var jwt = JwtTokenGenerator.GenerateJwtToken(user.Id, "en", user.SessionId.ToString(), AuthOptions.SecureKey, AuthOptions.Issuer, AuthOptions.Audience);
                    var permissions = BSF.UserRoleService.GetUserAllPermissions(user);
                    jwt.Permissions = GetPermissionsWithoutStudyPermissions(permissions);
                    jwt.Role = user.Role;
                    jwt.Name = user.Name;
                    CommitTransaction();
                    return Ok(jwt);
                }
            }
            catch (Exception exception)
            {
                // TODO: handle errors
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo(), jwtTokenRequestVM);
            }

            return BadRequest("Invalid username or password.");
        }

        private List<string> GetPermissionsWithoutStudyPermissions(List<string> permissions)
        {
            //permissions - study specific permissions
            return permissions;
        }


        //private void UpdateDatabase(string user)
        //{
        //    try
        //    {
        //        if (user == AppSettings.AdminLoginEmailId || AppSettings.Environment != "Production")
        //        {
        //            using var cmdExecutor = new SqlCommandExecutor();
        //            DatabaseMigrator.UpgradeMainDatabase(cmdExecutor);
        //        }
        //    }
        //    catch { }
        //}
        //private JwtToken GetJwtToken(LoginUserVM user)
        //{
        //    var jobprovider = SF.ApplicationUserSerivce.GetApplicationUser(user.Id);

        //    if (jobprovider != null)
        //    {
        //        jobprovider.UpdateLastSignIn();
        //        AddOperationLog(jobprovider, Operation.User_Login, $"{jobprovider.Name} login into system.");
        //    }
        //    DateTime now = DateTime.UtcNow;
        //    // add the registered claims for JWT (RFC7519).
        //    // For more info, see https:
        //    //tools.ietf.org/html/rfc7519#section-4.1
        //    var claims = new[]
        //    {
        //                new Claim(JwtRegisteredClaimNames.Iss, AppSettings.Issuer),
        //                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
        //                // TODO: add additional claims here
        //            };
        //    // Create the JWT and write it to a string
        //    var token = new JwtSecurityToken(
        //        issuer: AppSettings.Issuer,
        //    // audience: JWTController.Issuer,
        //    claims: claims,
        //    //   notBefore: now,
        //    expires: now.Add(TokenExpiration),
        //    signingCredentials: SigningCredentials);
        //    var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    // build the json response
        //    var jwt = new JwtToken
        //    {
        //        access_token = encodedToken,
        //        expiration = (int)TokenExpiration.TotalSeconds
        //    };

        //    return jwt;
        //}
    }
}