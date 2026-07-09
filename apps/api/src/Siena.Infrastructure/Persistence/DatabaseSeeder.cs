using Microsoft.EntityFrameworkCore;
using Siena.Domain;
using Siena.Domain.Events;
using Siena.Domain.Users;
using Siena.Infrastructure.Persistence.Entities;

namespace Siena.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SienaDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        dbContext.Users.AddRange(
            new UserEntity
            {
                Id = "user-admin-dev",
                PhoneNumber = "+5511999990001",
                DisplayName = "Admin DEV",
                Role = DomainLabels.ToLabel(UserRole.Admin)
            },
            new UserEntity
            {
                Id = "user-coach-dev",
                PhoneNumber = "+5511999990002",
                DisplayName = "Comissão DEV",
                Role = DomainLabels.ToLabel(UserRole.Coach)
            },
            new UserEntity
            {
                Id = "user-athlete-dev-1",
                PhoneNumber = "+5511999990003",
                DisplayName = "Atleta DEV 1",
                Role = DomainLabels.ToLabel(UserRole.Athlete),
                Position = DomainLabels.ToLabel(PlayerPosition.Levantadora)
            },
            new UserEntity
            {
                Id = "user-athlete-dev-2",
                PhoneNumber = "+5511999990004",
                DisplayName = "Atleta DEV 2",
                Role = DomainLabels.ToLabel(UserRole.Athlete),
                Position = DomainLabels.ToLabel(PlayerPosition.Ponteiro)
            });

        dbContext.Events.AddRange(
            new EventEntity
            {
                Id = "liga-nacional-minas-2026-03-15",
                Title = "Liga Nacional — Siena vs. Minas T.C.",
                Type = DomainLabels.ToLabel(EventType.LigaNacional),
                Category = DomainLabels.ToLabel(EventCategory.Masculino),
                StartsAt = Utc("2026-03-15T19:30:00-03:00"),
                Location = "Ginásio Principal",
                Opponent = "Minas T.C.",
                Description = "Partida da Liga Nacional no ginásio principal da A.E. Siena."
            },
            new EventEntity
            {
                Id = "treino-fisico-2026-09-15",
                Title = "Treino Físico",
                Type = DomainLabels.ToLabel(EventType.TreinoFisico),
                Category = DomainLabels.ToLabel(EventCategory.Feminino),
                StartsAt = Utc("2026-09-15T08:00:00-03:00"),
                Location = "Centro de Treinamento",
                Description = "Sessão de preparação física para o elenco feminino."
            },
            new EventEntity
            {
                Id = "amistoso-sesi-2026-03-22",
                Title = "Amistoso — Siena vs. Sesi-SP",
                Type = DomainLabels.ToLabel(EventType.Amistoso),
                Category = DomainLabels.ToLabel(EventCategory.Masculino),
                StartsAt = Utc("2026-03-22T20:00:00-03:00"),
                Location = "Fora de Casa",
                Opponent = "Sesi-SP",
                Description = "Amistoso preparatório fora de casa."
            });

        dbContext.Videos.AddRange(
            new VideoEntity
            {
                Id = "video-liga-nacional-2026",
                Title = "Melhores momentos — Liga Nacional 2026",
                Url = "https://example.com/videos/liga-nacional-2026",
                DurationSeconds = 765,
                PublishedAt = Utc("2026-03-10T14:00:00-03:00"),
                Views = 128
            },
            new VideoEntity
            {
                Id = "video-treino-fisico-marco",
                Title = "Treino físico — março 2026",
                Url = "https://example.com/videos/treino-fisico-marco",
                DurationSeconds = 420,
                PublishedAt = Utc("2026-03-05T09:30:00-03:00"),
                Views = 54
            },
            new VideoEntity
            {
                Id = "video-amistoso-sesi",
                Title = "Amistoso Siena vs. Sesi-SP — resumo",
                Url = "https://example.com/videos/amistoso-sesi",
                DurationSeconds = 612,
                PublishedAt = Utc("2026-02-28T18:15:00-03:00"),
                Views = 203
            });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static DateTimeOffset Utc(string value) =>
        DateTimeOffset.Parse(value).ToUniversalTime();
}
