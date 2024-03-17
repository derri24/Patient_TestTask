namespace Patient.TestTask.RestApi.Configuration;

public interface IDatabaseConfiguration
{
    string DatabaseName { get; }
    string ConnectionString { get; }
}

public class DatabaseConfiguration : IDatabaseConfiguration
{
    public string DatabaseName => "test_task";
    public string ConnectionString => "mongodb://mongodb:27017";
}