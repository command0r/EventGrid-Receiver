using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;

namespace EventGrid_receiver.Services;

public interface IEventGridService
{
    Task PublishTopic(EventGridEvent eventPayload);

    Task PublishTopic(string subject, string eventType, object eventData);

    Task PublishTopic(string topic, string subject, string eventType, object eventData);

    Task PublishTopic(string topic, string subject, string eventType, object eventData, string id,
        string metadataVersion, string dataVersion);

    SubscriptionValidationResponse ValidateWebhook(EventGridEvent payload);
}