using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "yPKdE345%%AwqEfASrDFZCBbsaEvYUidf";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;
        private List<UserAccount> _userAccounts;

        public JwtTokenHandler()
        {
            _userAccounts = new List<UserAccount>()
            {
                new UserAccount{ UserName="admin", Password="admin123", Rol="Administrator" },
                new UserAccount{ UserName="user01", Password="user01", Rol="User" }
            };
        }

        public AuthenticationResponse GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if (AreThereCredentialsInformation(authenticationRequest.UserName, authenticationRequest.Password))
            {
                return null;
            }

            var _userAccounts = AreValidCredentials(authenticationRequest.UserName, authenticationRequest.Password);
            if (_userAccounts == null)
            {
                return null;
            }

            return GetAuthenticationResponse(authenticationRequest.UserName, _userAccounts.Rol);
        }

        private AuthenticationResponse GetAuthenticationResponse(string userName, string password)
        {
            var securityTokenDescriptor = GetSecurityTokenDescriptor(userName, password);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            return new AuthenticationResponse
            {
                UserName = userName,
                ExpiresIn = (int)DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS).Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token,
            };
        }

        private bool AreThereCredentialsInformation(string userName, string password)
        {
            return string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password);
        }

        private UserAccount AreValidCredentials(string userName, string password)
        {
            var userAccount = _userAccounts.FirstOrDefault(x => x.UserName.Equals(userName)
                                                             && x.Password.Equals(password));
            return userAccount;
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(string userName, string role)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, userName),
                    // Claim for microservice endpoints
                    //new Claim(ClaimTypes.Role, role)

                    // Claim for api gateway
                    new Claim("Role", role)
                }),
                SigningCredentials = new SigningCredentials(
                                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT_SECURITY_KEY)),
                                            SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS)
            };
        }
    }
}
