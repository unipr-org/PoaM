using Imu.Shared;

namespace Imu.Business.Abstraction
{
    public interface IBusiness
    {
        Task CreateCategoriaCatastale(CategoriaCatastaleInsertDto categoriaCatastaleInsertDto, CancellationToken cancellationToken = default);
        Task CreateImmobile(ImmobileInsertDto immobileInsertDto, CancellationToken cancellationToken = default);
        Task AssociaAnagraficaImmobile(AssociaAnagraficaImmobileDto associaAnagraficaImmobileDto, CancellationToken cancellationToken = default);
    }
}
