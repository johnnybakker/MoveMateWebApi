using MoveMate.Models.Data;

namespace MoveMate.API.Services {

	public interface ITokenFactory {
		Task<string> Create(Session session);
	}
}