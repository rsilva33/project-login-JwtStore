namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository, IService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region [01 - Validar requisicao]
        try
        {
            var response = Specification.Ensure(request);

            if (response.IsValid)
                return new Response("Invalid request", 400, response.Notifications);
        }
        catch
        {
            return new Response("Invalid request", 500);
        }
        #endregion

        #region [02 - Gerar objetos]
        Email email;
        Password password;
        User user;

        try
        {
            email = new Email(request.Email);
            password = new Password(request.Password);
            user = new User(request.Name, email, password);
        }
        catch (Exception ex)
        {
            return new Response(ex.Message, 400);
        }
        #endregion

        #region [03 - Verificar se o usuario existe no banco]
        try
        {
            var exists = await _repository.AnyAsync(request.Email, cancellationToken);

            if (exists)
                return new Response("This e-mail is already in use.", 400);
        }
        catch
        {
            return new Response("Failed to verify registered e-mail.", 500);
        }
        #endregion

        #region [04 - Persistir os dados]
        try
        {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch
        {
            return new Response("Failed to persist data.", 500);
        }
        #endregion

        #region [05 - Enviar um e-mail de ativacao]
        try
        {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch 
        {
            // Do nothing
        }
        #endregion

        return new Response("Create account.", new ResponseData(user.Id, user.Name, user.Email));
    }
}
