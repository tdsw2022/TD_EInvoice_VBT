using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class TaxSubtotal
    {
        public double TaxableAmount { get; set; }
        public double TaxAmount { get; set; }
        public double CalculationSequenceNumeric { get; set; }
        public double TransactionCurrencyTaxAmount { get; set; }
        public double Percent { get; set; }
        public Measure BaseUnitMeasure { get; set; }
        public double PerUnitAmount { get; set; }
        public TaxCategory TaxCategory { get; set; }

    }
}