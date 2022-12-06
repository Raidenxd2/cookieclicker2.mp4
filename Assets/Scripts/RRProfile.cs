using System;

[Serializable]
public class RRProfile
{
    public string acountId {get;set;}
    public string username { get; set; }
    public string displayName {get;set;}
    public string profileImage { get; set; }
    public string bannerImage {get;set;}
    public bool isJunior {get;set;}
    public string platforms {get;set;}
    public string personalPronouns {get;set;}
    public string identityFlags {get;set;}
    public string createdAt {get;set;}
}
