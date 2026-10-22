namespace Identity.Core.Enums
{
    // Stored for admins only. API responses stay generic ("Invalid email or password.").
    public enum ActivityFailureReason
    {
        EmailAlreadyRegistered = 1,

        UnknownEmail,

        InvalidPassword,

        LockedOut,

        InactiveUser,

        WrongCurrentPassword,

        TokenReused,

        InvalidResetToken,
    }
}
