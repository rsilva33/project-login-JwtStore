namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Create;

public class Service : IService
{
    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
    {
        const string subject = "Verify your accont";
        var client = new SendGridClient(Configuration.SendGrid.ApiKey);
        var from = new EmailAddress(Configuration.Email.DefaultFromEmail,Configuration.Email.DefaultFromName);
        var to = new EmailAddress(user.Email, user.Name);
        var content = $"Code {user.Email.Verification.Code}";
        var message = MailHelper.CreateSingleEmail(from, to, subject, content, content);

        await client.SendEmailAsync(message, cancellationToken);

    }
}
