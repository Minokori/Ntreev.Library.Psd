using System.Diagnostics.CodeAnalysis;

namespace Ntreev.Library.Psd.Services;

public partial class PsdDocumentManager
    {
    private static KeyValuePair<Uri, PsdDocument> StringPairToUriPair(
        KeyValuePair<string, PsdDocument> item
    ) => new(new(Path.GetFullPath(item.Key)), item.Value);

    public PsdDocument this[string filename]
        {
        get => this[new Uri(Path.GetFullPath(filename))];
        set => this[new Uri(Path.GetFullPath(filename))] = value;
        }

    public void Add(string filename, PsdDocument value) =>
        Add(new Uri(Path.GetFullPath(filename)), value);

    public bool ContainsKey(string key) => ContainsKey(new Uri(Path.GetFullPath(key)));

    public bool Remove(string key) => Remove(new Uri(Path.GetFullPath(key)));

    public bool TryAdd(string key, PsdDocument value)
        {
        var uri = new Uri(Path.GetFullPath(key));
        return TryAdd(uri, value);
        }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out PsdDocument value) =>
        throw new NotImplementedException();

    #region 和 Dictionary<Uri, PsdDocument> 保持一致, 显式实现的方法

    ICollection<string> IDictionary<string, PsdDocument>.Keys =>
        (ICollection<string>)Keys.Select(i => i.LocalPath);

    ICollection<PsdDocument> IDictionary<string, PsdDocument>.Values => Values;

    void ICollection<KeyValuePair<string, PsdDocument>>.Add(
        KeyValuePair<string, PsdDocument> item
    ) => Add(new Uri(Path.GetFullPath(item.Key)), item.Value);

    bool ICollection<KeyValuePair<string, PsdDocument>>.Contains(
        KeyValuePair<string, PsdDocument> item
    ) =>
        ((ICollection<KeyValuePair<Uri, PsdDocument>>)this).Contains(
            new(new Uri(Path.GetFullPath(item.Key)), item.Value)
        );

    void ICollection<KeyValuePair<string, PsdDocument>>.CopyTo(
        KeyValuePair<string, PsdDocument>[] array,
        int arrayIndex
    )
        {
        var uriArray = array.Select(StringPairToUriPair)
                .ToArray();
        ((ICollection<KeyValuePair<Uri, PsdDocument>>)this).CopyTo(uriArray, arrayIndex);
        }

    bool ICollection<KeyValuePair<string, PsdDocument>>.IsReadOnly =>
        ((ICollection<KeyValuePair<Uri, PsdDocument>>)this).IsReadOnly;

    bool ICollection<KeyValuePair<string, PsdDocument>>.Remove(
        KeyValuePair<string, PsdDocument> item
    ) => ((ICollection<KeyValuePair<Uri, PsdDocument>>)this).Remove(StringPairToUriPair(item));

    IEnumerator<KeyValuePair<string, PsdDocument>> IEnumerable<
        KeyValuePair<string, PsdDocument>
    >.GetEnumerator()
        {
        var enumerator = ((IEnumerable<KeyValuePair<Uri, PsdDocument>>)this).GetEnumerator();

        while (enumerator.MoveNext())
            {
            var item = enumerator.Current;
            yield return new(item.Key.LocalPath, item.Value);
            }
        }
    #endregion
    }
