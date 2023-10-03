using MoveMate.Models.Data;

namespace MoveMate.Services {

	public interface ITokenFactory {
		Task<string> Create(Session session);
	}
}