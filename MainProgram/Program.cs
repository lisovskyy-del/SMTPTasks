using System.Net;
using System.Net.Mail;

class SmtpNativeExample
{
    static async Task Main()
    {
        // 1. Створити SmtpClient
        using var smtpClient = new SmtpClient("localhost")
        {
            Port = 1025,
            EnableSsl = false,          // MailHog не потребує SSL
            Credentials = CredentialCache.DefaultNetworkCredentials,
            DeliveryMethod = SmtpDeliveryMethod.Network,
        };

        // 2. Створити повідомлення
        var message = new MailMessage
        {
            From = new MailAddress("sender@example.com", "Відправник"),
            Subject = "Тестовий лист через System.Net.Mail",
        };

        message.IsBodyHtml = true;
        message.Body = """
    <html>
    <body>
        <h1>Привіт!</h1>
        <p>Це <strong>HTML</strong>-лист, надісланий через <em>System.Net.Mail</em>.</p>
    </body>
    </html>
    """;

        // 3. Додати отримувача
        message.To.Add(new MailAddress("recipient@example.com", "Отримувач"));
        message.CC.Add("cc@example.com");
        message.Bcc.Add("bcc@example.com");

        var attachment = new Attachment("report.txt", System.Net.Mime.MediaTypeNames.Text.Plain);
        message.Attachments.Add(attachment);

        // AlternateView для text/plain
        var plainView = AlternateView.CreateAlternateViewFromString(
            "Привіт! Це текстова версія листа.",
            null,
            "text/plain"
        );

        // AlternateView для text/html
        var htmlView = AlternateView.CreateAlternateViewFromString(
            "<h1>Привіт!</h1><p>Це HTML-версія листа.</p>",
            null,
            "text/html"
        );

        message.AlternateViews.Add(plainView);
        message.AlternateViews.Add(htmlView);

        // 4. Надіслати
        try
        {
            await smtpClient.SendMailAsync(message);
            Console.WriteLine("✓ Лист надіслано успішно!");
            Console.WriteLine("Відкрийте http://localhost:8025 щоб побачити лист.");
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"✗ Помилка SMTP: {ex.StatusCode} — {ex.Message}");
        }
    }
}