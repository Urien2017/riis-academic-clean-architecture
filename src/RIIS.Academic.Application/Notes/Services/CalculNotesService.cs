
using RIIS.Academic.Application.Abstractions.Services;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Notes.Services;

public class CalculNotesService : ICalculNotesService
{
    public decimal? CalculerMoyenneElementConstitutif(decimal? moyenneCcon, decimal? moyenneCc, decimal? moyenneSnOuSr)
    {
        if (moyenneCcon is null || moyenneCc is null || moyenneSnOuSr is null)
        {
            return null;
        }

        return Math.Round((moyenneCcon.Value * 0.20m) + (moyenneCc.Value * 0.10m) + (moyenneSnOuSr.Value * 0.70m), 2);
    }

    public bool EstEligibleRattrapage(decimal? moyenneElementConstitutif)
        => moyenneElementConstitutif.HasValue && moyenneElementConstitutif.Value < 10m;

    public decimal CalculerCreditsAcquis(MaquetteElementConstitutif maquetteElementConstitutif, decimal? moyenneRetenue)
        => moyenneRetenue.HasValue && moyenneRetenue.Value >= 10m ? maquetteElementConstitutif.Credits : 0m;
}
