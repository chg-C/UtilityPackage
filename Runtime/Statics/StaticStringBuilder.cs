using System.Text;

namespace CHG.Utilities.Statics
{
    public static class StaticStringBuilder
    {
        private static StringBuilder builder = new StringBuilder();
        private static readonly object _lock = new object();

        public static StringBuilder Start(bool clear = true)
        {
            if(clear)
                builder.Clear();
            
            return builder;
        }
    }
}