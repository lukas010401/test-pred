using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testPred2
{
    public class Montant
    {
        [LoadColumn(0)]
        public float Direction { get; set; }
        [LoadColumn(1)]
        public float NombreCollaborateurs { get; set; }
        [LoadColumn(2)]
        public float Annee { get; set; }
        [LoadColumn(3)]
        public float Mois { get; set; }
        [LoadColumn(4)]
        public float SommeMontantTotalPaye { get; set; }

    }

    public class MontantPred
    {
        [ColumnName("Score")]
        public float SommeMontantTotalPaye { get; set; }

    }
}
