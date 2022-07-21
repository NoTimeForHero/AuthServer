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

            var service = new AccessService(config);
            var hasAccess = service.HasAccess(app1, userLogin);
            Assert.AreEqual(true, hasAccess);
        }

        [TestMethod]
        public void HasGroupAccess()
        {
            var app1 = new Application();
            var config = new Config();
            var user1 = new User();
            user1.Providers.Add("mail1", new HashSet<string> { "mail1@example.org", "mail2@example.org" });
            config.Users.Add(nameof(user1), user1);
            config.Applications.Add("app1", app1);

            var user2 = new User();
            user2.Providers.Add("mail1", new HashSet<string> { "mail3@example.org", "mail4.example.org" });
            user2.Providers.Add("prov1", new HashSet<string> { "id52323" });
            config.Users.Add(nameof(user2), user2);

            config.Groups.Add("group1", new HashSet<string> { nameof(user1), nameof(user2) });
            config.Groups.Add("group2", new HashSet<string> { nameof(user1), nameof(user2) });
            app1.Access.Add("Group:group1");
            app1.Access.Add("Group:group2");
            app1.Access.Add("Group:invalid_name"); // Проверка на краш от несуществующей группы

            var service = new AccessService(config);
            var isUser1HasAccess = service.HasAccess(app1, new AuthUserInfo { Provider = "mail1", Id = "mail1@example.org" });
            var isUser2HasAccess = service.HasAccess(app1, new AuthUserInfo { Provider = "prov1", Id = "id52323" });
            Assert.AreEqual(true, isUser1HasAccess);
            Assert.AreEqual(true, isUser2HasAccess);
        }
    }
}