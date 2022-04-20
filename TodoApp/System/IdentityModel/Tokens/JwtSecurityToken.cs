namespace System.IdentityModel.Tokens
{
    internal class JwtSecurityToken
    {
        private object jwt;

        public JwtSecurityToken(object jwt)
        {
            this.jwt = jwt;
        }
    }
}