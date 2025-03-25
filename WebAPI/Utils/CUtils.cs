using BusinessObjects.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAPI.Utils
{
    public class CUtils
    {
        public class EmptyStringToNullableIntConverter : JsonConverter<short?>
        {
            public override short? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    string str = reader.GetString();
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return null; // Chuyển đổi chuỗi rỗng thành null
                    }
                    if (short.TryParse(str, out short result))
                    {
                        return result;
                    }
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetInt16();
                }

                throw new JsonException($"Không thể chuyển đổi giá trị JSON thành số nguyên nullable: {reader.GetString()}");
            }

            public override void Write(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                {
                    writer.WriteNumberValue(value.Value);
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
        }
        public class EmptyStringToNullableBooleanConverter : JsonConverter<bool?>
        {
            public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null; // Handle null token
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    string str = reader.GetString();
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return null; // Convert empty strings to null
                    }

                    // Handle string representations of booleans like "true", "false", "1", "0"
                    if (str.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    if (str.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    if (str == "1")
                    {
                        return true; // "1" string is considered true
                    }

                    if (str == "0")
                    {
                        return false; // "0" string is considered false
                    }
                }
                else if (reader.TokenType == JsonTokenType.True)
                {
                    return true; // Handle boolean true token
                }
                else if (reader.TokenType == JsonTokenType.False)
                {
                    return false; // Handle boolean false token
                }

                throw new JsonException($"Cannot convert value to nullable boolean: {reader.GetString()}");
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                {
                    writer.WriteBooleanValue(value.Value); // Write the boolean value
                }
                else
                {
                    writer.WriteNullValue(); // Write null if the value is nullable
                }
            }
        }

        public class TagCollectionConverter : JsonConverter<ICollection<Tag>>
        {
            public override ICollection<Tag> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null; // Nếu dữ liệu là null, trả về null
                }

                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var tags = new List<Tag>();

                    // Đọc từng phần tử trong mảng
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }

                        if (reader.TokenType == JsonTokenType.String)
                        {
                            var tagId = reader.GetString(); // Lấy chuỗi ID từ mảng

                            // Tạo một đối tượng Tag với ID từ chuỗi
                            var tag = new Tag
                            {
                                TagId = int.Parse(tagId) // Chuyển chuỗi thành số nguyên
                            };

                            tags.Add(tag);
                        }
                    }

                    return tags;
                }

                throw new JsonException("Expected StartArray token for ICollection<Tag>");
            }

            public override void Write(Utf8JsonWriter writer, ICollection<Tag> value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                // Ghi từng Tag trong danh sách ra JSON dưới dạng chuỗi
                foreach (var tag in value)
                {
                    writer.WriteStringValue(tag.TagId.ToString()); // Chuyển Id thành chuỗi
                }

                writer.WriteEndArray();
            }
        }

    }
}
