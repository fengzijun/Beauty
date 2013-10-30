using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Security;
using Beauty.InterFace;
using Beauty.Service;
using Beauty.Model;
using System.Runtime.Serialization.Json;

namespace Nodus.Authentication
{
  
   
    //using Nodus.Applications.Membership.Service;

    public class CustomIdentity : ICustomIdentity
    {
     

        /// <summary>
        /// Authenticate and get identity out with roles
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Instance of identity</returns>
        public static CustomIdentity GetCustomIdentity(string userName,string id)
        {
            CustomIdentity identity = new CustomIdentity();
            identity.IsAuthenticated = true;
            identity.Name = userName;
            identity.Id = id;
            var roles = new string[] { };
            IUser userservice = new UserService();
       
            User user = userservice.Get(new Guid(id));

            roles = new string[] { user.Role.ToString() };
            identity.Roles = roles;
            return identity;
        }

        private CustomIdentity() { }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public string Id {get;set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public string[] Roles { get; set; }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return false;
            }

            string[] temproles = role.Split(new char[] { ',' });
            foreach (string temprole in temproles)
            {
                foreach (string r in Roles)
                {
                    if (temprole.ToUpper() == r.ToUpper())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Create serialized string for storing in a cookie
        /// </summary>
        /// <returns>String representation of identity</returns>
        public string ToJson()
        {
            string returnValue = string.Empty;
            IdentityRepresentation representation = new IdentityRepresentation()
            {
                IsAuthenticated = this.IsAuthenticated,
                Name = this.Name,
                Roles = string.Join("|", this.Roles),
                ID = this.Id
            };

            DataContractJsonSerializer jsonSerializer = 
                new DataContractJsonSerializer(typeof(IdentityRepresentation));
            using (MemoryStream stream = new MemoryStream())
            {
                jsonSerializer.WriteObject(stream, representation);
                stream.Flush();
                byte[] json = stream.ToArray();
                returnValue = Encoding.UTF8.GetString(json, 0, json.Length);
            }

            return returnValue;
        }

        /// <summary>
        /// Create identity from a cookie data
        /// </summary>
        /// <param name="cookieString">String stored in cookie, created via ToJson method</param>
        /// <returns>Instance of identity</returns>
        public static ICustomIdentity FromJson(string cookieString)
        {
            try
            {
                if (string.IsNullOrEmpty(cookieString))
                    return null;
                IdentityRepresentation serializedIdentity = null;
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(cookieString)))
                {
                    DataContractJsonSerializer jsonSerializer =
                        new DataContractJsonSerializer(typeof(IdentityRepresentation));
                    serializedIdentity = jsonSerializer.ReadObject(stream) as IdentityRepresentation;
                }
                CustomIdentity identity = new CustomIdentity()
                {
                    IsAuthenticated = serializedIdentity.IsAuthenticated,
                    Name = serializedIdentity.Name,
                    Roles = serializedIdentity.Roles
                        .Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries),
                    Id = serializedIdentity.ID
                };
                return identity;
            }
            catch
            {
                return null;
            }
        }

    }
}