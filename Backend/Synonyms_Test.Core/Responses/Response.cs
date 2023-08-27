namespace Synonyms_Test.Core.Responses
{
    public class Response
    {
        internal Response(int[] errorCodes) => ErrorCodes = errorCodes;

        public int[] ErrorCodes { get; set; }
    }
}
