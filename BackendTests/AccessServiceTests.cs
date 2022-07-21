using AuthServer;
using AuthServer.Data;

namespace BackendTests
{
    [TestClass]
    public class AccessServiceTests
    {
        private static AuthUserInfo User => new() { Provider = "provider1", Id = "user1@example.org" };

        [TestMethod]
        public void NoAnyAccess()
        {
            var app1 = new Application();
            var user1 = User;
            var config = new Config();
            config.Applications.Add("app1", app1);

            var service = new AccessService(config);
            var has = service.HasAccess(app1, user1);
            Assert.AreEqual(false, has);
        }

        [TestMethod]
        public void NoAccess()
        {
            var app1 = new Application();
            var config = new Config();
            var user1 = new User();
            user1.Providers.Add("mail1", new HashSet<string> { "mail1@example.org", "mail2@example.org" });
            config.Users.Add(nameof(user1), user1);
            config.Applications.Add("app1", app1);

            var userLogin = new AuthUserInfo { Provider = "mail1", Id = "mail1@example.org" };

            var service = new AccessService(config);
            var hasAccess = service.HasAccess(app1, userLogin);
            Assert.AreEqual(false, hasAccess);
        }

        [TestMethod]
        public void HasUserAccess()
        {
            var app1 = new Application();
            var config = new Config();
            var user1 = new User();
            user1.Providers.Add("mail1", new HashSet<string> { "mail1@example.org", "mail2@example.org" });
            config.Users.Add(nameof(user1), user1);
            config.Applications.Add("app1", app1);

            var userLogin = new AuthUserInfo { Provider = "mail1", Id = "mail1@example.org" };
            app1.Access.Add(nameof(user1));

            /*
            var user2 = new User();
            user2.Providers.Add("mail1", new HashSet<string> { "mail3@example.org", "mail4.example.org" });
            user2.Providers.Add("prov1", new HashSet<string> { "id52323" });
            config.Users.Add(nameof(user2), user2);
            */

            var service = new AccessService(config);
            var hasAccess = service.HasAccess(app1, userLogin);
            Assert.AreEqual(true, hasAccess);
        }
    }
}