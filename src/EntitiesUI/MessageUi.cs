namespace EntitiesUi;

public class MessageUi
{
    public int MessageId { get; set; }
    public string MessageKey { get; set; }
    public string MessageValue { get; set; }
    public string MessageCulture { get; set; }

    public MessageUi(string key, string value, string culture)
    {
        MessageKey = key;
        MessageValue = value;
        MessageCulture = culture;
    }
}