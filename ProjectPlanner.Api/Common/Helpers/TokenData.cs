using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ProjectPlanner.Api.Common.Helpers
{
    public static class TokenData
    {
        public static T MapTokenDataToObject<T>(ClaimsPrincipal principal) where T : new()
        {
            T obj = new T();

            var properties = typeof(T).GetProperties().ToList();

            var tes = typeof(JwtRegisteredClaimNames).GetProperties().ToList();

            foreach (var claim in principal.Claims)
            {
                var property = properties.Select(x => x).Where(x => x.Name.ToLower() == claim.Type.ToLower()).FirstOrDefault();

                if (property != null)
                {
                    property.SetValue(obj, Convert.ChangeType(claim.Value, property.PropertyType));
                } else if (claim.Type == JwtRegisteredClaimNames.Email)
                {
                    property = properties.Select(x => x).Where(x => x.Name == "Email").FirstOrDefault();
                    property.SetValue(obj, Convert.ChangeType(claim.Value, property.PropertyType));
                }
            }

            return obj;
        }
    }
}
