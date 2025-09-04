namespace Core.io;

public sealed class CSVHelper
{
    internal static object? ParseType(Type type, string serialized)
    {
        if (type.IsEnum) return Enum.TryParse(type, serialized, out var result) ? result : null;
        return ParsePrimitive(type, serialized);
    }

    private static object? ParsePrimitive(Type type, string serialized)
    {
        object? obj = null;
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.String:
                obj = serialized;
                break;
            case TypeCode.Int16:
                if (short.TryParse(serialized, out var @int16)) obj = @int16;
                break;
            case TypeCode.Int32:
                if (int.TryParse(serialized, out var @int32)) obj = @int32;
                break;
            case TypeCode.Int64:
                if (long.TryParse(serialized, out var @int64)) obj = @int64;
                break;
            case TypeCode.UInt16:
                if (ushort.TryParse(serialized, out var @uint16)) obj = @uint16;
                break;
            case TypeCode.UInt32:
                if (uint.TryParse(serialized, out var @uint32)) obj = @uint32;
                break;
            case TypeCode.UInt64:
                if (ulong.TryParse(serialized, out var @uint64)) obj = @uint64;
                break;
            case TypeCode.Single:
                if (float.TryParse(serialized, out var @single)) obj = @single;
                break;
            case TypeCode.Double:
                if (double.TryParse(serialized, out var @double)) obj = @double;
                break;
            case TypeCode.Decimal:
                if (decimal.TryParse(serialized, out var @decimal)) obj = @decimal;
                break;
            case TypeCode.Char:
                if (char.TryParse(serialized, out var @char)) obj = @char;
                break;
            case TypeCode.Byte:
                if (byte.TryParse(serialized, out var @byte)) obj = @byte;
                break;
            case TypeCode.SByte:
                if (sbyte.TryParse(serialized, out var @sbyte)) obj = @sbyte;
                break;
            case TypeCode.Boolean:
                if (bool.TryParse(serialized, out var @boolean)) obj = @boolean;
                break;
            default:
                throw new ArgumentException($"Unsupported type: {type.FullName}");
        }
        return obj;
    }
}
