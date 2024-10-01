namespace Authentication.Lib.Extensions.Email
{
    public class EmailSettings
    {
        public string? SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public string? SenderEmail { get; set; }
    }
}
