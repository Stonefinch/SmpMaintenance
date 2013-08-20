using System.Web.Caching;

namespace StoneFinch.SmpMaintenance.Views.Web.Interop
{
    public class HttpRuntimeCacheReferenceDictionaryProvider : IReferenceDictionaryProvider
    {
        public HttpRuntimeCacheReferenceDictionaryProvider(Cache cache)
        {
            this.Cache = cache;
        }

        private Cache Cache { get; set; }

        public ReferenceDictionary GetReferenceDictionary()
        {
            var rd = this.Cache[ReferenceDictionary.HttpRuntimeCacheKeyReferenceDictionary] as ReferenceDictionary;

            if (rd == null)
            {
                rd = new ReferenceDictionary();
                this.Cache[ReferenceDictionary.HttpRuntimeCacheKeyReferenceDictionary] = rd;
            }

            return rd;
        }
    }
}