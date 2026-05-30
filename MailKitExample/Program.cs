using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

class GmailExample
{
    static async Task Main()
    {
        var gmailUser = "";
        var gmailAppPassword = "xxxx xxxx xxxx xxxx"; // App Password з Google Account

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("My Name", gmailUser));
        message.To.Add(new MailboxAddress("Recipient", "recipient@example.com"));
        message.Subject = "Тест MailKit через Gmail";

        message.Body = new TextPart("plain")
        {
            Text = "Привіт! Це реальний лист через Gmail SMTP."
        };

        using var client = new SmtpClient();

        // Підключення до Gmail SMTP із шифруванням STARTTLS
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        // Автентифікація
        await client.AuthenticateAsync(gmailUser, gmailAppPassword);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        Console.WriteLine("✓ Лист надіслано через Gmail!");
    }
}