namespace Identity.Core.Enums
{
    // Security events written by AuthService through IActivityLogger.
    public enum ActivityAction
    {
        Register = 1,

        Login,

        Logout,

        ChangePassword,

        // The failed login that reached the lockout threshold.
        AccountLocked,

        // An already-rotated refresh token was presented; every session of the user was revoked.
        TokenReuseDetected,

        UpdateProfile,

        RevokeSession,

        ForgotPassword,

        ResetPassword,

        AdminUnlockUser,

        AdminRevokeSessions,
    }
}
