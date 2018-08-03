namespace BuildTrigger.VSTS
{
    public class VstsBuildOptions
    {
        public string Instance { get; set; }
        public string Project { get; set; }
        public string TokenBase64 { get; set; }
    }
}