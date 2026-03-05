namespace GenericFunctions.EFNoSQL
{
    /// <summary>
    /// This static class stores the database connection information
    /// </summary>
    public static class IMongoDbSettings
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
    }
}
