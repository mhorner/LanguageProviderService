using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.InteropServices;
using Data;
using Data.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Messages;

[ApiController]
[Route("/api/[controller]")]
public class MessageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public IUnitOfWork UnitOfWork { get; set; }

    public MessageController()
    {
        _unitOfWork = RuntimeUnitOfWork.CreateUnitOfWork();
    }
    
    [HttpGet("", Name = "GetAllMessage")]
    public IActionResult GetAllMessages(string culture)
    {
        return Ok(_unitOfWork.Messages.GetAllMessages(culture).Result);
    }

    [HttpGet("{id}", Name = "GetMessageById")]
    public IActionResult GetMessageById(Guid id)
    {
        var message = _unitOfWork.Messages.GetMessageById(id).Result;
        if (message == null) return NotFound();
        return MessageOk(_unitOfWork.Messages.GetMessageById(id).Result);
    }

    /// <summary>
    /// Get the matching message by key.
    /// </summary>
    /// <param name="key">The key of the message.</param>
    /// <returns>A matching message by key.</returns>
    [HttpGet("key/{key}", Name = "GetMessageByKey")]
    public IActionResult GetMessageByKey(string key)
    {
        var message = _unitOfWork.Messages.GetMessageByKey(key).FirstOrDefault();
        if (message == null)
        {
            return NotFound();
        }

        return MessageOk(message);
    }

    /// <summary>
    /// Adds a new message to the message repository
    /// </summary>
    /// <param name="key">The message key.</param>
    /// <param name="value">The message value.</param>
    /// <param name="culture">The culture for the message.</param>
    /// <returns></returns>
    [HttpPost("", Name = "CreateMessage")]
    public IActionResult CreateMessage(string key, string value, string culture)
    {
        var message = new Message(id: Guid.NewGuid(), key: key, value: value, culture: culture);
        
        var task = _unitOfWork.Messages.InsertMessage(message);
        if (!task.Result)
            return BadRequest($"Message with key {key} already exists, duplicates cannot be created.");
        
        return MessageOk(message);
    }

    /// <summary>
    /// Deletes a message by Id.
    /// </summary>
    /// <param name="id">Id of the record to delete from the database.</param>
    /// <returns>True if successful, false otherwise.</returns>
    [HttpDelete("{id}", Name = "DeleteMessage")]
    public IActionResult DeleteMessage(Guid id)
    {
        var isMessageDeleted = _unitOfWork.Messages.DeleteMessage(id).Result;
        if (!isMessageDeleted) return NotFound();
        return MessageOk(null);
    }

    /// <summary>
    /// Update an existing message specified by the Id.
    /// </summary>
    /// <param name="id">The id of the existing message</param>
    /// <param name="value">The new value of the message.</param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateMessage")]
    public IActionResult UpdateMessage(Guid id, string value)
    {
        var isMessageUpdated = _unitOfWork.Messages.UpdateMessage(id, value).Result;
        var updatedMessage = _unitOfWork.Messages.GetMessageById(id).Result;
        return isMessageUpdated ? Ok(updatedMessage) : NotFound();
    }
    
    private IActionResult MessageOk(object? entity)
    {
        _unitOfWork.Save();
        return base.Ok(entity);
    }
}