using System.Diagnostics;

namespace vNextApplication.Services
{
    public class DebugMailService:IMailService
    {
        public bool SendMail(string to, string @from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail, To: {to}, Subject: {subject}");
            return true;

        }
    }
}
