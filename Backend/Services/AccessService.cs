using TUnique = System.ValueTuple<string, string>;

namespace AuthServer.Data
{
    public class AccessService
    {
        private readonly Config config;
        public Dictionary<TUnique, string> UserByLoginInfo;

        public AccessService(Config config)
        {
            this.config = config;

            UserByLoginInfo = config.Users
                .Select(x => new { UserId = x.Key, Data = User.FlattenProviders(x.Value) })
                .SelectMany(pair => pair.Data.Select((entry) => new { Key = entry, Value = pair.UserId }))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private IEnumerable<string> GetUserGroups(string userId)
            => config.Groups.Where(x => x.Value.Contains(userId)).Select(x => x.Key);

        public bool HasAccess(Application application, AuthUserInfo auth)
        {
            var info = (auth.Provider, auth.Id);
            if (!UserByLoginInfo.TryGetValue(info, out var userId)) return false;

            if (application.AccessUsers.Contains(userId)) return true;

            var usersInGroups = application.AccessGroups
                .Where(groupName => config.Groups.ContainsKey(groupName))
                .SelectMany(name => config.Groups[name])
                .ToHashSet();

            if (usersInGroups.Contains(userId)) return true;

            return false;
        }
    }
}
