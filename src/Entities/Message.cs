using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Message {
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The key used to reference the message in code.
    /// </summary>
    public string Key {get; set;}
    
    /// <summary>
    /// The message value referenced by the key.
    /// </summary>
    public string Value {get; set;}

    /// <summary>
    /// The culture designation for the message value.  This should conform to standard
    /// culture designations, i.e. en-US, es-MX, etc.
    /// </summary>
    public string Culture {get; set;}

    public Message(Guid id, string key, string value, string culture)
    {
        Id = id;
        Key = key;
        Value = value;
        Culture = culture;
    }
}