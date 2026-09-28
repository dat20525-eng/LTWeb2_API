using LTWeb2_API.Models.Domain;
using LTWeb2_API.Models.DTO;

namespace LTWeb2_API.Repositories
{
    public interface IPublisherRepository
    {
        List<PublisherDTO> GetAllPublishers();

        PublisherNoIdDTO GetPublisherById(int id);

        AddPublisherRequestDTO AddPublisher(
            AddPublisherRequestDTO addPublisherRequestDTO);

        PublisherNoIdDTO UpdatePublisherById(
            int id,
            PublisherNoIdDTO publisherNoIdDTO);

        Publisher? DeletePublisherById(int id);
    }
}