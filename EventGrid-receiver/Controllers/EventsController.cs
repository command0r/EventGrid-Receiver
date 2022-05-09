using EventGrid_receiver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;

namespace EventGrid_receiver.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventGridService _eventGrid;
    private readonly ILogger<EventsController> _logger;

    public EventsController(ILogger<EventsController> logger, IEventGridService eventGrid)
    {
        _logger = logger;
        _eventGrid = eventGrid;
    }

    [HttpPost]
    public IActionResult Post([FromBody] EventGridEvent[] events)
    {
        foreach (var eventGridEvent in events)
        {
            _logger.Log(LogLevel.Information, "Event Grid Event Received. Type: " + eventGridEvent.EventType);

            // 1. If there is no EventType through a bad request
            if (eventGridEvent == null)
                return BadRequest();

            // 2. If the EventType is the Event Grid handshake event, respond with a SubscriptionValidationResponse
            if (eventGridEvent.EventType == EventTypes.EventGridSubscriptionValidationEvent)
                return Ok(_eventGrid.ValidateWebhook(eventGridEvent));

            // 3. If the EventType is a return message, send a message to Discord

            if (eventGridEvent.EventType == "sample.eventgridhandler.testappmessage")
            {
                _logger.Log(LogLevel.Information, "Message Received: " + eventGridEvent.Data);
                return Ok();
            }

            _logger.Log(LogLevel.Error, "Unhandled Message Type: " + eventGridEvent.EventType);
            return BadRequest();
        }

        return BadRequest();
    }
}