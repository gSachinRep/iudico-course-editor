namespace FireFly.CourseEditor.Common
{
    using System;

    /// <summary>
    /// Base exception to all exception can be raising by this application
    /// </summary>
    [Serializable]
    public class FireFlyException : ApplicationException
    {
        public FireFlyException()
        {
        }

        public FireFlyException(
            [NotNull]
            string message)

            : base(message)
        {
        }

        public FireFlyException([NotNull]string message, [NotNull]Exception innerException): base(message, innerException)
        {
        }

        [StringFormatMethod("fmtMessage")]
        public FireFlyException([NotNull]string fmtMessage, params object[] args)
            : this(fmtMessage, null, args)
        {   
        }

        [StringFormatMethod("fmtMessage")]
        public FireFlyException([NotNull] string fmtMessage, [NotNull] Exception innerException, params object[] args)
            : this(string.Format(fmtMessage, args), innerException)
        {
        }
    }
}