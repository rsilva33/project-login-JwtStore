namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create;

// Trata a questao de contratos, permitir que a requisicao possa ser criada
public static class Specification
{
    public static Contract<Notification> Ensure(Request request)
        => new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Name.Length, 160, "Name", "The name must contain less than 160 characters.")
            .IsGreaterThan(request.Name.Length, 3, "Name", "The name must contain more than 3 characters.")
            .IsLowerThan(request.Password.Length, 40, "Password", "Password must contain less than 40 characters.")
            .IsGreaterThan(request.Password.Length, 8, "Password", "Password must contain more than 8 characters.")
            .IsEmail(request.Email, "Email", "Invalid e-mail.");
}
