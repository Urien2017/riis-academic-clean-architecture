using RIIS.Academic.Application.Dashboard.Dtos;

namespace RIIS.Academic.Application.Dashboard.Services;

public interface IDashboardAcademiqueService
{
    Task<DashboardAcademiqueDto> GetDashboardAcademiqueAsync(
        DashboardAcademiqueFilterDto? filter = null,
        CancellationToken cancellationToken = default);
}
