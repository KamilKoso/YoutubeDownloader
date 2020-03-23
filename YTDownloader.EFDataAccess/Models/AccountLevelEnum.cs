namespace YTDownloader.EFDataAccess.Models
{
    public enum AccountLevel
    {
        None = 0, //Registered, not confirmed e-mail. 
        Standard = 1, //Registered, confirmed e-mail.
        Gold = 2, //Registered, bought YTGold
    }
}
