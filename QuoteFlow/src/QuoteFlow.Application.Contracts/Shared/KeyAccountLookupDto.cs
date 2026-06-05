
namespace QuoteFlow.Shared;
public class KeyAccountLookupDto<TKey> : LookupDto<TKey>
{
    public TKey? KeyAccountClassId { get; set; }
    public TKey? KeyAccountTypeId { get; set; }

    public string KeyAccountClassName { get; set; } = null!;
    public string KeyAccountTypeName { get; set; } = null!;
}
