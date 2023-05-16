using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Helpers
{
    public class Vector3dConverter : JsonConverter<Vector3d>
    {
        public override Vector3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<double> values = new List<double>();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected StartArray token");

            for (int i = 0; i < 3; i++)
            {
                reader.Read();
                if (reader.TokenType != JsonTokenType.Number)
                    throw new JsonException("Expected Number token");
                values.Add(reader.GetDouble());
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException("Expected EndArray token");

            return new Vector3d(values[0], values[1], values[2]);
        }

        public override void Write(Utf8JsonWriter writer, Vector3d value, JsonSerializerOptions options)
        {

            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
    }

    public class Vector4dConverter : JsonConverter<Vector4d>
    {
        public override Vector4d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<double> values = new List<double>();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected StartArray token");

            for (int i = 0; i < 4; i++)
            {
                reader.Read();
                if (reader.TokenType != JsonTokenType.Number)
                    throw new JsonException("Expected Number token");
                values.Add(reader.GetDouble());
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException("Expected EndArray token");

            return new Vector4d(values[0], values[1], values[2], values[3]);
        }

        public override void Write(Utf8JsonWriter writer, Vector4d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteNumberValue(value.W);
            writer.WriteEndArray();
        }
    }

    public static class VectorExtensionMethods
    {
        public static Vector3d Reflect(this Vector3d vector, Vector3d normal)
        {
            Vector3d I = vector.Normalized();
            Vector3d N = normal.Normalized();
            return I - 2.0 * Vector3d.Dot(N, I) * N;
        }

        public static Vector3d Refract(this Vector3d vector, Vector3d normal, double eta)
        {
            Vector3d I = vector.Normalized();
            Vector3d N = normal.Normalized();
            var cos_theta = double.Abs(Vector3d.Dot(-I, N));
            Vector3d r_out_perp = eta * (I + cos_theta * N);
            Vector3d r_out_parallel = -double.Sqrt(double.Abs(1.0 - r_out_perp.LengthSquared)) * N;
            return r_out_perp + r_out_parallel;
        }

        public static float[] ToFloatArray(this Vector3d vector)
        {
            return new float[] { (float)vector.X, (float)vector.Y, (float)vector.Z };
        }
    }
}
