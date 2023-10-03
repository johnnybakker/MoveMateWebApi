
using MoveMate.API.Database;

namespace MoveMate.API.Repositories;

public abstract class Repository {			
	protected readonly IMoveMateDbContext _context;

	public Repository(IMoveMateDbContext context) {
		_context = context;

	}
}