using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace MoveMateWebApi.Services;

public class FireBaseService {

	public FirebaseMessaging messaging => FirebaseMessaging.GetMessaging(app);

	protected readonly FirebaseApp app;

	public FireBaseService() {

		string path = @"C:\Users\Johnny\Downloads\movemate-630a7-firebase-adminsdk-4niho-d3ed8e3b69.json";

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