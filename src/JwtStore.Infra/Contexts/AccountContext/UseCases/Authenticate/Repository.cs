﻿using IRepository = JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts.IRepository;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate;

public class Repository : IRepository
{
    private readonly AppDbContext _context;
    public Repository(AppDbContext context) => _context = context;

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context
                             .Users
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Email.Address == email, cancellationToken);
    }
}
