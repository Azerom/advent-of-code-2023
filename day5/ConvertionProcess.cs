namespace day5
{
    public class ConvertionProcess
    {
        private readonly LinkedList<Convertor> Convertors = new();

        public void AddConvertionStep(Convertor convertor)
        {
            Convertors.AddLast(convertor);
        }

        public long Convert(long source)
            => Process<long>(source, (c, s) => c.Convert(s));

        public IEnumerable<Range> ConvertRange(Range range) 
            => Process<IEnumerable<Range>>([range], (c, s) => c.ConvertRangeList(s));

        private T Process<T>(T source, Func<Convertor, T, T> operation)
        {
            T currentObject = source;
            var currentStep = Convertors.First;

            while (currentStep != null)
            {
                currentObject = operation(currentStep.Value, currentObject);
                currentStep = currentStep.Next;
            }

            return currentObject;
        }
    }
}