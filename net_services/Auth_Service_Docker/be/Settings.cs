namespace be
{
    public class Settings
    {
        public Dictionary<string, string> JWT { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public Dictionary<string, string> ServiceURLS { get; set; }

        public Settings() { 
            this.JWT = new Dictionary<string, string>();
            this.ConnectionStrings = new Dictionary<string, string>(); 
            this.ServiceURLS = new Dictionary<string, string>();
        }
    }
}
