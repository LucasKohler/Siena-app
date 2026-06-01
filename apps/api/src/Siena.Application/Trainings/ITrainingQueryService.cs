namespace Siena.Application.Trainings;

public interface ITrainingQueryService
{
    Task<NextTrainingDto?> GetNextTrainingAsync(
        string userId,
        CancellationToken cancellationToken);
}
