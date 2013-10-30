using System.Security.Principal;

namespace Nodus.Authentication
{
    public interface ICustomIdentity : IIdentity
    {
        bool IsInRole(string role);
        string ToJson();
        string[] Roles { get; set; }
        string Id { get; set; }
    }
}