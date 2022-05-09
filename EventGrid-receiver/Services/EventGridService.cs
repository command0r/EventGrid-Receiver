using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace EventGrid_receiver.Services;

public class EventGridService : IEventGridService
{
    private EventGridClient _eventClient;
    private string _eventGridEndPoint;
    private string _eventHost;

    public EventGridService(string eventGridKey, string eventGridEndPoint)
    {
        _eventGridEndPoint = eventGridEndPoint;
        _eventHost = new Uri(_eventGridEndPoint).Host;

        TopicCredentials eventGridCredentials = new TopicCredentials(eventGridKey);
        _eventClient = new EventGridClient(eventGridCredentials);
    }

    public async Task PublishTopic(EventGridEvent eventData)
    {
        var payload = new List<EventGridEvent>();
        payload.Add(eventData);

        await _eventClient.PublishEventsAsync(_eventHost, payload);
    }

    public async Task PublishTopic(string subject, string eventType, object eventData)
    {
        var id = Guid.NewGuid().ToString();

        List<EventGridEvent> payload = new List<EventGridEvent>
        {
            new EventGridEvent(id, subject, eventData, eventType, DateTime.Now, "1.0")
        };

        await _eventClient.PublishEventsAsync(_eventHost, payload);
    }

    public async Task PublishTopic(string topic, string subject, string eventType, object eventData)
    {
        var id = Guid.NewGuid().ToString();

        List<EventGridEvent> payload = new List<EventGridEvent>
        {
            new EventGridEvent(id, subject, eventData, eventType, DateTime.Now, "1.0", topic)
        };

        await _eventClient.PublishEventsAsync(_eventHost, payload);
    }

    public async Task PublishTopic(string topic, string subject, string eventType, object eventData, string id,
        string metadataVersion, string dataVersion)
    {
        List<EventGridEvent> payload = new List<EventGridEvent>
        {
            new EventGridEvent(id, subject, eventData, eventType, DateTime.Now, dataVersion, topic, metadataVersion)
        };

        await _eventClient.PublishEventsAsync(_eventHost, payload);
    }

    public SubscriptionValidationResponse ValidateWebhook(EventGridEvent payload)
    {
        var response = JsonConvert.DeserializeObject<SubscriptionValidationEventData>(payload.Data.ToString());
        return new SubscriptionValidationResponse(response.ValidationCode);
    }
}