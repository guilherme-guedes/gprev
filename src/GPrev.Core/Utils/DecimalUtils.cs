using System.Globalization;

namespace GPrev.Core.Utils
{
    internal class DecimalUtils
    {
        private static readonly CultureInfo _culturaBr = new("pt-BR");

        internal static bool TentarConverterValor(string valor, out decimal resultado)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                resultado = 0;
                return false;
            }

            var valorLimpo = valor.Replace(" ", "");

            return decimal.TryParse(valorLimpo, NumberStyles.Number, _culturaBr, out resultado);
        }
    }
}
