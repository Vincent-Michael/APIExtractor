namespace APIExtractor
{
    public static class Settings
    {
        private static readonly Configuration Conf = new Configuration();

        public static readonly string[] APIKey = Conf.GetStringList("APIKey", new string[0]);
        public static readonly string Locale = Conf.GetString("Locale", "en_US");
    }
}
