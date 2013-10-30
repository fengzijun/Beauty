using System;

namespace Nodus.Authentication
{
    /// <summary>
    /// Private members have short names to preserve space using json serialization
    /// </summary>
    public class IdentityRepresentation
    {
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private bool ia;

        public bool IsAuthenticated
        {
            get { return ia; }
            set { ia = value; }
        }

        private string n;

        public string Name
        {
            get { return n; }
            set { n = value; }
        }

        private string r;

        public string Roles
        {
            get { return r; }
            set { r = value; }
        }
        
    }
}