namespace FB.QuickCommenter.Model
{
    public enum FbErrorSubCodes
    {
        None = 0,
        EmptyJson = -1,
        DomainBanned = 1346003,
        SomethingIsBanned = 1487390,
        CardInUseTooManyTimes= 2078073,
        CardDeclined = 2078019,
        BmLimit = 1690114,
        InvalidToken = 452,
        InvalidToken2 = 464,
        Checkpoint = 490,
        Checkpoint2 = 459,
        RiskPayment = 1815066,
        DoesNotExist = 33,
        Other = 666
    }
}
