using Entities;
using NUnit.Framework.Internal;

namespace Tests.Controllers.Messages;

public class MessageControllerTests : UnitOfWorkTest
{
    private Logger _logger = new Logger("Message Tests", InternalTraceLevel.Debug, new StringWriter());
    private MessageController _sut;

    [SetUp]
    public void SetUp()
    {
        var unitOfWork = CreateUnitOfWork();
        _sut = new MessageController();
        _sut.UnitOfWork = unitOfWork;
    }

    [TearDown]
    public void TearDown()
    {
        _sut.UnitOfWork.Dispose();
        _sut.UnitOfWork = null;
        _sut = null;
    }

    [Test]
    public void GetMessageById_Returns400WhenMessageNotFound()
    {
        var guid = Guid.NewGuid();
        var getResult = Cast(_sut.GetMessageById(guid), out Message getMessage);
        Assert.That(getResult, Is.EqualTo(HTTP_NOTFOUND));
        Assert.Null(getMessage);
    }
    
    [Test]
    public void DeleteMessage_Returns400WhenMessageNotFound()
    {
        var guid = Guid.NewGuid();
        var getResult = Cast(_sut.DeleteMessage(guid), out Message getMessage);
        Assert.That(getResult, Is.EqualTo(HTTP_NOTFOUND));
        Assert.Null(getMessage);
    }
    
    [Test]
    public void GetMessageByKey_Returns400WhenMessageNotFound()
    {
        var key = "AnyNotFoundKey";
        var getResult = Cast(_sut.GetMessageByKey(key), out Message getMessage);
        Assert.That(getResult, Is.EqualTo(HTTP_NOTFOUND));
        Assert.Null(getMessage);
    }
    
    [Test]
    public void GetMessageByKey_Returns200WhenMessageFound()
    {
        var CreateMessageKey = "CreateMessage_GetMessageByIdReturnsTheNewMessageById";
        var CreateMessageValue = "Create a new message, in English!";
        var CreateMessageCulture = "en-US";

        var createResult = Cast(_sut.CreateMessage(CreateMessageKey, CreateMessageValue, CreateMessageCulture),
            out Message createMessage);
        Assert.That(createResult, Is.EqualTo(HTTP_OK));
        Assert.That(createMessage.Key, Is.EqualTo(CreateMessageKey));
        
        var getResult = Cast(_sut.GetMessageByKey(CreateMessageKey), out Message getMessage);
        Assert.That(getResult, Is.EqualTo(HTTP_OK));
        Assert.That(getMessage.Id, Is.EqualTo(createMessage.Id));
    }
    
    [Test]
    public void CreateMessage_GetMessageByIdReturnsTheNewMessageById()
    {
        var CreateMessageKey = "CreateMessage_GetMessageByIdReturnsTheNewMessageById";
        var CreateMessageValue = "Create a new message, in English!";
        var CreateMessageCulture = "en-US";

        var createResult = Cast(_sut.CreateMessage(CreateMessageKey, CreateMessageValue, CreateMessageCulture),
            out Message newMessage);
        var getResult = Cast(_sut.GetMessageById(newMessage.Id), out Message readMessage);
        var deleteNewMessageResult = Cast(_sut.DeleteMessage(newMessage.Id), out Message _);

        Assert.That(createResult, Is.EqualTo(HTTP_OK));
        Assert.That(getResult, Is.EqualTo(HTTP_OK));
        Assert.That(readMessage.Id, Is.EqualTo(newMessage.Id));
        Assert.That(readMessage.Key, Is.EqualTo(newMessage.Key));
        Assert.That(readMessage.Value, Is.EqualTo(newMessage.Value));
        Assert.That(readMessage.Culture, Is.EqualTo(newMessage.Culture));
        Assert.That(deleteNewMessageResult, Is.EqualTo(HTTP_OK));
    }

    [Test]
    public void CreateMessage_FailsWhenAMessageExistsWithTheSameKey()
    {
        const string CreateMessageKey = "CreateMessage_FailsWhenAMessageExistsWithTheSameKey";
        const string CreateMessageValue = "Create a new message, in English!";
        const string CreateMessageCulture = "en-US";

        var createResult = Cast(_sut.CreateMessage(CreateMessageKey, CreateMessageValue, CreateMessageCulture),
            out Message createMessage);
        var duplicateCreateResult = Cast(_sut.CreateMessage(CreateMessageKey, CreateMessageValue, CreateMessageCulture),
            out Message duplicateMessage);
        var deleteNewMessageResult = Cast(_sut.DeleteMessage(createMessage.Id), out Message _);

        Assert.That(createResult, Is.EqualTo(HTTP_OK));
        Assert.That(duplicateCreateResult, Is.EqualTo(HTTP_BADREQUEST));
        Assert.That(createMessage, Is.TypeOf<Message>());
        Assert.That(duplicateMessage, Is.Null);
        Assert.That(deleteNewMessageResult, Is.EqualTo(HTTP_OK));
    }

    [Test]
    public void GetAllMessages_ReturnsAllMessagesForEnUs()
    {
        var messageSizeCount = 1000;
        var emptyGetResult = Cast(_sut.GetAllMessages("en-US"), out IList<Message> emptyMessageList);
        Assert.That(emptyGetResult, Is.EqualTo(HTTP_OK));
        Assert.That(emptyMessageList, Is.Empty);

        var CreateMessageKey = "GetAllMessages_ReturnsAllMessagesForEnUs{0}";
        const string CreateMessageValue = "Create a new message, in English!";
        const string CreateMessageCulture = "en-US";
        for (var i = 0; i < messageSizeCount; i++)
        {
            var createResult = Cast(
                _sut.CreateMessage(string.Format(CreateMessageKey, i), CreateMessageValue, CreateMessageCulture),
                out Message _);
            Assert.That(createResult, Is.EqualTo(HTTP_OK));
        }

        var populatedGetResult = Cast(_sut.GetAllMessages("en-US"), out IList<Message> populateMessageList);
        Assert.That(populateMessageList, Has.Count.EqualTo(messageSizeCount));

        for (var i = 0; i < messageSizeCount; i++)
        {
            var deleteNewMessageResult = Cast(_sut.DeleteMessage(populateMessageList[i].Id), out Message _);
            Assert.That(deleteNewMessageResult, Is.EqualTo(HTTP_OK));
        }
    }

    [Test]
    public void UpdateMessage_ReturnsUpdateMessageSuccessful()
    {
        var CreateMessageKey = "UpdateMessage_ReturnsUpdateMessageSuccessful";
        var UpdateMessageKey = "UPDATED: UpdateMessage_ReturnsUpdateMessageSuccessful";
        const string CreateMessageValue = "Create a new message, in English!";
        const string CreateMessageCulture = "en-US";
        
        var createResult = Cast(_sut.CreateMessage(CreateMessageKey, CreateMessageValue, CreateMessageCulture),
            out Message createMessage);
        Assert.That(createResult, Is.EqualTo(HTTP_OK));

        var updateResult = Cast(_sut.UpdateMessage(createMessage.Id, UpdateMessageKey), out Message updateMessage);
        Assert.That(updateResult, Is.EqualTo(HTTP_OK));
        Assert.That(updateMessage.Value, Is.EqualTo(UpdateMessageKey));
    }
}