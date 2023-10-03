
using MoveMate.Database;

namespace MoveMate.Repositories;

public abstract class Repository : IDisposable {			
	protected readonly MoveMateDbContext _context;

	public Repository(MoveMateDbContext context) {
		_context = context;

	}

	public void Dispose()
	{
		_context.Dispose();
	}
}