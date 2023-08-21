using MoveMateWebApi.Models.Data;

namespace MoveMateWebApi.Services {

	public interface ITokenFactory {
		Task<string> Create(Session session);
	}
}