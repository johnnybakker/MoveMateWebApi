using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MoveMate.Models.Data;
using Newtonsoft.Json.Linq;

namespace MoveMate.API.Services;

public class FireBaseService : INotificationService {

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

    public async Task BroadcastAsync(string title, string body, IEnumerable<string> identifiers)
    {
        var message = new MulticastMessage
        {
			Notification = new Notification
			{
                Title = title,
				Body = body
            },
            Tokens = identifiers.ToList()
        };

        var _ = await messaging.SendMulticastAsync(message);
    }
}