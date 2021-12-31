namespace NetworkCheck.Service
{
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        internal const int DEFAULT_PORT = 80;

        /// <summary>
        /// 
        /// </summary>
        internal const int DEFAULT_TIMEOUT = 1000;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static string GetBoolFlag(bool b)
        {
            return b ? "√" : "×";
        }
    }
}