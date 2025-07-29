

namespace TodoApp.Config
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string TodoCollectionName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}