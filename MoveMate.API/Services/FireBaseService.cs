using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace MoveMate.Services;

public class FireBaseService {

	public FirebaseMessaging messaging => FirebaseMessaging.GetMessaging(app);

	protected readonly FirebaseApp app;

	public FireBaseService(IConfiguration configuration) {

		string path = configuration["Firebase:CredentialFile"]!;

		var options = new AppOptions {
			ServiceAccountId = "firebase-adminsdk-4niho@movemate-630a7.iam.gserviceaccount.com",
			ProjectId = "movemate-630a7",
			Credential = GoogleCredential.FromFile(path),
		};

		app = FirebaseApp.Create(options, "MoveMate");
	}

	public string GetName() {
		return app.Name;
	}
}