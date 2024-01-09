namespace Utility.Kafka.Constants;

/// <summary>
/// Dominio valori Operations
/// </summary>
public static class Operations {

    /// <summary>
    ///  Valore: I - Insert
    /// </summary>
    public const string Insert = "I";

    /// <summary>
    ///  Valore: U - Update
    /// </summary>
    public const string Update = "U";

    /// <summary>
    ///  Valore: D - Delete
    /// </summary>
    public const string Delete = "D";

    public static string GetStringValue(Enum valueEnum) {
        return valueEnum switch {
            Enumeration.Insert => Insert,
            Enumeration.Update => Update,
            Enumeration.Delete => Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(valueEnum), $"{nameof(valueEnum)} contains an invalid value '{valueEnum}'")
        };
    }

    public static bool IsValid(string value) =>
        value == Insert ||
        value == Update ||
        value == Delete;

    public enum Enumeration {
        Insert,
        Update,
        Delete
    }
}