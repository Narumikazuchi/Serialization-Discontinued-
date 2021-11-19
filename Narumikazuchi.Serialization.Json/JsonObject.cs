namespace Narumikazuchi.Serialization.Json;

/// <summary>
/// Represents a JSON-Object in a JSON-String.
/// </summary>
public sealed partial class JsonObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonObject"/> class.
    /// </summary>
    public JsonObject()
    { }

    /// <summary>
    /// Gets whether this object has a member with the specified name.
    /// </summary>
    /// <param name="member">The name of the member to find.</param>
    /// <returns><see langword="true"/> if the a member with the specified name exists within the object; otherwise, <see langword="false"/></returns>
    public Boolean HasMember([DisallowNull] String member)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(member);
        return this._members.ContainsKey(member);
    }

    /// <inheritdoc/>
    public override String ToString()
    {
        StringBuilder builder = new();
        foreach (String member in this._members.Keys)
        {
            if (builder.Length > 0)
            {
                builder.Append(", ");
            }
            builder.Append($"\"{member}\": ");
            JsonElement? element = this._members[member];
            if (element is not null)
            {
                builder.Append(element.ToString());
                continue;
            }
            builder.Append("null");
        }
        return $"{{ {builder} }}";
    }

    /// <summary>
    /// Creates a <see cref="JsonObject"/> from the given JSON-String.
    /// </summary>
    /// <param name="json">The JSON-String to parse.</param>
    /// <returns>The JSON-Object that is represented by the specified JSON-String</returns>
    /// <exception cref="FormatException"/>
    [return: MaybeNull]
    public static JsonObject? FromJsonString([DisallowNull] String json)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(json);
        if (IsJsonNullString(json))
        {
            return null;
        }

        JsonObject result = new();

        if (json[0] != '{')
        {
            throw new FormatException();
        }
        Int32 index = 1;
        ReadObjectString(result,
                         json,
                         ref index);
        return result;
    }

    /// <summary>
    /// Checks whether the specified JSON-String represents <see langword="null"/>.
    /// </summary>
    /// <param name="json">The JSON-String to parse.</param>
    /// <returns><see langword="true"/> if the JSON-String represents <see langword="null"/>; otherwise, <see langword="false"/></returns>
    public static Boolean IsJsonNullString([DisallowNull] String json)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(json);
        return json.Length == 4 &&
               json == "null";
    }

    /// <summary>
    /// Gets the object for the specified member name.
    /// </summary>
    /// <param name="memberName">The name of the member to get.</param>
    /// <returns>The value for the given member name</returns>
    /// <exception cref="KeyNotFoundException"/>
    [MaybeNull]
    public JsonElement? this[[DisallowNull] String memberName]
    {
        get
        {
            ExceptionHelpers.ThrowIfArgumentNull(memberName);
            if (!this._members.ContainsKey(memberName))
            {
                throw new KeyNotFoundException();
            }
            return this._members[memberName];
        }
    }

    /// <summary>
    /// Gets all member names contained in this object.
    /// </summary>
    [NotNull]
    public IEnumerable<String> Members => this._members.Keys;
}

partial class JsonObject : JsonElement
{
    internal void Add(String memberName,
                      JsonElement? element)
    {
        ExceptionHelpers.ThrowIfArgumentNull(memberName);

        if (this._members.ContainsKey(memberName))
        {
            throw new ArgumentException("Member with the specified name already exists.", 
                                        nameof(memberName));
        }

        this._members.Add(memberName, 
                          element);
    }

    private static void ReadObjectString(JsonObject jsonObject,
                                         String json,
                                         ref Int32 index)
    {
        do
        {
            if (Char.IsWhiteSpace(json[index]))
            {
                continue;
            }
            if (json[index] == '"')
            {
                index++;
                ReadMemberString(json,
                                 ref index,
                                 out String member,
                                 out JsonElement? element);
                jsonObject.Add(member,
                               element);
                if (json[index] == '}')
                {
                    index++;
                    break;
                }
                continue;
            }
        } while (++index < json.Length);
    }
        
    private static void ReadMemberString(String json,
                                         ref Int32 index,
                                         out String member,
                                         out JsonElement? element)
    {
        member = String.Empty;
        element = null;
        Boolean readMember = true;
        Boolean expectColon = false;
        do
        {
            if (expectColon &&
                Char.IsWhiteSpace(json[index]))
            {
                continue;
            }
            if (expectColon &&
                json[index] == ':')
            {
                index++;
                ReadValueString(json,
                                ref index,
                                out element);
                return;
            }

            if (readMember &&
                json[index] == '"')
            {
                readMember = false;
                expectColon = true;
                continue;
            }
            if (readMember)
            {
                member += json[index];
                continue;
            }
        } while (++index < json.Length);
    }

    private static void ReadArrayString(JsonArray jsonArray,
                                        String json,
                                        ref Int32 index)
    {
        do
        {
            if (Char.IsWhiteSpace(json[index]))
            {
                continue;
            }
            if (json[index] == '"')
            {
                ReadValueString(json,
                                ref index,
                                out JsonElement? element);
                jsonArray.Add(element);
                if (json[index] == ']')
                {
                    index++;
                    break;
                }
            }
        } while (++index < json.Length);
    }

    private static void ReadValueString(String json,
                                        ref Int32 index, 
                                        out JsonElement? element)
    {
        element = null;
        String value = String.Empty;
        Boolean moveToValue = true;
        Boolean readValue = false;
        do
        {
            if (moveToValue &&
                Char.IsWhiteSpace(json[index]))
            {
                continue;
            }
            if (moveToValue &&
                !Char.IsWhiteSpace(json[index]))
            {
                if (json[index] == '{')
                {
                    index++;
                    JsonObject jsonObject = new();
                    ReadObjectString(jsonObject,
                                     json,
                                     ref index);
                    element = jsonObject;
                    return;
                }
                if (json[index] == '[')
                {
                    index++;
                    JsonArray jsonArray = new();
                    ReadArrayString(jsonArray,
                                    json,
                                    ref index);
                    element = jsonArray;
                    return;
                }
                moveToValue = false;
                readValue = true;
            }

            if (readValue &&
                json[index] is ',' 
                            or '}'
                            or ']')
            {
                if (value.Length > 0)
                {
                    if (value[0] != '"' ||
                        (value[0] == '"' &&
                        value[^1] == '"'))
                    {
                        break;
                    }
                }
            }
            if (readValue)
            {
                if (value.Length > 0 &&
                    value[0] != '"' &&
                    Char.IsWhiteSpace(json[index]))
                {
                    continue;
                }
                value += json[index];
                continue;
            }
        } while (++index < json.Length);

        if (value == "null")
        {
            element = null;
            return;
        }
        if (value[0] == '"' &&
            value[^1] == '"')
        {
            element = new __JsonElement<String>(value[1..^1]);
            return;
        }
        if (Boolean.TryParse(value,
                             out Boolean binary))
        {
            element = new __JsonElement<Boolean>(binary);
            return;
        }
        if (Int64.TryParse(value, 
                           out Int64 integer))
        {
            element = new __JsonElement<Int64>(integer);
            return;
        }
        if (UInt64.TryParse(value,
                            out UInt64 biginteger))
        {
            element = new __JsonElement<UInt64>(biginteger);
            return;
        }
        if (Double.TryParse(value,
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out Double floating))
        {
            element = new __JsonElement<Double>(floating);
            return;
        }
    }

    private readonly Dictionary<String, JsonElement?> _members = new();
}

partial class JsonObject : IEnumerable<JsonElement?>
{
    /// <inheritdoc/>
    public IEnumerator<JsonElement?> GetEnumerator()
    {
        foreach (JsonElement? element in this._members.Values)
        {
            yield return element;
        }
        yield break;
    }
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}