using konsi_api.Models.Events;

namespace konsi_api.Models.Interfaces
{
    public interface IPublishCpfService
    {
        public void Publish(CpfSearchedEvent @event);
    }
}
