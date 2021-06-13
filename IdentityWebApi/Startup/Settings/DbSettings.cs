namespace IdentityWebApi.Startup.Settings
{
    public class DbSettings
    {
        public string Host { get; set; }
        
        public int Port { get; set; }
        
        public string Instance { get; set; }
        
        public string User { get; set; }
        
        public string Password { get; set; }

        public string ConnectionString => $"Server=tcp:{Host},{Port};Database={Instance};User={User};Password={Password};MultipleActiveResultSets=True;";
    }
}