using System.Text.RegularExpressions;

namespace day5
{
    public partial class Convertor
    {
        const string mapRegex = @"(?'destination'\d+) (?'source'\d+) (?'lenght'\d+)";

        [GeneratedRegex(mapRegex)]
        private static partial Regex MapRegex();

        public record Map(long SourceFirst, long SourceLast, long DestinationFirst, long DestinationLast);

        public IEnumerable<Map> Maps { get; set; }

        public Convertor(IEnumerable<Map> maps)
        {
            Maps = maps;
        }

        public static Convertor FromStrings(IEnumerable<string> strings)
        {
            var maps = strings.Select(s =>
            {
                var match = MapRegex().Match(s);

                var destination = long.Parse(match.Groups["destination"].Value);
                var source = long.Parse(match.Groups["source"].Value);
                var lenght = long.Parse(match.Groups["lenght"].Value);

                return new Map(source, source + lenght - 1, destination, destination + lenght - 1);
            });
            return new Convertor(maps);
        }

        public long Convert(long sourceId)
        {
            var map = Maps.FirstOrDefault(m => m.SourceFirst <= sourceId && sourceId <= m.SourceLast);

            return Convert(sourceId, map);
        }

        public static long Convert(long sourceId, Map? map)
        {
            return sourceId - map?.SourceFirst + map?.DestinationFirst ?? sourceId;
        }

        public IEnumerable<Range> ConvertRangeList(IEnumerable<Range> sourceRange)
        {
            List<Range> result = [];

            foreach (Range range in sourceRange)
            {
                result.AddRange(ConvertRange(range));
            }

            return result;
        }

        private List<Range> ConvertRange(Range range)
        {
            List<Range> resultRanges = [];

            Range currentRange = range;
            Map? currentMap;

            bool finished = false;

            //On tente de recuperer une Map qui contient le premier élement de la range
            currentMap = Maps.FirstOrDefault(m => m.SourceFirst <= currentRange.First && currentRange.First <= m.SourceLast);

            //Si aucune Map ne correspond au premier élément de la range, on vérifie si une autre Map contient une partie de la range
            if (currentMap == null)
            {
                currentMap = GetNextMap(currentRange.First);

                //Si aucune map ne contient une partie de la range, on retourne sans modifier la range
                if (currentMap == null)
                {
                    resultRanges.Add(currentRange);
                    finished = true;
                }
                else
                {
                    //Si une Map est trouvé on ajoute aux resultats une range du début de la range en cours d'analyse jusqu'au début de la Map
                    //(pas de conversion pour cette nouvelle range, car pas de Map qui correspond)
                    //Puis on continue normalement pour le reste de la range initiale.
                    resultRanges.Add(new Range(currentRange.First, currentMap.SourceFirst - 1));
                    currentRange = new Range(currentMap.SourceFirst, currentRange.Last);
                }

            }
            else
            {
                //Si la Map peut contenir l'entiéretée de la range, on convertit son début et sa fin et on ajoute aux resultats
                //puis on retourne : il ne reste plus rien a convertir
                if (CanMapContainRange(currentRange, currentMap))
                {
                    resultRanges.Add(new Range(Convert(currentRange.First, currentMap), Convert(currentRange.Last, currentMap)));
                    finished = true;
                }
                //Sinon, cela signifie que cette Map peut convertir une partie de la range mais pas son intégralitée :
                // la Map se termine avant la Range
                //On ajoute aux resultats une nouvelle Range convertie jusqu'a la fin de la Map et on continue avec le reste de la Range
                else
                {
                    resultRanges.Add(new Range(Convert(currentRange.First, currentMap), currentMap.DestinationLast));
                    currentRange = new Range(currentMap.SourceLast + 1, currentRange.Last);
                }
            }

            //Si la conversion n'est pas terminée, on convertis ce qui reste
            //Initialement j'utilisais un while(true), je l'évite avec la récursion mais il est possible qu'on perde en lisibilité
            if (!finished)
            {
                resultRanges.AddRange(ConvertRange(currentRange));
            }

            return resultRanges;
        }

        private Map? GetNextMap(long id)
        {
            return Maps.Where(m => m.SourceFirst >= id && m.SourceLast <= id).OrderBy(m => m.SourceFirst).FirstOrDefault();
        }

        private static bool CanMapContainRange(Range range, Map map)
        {
            return map.SourceFirst <= range.First && range.Last <= map.SourceLast;
        }

    }
}