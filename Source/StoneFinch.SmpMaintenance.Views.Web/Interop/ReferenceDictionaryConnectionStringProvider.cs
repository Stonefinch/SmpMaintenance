using StoneFinch.SmpMaintenance.Data;

namespace StoneFinch.SmpMaintenance.Views.Web.Interop
{
    /// <summary>
    /// Connection string provider that retrieves current connection string stored in reference dictionary
    /// </summary>
    public class ReferenceDictionaryConnectionStringProvider : IConnectionStringProvider
    {
        public ReferenceDictionaryConnectionStringProvider(IReferenceDictionaryProvider referenceDictionaryProvider)
        {
            this.ReferenceDictionaryProvider = referenceDictionaryProvider;
        }

        private IReferenceDictionaryProvider ReferenceDictionaryProvider { get; set; }

        public string GetConnectionString()
        {
            return this.ReferenceDictionaryProvider.GetReferenceDictionary().ConnectionString;
        }
    }
}