using BusinessLogic.Dtos;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using BusinessLogic.Exceptions;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Jose;

namespace BusinessLogic.Service
{
    public class ApplicationJWTService : ExceptionService
    {
        private readonly Models.JwtSettings _settings;

        public ApplicationJWTService(IOptions<Models.JwtSettings> options)
        {
            _settings = options.Value;
        }

        public GenericResult<JWTDtoResponse> generate(JWTDtoRequest applicationJWTDto)
        {           
            GenericResult<JWTDtoResponse> result = new GenericResult<JWTDtoResponse>();

            try
            {

                string[] errores = this.Valida(applicationJWTDto);

                if (errores.Length > 0)
                {
                    throw new BadRequestException("Solicitud no válida.", errores);
                }

                string jwt_out = CreateJWT(
                    applicationJWTDto.name, 
                    applicationJWTDto.email, 
                    applicationJWTDto.groups, 
                    _settings.PrivateCertificateFile, 
                    _settings.KeyID, 
                    _settings.Issuer
                );
                var resp = new JWTDtoResponse
                {
                    Token = jwt_out,
                    Url = _settings.TenantUrl,
                    WebIntegrationID = _settings.WebIntegrationID
                };
                //return Ok(resp);

                result.Resultado = resp;
                result.CreacionExitosa();

                return result;

            }
            catch (Exception e)
            {
                result = this.GeneraError<JWTDtoResponse>(e);
            }

            return result;
        }

        public static string CreateJWT(string name, string email, string[] groups, string privateCertificateFile, string keyId, string issuer)
        {
            try
            {
                //Read the content of the private certificate file
                if (!File.Exists(privateCertificateFile))
                {
                    throw new FileNotFoundException(string.Format("The file {0} could not be found", privateCertificateFile));
                }
                string privateKey = File.ReadAllText(privateCertificateFile);

                //Prepare the unix time values required for the signing process. All time values must be of type integer
                var dtOffsetNow = DateTimeOffset.UtcNow;
                var creationTime = (int)dtOffsetNow.ToUnixTimeSeconds();
                var expTime = (int)dtOffsetNow.AddSeconds(500).ToUnixTimeSeconds();
                var nbfTime = (int)dtOffsetNow.AddSeconds(-1000).ToUnixTimeSeconds();

                //Prepare the header
                Dictionary<string, object> headers = new Dictionary<string, object>
                {
                    { "typ", "JWT"},
                    { "alg", "RS256" },
                    { "kid", keyId }
                };

                //Prepare the payload
                var payload = new Dictionary<string, object>
                {
                    //{ "sub", Guid.NewGuid().ToString() },
                    { "sub", email },
                    { "subType", "user" },
                    { "name", name },
                    { "email", email },
                    { "email_verified", "true"},
                    { "iss", issuer },
                    { "iat", creationTime }, //value must be of type integer
                    { "nbf", nbfTime }, //value must be of type integer
                    { "exp", expTime }, //value must be of type integer
                    { "jti", Guid.NewGuid().ToString() },
                    { "aud", "qlik.api/login/jwt-session" },
                    { "groups", groups }
                };

                //Create and return the JWT token               
                var token = CreateToken(payload, privateKey, headers);

                return token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public static string CreateToken(Dictionary<string, object> payload, string privateRsaKey, Dictionary<string, object> headers)
        {
            RSAParameters rsaParams = new RSAParameters();

            try
            {
                using (var tr = new StringReader(privateRsaKey))
                {
                    var pemReader = new PemReader(tr);

                    //We need to check the first line of the certificate file for reading the RSA keys/parameters 
                    if (privateRsaKey.Contains("-----BEGIN RSA PRIVATE KEY-----"))
                    {
                        if (!(pemReader.ReadObject() is AsymmetricCipherKeyPair keyPair))
                        {
                            throw new Exception("Could not read RSA private key");
                        }
                        var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                        rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
                    }
                    else if (privateRsaKey.Contains("-----BEGIN PRIVATE KEY-----"))
                    {
                        if (!(pemReader.ReadObject() is AsymmetricKeyParameter keyParam))
                        {
                            throw new Exception("Could not read RSA private key");
                        }
                        var privateRsaParams = keyParam as RsaPrivateCrtKeyParameters;
                        rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
                    }
                    else
                    {
                        //Handle extra cases if required
                        throw new Exception("Unknown key format. This key is not supported yet");
                    }
                }

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaParams);
                    return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256, extraHeaders: headers);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public string[] Valida(JWTDtoRequest applicationJWTDto)
        {

            List<string> errores = new List<string>();

            if (applicationJWTDto == null)
            {
                errores.Add("Los datos son requeridos.");
            }

            if (string.IsNullOrEmpty(applicationJWTDto.email))
            {
                errores.Add("El campo Email es obligatorio");
            }

            if (string.IsNullOrEmpty(applicationJWTDto.name))
            {
                errores.Add("El campo Nombre es obligatorio");
            }

            return errores.ToArray();

        }
    }
}
