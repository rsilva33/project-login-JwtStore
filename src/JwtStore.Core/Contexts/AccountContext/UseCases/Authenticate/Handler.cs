namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly Contracts.IRepository _repository;

    public Handler(Contracts.IRepository repository) => _repository = repository;

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region [01 - Validar requisicao]
        try
        {
            var response = Specification.Ensure(request);

            if (!response.IsValid)
                return new Response("Invalid request", 400, response.Notifications);
        }
        catch
        {
            return new Response("Invalid request", 500);
        }
        #endregion

        #region [02 - Recuperar perfil]
        User user;

        try
        {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
                return new Response("Profile not found", 404);
        }
        catch
        {
            return new Response("We were unable to retrieve your profile", 500);
        }
        #endregion

        #region [03 - Checa se a senha é válida]

        if (!user.Password.Challenge(request.Password))
            return new Response("Username or password is invalid.", 400);

        #endregion

        #region [04 - Checa se a conta está verificada]
        try
        {
            if (!user.Email.Verification.IsActive)
                return new Response("Invalid account.", 400);
        }
        catch
        {
            return new Response("Unable to verify your profile.", 500);
        }

        #endregion

        #region [05 - Retorna os dados]
        try
        {
            var data = new ResponseData
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = Array.Empty<string>(),
            };

            return new Response(string.Empty, data);
        }
        catch
        {
            return new Response("Unable to obtain profile data.", 500);
        }

        #endregion
    }
}
