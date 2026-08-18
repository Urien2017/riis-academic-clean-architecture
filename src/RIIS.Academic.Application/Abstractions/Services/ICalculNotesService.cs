
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Abstractions.Services;

public interface ICalculNotesService
{
    decimal? CalculerMoyenneElementConstitutif(decimal? moyenneCcon, decimal? moyenneCc, decimal? moyenneSnOuSr);
    bool EstEligibleRattrapage(decimal? moyenneElementConstitutif);
    decimal CalculerCreditsAcquis(MaquetteElementConstitutif maquetteElementConstitutif, decimal? moyenneRetenue);
}
